using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using VIRTUAL_CLASSE_FORMATION.Data;
using VIRTUAL_CLASSE_FORMATION.Dtos;

namespace VIRTUAL_CLASSE_FORMATION.Services
{
    public interface IAvatarIaService
    {
        Task<AvatarSessionDto> DemarrerSessionAsync(StartAvatarSessionDto dto);
    }

    // Integration reelle de l'API Anam.ai (https://docs.anam.ai).
    // Le backend echange la cle API (secrete, jamais exposee au navigateur)
    // contre un jeton de session de courte duree. Ce jeton est ensuite
    // utilise cote client avec le SDK JS Anam (@anam-ai/js-sdk) pour
    // demarrer le flux video/audio en temps reel.
    public class AvatarIaService : IAvatarIaService
    {
        private readonly HttpClient httpClient;
        private readonly AppDbContext context;
        private readonly int maxSessionLengthSeconds;

        public AvatarIaService(HttpClient httpClient, IConfiguration configuration, AppDbContext context)
        {
            this.httpClient = httpClient;
            this.context = context;

            var section = configuration.GetSection("AnamIA");
            this.httpClient.BaseAddress = new Uri("https://api.anam.ai/");
            this.httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", section["ApiKey"]);

            maxSessionLengthSeconds = int.TryParse(section["MaxSessionLengthSeconds"], out var v) ? v : 1800;
        }

        public async Task<AvatarSessionDto> DemarrerSessionAsync(StartAvatarSessionDto dto)
        {
            var formateur = await context.Formateurs.FindAsync(dto.FormateurId);
            if (formateur == null)
                throw new InvalidOperationException("Formateur introuvable.");

            var module = await context.Modules
                .Include(m => m.Modalites)
                .FirstOrDefaultAsync(m => m.Id == dto.ModuleFormationId);
            if (module == null)
                throw new InvalidOperationException("Module introuvable.");

            object personaConfig;

            if (!string.IsNullOrWhiteSpace(formateur.PersonaId))
            {
                // Methode recommandee : reference directe au persona cree dans lab.anam.ai.
                // Toute la config (avatar, voix, LLM, prompt) est deja geree dans le Lab.
                personaConfig = new { personaId = formateur.PersonaId };
            }
            else
            {
                // Methode manuelle (fallback) : construit la config a partir des champs
                // avatarId/voiceId/llmId, et injecte le contenu du module dans le prompt.
                var contenuModule = string.Join(
                    "\n",
                    module.Modalites?.Select(m => $"- {m.Titre} ({m.Type}) : {m.Contenu}") ?? Enumerable.Empty<string>()
                );

                var systemPromptComplet =
                    $"{formateur.SystemPrompt}\n\n" +
                    $"Tu presentes actuellement le module \"{module.Titre}\". " +
                    $"Voici son contenu, sers-toi en pour presenter et repondre aux questions :\n{contenuModule}";

                personaConfig = new
                {
                    name = formateur.Nom,
                    avatarId = formateur.AvatarId,
                    avatarModel = formateur.AvatarModel,
                    voiceId = formateur.VoiceId,
                    llmId = formateur.LlmId,
                    systemPrompt = systemPromptComplet,
                    maxSessionLengthSeconds,
                    skipGreeting = false
                };
            }

            var payload = new
            {
                clientLabel = $"formation-{module.FormationId}-module-{module.Id}",
                personaConfig
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("v1/auth/session-token", content);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();
            using var json = JsonDocument.Parse(body);

            var sessionToken = json.RootElement.GetProperty("sessionToken").GetString() ?? string.Empty;

            return new AvatarSessionDto { SessionToken = sessionToken };
        }
    }
}
