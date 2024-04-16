using Attendance_Time_Tracking.Models;
using Attendance_Time_Tracking.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Attendance_Time_Tracking.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
    {
        readonly IAdminRepo adminRepo;
        readonly IUserRepo userRepo;
        readonly IStudentRepo stdRepo;
        readonly IEmployeeRepo empRepo;
        readonly ITrackRepo trackRepo;
        readonly IIntakeRepo intakeRepo;
        readonly IInstructorRepo instructorRepo;

        public AdminController(IStudentRepo _studentRepo, IUserRepo _userRepo, IAdminRepo _adminRepo, IEmployeeRepo _empRepo, ITrackRepo _trackRepo, IIntakeRepo _intakeRepo, IInstructorRepo _instructorRepo)
        {
            stdRepo = _studentRepo;
            userRepo = _userRepo;
            adminRepo = _adminRepo;
            empRepo = _empRepo;
            trackRepo = _trackRepo;
            intakeRepo=_intakeRepo;
            instructorRepo=_instructorRepo;
        }
        public IActionResult Index()
        {
            ViewBag.NoStudents=stdRepo.GetAll().Count();
            ViewBag.NoInstructors=userRepo.GetAll().Where(a => a.Role==UserRole.Instructor || a.Role==UserRole.Supervisor).Count();
            ViewBag.NoEmployees=empRepo.GetAllEmployees().Count();
            ViewBag.NoTracks=trackRepo.GetAllTracks().Count();
            return View();
        }

        #region Student
       

        //================Student==============
        public IActionResult AdminStudents()
        {
            var model = stdRepo.GetAll();
            ViewBag.Tracks=adminRepo.GetAllTracks().Where(a=>a.Status==true).ToList();    
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
        #endregion

        #region Instructor
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
        #endregion

        #region Employee

        //================Employee============
        public IActionResult AdminEmployees()
        {
            var model = empRepo.GetAllEmployees();
            return View(model);
        }
        public IActionResult AdminDeleteEmployee(int id)
        {
            empRepo.DeleteEmployee(id);
            return RedirectToAction("AdminEmployees");
        }
        public IActionResult AdminAddEmployee(Employee emp)
        {
            if (ModelState.IsValid)
            {
                empRepo.AddEmpployee(emp);
                return RedirectToAction("AdminEmployees");
            }
            return RedirectToAction("AdminEmployees");
        }
        public IActionResult AdminEditEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                empRepo.UpdateEmp(employee);
                return RedirectToAction("AdminEmployees");
            }
            return RedirectToAction("AdminEmployees");
        }
        #endregion

        #region Tracks
        //================Tracks================
        public IActionResult AdminTracks()
        {
            var model = trackRepo.GetAllTracks();
            /*var user=userRepo.GetAll().Where(a=>a.Role==UserRole.Instructor).Take(10).ToList();*/
            var user = userRepo.GetAll().Where(a => a.Role==UserRole.Instructor).ToList();

            var allInstructor = userRepo.GetAll().Where(a=>a.Role==UserRole.Instructor || a.Role==UserRole.Supervisor).Distinct().ToList();
            ViewBag.AllInstructors=allInstructor;
          /*  List<User> Instructors = new List<User>();
            foreach (var usr in user)
            {
                if (usr.Role==UserRole.Instructor)
                {
                    Instructors.Add(usr);
                }
            }*/
            /*ViewBag.Instructors=Instructors.ToList();*/
            ViewBag.Intakes=intakeRepo.GetIntakeList();
            ViewBag.Instructors=user;
            return View(model);
        }
        public IActionResult AdminDeleteTrack(int id)
        {
            trackRepo.DeleteTrack(id);
            return RedirectToAction("AdminTracks");
        }
        public IActionResult AdminAddTrack(Track track1)
        {
            
            if (ModelState.IsValid)
            {
                instructorRepo.ChangeInstructorToSupervisor(track1.SupID);
                trackRepo.AddTrack(track1);

                return RedirectToAction("AdminTracks");
            }
            return RedirectToAction("AdminTracks");
        }
        public IActionResult AdminEditTrack(Track track)
        {
           
            if (ModelState.IsValid)
            {
                instructorRepo.ChangeInstructorToSupervisor(track.SupID);
                trackRepo.UpdateTrack(track);
                return RedirectToAction("AdminTracks");
            }
            return RedirectToAction("AdminTracks");
        }
        #endregion

    }
}
