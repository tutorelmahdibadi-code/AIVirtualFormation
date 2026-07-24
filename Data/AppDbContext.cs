using Microsoft.EntityFrameworkCore;
using VIRTUAL_CLASSE_FORMATION.Models;

namespace VIRTUAL_CLASSE_FORMATION.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Utilisateur> Utilisateurs => Set<Utilisateur>();
        public DbSet<Formateur> Formateurs => Set<Formateur>();
        public DbSet<Categorie> Categories => Set<Categorie>();
        public DbSet<Formation> Formations => Set<Formation>();
        public DbSet<ModuleFormation> Modules => Set<ModuleFormation>();
        public DbSet<Modalite> Modalites => Set<Modalite>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<ReponseOption> ReponseOptions => Set<ReponseOption>();
        public DbSet<Inscription> Inscriptions => Set<Inscription>();
        public DbSet<SessionFormation> Sessions => Set<SessionFormation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Utilisateur>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Utilisateur>()
                .HasOne(u => u.Formateur)
                .WithOne(f => f.Utilisateur)
                .HasForeignKey<Formateur>(f => f.UtilisateurId);

            modelBuilder.Entity<Formation>()
                .HasOne(f => f.Categorie)
                .WithMany(c => c.Formations)
                .HasForeignKey(f => f.CategorieId);

            modelBuilder.Entity<Formation>()
                .HasOne(f => f.Formateur)
                .WithMany(fo => fo.Formations)
                .HasForeignKey(f => f.FormateurId);

            modelBuilder.Entity<ModuleFormation>()
                .HasOne(m => m.Formation)
                .WithMany(f => f.Modules)
                .HasForeignKey(m => m.FormationId);

            modelBuilder.Entity<Modalite>()
                .HasOne(m => m.ModuleFormation)
                .WithMany(mf => mf.Modalites)
                .HasForeignKey(m => m.ModuleFormationId);

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Modalite)
                .WithMany(m => m.Questions)
                .HasForeignKey(q => q.ModaliteId);

            modelBuilder.Entity<ReponseOption>()
                .HasOne(r => r.Question)
                .WithMany(q => q.Options)
                .HasForeignKey(r => r.QuestionId);

            modelBuilder.Entity<Inscription>()
                .HasOne(i => i.Apprenant)
                .WithMany(u => u.Inscriptions)
                .HasForeignKey(i => i.ApprenantId);

            modelBuilder.Entity<Inscription>()
                .HasOne(i => i.Formation)
                .WithMany(f => f.Inscriptions)
                .HasForeignKey(i => i.FormationId);

            modelBuilder.Entity<SessionFormation>()
                .HasOne(s => s.Formation)
                .WithMany(f => f.Sessions)
                .HasForeignKey(s => s.FormationId);
        }
    }
}
