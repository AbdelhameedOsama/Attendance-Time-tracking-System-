﻿// <auto-generated />
using System;
using Attendance_Time_Tracking.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Attendance_Time_Tracking.Migrations
{
    [DbContext(typeof(AttendanceContext))]
    [Migration("20240402134102_addingAddress")]
    partial class addingAddress
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.HasSequence("UserSequence");

            modelBuilder.Entity("Attendance_Time_Tracking.Models.Attendance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("InstructorId")
                        .HasColumnType("int");

                    b.Property<bool>("IsPresent")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProgramId")
                        .HasColumnType("int");

                    b.Property<int>("ScheduleId")
                        .HasColumnType("int");

                    b.Property<int>("TrackId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("InstructorId");

                    b.HasIndex("ProgramId");

                    b.HasIndex("ScheduleId");

                    b.HasIndex("TrackId");

                    b.HasIndex("UserId");

                    b.ToTable("Attendance");
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.Intake", b =>
                {
                    b.Property<int>("IntakeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IntakeId"));

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProgramId")
                        .HasColumnType("int");

                    b.Property<int?>("ProgramsProgramId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("IntakeId");

                    b.HasIndex("ProgramId");

                    b.HasIndex("ProgramsProgramId");

                    b.ToTable("Intakes");
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int?>("StudentId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.HasIndex("UserId");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.Programs", b =>
                {
                    b.Property<int>("ProgramId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProgramId"));

                    b.Property<string>("ProgramType")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("ProgramId");

                    b.ToTable("Programs");
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.Schedue", b =>
                {
                    b.Property<int>("ScheduleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ScheduleId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("InstructorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("TrackId")
                        .HasColumnType("int");

                    b.HasKey("ScheduleId");

                    b.HasIndex("InstructorId");

                    b.HasIndex("TrackId");

                    b.ToTable("Schedue");
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.Track", b =>
                {
                    b.Property<int>("TrackId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TrackId"));

                    b.Property<int>("IntakeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProgramId")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int>("SupervisorId")
                        .HasColumnType("int");

                    b.HasKey("TrackId");

                    b.HasIndex("IntakeId");

                    b.HasIndex("ProgramId");

                    b.HasIndex("SupervisorId");

                    b.ToTable("Tracks");
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("NEXT VALUE FOR [UserSequence]");

                    SqlServerPropertyBuilderExtensions.UseSequence(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<int>("DeptId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Phone")
                        .HasColumnType("int");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Users");

                    b.UseTpcMappingStrategy();
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.Employee", b =>
                {
                    b.HasBaseType("Attendance_Time_Tracking.Models.User");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.Instructor", b =>
                {
                    b.HasBaseType("Attendance_Time_Tracking.Models.User");

                    b.Property<bool>("IsSupervisor")
                        .HasColumnType("bit");

                    b.Property<int?>("instructorId")
                        .HasColumnType("int");

                    b.HasIndex("instructorId");

                    b.ToTable("Instructors");
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.Student", b =>
                {
                    b.HasBaseType("Attendance_Time_Tracking.Models.User");

                    b.Property<string>("Faculty")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("Graduation_year")
                        .HasColumnType("date");

                    b.Property<string>("Specialization")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TrackId")
                        .HasColumnType("int");

                    b.Property<string>("University")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("studentId")
                        .HasColumnType("int");

                    b.HasIndex("studentId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.Attendance", b =>
                {
                    b.HasOne("Attendance_Time_Tracking.Models.Instructor", null)
                        .WithMany("Attendances")
                        .HasForeignKey("InstructorId");

                    b.HasOne("Attendance_Time_Tracking.Models.Programs", "Program")
                        .WithMany()
                        .HasForeignKey("ProgramId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Attendance_Time_Tracking.Models.Schedue", "Schedule")
                        .WithMany()
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Attendance_Time_Tracking.Models.Track", "Track")
                        .WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Attendance_Time_Tracking.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Program");

                    b.Navigation("Schedule");

                    b.Navigation("Track");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.Intake", b =>
                {
                    b.HasOne("Attendance_Time_Tracking.Models.Programs", "Program")
                        .WithMany()
                        .HasForeignKey("ProgramId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Attendance_Time_Tracking.Models.Programs", null)
                        .WithMany("ProgramIntakes")
                        .HasForeignKey("ProgramsProgramId");

                    b.Navigation("Program");
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.Permission", b =>
                {
                    b.HasOne("Attendance_Time_Tracking.Models.Student", null)
                        .WithMany("Permissions")
                        .HasForeignKey("StudentId");

                    b.HasOne("Attendance_Time_Tracking.Models.User", "UserNavigation")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserNavigation");
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.Schedue", b =>
                {
                    b.HasOne("Attendance_Time_Tracking.Models.Instructor", "Instructor")
                        .WithMany()
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Attendance_Time_Tracking.Models.Track", "Track")
                        .WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Instructor");

                    b.Navigation("Track");
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.Track", b =>
                {
                    b.HasOne("Attendance_Time_Tracking.Models.Intake", "Intake")
                        .WithMany("Tracks")
                        .HasForeignKey("IntakeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Attendance_Time_Tracking.Models.Programs", "Program")
                        .WithMany()
                        .HasForeignKey("ProgramId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Attendance_Time_Tracking.Models.Instructor", "Supervisor")
                        .WithMany()
                        .HasForeignKey("SupervisorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Intake");

                    b.Navigation("Program");

                    b.Navigation("Supervisor");
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.User", b =>
                {
                    b.HasOne("Attendance_Time_Tracking.Models.Department", null)
                        .WithMany("Users")
                        .HasForeignKey("DepartmentId");
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.Instructor", b =>
                {
                    b.HasOne("Attendance_Time_Tracking.Models.User", "instructor")
                        .WithMany()
                        .HasForeignKey("instructorId");

                    b.Navigation("instructor");
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.Student", b =>
                {
                    b.HasOne("Attendance_Time_Tracking.Models.User", "student")
                        .WithMany()
                        .HasForeignKey("studentId");

                    b.Navigation("student");
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.Department", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.Intake", b =>
                {
                    b.Navigation("Tracks");
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.Programs", b =>
                {
                    b.Navigation("ProgramIntakes");
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.Instructor", b =>
                {
                    b.Navigation("Attendances");
                });

            modelBuilder.Entity("Attendance_Time_Tracking.Models.Student", b =>
                {
                    b.Navigation("Permissions");
                });
#pragma warning restore 612, 618
        }
    }
}
