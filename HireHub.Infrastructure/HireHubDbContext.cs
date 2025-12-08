using HireHub.Core.Data.Models;
using HireHub.Core.Utils.Common;
using Microsoft.EntityFrameworkCore;

namespace HireHub.Infrastructure;

public class HireHubDbContext : DbContext
{
    public HireHubDbContext(DbContextOptions<HireHubDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Slot> Slots => Set<Slot>();
    public DbSet<UserSlot> UserSlots => Set<UserSlot>();
    public DbSet<Candidate> Candidates => Set<Candidate>();
    public DbSet<Feedback> Feedbacks => Set<Feedback>();
    public DbSet<CandidateMap> CandidateMaps => Set<CandidateMap>();
    public DbSet<Reassign> Reassigns => Set<Reassign>();

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
                .HasConversion(Helper.TimeConverter)
                .IsRequired();

            b.Property(e => e.EndTime)
                .HasColumnName("end_time")
                .HasColumnType("varchar(20)")
                .HasConversion(Helper.TimeConverter)
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

            b.Property(e => e.IsLocked)
                .HasColumnName("is_locked")
                .HasColumnType("BIT")
                .IsRequired();

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

            b.Property(e => e.Email)
                .HasColumnName("email_id")
                .HasColumnType("varchar(250)")
                .IsRequired();

            b.Property(e => e.PhoneNumber)
                .HasColumnName("phone_number")
                .HasColumnType("varchar(32)")
                .IsRequired();

            b.Property(e => e.Experience)
                .HasColumnName("candidate_exp")
                .HasColumnType("int")
                .IsRequired();

            b.Property(e => e.CurrentPosition)
                .HasColumnName("current_position")
                .HasColumnType("varchar(150)")
                .IsRequired();

            b.Property(e => e.ResumeUrl)
                .HasColumnName("resume_url")
                .HasColumnType("varchar(100)")
                .IsRequired(false);
        });

        modelBuilder.Entity<Feedback>(b =>
        {
            b.ToTable("feedback");
            b.HasKey(e => e.Id);

            b.Property(e => e.Id)
                .HasColumnName("feedback_id")
                .HasColumnType("int")
                .IsRequired();

            b.Property(e => e.StarRating)
                .HasColumnName("star_rating")
                .HasColumnType("int")
                .IsRequired();

            b.Property(e => e.TechnicalSkills)
                .HasColumnName("technical_skills")
                .HasColumnType("varchar(max)")
                .IsRequired();

            b.Property(e => e.CommunicationSkill)
                .HasColumnName("communication_skills")
                .HasColumnType("varchar(max)")
                .IsRequired();

            b.Property(e => e.ProblemSolvingAbility)
                .HasColumnName("problem_solving_ability")
                .HasColumnType("varchar(max)")
                .IsRequired();

            b.Property(e => e.OverallFeedback)
                .HasColumnName("overall_feedback")
                .HasColumnType("varchar(max)")
                .IsRequired();

            b.Property(e => e.Recommendation)
                .HasColumnName("recommendation")
                .HasColumnType("varchar(20)")
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

            b.Property(e => e.ScheduledTime)
                .HasColumnName("scheduled_time")
                .HasColumnType("varchar(20)")
                .HasConversion(Helper.TimeConverter)
                .IsRequired();

            b.Property(e => e.IsPresent)
                .HasColumnName("is_present")
                .HasColumnType("BIT")
                .IsRequired(false);

            b.Property(e => e.InterviewRounds)
                .HasColumnName("interview_rounds")
                .HasColumnType("int")
                .IsRequired();

            b.Property(e => e.FeedbackId)
                .HasColumnName("feedback_id")
                .HasColumnType("int")
                .IsRequired(false);
            b.HasIndex(e => e.FeedbackId).IsUnique();

            // Relationship: CandidateMap → Candidate
            b.HasOne(cm => cm.Candidate)
                .WithMany(c => c.CandidateMaps)
                .HasPrincipalKey(c => c.Id)
                .HasForeignKey(cm => cm.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: CandidateMap → UserSlot
            b.HasOne(cm => cm.UserSlot)
                .WithMany(us => us.CandidateMaps)
                .HasPrincipalKey(us => us.Id)
                .HasForeignKey(cm => cm.UserSlotId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: CandidateMap → Feedback
            b.HasOne(cm => cm.Feedback)
                .WithOne(f => f.CandidateMap)
                .HasPrincipalKey<Feedback>(f => f.Id)
                .HasForeignKey<CandidateMap>(cm => cm.FeedbackId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Reassign>(b =>
        {
            b.ToTable("reassign");
            b.HasKey(e => e.Id);

            b.Property(e => e.Id)
                .HasColumnName("reassign_id")
                .HasColumnType("int")
                .IsRequired();

            b.Property(e => e.CandidateId)
                .HasColumnName("candidate_id")
                .HasColumnType("int")
                .IsRequired();

            b.Property(e => e.OldUserSlotId)
                .HasColumnName("old_userslot_id")
                .HasColumnType("int")
                .IsRequired();

            b.Property(e => e.NewUserSlotId)
                .HasColumnName("new_userslot_id")
                .HasColumnType("int")
                .IsRequired();

            b.HasIndex(e => new { e.CandidateId, e.OldUserSlotId, e.NewUserSlotId }).IsUnique();

            b.Property(e => e.Reason)
                .HasColumnName("reason")
                .HasColumnType("Varchar(50)")
                .IsRequired();

            b.Property(e => e.AdditionalNotes)
                .HasColumnName("additional_notes")
                .HasColumnType("Varchar(max)")
                .IsRequired(false);

            // Relationship: Reassign → Candidate
            b.HasOne(r => r.Candidate)
                .WithMany() // no back-reference collection needed
                .HasForeignKey(r => r.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Reassign → OldUserSlot
            b.HasOne(r => r.OldUserSlot)
                .WithMany() // no back-reference collection needed
                .HasForeignKey(r => r.OldUserSlotId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Reassign → NewUserSlot
            b.HasOne(r => r.NewUserSlot)
                .WithMany() // no back-reference collection needed
                .HasForeignKey(r => r.NewUserSlotId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
