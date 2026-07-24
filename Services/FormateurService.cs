using Microsoft.EntityFrameworkCore;
using VIRTUAL_CLASSE_FORMATION.Data;
using VIRTUAL_CLASSE_FORMATION.Models;

namespace VIRTUAL_CLASSE_FORMATION.Services
{
    public interface IFormateurService
    {
        Task<List<Formateur>> GetAllAsync();
        Task<Formateur?> GetByIdAsync(int id);
        Task<Formateur> CreateAsync(Formateur formateur);
        Task<bool> UpdateAsync(int id, Formateur formateur);
        Task<bool> DeleteAsync(int id);
    }

    public class FormateurService : IFormateurService
    {
        private readonly AppDbContext context;

        public FormateurService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Formateur>> GetAllAsync()
            => await context.Formateurs.ToListAsync();

        public async Task<Formateur?> GetByIdAsync(int id)
            => await context.Formateurs.FindAsync(id);

        public async Task<Formateur> CreateAsync(Formateur formateur)
        {
            context.Formateurs.Add(formateur);
            await context.SaveChangesAsync();
            return formateur;
        }

        public async Task<bool> UpdateAsync(int id, Formateur formateur)
        {
            var existant = await context.Formateurs.FindAsync(id);
            if (existant == null) return false;

            existant.Nom = formateur.Nom;
            existant.Biographie = formateur.Biographie;
            existant.AvatarId = formateur.AvatarId;
            existant.AvatarModel = formateur.AvatarModel;
            existant.VoiceId = formateur.VoiceId;
            existant.LlmId = formateur.LlmId;
            existant.SystemPrompt = formateur.SystemPrompt;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existant = await context.Formateurs.FindAsync(id);
            if (existant == null) return false;

            context.Formateurs.Remove(existant);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
