using Microsoft.EntityFrameworkCore;
using VIRTUAL_CLASSE_FORMATION.Data;
using VIRTUAL_CLASSE_FORMATION.Models;

namespace VIRTUAL_CLASSE_FORMATION.Services
{
    public interface ICategorieService
    {
        Task<List<Categorie>> GetAllAsync();
        Task<Categorie?> GetByIdAsync(int id);
        Task<Categorie> CreateAsync(Categorie categorie);
        Task<bool> UpdateAsync(int id, Categorie categorie);
        Task<bool> DeleteAsync(int id);
    }

    public class CategorieService : ICategorieService
    {
        private readonly AppDbContext context;

        public CategorieService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Categorie>> GetAllAsync()
            => await context.Categories.ToListAsync();

        public async Task<Categorie?> GetByIdAsync(int id)
            => await context.Categories.FindAsync(id);

        public async Task<Categorie> CreateAsync(Categorie categorie)
        {
            context.Categories.Add(categorie);
            await context.SaveChangesAsync();
            return categorie;
        }

        public async Task<bool> UpdateAsync(int id, Categorie categorie)
        {
            var existant = await context.Categories.FindAsync(id);
            if (existant == null) return false;

            existant.Nom = categorie.Nom;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existant = await context.Categories.FindAsync(id);
            if (existant == null) return false;

            context.Categories.Remove(existant);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
