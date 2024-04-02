using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance_Time_Tracking.Models
{
    public enum UserRole { Employee, Student, Instructor, Supervisor}
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is required")]
        [MinLength(3,ErrorMessage ="Name Must be 3 letters or more")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^([\w-\.]{3,20})+@([\w-]+\.)+[\w-]{2,4}$",ErrorMessage ="invalid email")]
        public string Email { get; set; }
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$%_])[A-Za-z\d@$%_]{8,}$", ErrorMessage = "Must be 8 characters or more.\r\nMust contain at least one uppercase letter.\r\nMust contain at least one lowercase letter.\r\nMust contain at least one digit.\r\nMust contain at least one special character from @$%_")]
        public string Password { get; set; }
        public int Phone { get; set; }
        public UserRole Role { get; set; }
        [ForeignKey("DeptId")]
        public int DeptId { get; set; }


    }
}
