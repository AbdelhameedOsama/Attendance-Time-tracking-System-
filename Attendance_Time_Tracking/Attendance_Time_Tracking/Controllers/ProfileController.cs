using Attendance_Time_Tracking.Data;
using Attendance_Time_Tracking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize(Roles = "Student")]
public class ProfileController : Controller
{
    private readonly AttendanceContext _context;

    public ProfileController(AttendanceContext context)
    {
        _context = context;
    }

    // Existing Details action method
    public async Task<IActionResult> Details()
    {
        var currentUserId = GetCurrentUserId();
        var student = await _context.Users
            .OfType<Student>()
            .AsNoTracking() // Add this line
            .Include(s => s.Permissions)
            .FirstOrDefaultAsync(s => s.ID == currentUserId);

        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }





    // New Edit action method
    public async Task<IActionResult> Edit()
    {
        var currentUserId = GetCurrentUserId();
        var student = await _context.Users
            .OfType<Student>()
            .FirstOrDefaultAsync(s => s.ID == currentUserId);

        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Student student)
    {
        var currentUserId = GetCurrentUserId();
        if (student.ID != currentUserId)
        {
            return Forbid();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var studentInDb = await _context.Users
                    .OfType<Student>()
                    .FirstOrDefaultAsync(s => s.ID == currentUserId);

                if (studentInDb == null)
                {
                    return NotFound();
                }

                // Update the properties of the studentInDb object
                studentInDb.Email = student.Email;
                studentInDb.Password = student.Password;
                studentInDb.Name = student.Name;
                studentInDb.Address = student.Address;
                studentInDb.Phone = student.Phone;
                studentInDb.University = student.University;
                studentInDb.Faculty = student.Faculty;
                studentInDb.Specialization = student.Specialization;
                studentInDb.Graduation_year = student.Graduation_year;

                await _context.SaveChangesAsync();
                return RedirectToAction("Details");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(student.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        else
        {
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
        }
        return View(student);
    }









    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.ID == id);
    }


    private int GetCurrentUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    }

}
