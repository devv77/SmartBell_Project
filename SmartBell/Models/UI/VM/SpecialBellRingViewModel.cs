using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.UI.VM
{
    /// <summary>
    /// View model for inserting a special bellring.
    /// </summary>
    public class SpecialBellRingViewModel
    {
        /// <summary>
        /// Specifies a bellring which has the type of special.
        /// </summary>
        public BellRing bellRing { get; set; }

        /// <summary>
        /// Specifies the containing output.
        /// </summary>
        public string fileName{ get; set; }
    }
}
