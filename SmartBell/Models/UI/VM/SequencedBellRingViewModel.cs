using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.UI.VM
{
    /// <summary>
    /// View model for inserting sequenced bellrings.
    /// </summary>
    public class SequencedBellRingViewModel
    {
        /// <summary>
        /// Specifies a bellring which is sequenced.
        /// </summary>
        public BellRing bellRing { get; set; }
        /// <summary>
        /// Specifies the containing outputs.
        /// </summary>
        public List<OutputPath> outputPaths { get; set; }
    }
}
