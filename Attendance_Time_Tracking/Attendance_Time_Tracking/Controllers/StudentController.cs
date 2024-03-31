using Attendance_Time_Tracking.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance_Time_Tracking.Controllers
{
	//[Authorize(Roles = "Student")]
	public class StudentController : Controller
	{
		readonly IStudentRepo SRepo;
		readonly IUserRepo URepo;
		public StudentController(IStudentRepo _studentRepo, IUserRepo _userRepo)
		{
			SRepo = _studentRepo;
			URepo = _userRepo;
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}
