using Data;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IHolidayRepository : IRepository<Holiday>
    {

    }
    public class HolidayRepository : Repository<Holiday>, IHolidayRepository
    {
        public HolidayRepository(SbDbContext context) : base(context)
        {
        }

        public override Holiday GetOne(string id)
        {
            return GetAll().SingleOrDefault(x => x.Id == id);
        }
    }
}
