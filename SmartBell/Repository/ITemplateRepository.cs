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
    public interface ITemplateRepository : IRepository<Template>
    {
    }

    public class TemplateRepository : Repository<Template>, ITemplateRepository
    {
        public TemplateRepository(SbDbContext context) : base(context)
        {
        }

        public override Template GetOne(string id)
        {
            return GetAll().SingleOrDefault(x => x.Id == id);
        }
    }
}
