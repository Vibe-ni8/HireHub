using HireHub.Core.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HireHub.Infrastructure;

public class HireHubDbContext : DbContext
{
    public HireHubDbContext(DbContextOptions<HireHubDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(b =>
        {
            b.ToTable("user");
            b.HasKey(e => e.UserId);

            b.Property(e => e.UserId).HasColumnName("user_id")
            .HasColumnType("int").IsRequired();

            b.Property(e => e.UserName).HasColumnName("user_name")
            .HasColumnType("Varchar(max)").IsRequired();

            b.Property(e => e.EmailId).HasColumnName("email_id")
            .HasColumnType("Varchar(max)").IsRequired();

            b.Property(e => e.Role).HasColumnName("role")
            .HasColumnType("Varchar(max)").IsRequired();

            b.Property(e => e.PhoneNumber).HasColumnName("phone_number")
            .HasColumnType("Varchar(max)").IsRequired();

            b.Property(e => e.IsActive).HasColumnName("is_active")
            .HasColumnType("Varchar(max)").IsRequired();

            b.Property(e => e.PasswordHash).HasColumnName("password_hash")
            .HasColumnType("Varchar(max)").IsRequired(false);
        });
    }
}
