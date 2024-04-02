﻿using Attendance_Time_Tracking.Models;
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

            //Attendance to User
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
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

            //Schedule to Track
            modelBuilder.Entity<Schedue>()
                .HasOne(s => s.Track)
                .WithMany()
                .HasForeignKey(s => s.TrackId)
                .OnDelete(DeleteBehavior.NoAction);

            //Schedule to Instructor
            modelBuilder.Entity<Schedue>()
                .HasOne(s => s.Instructor)
                .WithMany()
                .HasForeignKey(s => s.InstructorId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);


            base.OnModelCreating(modelBuilder);
        }
    }
}
