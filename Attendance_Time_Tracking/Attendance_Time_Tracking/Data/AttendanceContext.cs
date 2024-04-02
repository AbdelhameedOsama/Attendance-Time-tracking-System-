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
        public DbSet<Department> Departments { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Programs> Programs { get; set; }
        public DbSet<Intake> Intakes { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Schedue> Leaves { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(user=> 
            { 
                user.UseTpcMappingStrategy(); 
            });

            //Attendance to User
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            //Attendance to Schedule
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Schedule)
                .WithMany()
                .HasForeignKey(a => a.ScheduleId)
                .OnDelete(DeleteBehavior.NoAction);

            //Attendance to Program
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Program)
                .WithMany()
                .HasForeignKey(a => a.ProgramId)
                .OnDelete(DeleteBehavior.NoAction);

            //Attendance to Track
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Track)
                .WithMany()
                .HasForeignKey(a => a.TrackId)
                .OnDelete(DeleteBehavior.NoAction);

            //Track to Program
            modelBuilder.Entity<Track>()
                .HasOne(t => t.Program)
                .WithMany()
                .HasForeignKey(t => t.ProgramId)
                .OnDelete(DeleteBehavior.NoAction);

            //Track to Instructor
            modelBuilder.Entity<Track>()
                .HasOne(t => t.Supervisor)
                .WithMany()
                .HasForeignKey(t => t.SupervisorId)
                .OnDelete(DeleteBehavior.NoAction);

            //Track to Intake
            modelBuilder.Entity<Track>()
                .HasOne(t => t.Intake)
                .WithMany(i => i.Tracks)
                .HasForeignKey(t => t.IntakeId)
                .OnDelete(DeleteBehavior.NoAction);


            //Intake to Program
            modelBuilder.Entity<Intake>()
                .HasOne(i => i.Program)
                .WithMany()
                .HasForeignKey(i => i.ProgramId)
                .OnDelete(DeleteBehavior.NoAction);

            ////Schedule to Track
            //modelBuilder.Entity<Schedue>()
            //    .HasOne(s => s.Track)
            //    .WithMany()
            //    .HasForeignKey(s => s.TrackId)
            //    .OnDelete(DeleteBehavior.NoAction);

            ////Schedule to Instructor
            //modelBuilder.Entity<Schedue>()
            //    .HasOne(s => s.Instructor)
            //    .WithMany()
            //    .HasForeignKey(s => s.InstructorId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //Attendance foreign keys
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Program)
                .WithMany()
                .HasForeignKey(a => a.ProgramId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Schedule)
                .WithMany()
                .HasForeignKey(a => a.ScheduleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Track)
                .WithMany()
                .HasForeignKey(a => a.TrackId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Permission>()
                .HasOne(p => p.SupNavigation)
                .WithOne()
                .HasForeignKey<Permission>(p => p.SupId)
                .OnDelete(DeleteBehavior.SetNull);
                

            modelBuilder.Entity<Permission>()
                .HasOne(s => s.StdNavigation)
                .WithOne()
                .HasForeignKey<Permission>(s => s.StdId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Permission>()
                .HasKey(p => new { p.StdId, p.Date });

            base.OnModelCreating(modelBuilder);
        }
    }
}
