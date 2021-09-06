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
    public interface ITemplateElementRepository:IRepository<TemplateElement>
    {
    }
    public class TemplateEelementRepository : Repository<TemplateElement>, ITemplateElementRepository
    {
        public TemplateEelementRepository(SbDbContext context) : base(context)
        {
        }
        public override TemplateElement GetOne(string id)
        {
            return GetAll().SingleOrDefault(x => x.Id == id);
        }
    }
}
