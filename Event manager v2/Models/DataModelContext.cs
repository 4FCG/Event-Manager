namespace Event_manager_v2.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataModelContext : DbContext
    {
        public DataModelContext()
            : base("name=DataModelContext")
        {
        }

        public virtual DbSet<Activiteit> Activiteits { get; set; }
        public virtual DbSet<Beheerder> Beheerders { get; set; }
        public virtual DbSet<Deelnemer> Deelnemers { get; set; }
        public virtual DbSet<Evenement> Evenements { get; set; }
        public virtual DbSet<EvenementBeheerder> EvenementBeheerders { get; set; }
        public virtual DbSet<Wijziging> Wijzigings { get; set; }
        public virtual DbSet<WijzigingsType> WijzigingsTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activiteit>()
                .Property(e => e.naam)
                .IsUnicode(false);

            modelBuilder.Entity<Activiteit>()
                .Property(e => e.beschrijving)
                .IsUnicode(false);

            modelBuilder.Entity<Beheerder>()
                .Property(e => e.voornaam)
                .IsUnicode(false);

            modelBuilder.Entity<Beheerder>()
                .Property(e => e.achternaam)
                .IsUnicode(false);

            modelBuilder.Entity<Beheerder>()
                .Property(e => e.gebruikersnaam)
                .IsUnicode(false);

            modelBuilder.Entity<Beheerder>()
                .Property(e => e.wachtwoord)
                .IsUnicode(false);

            modelBuilder.Entity<Beheerder>()
                .HasMany(e => e.EvenementBeheerders)
                .WithRequired(e => e.Beheerder1)
                .HasForeignKey(e => e.beheerder)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Deelnemer>()
                .Property(e => e.voornaam)
                .IsUnicode(false);

            modelBuilder.Entity<Deelnemer>()
                .Property(e => e.achternaam)
                .IsUnicode(false);

            modelBuilder.Entity<Deelnemer>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Evenement>()
                .Property(e => e.naam)
                .IsUnicode(false);

            modelBuilder.Entity<Evenement>()
                .Property(e => e.beschrijving)
                .IsUnicode(false);

            modelBuilder.Entity<Evenement>()
                .HasMany(e => e.Activiteits)
                .WithRequired(e => e.Evenement1)
                .HasForeignKey(e => e.evenement)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Evenement>()
                .HasMany(e => e.Deelnemers)
                .WithRequired(e => e.Evenement1)
                .HasForeignKey(e => e.evenement)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Evenement>()
                .HasMany(e => e.EvenementBeheerders)
                .WithRequired(e => e.Evenement1)
                .HasForeignKey(e => e.evenement)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EvenementBeheerder>()
                .HasMany(e => e.Activiteits)
                .WithRequired(e => e.EvenementBeheerder)
                .HasForeignKey(e => e.evenement_beheerder)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EvenementBeheerder>()
                .HasMany(e => e.Wijzigings)
                .WithRequired(e => e.EvenementBeheerder)
                .HasForeignKey(e => e.beheerder)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Wijziging>()
                .Property(e => e.naam)
                .IsUnicode(false);

            modelBuilder.Entity<Wijziging>()
                .Property(e => e.beschrijving)
                .IsUnicode(false);

            modelBuilder.Entity<Wijziging>()
                .Property(e => e.jsonData)
                .IsUnicode(false);

            modelBuilder.Entity<Wijziging>()
                .Property(e => e.jsonClassType)
                .IsUnicode(false);

            modelBuilder.Entity<WijzigingsType>()
                .Property(e => e.naam)
                .IsUnicode(false);

            modelBuilder.Entity<WijzigingsType>()
                .HasMany(e => e.Wijzigings)
                .WithRequired(e => e.WijzigingsType)
                .HasForeignKey(e => e.type)
                .WillCascadeOnDelete(false);
        }
    }
}
