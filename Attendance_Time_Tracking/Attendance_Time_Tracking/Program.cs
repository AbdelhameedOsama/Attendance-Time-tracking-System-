using Attendance_Time_Tracking.Data;
using Attendance_Time_Tracking.Repos;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Time_Tracking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AttendanceContext>(options =>
                           options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IAdminRepo, AdminRepo>();
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<IStudentRepo, StudentRepo>();
            builder.Services.AddScoped<IInstructorRepo, InstructorRepo>();
            builder.Services.AddScoped<IEmployeeRepo, EmployeeRepo>();
            builder.Services.AddScoped<ITrackRepo,TrackRepo>(); 
            builder.Services.AddScoped<ISARepo, SARepo>();
            builder.Services.AddScoped<IIntakeRepo, IntakeRepo>();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.LoginPath = "/Account/Login";
					//options.AccessDeniedPath = "/Account/AccessDenied";
				});
			builder.Services.AddSession();




            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();   

            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",

                pattern: "{controller=Account}/{action=Login}/{id?}");
            app.Run();
        }
    }
}
