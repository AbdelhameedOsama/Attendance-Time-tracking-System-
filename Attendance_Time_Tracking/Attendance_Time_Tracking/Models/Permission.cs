using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance_Time_Tracking.Models
{
    public enum PermissionTypes { Late_Arrival, Absence}
    public enum PermissionStatus { Pendding, Approved, Rejected}

    public class Permission
    {
        // composite key
        public int StdId { get; set; }
        public Student StdNavigation { get; set; }
        public DateTime Date { get; set; }


        public string Reason { get; set; }
        public PermissionTypes Type { get; set; }
        public PermissionStatus Status { get; set; }

        public int SupId { get; set; }
        public Instructor SupNavigation { get; set; }

    }
}
