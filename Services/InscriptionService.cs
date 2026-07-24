using Microsoft.EntityFrameworkCore;
using VIRTUAL_CLASSE_FORMATION.Data;
using VIRTUAL_CLASSE_FORMATION.Models;

namespace VIRTUAL_CLASSE_FORMATION.Services
{
    public interface IInscriptionService
    {
        Task<List<Inscription>> GetByApprenantAsync(int apprenantId);
        Task<List<Inscription>> GetByFormationAsync(int formationId);
        Task<Inscription> InscrireAsync(int apprenantId, int formationId);
        Task<bool> MettreAJourProgressionAsync(int inscriptionId, int progression);
    }

    public class InscriptionService : IInscriptionService
    {
        private readonly AppDbContext context;

        public InscriptionService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Inscription>> GetByApprenantAsync(int apprenantId)
            => await context.Inscriptions
                .Include(i => i.Formation)
                .Where(i => i.ApprenantId == apprenantId)
                .ToListAsync();

        public async Task<List<Inscription>> GetByFormationAsync(int formationId)
            => await context.Inscriptions
                .Where(i => i.FormationId == formationId)
                .ToListAsync();

        public async Task<Inscription> InscrireAsync(int apprenantId, int formationId)
        {
            bool dejaInscrit = await context.Inscriptions
                .AnyAsync(i => i.ApprenantId == apprenantId && i.FormationId == formationId);
            if (dejaInscrit)
                throw new InvalidOperationException("Apprenant deja inscrit a cette formation.");

            var apprenant = await context.Utilisateurs.FindAsync(apprenantId);
            if (apprenant == null || apprenant.Role != Role.Apprenant)
                throw new InvalidOperationException("Utilisateur invalide pour une inscription.");

            var formation = await context.Formations.FindAsync(formationId);
            if (formation == null)
                throw new InvalidOperationException("Formation introuvable.");

            var inscription = new Inscription
            {
                ApprenantId = apprenantId,
                FormationId = formationId,
                DateInscription = DateTime.UtcNow,
                Statut = StatutInscription.EnCours,
                Progression = 0
            };

            context.Inscriptions.Add(inscription);
            await context.SaveChangesAsync();
            return inscription;
        }

        public async Task<bool> MettreAJourProgressionAsync(int inscriptionId, int progression)
        {
            var inscription = await context.Inscriptions.FindAsync(inscriptionId);
            if (inscription == null) return false;

            inscription.Progression = Math.Clamp(progression, 0, 100);
            if (inscription.Progression == 100)
                inscription.Statut = StatutInscription.Terminee;

            await context.SaveChangesAsync();
            return true;
        }
    }
}
