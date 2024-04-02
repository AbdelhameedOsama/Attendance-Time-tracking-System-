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
        public IActionResult Index()
        {

            return View();
        }
    }
}
