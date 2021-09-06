using Data;
using System;
using System.Collections.Generic;

namespace Logic
{
    public interface IReadLogic
    {
        IList<BellRing> GetBellRingsForDay(DateTime dayDate);
        IList<OutputPath> GetAllOutputPathsForDay(DateTime dayDate);
        void GetAllFiles(IList<OutputPath> outputs);
        IList<OutputPath> GetOutputPathsForBellRing(string id, List<OutputPath> outputPaths);
    }
}
