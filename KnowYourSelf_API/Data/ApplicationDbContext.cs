using KnowYourSelf_API.Models;
using Microsoft.EntityFrameworkCore;

namespace KnowYourSelf_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Therapist> Therapists { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Blogpost> Blogposts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure one-to-one relationship between User and Therapist
            modelBuilder.Entity<User>()
                .HasOne(u => u.Therapist)
                .WithOne(t => t.User) 
                .HasForeignKey<Therapist>(t => t.UserId); // Foreign key

            // Configure one-to-one relationship between User and Patient
            modelBuilder.Entity<User>()
                .HasOne(u => u.Patient) 
                .WithOne(p => p.User) 
                .HasForeignKey<Patient>(p => p.UserId); // Foreign key
            modelBuilder.Entity<User>().HasData(
            );
            base.OnModelCreating(modelBuilder);

            
        }
    }
}
