using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    /// <summary>Specifies the types of holidays 0=Manually setup , 1=Break ,2=Holiday</summary>
    public enum HolidayType
    {
        /// <summary>Manual holidays are unexpected holidays caused by a specific reasoining. </summary>
        Manual,
        /// <summary>Break is a specified break like the summer break.</summary>
        Break,
        /// <summary>Holidays are specified by the calender for example dec 25 is christmas.</summary>
        Holiday
    }
    /// <summary>Holidays are specific time range when there are no bell rings for a specific reason.</summary>
    public class Holiday
    {
        /// <summary>Unique Id used for database storage.</summary>
        [Key]
        public string Id { get; set; }

        /// <summary>Defines the name of a holiday.</summary>
        public string Name { get; set; }

        /// <summary>Specifies the type of the holiday 0=Manually setup , 1=Break ,2=Holiday.</summary>
        [Required]
        public HolidayType Type { get; set; }

        /// <summary>Specifies the starting time of a holiday.</summary>
        [Required]
        public DateTime StartTime{ get; set; }

        /// <summary>Specifies the ending time of a holiday.</summary>
        [Required]
        public DateTime EndTime { get; set; }
    }
}
