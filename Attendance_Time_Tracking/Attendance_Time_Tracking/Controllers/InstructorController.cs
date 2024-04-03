using Attendance_Time_Tracking.Models;
using Attendance_Time_Tracking.Repos;
using Microsoft.AspNetCore.Mvc;

namespace Attendance_Time_Tracking.Controllers
{
    public class InstructorController : Controller
    {
        readonly IInstructorRepo IRepo;
        readonly IUserRepo URepo;
        public InstructorController(IInstructorRepo _instructorRepo, IUserRepo _userRepo)
        {
            IRepo = _instructorRepo;
            URepo = _userRepo;
        }
        public  IActionResult Index()
        {
            User Instructor =  IRepo.GetUserByID(URepo.GetUserId(User)).Result;

			return View(Instructor);
        }
        public IActionResult Permissions()
        {
            var permissions = IRepo.GetPermissionsBySupevisorID(URepo.GetUserId(User)).Result;
            return View(permissions);
        }
    }
}
