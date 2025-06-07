using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PTFGym.Models;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Trener> Trener { get; set; }
    public DbSet<Termin> Termin { get; set; }
    public DbSet<Clan> Clan { get; set; }
    public DbSet<Clanarina> Clanarina { get; set; }
    public DbSet<Rezervacija> Rezervacija { get; set; }
    public DbSet<Napredak> Napredak { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rezervacija>().ToTable("Rezervacija");
        modelBuilder.Entity<Clanarina>().ToTable("Clanarina");
        modelBuilder.Entity<Napredak>().ToTable("Napredak");
        modelBuilder.Entity<Trener>().ToTable("Trener");
        modelBuilder.Entity<Termin>().ToTable("Termin")
        .Property(t => t.Id)
        .ValueGeneratedOnAdd();
        modelBuilder.Entity<Clan>().ToTable("Clan");

        modelBuilder.Entity<ApplicationUser>()
            .HasOne(u => u.Clan)
            .WithMany()
            .HasForeignKey(u => u.ClanId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ApplicationUser>()
            .HasOne(u => u.Trener)
            .WithMany()
            .HasForeignKey(u => u.TrenerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Termin>()
        .HasMany(t => t.Clanovi)
        .WithMany(c => c.Termini)
        .UsingEntity(j => j.ToTable("TerminClan"));

        base.OnModelCreating(modelBuilder);
    }
}
