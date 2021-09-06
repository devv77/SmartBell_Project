using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// OutputPath is a child entity of Bellring.
    /// Specifies a filepath which connects to a Bellring entity.
    /// </summary>
    public class OutputPath
    {
        /// <summary>Unique Id used for database storage.</summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// The path of the file where the element's output is specified.
        /// There are 2 possibilites [*].mp3 or [*].txt
        /// </summary>
        [StringLength(50)]
        [Required]
        public string FilePath { get; set; }

        /// <summary>Specifies the index of the sequence.</summary>
        public int SequenceID { get; set; }

        /// <summary>Specifies the id of the parent for the database connection.</summary>
        public string BellRingId { get; set; }

        /// <summary>Specifies the parent of this child element for the database connection.</summary>
        [NotMapped]
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual BellRing ParentBellRing { get; set; }

    }
}
