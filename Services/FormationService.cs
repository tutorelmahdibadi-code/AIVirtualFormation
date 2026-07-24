using Microsoft.EntityFrameworkCore;
using VIRTUAL_CLASSE_FORMATION.Data;
using VIRTUAL_CLASSE_FORMATION.Models;

namespace VIRTUAL_CLASSE_FORMATION.Services
{
    public interface IFormationService
    {
        Task<List<Formation>> GetAllAsync();
        Task<Formation?> GetByIdAsync(int id);
        Task<Formation> CreateAsync(Formation formation);
        Task<bool> UpdateAsync(int id, Formation formation);
        Task<bool> DeleteAsync(int id);
    }

    public class FormationService : IFormationService
    {
        private readonly AppDbContext context;

        public FormationService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Formation>> GetAllAsync()
            => await context.Formations
                .Include(f => f.Categorie)
                .Include(f => f.Formateur)
                .Include(f => f.Modules)
                .ToListAsync();

        public async Task<Formation?> GetByIdAsync(int id)
            => await context.Formations
                .Include(f => f.Categorie)
                .Include(f => f.Formateur)
                .Include(f => f.Modules!)
                    .ThenInclude(m => m.Modalites)
                .FirstOrDefaultAsync(f => f.Id == id);

        public async Task<Formation> CreateAsync(Formation formation)
        {
            bool categorieExiste = await context.Categories.AnyAsync(c => c.Id == formation.CategorieId);
            if (!categorieExiste)
                throw new InvalidOperationException("Categorie introuvable.");

            bool formateurExiste = await context.Formateurs.AnyAsync(f => f.Id == formation.FormateurId);
            if (!formateurExiste)
                throw new InvalidOperationException("Formateur introuvable.");

            context.Formations.Add(formation);
            await context.SaveChangesAsync();
            return formation;
        }

        public async Task<bool> UpdateAsync(int id, Formation formation)
        {
            var existant = await context.Formations.FindAsync(id);
            if (existant == null) return false;

            existant.Nom = formation.Nom;
            existant.Description = formation.Description;
            existant.CategorieId = formation.CategorieId;
            existant.FormateurId = formation.FormateurId;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existant = await context.Formations.FindAsync(id);
            if (existant == null) return false;

            context.Formations.Remove(existant);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
