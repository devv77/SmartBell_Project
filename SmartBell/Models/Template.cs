using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models
{
    /// <summary>Template has a pattern of bellrings for a day.</summary>
    public class Template
    {
        /// <summary>Unique Id used for database storage.</summary>
        [Key]
        public string Id { get; set; }

        /// <summary>Specifies the name of a bellringing pattern.</summary>
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        /// <summary>Database storage reference for pattern elements.</summary>
        [NotMapped]
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual ICollection<TemplateElement> TemplateElements { get; set; }

    }
}
