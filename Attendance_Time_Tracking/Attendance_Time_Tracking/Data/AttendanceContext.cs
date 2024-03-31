using Attendance_Time_Tracking.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Time_Tracking.Data
{
    public class AttendanceContext:DbContext
    {
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

        public AttendanceContext()
        {
        }
        public AttendanceContext(DbContextOptions<AttendanceContext> options) : base(options)
        {
        }
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
            base.OnModelCreating(modelBuilder);
        }
    }
}
