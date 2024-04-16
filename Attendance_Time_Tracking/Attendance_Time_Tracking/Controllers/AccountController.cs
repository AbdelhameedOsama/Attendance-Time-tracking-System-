using Attendance_Time_Tracking.Models;
using Attendance_Time_Tracking.Repos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Attendance_Time_Tracking.Controllers
{
	public class AccountController : Controller
	{
		readonly IUserRepo userRepo;
		public AccountController(IUserRepo _userRepo)
		{
			userRepo = _userRepo;
		}
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> LoginAsync(LoginViewModel login)
		{
				if (ModelState.IsValid)
				{
					var user = userRepo.GetUser(login.UserEmail, login.UserPass);
					if (user != null)
					{
						ClaimsIdentity identity = new(new[]
						{
					new Claim(ClaimTypes.Email, user.Email),
					new Claim(ClaimTypes.Name, user.Name),
					new Claim(ClaimTypes.Role, user.Role.ToString()),
					new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()) //add the user id to the claims
				}, CookieAuthenticationDefaults.AuthenticationScheme);
						ClaimsPrincipal principal = new(identity);
						await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
						if (user.Role.ToString() == "Student")
						{
							return RedirectToAction("Index", "Student");
						}
						else if (user.Role.ToString() == "Instructor")
						{						
							return RedirectToAction("Index", "Instructor");
						}
						else if (user.Role.ToString() == "Supervisor")
						{	
							return RedirectToAction("Index", "Instructor");
						}
						else if (user.Role.ToString() == "Employee")
						{
							if(userRepo.GetEmployeeType(user.ID).Result == Emp_Types.Security)
							{
								return RedirectToAction("Index", "Employee");
							}
							else
							{
								return RedirectToAction("Index", "StudentAffairs");
							}
							
						}
						else if(user.Role.ToString() == "Admin")
					{
                            return RedirectToAction("Index", "Admin");
                        }
                        else
						{
							return RedirectToAction("Login");
						}
					}
					else
					{
						ModelState.AddModelError("", "Invalid Email or Password");
					}
				}
				return View(login);
			}



		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			return RedirectToAction("Login");
		}
	}
}
