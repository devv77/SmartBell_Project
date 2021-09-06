using Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class ReadLogic
    {
        IBellRingRepository bellRingRepo;
        ITemplateRepository templateRepo;
        IHolidayRepository holidayRepo;
        ITemplateElementRepository templateElementRepo;
        IOutputPathRepository outputPathRepo;
        public ReadLogic(IBellRingRepository bellRingRepo, ITemplateRepository templateRepo, IHolidayRepository holidayRepo, ITemplateElementRepository templateElementRepo, IOutputPathRepository outputPathRepo)
        {
            this.bellRingRepo = bellRingRepo;
            this.templateRepo = templateRepo;
            this.holidayRepo = holidayRepo;
            this.templateElementRepo = templateElementRepo;
            this.outputPathRepo = outputPathRepo;
        }

        // Get one
        public BellRing GetOneBellring(string id)
        {
            return bellRingRepo.GetOne(id);
        }

        public Holiday GetOneHoliday(string id)
        {
            return holidayRepo.GetOne(id);
        }

        public Template GetOneTemplate(string id)
        {
            return templateRepo.GetOne(id);
        }
        public TemplateElement GetOneTemplateElement(string id)
        {
            return templateElementRepo.GetOne(id);
        }
        public OutputPath GetOneOutputPath(string id)
        {
            return outputPathRepo.GetOne(id);
        }

        // Get all
        public IQueryable<BellRing> GetAllBellring()
        {
            return bellRingRepo.GetAll();
        }

        public IQueryable<Holiday> GetAllHoliday()
        {
            return holidayRepo.GetAll();
        }

        public IQueryable<Template> GetAllTemplate()
        {
            return templateRepo.GetAll();
        }
        public IQueryable<TemplateElement> GetAllTemplateElement()
        {
            return templateElementRepo.GetAll();
        }
        public IQueryable<OutputPath> GetAllOutputPath()
        {
            return outputPathRepo.GetAll();
        }

        public IQueryable<Template> GetAllSampleTemplate()
        {
            return templateRepo.GetAll().Where(x => x.Id.Length < 4);
        }

        public IQueryable<Holiday> GetAllCalendarHoliday()
        {
            return holidayRepo.GetAll().Where(x => x.Type.Equals(HolidayType.Holiday));
        }

        public IQueryable<TemplateElement> GetElementsForTemplate(string templateId)
        {
            return templateElementRepo.GetAll().Where(x => x.TemplateId == templateId);
        }

        public IQueryable<OutputPath> GetOutputsForBellRing(string bellringId)
        {
            return outputPathRepo.GetAll().Where(x => x.BellRingId == bellringId);
        }

        public IQueryable<BellRing> GetAllSequencedBellRings()
        {
            return bellRingRepo.GetAll().Where(x => x.Description != null && x.BellRingTime==new DateTime(1,1,1) &&
            GetAllOutputPath().Where(y=>y.BellRingId==x.Id).Select(y=>y.SequenceID).Contains(1) &&
            x.Type.Equals(BellRingType.Special));
        }
        public BellRing GetSequencedBellring(string id)
        {
            return GetAllSequencedBellRings().Where(x => x.Id == id).FirstOrDefault();
        }
        public IQueryable<Holiday> GetBreaksForDay(DateTime dayDate)
        {
            
            List<BellRing> Starts = bellRingRepo.GetAll().Where(x=>x.BellRingTime.Date==dayDate.Date && x.Type.Equals(BellRingType.Start)).OrderBy(x => x.BellRingTime).ToList<BellRing>();
            List<BellRing> Ends = bellRingRepo.GetAll().Where(x => x.BellRingTime.Date == dayDate.Date && x.Type.Equals(BellRingType.End)).OrderBy(x=>x.BellRingTime).ToList<BellRing>();
            List<Holiday> Breaks = new List<Holiday>();
            if (Starts.Count()==Ends.Count())
                for (int i = 0; i < Starts.Count(); i++)
                {
                        Holiday h = new Holiday
                        {
                            Id = new Guid().ToString(),
                            StartTime = Starts[i].BellRingTime,
                            EndTime= (DateTime)(Ends[i].BellRingTime+Ends[i].Interval),
                            Type=HolidayType.Break,
                        };
                        Breaks.Add(h);


                }
            return Breaks.AsQueryable();
            throw new Exception("Can't check for breaks, all start values must have an end value.");


        }
        public int FileOccurrence(string fileName)
        {
            return (outputPathRepo.GetAll().Where(x => x.FilePath == fileName).Count() +
                templateElementRepo.GetAll().Where(x => x.FilePath == fileName).Count());
        }
        public bool FileIsUsed(string filename)
        {
            return (FileOccurrence(filename) > 0);
        }
    }
}
