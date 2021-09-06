using Data;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IOutputPathRepository : IRepository<OutputPath>
    {

    }
    public class OutputPathRepository : Repository<OutputPath>, IOutputPathRepository
    {
        public OutputPathRepository(SbDbContext context) : base(context)
        {
        }

        public override OutputPath GetOne(string id)
        {
            return GetAll().SingleOrDefault(x => x.Id == id);
        }
    }
}
