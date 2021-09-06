using Models;
using Models.UI.VM;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class TemplateEditingLogic
    {

        private ITemplateRepository templateRepo;
        private IHolidayRepository holidayRepo;
        private ITemplateElementRepository templateElementRepo;
        private IOutputPathRepository outputPathRepo;
        private ModificationLogic modLogic;
        private ReadLogic readlogic;

        public TemplateEditingLogic(ITemplateRepository templateRepo, IHolidayRepository holidayRepo,
            ITemplateElementRepository templateElementRepo, IOutputPathRepository outputPathRepo,
            ReadLogic readLogic, ModificationLogic modLogic)
        {
            this.templateRepo = templateRepo;
            this.holidayRepo = holidayRepo;
            this.templateElementRepo = templateElementRepo;
            this.outputPathRepo = outputPathRepo;
            this.readlogic = readLogic;
        }

        public void DeleteLessonTemplateElement(TemplateElement templateElement)
        {
            switch (templateElement.Type)
            {
                case BellRingType.Start:
                    TemplateElement nextBellRinging = (from x in readlogic.GetElementsForTemplate(templateElement.TemplateId)
                                                       where x.BellRingTime > templateElement.BellRingTime
                                                       orderby x.BellRingTime ascending
                                                       select x).FirstOrDefault();
                    if (nextBellRinging != null)
                    {
                        templateElementRepo.Delete(nextBellRinging);
                    }
                    break;

                case BellRingType.End:
                    TemplateElement previousBellRinging = (from x in readlogic.GetElementsForTemplate(templateElement.TemplateId)
                                                           where x.BellRingTime < templateElement.BellRingTime
                                                           orderby x.BellRingTime descending
                                                           select x).FirstOrDefault();
                    if (previousBellRinging != null)
                    {
                        templateElementRepo.Delete(previousBellRinging);
                    }
                    break;
            }
            templateElementRepo.Delete(templateElement);
        }
        public void InsertLessonTemplateElements
          (Lesson startOfLesson, Lesson endOfLesson,
           string startFileName, string endFileName, string templateId, string templateName)
        {
            if (startOfLesson == default || endOfLesson == default
                || startFileName == default || endFileName == default)
            {
                throw new Exception("All parameters must be declared in the body.");
            }
            if (startOfLesson.BellRingTime > endOfLesson.BellRingTime)
            {
                throw new Exception("The start of a lesson must be earlier than the end.");
            }
            if ((TimeSpan)(endOfLesson.BellRingTime - startOfLesson.BellRingTime) > new TimeSpan(1, 0, 0))
            {
                throw new Exception("All bellRings must be with in a 60 minute range to each other.");
            }
            Template template = readlogic.GetOneTemplate(templateId);
            if (template == null)
            {
                if (templateName == null)
                {
                    throw new Exception("You must include a template name if it's newly created one.\nThe Id was not found.");
                }
                int i = 1;
                while (!VerifyTemplateName(templateName))
                {
                    templateName += i;
                    i++;
                }
                template = new Template { Id = Guid.NewGuid().ToString(), Name = templateName };
                templateRepo.InsertOne(template);
                templateId = template.Id;
            }
            TemplateElement startTemplateElement = new TemplateElement
            {
                Id = Guid.NewGuid().ToString(),
                FilePath = startFileName,
                TemplateId = template.Id,
                IntervalSeconds = startOfLesson.IntervalSeconds,
                BellRingTime = new DateTime(1, 1, 1, startOfLesson.BellRingTime.Hour, startOfLesson.BellRingTime.Minute,
                startOfLesson.BellRingTime.Second),
                Type = BellRingType.Start,
            };
            templateElementRepo.InsertOne(startTemplateElement);

            TemplateElement endTemplateElement = new TemplateElement
            {
                Id = Guid.NewGuid().ToString(),
                FilePath = endFileName,
                TemplateId = template.Id,
                IntervalSeconds = endOfLesson.IntervalSeconds,
                BellRingTime = new DateTime(1, 1, 1, endOfLesson.BellRingTime.Hour, endOfLesson.BellRingTime.Minute,
                endOfLesson.BellRingTime.Second),
                Type = BellRingType.Start,
            };
            templateElementRepo.InsertOne(endTemplateElement);
            if (TemplateLessonIntersect(startTemplateElement, endTemplateElement, templateId))
            {
                templateElementRepo.Delete(startTemplateElement);
                templateElementRepo.Delete(endTemplateElement);
                throw new Exception($"This lesson must not intersect with other lessons during date {startTemplateElement.BellRingTime:d}");
            }
            if (SingleIntersect(startTemplateElement, templateId) || SingleIntersect(endTemplateElement, templateId))
            {
                templateElementRepo.Delete(startTemplateElement);
                templateElementRepo.Delete(endTemplateElement);
                throw new Exception($"A bellring must not intersect with any other bellrigns during date {startTemplateElement.BellRingTime:d}");
            }
        }
        private bool VerifyTemplateName(string name)
        {
            IQueryable<Template> SameNames = templateRepo.GetAll().Where(x => x.Name == name);
            if (SameNames.Count() == 0)
            {
                return true;
            }
            return false;
        }
        private bool TemplateLessonIntersect(TemplateElement startTemplateElement, TemplateElement endTemplateElement, string templateId)
        {
            // checks if bellring sticks in between a lesson's bellring
            List<TemplateElement> starts = readlogic.GetElementsForTemplate(templateId).Where(x => x.Type.Equals(BellRingType.Start) && x.Id != startTemplateElement.Id).OrderBy(x => x.BellRingTime).ToList();
            List<TemplateElement> ends = readlogic.GetElementsForTemplate(templateId).Where(x => x.Type.Equals(BellRingType.End) && x.Id != endTemplateElement.Id).OrderBy(x => x.BellRingTime).ToList();
            if (starts.Count() != ends.Count())
            {
                throw new Exception($"This date : {startTemplateElement.BellRingTime:D} is not initialized properly, all start types must have a corresponding end type.");
            }
            for (int i = 0; i < starts.Count(); i++)
            {
                if (starts[i].BellRingTime <= startTemplateElement.BellRingTime && startTemplateElement.BellRingTime <= ends[i].BellRingTime + ends[i].Interval
                    || (starts[i].BellRingTime <= endTemplateElement.BellRingTime && endTemplateElement.BellRingTime <= ends[i].BellRingTime + ends[i].Interval))
                {
                    return true;
                }
            }
            return false;
        }

        private bool SingleIntersect(TemplateElement TemplateElement, string templateId)
        {
            List<TemplateElement> templateElements = readlogic.GetElementsForTemplate(templateId).Where(x => x.Id != TemplateElement.Id).OrderBy(x => x.BellRingTime).ToList();
            for (int i = 0; i < templateElements.Count(); i++)
            {
                if (templateElements[i].BellRingTime > TemplateElement.BellRingTime)
                {
                    if (templateElements[i].BellRingTime <= TemplateElement.BellRingTime || TemplateElement.BellRingTime + TemplateElement.Interval >= templateElements[i].BellRingTime)
                    {
                        return true;
                    }
                }
                else
                {
                    if (TemplateElement.BellRingTime >= templateElements[i].BellRingTime && TemplateElement.BellRingTime <= templateElements[i].BellRingTime + templateElements[i].Interval)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
