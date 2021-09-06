using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    /// <summary>
    /// The role of a bellring 0=Start of a lesson 1=end of a lesson 2= special reason
    /// </summary>
    public enum BellRingType
    {
        /// <summary>Indicates the start of a lesson.</summary>
        Start,
        /// <summary>Indicates the end of a lesson.</summary>
        End,
        /// <summary>Indicates that it's not part of a regular bellringing.</summary>
        Special
    }

    /// <summary>BellRing is a sound done by the ouput device for a specific reason at a specific time range.</summary>
    public class BellRing
    {
        /// <summary>Unique Id used for database storage.</summary>
        [Key]
        public string Id { get; set; }
        /// <summary>Describes a bellring.</summary>
        public string Description { get; set; }
        /// <summary>The start time of outputing sound.</summary>
        [Required]
        public DateTime BellRingTime { get; set; }

        /// <summary>Read only parameter, interval of outputing sound.</summary>
        public TimeSpan Interval { get; set; } //TODO this property should not be null, set automatically.

        /// <summary>The interval of outputing sound in seconds for manual setup.</summary>
        public int IntervalSeconds { get; set; }

        /// <summary>
        /// Database storage reference of storing one or multiple outputPaths for a bellringing.
        /// </summary>
        [NotMapped]
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual ICollection<OutputPath> OutputPaths { get; set; }

        /// <summary>Specifies the reasoning of a bellring 0=Start of a lesson 1=end of a lesson 2= special reason</summary>
        [Required]
        [Range(0,2)]
        public BellRingType Type { get; set; }

    }
}
