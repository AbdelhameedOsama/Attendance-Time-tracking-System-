using Attendance_Time_Tracking.Models;
using Attendance_Time_Tracking.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Attendance_Time_Tracking.Controllers
{
	[Authorize(Roles = "Instructor,Supervisor" )]

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
			User Instructor = IRepo.GetUserByID(URepo.GetUserId(User)).Result;

			return View(Instructor);
		}
		//permissions
		public IActionResult Permissions()
		{
			var permissions = IRepo.GetPermissionsBySupevisorID(URepo.GetUserId(User)).Result;
            return View(permissions);
		}
		public async Task<IActionResult> Delete(int id, DateTime date)
		{
			var permission = await IRepo.GetPermission(id, date);
			if (permission != null)
			{
				await IRepo.DeletePermission(permission);
			}
            return RedirectToAction("Permissions");
		}
		public async Task<IActionResult> Approve(int id, DateTime date)
		{
			var permission = await IRepo.GetPermission(id, date);
			if (permission != null)
			{
				await IRepo.ApprovePermission(permission);
			}
			return RedirectToAction("Permissions");
		}
		public async Task<IActionResult> Reject(int id, DateTime date)
		{
			var permission = await IRepo.GetPermission(id, date);
			if (permission != null)
			{
				await IRepo.RejectPermission(permission);
			}
			return RedirectToAction("Permissions");
		}

		//Create schedule
		public async Task<IActionResult> CreateSchedule()
		{
			int supId = URepo.GetUserId(User);
			ViewBag.SupId = supId;
			var track = await IRepo.GetTrackBySupervisorId(supId);
			var trackID = track.ID;
			ViewBag.TrackID = trackID;
            return View();
		}
		[HttpPost]
		public async Task<IActionResult> CreateSchedule(Schedule schedule)
		{

	
			if (ModelState.IsValid)
			{
				await IRepo.CreateSchedule(schedule);
				return RedirectToAction("Index");
			}

			return View();
		}
        //SChedule
/*        public async Task<IActionResult> Schedules()
        {
            int supId = URepo.GetUserId(User);
            var track = await IRepo.GetTrackBySupervisorId(supId);
            var schedules = await IRepo.GetSchedulesBySupId(supId);
			// Filter schedules with date after today and order by date and show only the first 7
			schedules = schedules.Where(s => s.Date >= DateOnly.FromDateTime(DateTime.Now)).OrderBy(s => s.Date).Take(7).ToList();
			ViewBag.SupId = supId;
			ViewBag.TrackID = track.ID;


			ViewBag.TrackName = track.Name;
            return View(schedules);
        }*/
		public async Task<IActionResult> AllSchedules()
		{
			var tracks = await IRepo.TracksInScheduels();
			ViewData["Role"] = User.FindFirst(ClaimTypes.Role).Value;
			ViewBag.Tracks = tracks;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> AllSchedules(int Track, DateOnly FromDate)
		{
            var tracks = await IRepo.TracksInScheduels();
            ViewData["Role"] = User.FindFirst(ClaimTypes.Role).Value;
            ViewBag.Tracks = tracks;
            var schedules = await IRepo.GetSchedules();
			if (Track != 0)
			{
				schedules = schedules.Where(s => s.Track.ID == Track && s.Date >= FromDate).OrderBy(s=>s.Date).Take(7).ToList();
				ViewBag.Schedules = schedules;
                return View();
			}
			return View(schedules);

        }
		//edit profile data
		public IActionResult EditProfile()
		{
			User user = IRepo.GetUserByID(URepo.GetUserId(User)).Result;
			return View(user);
		}
		[HttpPost]
		public IActionResult EditProfile(User user)
		{
			if (ModelState.IsValid)
			{
				 var returns =URepo.UpdateUser(user);
				if (returns.Result == "Email already exists")
				{
					ModelState.AddModelError("Email", "Email already exists");
					return View(user);
				}

				return RedirectToAction("Index");
			}
			return View(user);
		}
		//View Attendance
		public ActionResult Attendance()
		{
            ViewData["Role"] = User.FindFirst(ClaimTypes.Role).Value;
            return View();
        }
		[HttpPost]
		public async Task<IActionResult> Attendance(DateTime FromDate)
		{
			ViewData["Role"] = User.FindFirst(ClaimTypes.Role).Value;
			var attendances = await IRepo.GetAttendancesByInstructorID(URepo.GetUserId(User));
			attendances = attendances.Where(a => a.Date >= DateOnly.FromDateTime(FromDate)).OrderBy(d=>d.Date).Take(7).ToList();
			return View(attendances);
		}


	}
}
