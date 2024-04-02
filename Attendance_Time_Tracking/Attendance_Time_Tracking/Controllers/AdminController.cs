using Microsoft.AspNetCore.Mvc;

namespace Attendance_Time_Tracking.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
