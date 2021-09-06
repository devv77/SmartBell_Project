using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// TemplateElement is a child entity of template.
    /// Specifies a bellringing pattern element.
    /// </summary>
    public class TemplateElement
    {
        /// <summary>Unique Id used for database storage.</summary>
        [Key]
        public string Id { get; set; }

        /// <summary>Generalized start time of outputing sound where only the hour and minite properties are important.</summary>
        [Required]
        public DateTime BellRingTime { get; set; }

        /// <summary>The interval of outputing sound.</summary>
        public TimeSpan Interval { get; set; } //TODO this property should not be null, set automatically.

        /// <summary>The interval of outputing sound in seconds for manual setup.</summary>
        public int IntervalSeconds { get; set; }

        /// <summary>
        /// The path of the file where the element's output is specified.
        /// There are 2 possibilites [*].mp3 or [*].txt
        /// </summary>
        [StringLength(50)]
        [Required]
        public string FilePath { get; set; }

        /// <summary>
        /// Specifies the reasoning of a bellring 0=Start of a lesson 1=end of a lesson 2= special reason
        /// A template must never have the value of special.
        /// </summary>
        [Required]
        public BellRingType Type { get; set; }

        /// <summary>Specifies the id of the parent for the database connection.</summary>
        public string TemplateId { get; set; }

        /// <summary>Specifies the parent of this child element for the database connection.</summary>
        [NotMapped]
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Template ParentTemplate { get; set; }

    }
}
