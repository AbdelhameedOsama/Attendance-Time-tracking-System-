using Attendance_Time_Tracking.Models;
using Attendance_Time_Tracking.Repos;
using Microsoft.AspNetCore.Mvc;

namespace Attendance_Time_Tracking.Controllers
{
    public class AdminController : Controller
    {
        readonly IAdminRepo adminRepo;
        readonly IUserRepo userRepo;
        readonly IStudentRepo stdRepo;
        readonly IEmployeeRepo empRepo;
        readonly ITrackRepo trackRepo;
        public AdminController(IStudentRepo _studentRepo, IUserRepo _userRepo, IAdminRepo _adminRepo, IEmployeeRepo _empRepo, ITrackRepo _trackRepo)
        {
            stdRepo = _studentRepo;
            userRepo = _userRepo;
            adminRepo=_adminRepo;
            empRepo=_empRepo;
            trackRepo=_trackRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        //================Student==============
        public IActionResult AdminStudents()
        {
            var model = stdRepo.GetAll();
            ViewBag.Tracks=adminRepo.GetAllTracks();    
            return View(model);
        }
        public IActionResult AdminDeleteStudent(int id)
        {
            //var std = stdRepo.GetStudentByID(id);
            stdRepo.DeleteStudent(id);
            return RedirectToAction("AdminStudents");
        }
        public IActionResult AdminAddStudent(Student std)
        {
            if (ModelState.IsValid)
            {
                stdRepo.Add(std);
                return RedirectToAction("AdminStudents");
            }
            return RedirectToAction("AdminStudents");
        }
        public IActionResult AdminEditStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                stdRepo.UpdateStd(student);
                return RedirectToAction("AdminStudents");
            }
            return RedirectToAction("AdminStudents");
        }

        //================Instructor==============
        public IActionResult AdminInstructors()
        {
            var model = userRepo.GetAll();
            ViewBag.Tracks=adminRepo.GetAllTracks();
            return View(model);
        }

        public IActionResult AdminDeleteInstructor(int id)
        {
            
            userRepo.DeleteUser(id);
            return RedirectToAction("AdminInstructors");
        }
        public IActionResult AdminAddInstructor(User inst)
        {
            if (ModelState.IsValid)
            {
                userRepo.AddUser(inst);
                return RedirectToAction("AdminInstructors");
            }
            return RedirectToAction("AdminInstructors");
        }
        public IActionResult AdminEditInstructor(User instructor)
        {
            if (ModelState.IsValid)
            {
                userRepo.UpdateInst(instructor);
                return RedirectToAction("AdminInstructors");
            }
            return RedirectToAction("AdminInstructors");
        }

        //================Employee==========
        public IActionResult AdminEmployees()
        {
            var model = empRepo.GetAllEmployees();
            return View(model);
        }

        //================Tracks==========
        public IActionResult AdminTracks()
        {
            var model = trackRepo.GetAllTracks();
            return View(model);
        }


    }
}
