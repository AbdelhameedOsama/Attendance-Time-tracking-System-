using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance_Time_Tracking.Models
{
    public enum ProgramType
    {
        IntensiveTraining,
        SummerProgram,
        ProfessionalProgram
    }
    [Table("Programs")]
    public class Programs
    {
        [Key]
        [Display(Name = "ID")]
        public int ProgramId { get; set; }

        [Required]
        [Display(Name = "Program Name")]
        [Column(TypeName = "nvarchar(50)")]
        public ProgramType ProgramType { get; set; }
        public virtual List<Intake> ProgramIntakes { get; set; } = new List<Intake>();
    }
}
