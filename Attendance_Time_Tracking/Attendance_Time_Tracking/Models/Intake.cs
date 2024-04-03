﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Attendance_Time_Tracking.Models
{

    [Table("Intakes")]
    public class Intake
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name="IntakeName")]
        public string Name { get; set; }
        
        public int ProgramId { get; set; }
        [ForeignKey("ProgramId")]
        public Programs Program { get; set; }
/*
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }


        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }*/
        public ICollection<Track> Tracks { get; set; }

    }
}
