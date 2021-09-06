using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Models.UI.VM
{
    /// <summary>
    /// Describe a lessons parameters as a viewModel.
    /// </summary>
    public class Lesson
    {
        /// <summary>
        /// Describes the reason of a bellringing.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Starting point a of a bellringing
        /// </summary>
        public DateTime BellRingTime { get; set; }
        /// <summary>
        /// Property which can determine static length of a bellringing in seconds.
        /// </summary>
        public int IntervalSeconds { get; set; }
    }
    /// <summary>
    /// View model for inserting lessons.
    /// </summary>
    public class LessonViewModel
    {
        /// <summary>
        /// Starting bellring of a lesson.
        /// </summary>
        public Lesson startBellRing { get; set; }
        /// <summary>
        /// Ending bellring of a lesson.
        /// </summary>
        public Lesson endBellring { get; set; }
        /// <summary>
        /// Starting output for the bellring of a lesson.
        /// </summary>
        public string startFilename { get; set; }
        /// <summary>
        /// Ending output for the bellring of a lesson.
        /// </summary>
        public string endFilename { get; set; }
    }
}
