using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Models
{
    public class ApplicationDbContext : IdentityDbContext<Utilizator, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Categorie> Categorii { get; set; }
        public DbSet<Autor> Autori { get; set; }
        public DbSet<Carte> Carti { get; set; }
        public DbSet<Imprumut> Imprumuturi { get; set; }
        public DbSet<Recenzie> Recenzii { get; set; }
        public DbSet<Eveniment> Evenimente { get; set; }
        public DbSet<Rezervare_Eveniment> Rezervari_Evenimente { get; set; }
        public DbSet<Lista_Dorinte> Lista_Dorinte { get; set; }
        public DbSet<Contact> Contacte { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Lista_Dorinte>()
                .HasKey(ld => new { ld.Id_Utilizator, ld.Id_Carte });

            modelBuilder.Entity<Carte>()
                .HasIndex(c => c.ISBN)
                .IsUnique()
                .HasFilter("[ISBN] IS NOT NULL");

            modelBuilder.Entity<Carte>()
                .Property(c => c.Status_Disponibilitate)
                .HasConversion<string>();

            modelBuilder.Entity<Recenzie>()
                .HasIndex(r => new { r.Id_Utilizator, r.Id_Carte })
                .IsUnique();

            modelBuilder.Entity<Imprumut>()
                .HasOne(i => i.Utilizator)
                .WithMany(u => u.Imprumuturi)
                .HasForeignKey(i => i.Id_Utilizator)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Imprumut>()
                .HasOne(i => i.Carte)
                .WithMany(c => c.Imprumuturi)
                .HasForeignKey(i => i.Id_Carte)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Recenzie>()
                .HasOne(r => r.Utilizator)
                .WithMany(u => u.Recenzii)
                .HasForeignKey(r => r.Id_Utilizator)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Recenzie>()
                .HasOne(r => r.Carte)
                .WithMany(c => c.Recenzii)
                .HasForeignKey(r => r.Id_Carte)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rezervare_Eveniment>()
                .HasOne(re => re.Utilizator)
                .WithMany(u => u.RezervariEvenimente)
                .HasForeignKey(re => re.Id_Utilizator)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Contact>()
                .HasOne(c => c.Utilizator)
                .WithMany(u => u.MesajeContact)
                .HasForeignKey(c => c.Id_Utilizator)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            modelBuilder.Entity<Imprumut>()
                .Property(i => i.Taxa_Penalizare)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Contact>()
                .Property(c => c.Status_Mesaj)
                .HasConversion<string>();
        }
    }
}
