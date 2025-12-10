using Microsoft.EntityFrameworkCore;
using GymManagementSystem.Models;
using GymManagementSystem.Models;
using GymManagementSystem.Models;

namespace GymManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Member>()
                .HasOne(m => m.Membership)
                .WithMany(ms => ms.Members)
                .HasForeignKey(m => m.MembershipID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Class>()
                .HasOne(c => c.Trainer)
                .WithMany(t => t.Classes)
                .HasForeignKey(c => c.TrainerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Member)
                .WithMany(m => m.Enrollments)
                .HasForeignKey(e => e.MemberID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Class)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.ClassID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
