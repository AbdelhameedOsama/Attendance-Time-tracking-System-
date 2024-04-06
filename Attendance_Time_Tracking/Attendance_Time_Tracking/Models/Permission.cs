using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Attendance_Time_Tracking.Models
{
    public enum PermissionTypes { Late_Arrival, Absence}

    public enum PermissionStatus { Pending, Approved, Rejected}


    public class Permission
    {
        // composite key
        public int StdId { get; set; }
        public Student Student { get; set; }
        public DateTime Date { get; set; }

        [Required]
        public string Reason { get; set; }
        public PermissionTypes Type { get; set; }
        public PermissionStatus Status { get; set; }

        [AllowNull]
        public int? SupId { get; set; }
        [AllowNull]
        public Instructor Supervisor { get; set; }

    }
}
