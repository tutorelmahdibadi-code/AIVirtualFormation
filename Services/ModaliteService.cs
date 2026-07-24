using Microsoft.EntityFrameworkCore;
using VIRTUAL_CLASSE_FORMATION.Data;
using VIRTUAL_CLASSE_FORMATION.Models;

namespace VIRTUAL_CLASSE_FORMATION.Services
{
    public interface IModaliteService
    {
        Task<List<Modalite>> GetByModuleAsync(int moduleId);
        Task<Modalite?> GetByIdAsync(int id);
        Task<Modalite> CreateAsync(Modalite modalite);
        Task<bool> UpdateAsync(int id, Modalite modalite);
        Task<bool> DeleteAsync(int id);
    }

    public class ModaliteService : IModaliteService
    {
        private readonly AppDbContext context;

        public ModaliteService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Modalite>> GetByModuleAsync(int moduleId)
            => await context.Modalites
                .Where(m => m.ModuleFormationId == moduleId)
                .ToListAsync();

        public async Task<Modalite?> GetByIdAsync(int id)
            => await context.Modalites
                .Include(m => m.Questions!)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(m => m.Id == id);

        public async Task<Modalite> CreateAsync(Modalite modalite)
        {
            bool moduleExiste = await context.Modules.AnyAsync(m => m.Id == modalite.ModuleFormationId);
            if (!moduleExiste)
                throw new InvalidOperationException("Module introuvable.");

            context.Modalites.Add(modalite);
            await context.SaveChangesAsync();
            return modalite;
        }

        public async Task<bool> UpdateAsync(int id, Modalite modalite)
        {
            var existant = await context.Modalites.FindAsync(id);
            if (existant == null) return false;

            existant.Titre = modalite.Titre;
            existant.Type = modalite.Type;
            existant.Contenu = modalite.Contenu;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existant = await context.Modalites.FindAsync(id);
            if (existant == null) return false;

            context.Modalites.Remove(existant);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
