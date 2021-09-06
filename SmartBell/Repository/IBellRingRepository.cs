using Data;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IBellRingRepository : IRepository<BellRing>
    {
        public IQueryable<BellRing> GetBellRingsForDay(DateTime dayDate);
        public void Update(string oid,BellRing nItem);
    }
    public class BellRingRepository : Repository<BellRing>, IBellRingRepository
    {
        public BellRingRepository(SbDbContext context) : base(context)
        {
        }

        public override BellRing GetOne(string id)
        {
            return GetAll().SingleOrDefault(x => x.Id == id);
        }
        public void Update(string oid, BellRing nItem)
        {
            BellRing old_bellRing = this.GetOne(oid);


            old_bellRing.Description = nItem.Description;
            old_bellRing.BellRingTime = nItem.BellRingTime;
            old_bellRing.Interval = nItem.Interval;
            old_bellRing.IntervalSeconds = nItem.IntervalSeconds;
            old_bellRing.Type = nItem.Type;
            this.SaveChanges();
        }
        public IQueryable<BellRing> GetBellRingsForDay(DateTime dayDate)
        {
            IQueryable<BellRing> BellringsOfDay = GetAll().Where(bellring => bellring.BellRingTime.Date == dayDate.Date);
            return BellringsOfDay;
        }
        
    }
}
