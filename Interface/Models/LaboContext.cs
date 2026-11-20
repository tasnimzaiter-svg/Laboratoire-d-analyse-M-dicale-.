using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace GestionLaboratoire
{
    public class LaboContext : DbContext
    {
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Medecin> Medecins { get; set; }
        public DbSet<Echantillon> Echantillons { get; set; }
        public DbSet<Analyse> Analyses { get; set; }
        public DbSet<Assurance> Assurances { get; set; }
        public DbSet<Facture> Factures { get; set; }
        public DbSet<Paiement> Paiements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Base placée à la racine du projet (3 niveaux au-dessus de bin\Debug\net8.0-windows\)
            string chemin = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, @"..\..\..\laboratoire.db"));
            options.UseSqlite($"Data Source={chemin}");
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            // Héritage TPH : une table Utilisateurs + une colonne "Role"
            mb.Entity<Utilisateur>()
                .HasDiscriminator<string>("Role")
                .HasValue<Utilisateur>("Utilisateur")
                .HasValue<Secretaire>("Secretaire")
                .HasValue<TechnicienLabo>("Technicien")
                .HasValue<Comptable>("Comptable")
                .HasValue<Administrateur>("Administrateur");

            // Relation optionnelle Patient -> Assurance (clé étrangère nullable idAssurance)
            mb.Entity<Patient>()
                .HasOne(p => p.assurance)
                .WithMany()
                .HasForeignKey(p => p.idAssurance)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            mb.Entity<Analyse>()
                .HasOne(a => a.echantillon)
                .WithMany(e => e.analyses)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            mb.Entity<Analyse>()
                .HasOne(a => a.medecin)
                .WithMany(m => m.analysesPrescrites)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            mb.Entity<Analyse>()
                .HasOne(a => a.facture)
                .WithMany(f => f.analyses)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}