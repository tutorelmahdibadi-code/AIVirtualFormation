using Microsoft.EntityFrameworkCore;
using VIRTUAL_CLASSE_FORMATION.Data;
using VIRTUAL_CLASSE_FORMATION.Models;

namespace VIRTUAL_CLASSE_FORMATION.Services
{
    public interface IModuleService
    {
        Task<List<ModuleFormation>> GetByFormationAsync(int formationId);
        Task<ModuleFormation?> GetByIdAsync(int id);
        Task<ModuleFormation> CreateAsync(ModuleFormation module);
        Task<bool> UpdateAsync(int id, ModuleFormation module);
        Task<bool> DeleteAsync(int id);
    }

    public class ModuleService : IModuleService
    {
        private readonly AppDbContext context;

        public ModuleService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<List<ModuleFormation>> GetByFormationAsync(int formationId)
            => await context.Modules
                .Where(m => m.FormationId == formationId)
                .OrderBy(m => m.Ordre)
                .ToListAsync();

        public async Task<ModuleFormation?> GetByIdAsync(int id)
            => await context.Modules
                .Include(m => m.Modalites)
                .FirstOrDefaultAsync(m => m.Id == id);

        public async Task<ModuleFormation> CreateAsync(ModuleFormation module)
        {
            bool formationExiste = await context.Formations.AnyAsync(f => f.Id == module.FormationId);
            if (!formationExiste)
                throw new InvalidOperationException("Formation introuvable.");

            context.Modules.Add(module);
            await context.SaveChangesAsync();
            return module;
        }

        public async Task<bool> UpdateAsync(int id, ModuleFormation module)
        {
            var existant = await context.Modules.FindAsync(id);
            if (existant == null) return false;

            existant.Titre = module.Titre;
            existant.Description = module.Description;
            existant.Ordre = module.Ordre;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existant = await context.Modules.FindAsync(id);
            if (existant == null) return false;

            context.Modules.Remove(existant);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
