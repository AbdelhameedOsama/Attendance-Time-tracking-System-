using System.ComponentModel.DataAnnotations;

namespace Attendance_Time_Tracking.Models
{
	public class LoginViewModel
	{
		[Required]
		public string UserEmail { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string UserPass { get; set; }
	}
}
