using Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class ClientLogic
    {
        ModificationLogic modLogic;
        ReadLogic readLogic;
        IBellRingRepository bellRingRepo;
        ITemplateRepository templateRepo;
        IOutputPathRepository outputPathRepo;

        public ClientLogic(ModificationLogic modLogic,IBellRingRepository bellRingRepo, ITemplateRepository templateRepo,ReadLogic readLoigc, IOutputPathRepository outputPathRepo)
        {
            this.modLogic = modLogic;
            this.readLogic = readLoigc;
            this.outputPathRepo = outputPathRepo;
            this.bellRingRepo = bellRingRepo;
            this.templateRepo = templateRepo;
        }

        public Template GetTemplateByName(string name)
        {
            return templateRepo.GetAll().Where(x => x.Name == name).FirstOrDefault();  
        }
        public IQueryable<TemplateElement> GetElementsForTemplate(string id)
        {
            return readLogic.GetElementsForTemplate(id);
        }
        public IQueryable<BellRing> GetBellRingsForDay(DateTime dayDate)
        {
            return bellRingRepo.GetBellRingsForDay(dayDate).OrderBy(x=>x.BellRingTime);
        }
        public IQueryable<OutputPath> GetAllOutputPathsForDay(DateTime dayDate)
        {
           IQueryable<BellRing> bellringsofday = bellRingRepo.GetBellRingsForDay(dayDate);
           return outputPathRepo.GetAll().Where(x => bellringsofday.Select(y => y.Id).Contains(x.BellRingId));
        }
    }
}
