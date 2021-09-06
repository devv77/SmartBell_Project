using Data;
using System;
using System.Collections.Generic;

namespace Logic
{
    public interface ITimeLogic
    {
        DateTime GetNetworkTime();
        BellRing GetNextBellringFromServer(DateTime dayDate);
        IList<BellRing> RemoveElapsedBellRing(string id,List<BellRing> list);
        IList<BellRing> RemoveAllElapsedBellRings(DateTime comaprertime, List<BellRing> list);
        BellRing GetNextBellringFromList(DateTime time, List<BellRing> list);
    }
}
