using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance_Time_Tracking.Models
{
    public enum PermissionTypes { Late_Arrival, Absence}
    public enum PermissionStatus { Pendding, Approved, Rejected}
    public class Permission
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public PermissionTypes Type { get; set; }
        public PermissionStatus Status { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User UserNavigation { get; set; }

    }
}
