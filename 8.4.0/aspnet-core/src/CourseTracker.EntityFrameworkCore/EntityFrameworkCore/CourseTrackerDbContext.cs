using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using CourseTracker.Authorization.Roles;
using CourseTracker.Authorization.Users;
using CourseTracker.MultiTenancy;
using CourseTracker.Entities;

namespace CourseTracker.EntityFrameworkCore
{
    public class CourseTrackerDbContext : AbpZeroDbContext<Tenant, Role, User, CourseTrackerDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public CourseTrackerDbContext(DbContextOptions<CourseTrackerDbContext> options)
            : base(options)
        {
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Learner> Learners { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);

            builder.Entity<Enrollment>()
                .HasOne(e => e.Learner)
                .WithMany(l => l.Enrollments)
                .HasForeignKey(e => e.LearnerId);
        }

    }

    
}
