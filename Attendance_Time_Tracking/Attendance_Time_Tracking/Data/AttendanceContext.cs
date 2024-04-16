using Attendance_Time_Tracking.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Time_Tracking.Data
{
    public class AttendanceContext:DbContext
    {
        public AttendanceContext()
        {
        }

        public AttendanceContext(DbContextOptions<AttendanceContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Programs> Programs { get; set; }
        public DbSet<Intake> Intakes { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(user =>
            {
                user.UseTptMappingStrategy();
            });
                        
           

            modelBuilder.Entity<Track>()
                .HasOne(t => t.Intake)
                .WithMany(i => i.Tracks)
                .HasForeignKey(t => t.IntakeId)
                .OnDelete(DeleteBehavior.NoAction);

            //add track to supervisor each track has one supervisor and each supervisor has one track
            modelBuilder.Entity<Track>()
				.HasOne(t => t.Supervisor)
				.WithMany(s => s.Tracks)
				.HasForeignKey(t => t.SupID)
				.OnDelete(DeleteBehavior.NoAction);
            


            //Intake to Program
            modelBuilder.Entity<Intake>()
                .HasOne(i => i.Program)
                .WithMany(s=>s.ProgramIntakes)
                .HasForeignKey(i => i.ProgramID)
                .OnDelete(DeleteBehavior.NoAction);



            modelBuilder.Entity<Permission>()
                .HasOne(p => p.Supervisor)
                .WithMany(i => i.Permissions)
                .HasForeignKey(p => p.SupId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Permission>()
                .HasOne(p => p.Student)
                .WithMany(s => s.Permissions)
                .HasForeignKey(s => s.StdId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Permission>()
                .HasKey(p => new { p.StdId, p.Date });

            modelBuilder.Entity<Schedule>()
                .HasOne(p => p.Supervisor)
                .WithMany(i => i.Schedules)
                .HasForeignKey(p => p.SupId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Track)
                .WithMany(t => t.Schedules)
                .HasForeignKey(s => s.TrackId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Schedule>()
                .Property(s => s.ID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.User)
                .WithMany(u => u.Attendances)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Attendance>()
                .HasKey(a => new { a.UserId, a.Date });

            base.OnModelCreating(modelBuilder);
        }
    }
}
