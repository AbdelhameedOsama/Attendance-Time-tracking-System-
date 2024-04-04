using Attendance_Time_Tracking.Repos;
using Microsoft.AspNetCore.Mvc;

namespace Attendance_Time_Tracking.Controllers
{
    public class EmployeeController : Controller
    {
        readonly IEmployeeRepo empRepo;
        
        public EmployeeController(IEmployeeRepo _empRepo)
        {
            empRepo = _empRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ViewAttendance()
        {
            var attendance = await empRepo.ViewAllAttendance();
            return PartialView(attendance);
            //return PartialView();
        }

        public IActionResult TakeAttendance()
        {
            return PartialView();
        }
    }
}
