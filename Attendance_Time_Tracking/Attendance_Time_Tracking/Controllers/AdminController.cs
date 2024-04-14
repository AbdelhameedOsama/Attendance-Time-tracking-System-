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
        public AdminController(IStudentRepo _studentRepo, IUserRepo _userRepo, IAdminRepo _adminRepo)
        {
            stdRepo = _studentRepo;
            userRepo = _userRepo;
            adminRepo=_adminRepo;
        }
        public IActionResult Index()
        {
            return View();
        }
        //Student
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

        //Instructor
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

    }
}
