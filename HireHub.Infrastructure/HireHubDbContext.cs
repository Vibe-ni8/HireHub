using HireHub.Core.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HireHub.Infrastructure;

public class HireHubDbContext : DbContext
{
    public HireHubDbContext(DbContextOptions<HireHubDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Slot> Slots => Set<Slot>();
    public DbSet<UserSlot> UserSlots => Set<UserSlot>();
    public DbSet<Candidate> Candidates => Set<Candidate>();
    public DbSet<CandidateMap> CandidateMaps => Set<CandidateMap>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(b =>
        {
            b.ToTable("user");
            b.HasKey(e => e.Id);

            b.Property(e => e.Id).HasColumnName("user_id")
            .HasColumnType("int").IsRequired();

            b.Property(e => e.Name).HasColumnName("user_name")
            .HasColumnType("Varchar(100)").IsRequired();

            b.Property(e => e.EmailId).HasColumnName("email_id")
            .HasColumnType("Varchar(250)").IsRequired();
            b.HasIndex(e => e.EmailId).IsUnique();

            b.Property(e => e.Role).HasColumnName("role")
            .HasColumnType("Varchar(50)").IsRequired();

            b.Property(e => e.PhoneNumber).HasColumnName("phone_number")
            .HasColumnType("Varchar(32)").IsRequired();
            b.HasIndex(e => e.PhoneNumber).IsUnique();

            b.Property(e => e.IsActive).HasColumnName("is_active")
            .HasColumnType("BIT").IsRequired();

            b.Property(e => e.PasswordHash).HasColumnName("password_hash")
            .HasColumnType("Varchar(max)").IsRequired(false);
        });

        modelBuilder.Entity<Slot>(b =>
        {
            b.ToTable("slot");
            b.HasKey(e => e.Id);

            b.Property(e => e.Id)
                .HasColumnName("slot_id")
                .HasColumnType("int")
                .IsRequired();

            b.Property(e => e.SlotDate)
                .HasColumnName("slot_date")
                .HasColumnType("date")
                .IsRequired();

            b.Property(e => e.StartTime)
                .HasColumnName("start_time")
                .HasColumnType("varchar(20)")
                .IsRequired();

            b.Property(e => e.EndTime)
                .HasColumnName("end_time")
                .HasColumnType("varchar(20)")
                .IsRequired();
        });

        modelBuilder.Entity<UserSlot>(b =>
        {
            b.ToTable("user_slot");
            b.HasKey(e => e.Id);

            b.Property(e => e.Id)
                .HasColumnName("user_slot_id")
                .HasColumnType("int")
                .IsRequired();

            b.Property(e => e.UserId)
                .HasColumnName("user_id")
                .HasColumnType("int")
                .IsRequired();

            b.Property(e => e.SlotId)
                .HasColumnName("slot_id")
                .HasColumnType("int")
                .IsRequired();

            b.HasIndex(e => new { e.UserId, e.SlotId }).IsUnique();

            // Relationship: UserSlot → User (Many-to-One)
            b.HasOne(us => us.User)
                .WithMany(u => u.UserSlots)
                .HasPrincipalKey(u => u.Id)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: UserSlot → Slot (Many-to-One)
            b.HasOne(us => us.Slot)
                .WithMany(s => s.UserSlots)
                .HasPrincipalKey(s => s.Id)
                .HasForeignKey(us => us.SlotId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Candidate>(b =>
        {
            b.ToTable("candidate");
            b.HasKey(e => e.Id);

            b.Property(e => e.Id)
                .HasColumnName("candidate_id")
                .HasColumnType("int")
                .IsRequired();

            b.Property(e => e.Name)
                .HasColumnName("candidate_name")
                .HasColumnType("varchar(100)")
                .IsRequired();
        });

        modelBuilder.Entity<CandidateMap>(b =>
        {
            b.ToTable("candidate_map");

            b.HasKey(e => new { e.CandidateId, e.UserSlotId }); // Composite Key

            b.Property(e => e.CandidateId)
                .HasColumnName("candidate_id")
                .HasColumnType("int")
                .IsRequired();

            b.Property(e => e.UserSlotId)
                .HasColumnName("user_slot_id")
                .HasColumnType("int")
                .IsRequired();

            b.HasIndex(e => e.CandidateId).IsUnique();

            // Relationship: CandidateMap → Candidate
            b.HasOne(cm => cm.Candidate)
                .WithMany(c => c.CandidateMaps)
                .HasPrincipalKey (c => c.Id)
                .HasForeignKey(cm => cm.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: CandidateMap → UserSlot
            b.HasOne(cm => cm.UserSlot)
                .WithMany(us => us.CandidateMaps)
                .HasPrincipalKey(us => us.Id)
                .HasForeignKey(cm => cm.UserSlotId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
