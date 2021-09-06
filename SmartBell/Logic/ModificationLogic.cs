using Microsoft.AspNetCore.StaticFiles;
using Models;
using Models.UI.VM;
using Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Logic
{
    public class ModificationLogic
    {
        private IBellRingRepository bellRingRepo;
        private ITemplateRepository templateRepo;
        private IHolidayRepository holidayRepo;
        private ITemplateElementRepository templateElementRepo;
        private IOutputPathRepository outputPathRepo;
        private ReadLogic readlogic;

        public ModificationLogic(IBellRingRepository bellRingRepo, ITemplateRepository templateRepo, IHolidayRepository holidayRepo, ITemplateElementRepository templateElementRepo, IOutputPathRepository outputPathRepo, ReadLogic readLogic)
        {
            this.bellRingRepo = bellRingRepo;
            this.templateRepo = templateRepo;
            this.holidayRepo = holidayRepo;
            this.templateElementRepo = templateElementRepo;
            this.outputPathRepo = outputPathRepo;
            this.readlogic = readLogic;
        }

        // Insert One and Multiple
        public void InsertBellRing(BellRing bellRing)
        {
            bellRing.Id = Guid.NewGuid().ToString();
            if (bellRing.IntervalSeconds > 0)
            {
                bellRing.Interval = new TimeSpan(0, 0, bellRing.IntervalSeconds);
            }
            bellRingRepo.InsertOne(bellRing);
            if (bellRing.IntervalSeconds == 0)
                SetBellRingIntervalByPath(bellRing.Id);
        }

        public void InsertMultipleBellRings(IList<BellRing> bellRings)
        {
            foreach (BellRing bellRing in bellRings)
            {
                if (bellRingRepo.GetOne(bellRing.Id) != null)
                {
                    InsertBellRing(bellRing); // Important to use the Logic's method which generates id-s
                }
                else
                {
                    bellRingRepo.Update(bellRing.Id, bellRingRepo.GetOne(bellRing.Id));
                }
            }
        }

        public void InsertSpecialBellring(BellRing specialBellRing, string fileName)
        {
            if (specialBellRing == default || fileName == default)
            {
                throw new Exception("All parameters must be declared in the body.");
            }
            if (specialBellRing.Description == null)
            {
                throw new Exception("All special bellrings must have a description to describe their purpose.");
            }
            specialBellRing.Id = Guid.NewGuid().ToString();
            specialBellRing.Type = BellRingType.Special;
            if (specialBellRing.IntervalSeconds > 0)
            {
                specialBellRing.Interval = new TimeSpan(0, 0, specialBellRing.IntervalSeconds);
            }
            bellRingRepo.InsertOne(specialBellRing);
            OutputPath outputPath = new OutputPath();
            outputPath.FilePath = fileName;
            outputPath.Id = Guid.NewGuid().ToString();
            outputPath.BellRingId = specialBellRing.Id;
            outputPath.SequenceID = 0;
            outputPathRepo.InsertOne(outputPath);
            SetBellRingIntervalByPath(specialBellRing.Id);
            if (SingleIntersect(specialBellRing) || BetweenLessonsIntersect(specialBellRing))
            {
                bellRingRepo.Delete(specialBellRing);
                throw new Exception($"A bellring must not intersect with any other bellrigns during date {specialBellRing.BellRingTime:d}");
            }
        }

        public void InsertLessonBellrings
           (Lesson startOfLesson, Lesson endOfLesson,
            string startFileName, string endFileName)
        {
            if (startOfLesson == default || endOfLesson == default
                || startFileName == default || /*startFileName == "" ||*/ endFileName == default /*|| endFileName == ""*/) // THIS SECTION IS NEEDED(TESTING)
            {
                throw new Exception("All parameters must be declared in the body.");
            }

            //TESTING PURPOSESˇ
            if (startFileName == "")
            {
                startFileName = "default.mp3";
            }
            if (endFileName == "")
            {
                endFileName = "default.mp3";
            }
            //THIS SECTION MUST BE REMOVED^
            if (startOfLesson.BellRingTime > endOfLesson.BellRingTime)
            {
                throw new Exception("The start of a lesson must be earlier than the end.");
            }
            if ((TimeSpan)(endOfLesson.BellRingTime - startOfLesson.BellRingTime) > new TimeSpan(1, 0, 0))
            {
                throw new Exception("All bellRings must be with in a 60 minute range to each other.");
            }
            BellRing startBellRing = new BellRing
            {
                Description = startOfLesson.Description,
                BellRingTime = startOfLesson.BellRingTime,
                IntervalSeconds = startOfLesson.IntervalSeconds,
            };
            startBellRing.Id = Guid.NewGuid().ToString();
            startBellRing.Type = BellRingType.Start;
            if (startBellRing.IntervalSeconds > 0)
            {
                startBellRing.Interval = new TimeSpan(0, 0, startBellRing.IntervalSeconds);
            }
            bellRingRepo.InsertOne(startBellRing);
            BellRing endBellring = new BellRing
            {
                Description = endOfLesson.Description,
                BellRingTime = endOfLesson.BellRingTime,
                IntervalSeconds = endOfLesson.IntervalSeconds,
            };
            endBellring.Id = Guid.NewGuid().ToString();
            endBellring.Type = BellRingType.End;
            if (startBellRing.IntervalSeconds > 0)
            {
                endBellring.Interval = new TimeSpan(0, 0, endBellring.IntervalSeconds);
            }
            bellRingRepo.InsertOne(endBellring);
            OutputPath startOutput = new OutputPath();
            startOutput.FilePath = startFileName;
            startOutput.Id = Guid.NewGuid().ToString();
            startOutput.BellRingId = startBellRing.Id;
            startOutput.SequenceID = 0;
            outputPathRepo.InsertOne(startOutput);
            OutputPath endOutput = new OutputPath();
            endOutput.FilePath = endFileName;
            endOutput.Id = Guid.NewGuid().ToString();
            endOutput.BellRingId = endBellring.Id;
            endOutput.SequenceID = 0;
            outputPathRepo.InsertOne(endOutput);
            SetBellRingIntervalByPath(startBellRing.Id);
            SetBellRingIntervalByPath(endBellring.Id);
            if (LessonIntersects(startBellRing, endBellring))
            {
                bellRingRepo.Delete(startBellRing);
                bellRingRepo.Delete(endBellring);
                throw new Exception($"This lesson must not intersect with other lessons during date {startBellRing.BellRingTime:d}");
            }
            if (SingleIntersect(startBellRing) || SingleIntersect(endBellring))
            {
                bellRingRepo.Delete(startBellRing);
                bellRingRepo.Delete(endBellring);
                throw new Exception($"A bellring must not intersect with any other bellrigns during date {startBellRing.BellRingTime:d}");
            }
        }

        private bool BetweenLessonsIntersect(BellRing bellRing)
        {
            List<BellRing> starts = bellRingRepo.GetAll().Where(x => x.Type.Equals(BellRingType.Start)).OrderBy(x => x.BellRingTime).ToList();
            List<BellRing> ends = bellRingRepo.GetAll().Where(x => x.Type.Equals(BellRingType.End)).OrderBy(x => x.BellRingTime).ToList();
            if (starts.Count() != ends.Count())
            {
                throw new Exception($"This date : {bellRing.BellRingTime:D} is not initialized properly, all start types must have a corresponding end type.");
            }
            for (int i = 0; i < starts.Count(); i++)
            {
                if (starts[i].BellRingTime <= bellRing.BellRingTime && bellRing.BellRingTime <= ends[i].BellRingTime + ends[i].Interval)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Defines whether bellring intersects happen at their intervals.
        /// </summary>
        /// <param name="bellRing">A bellring which is checked</param>
        /// <returns>A binary value which defines whether intersect's occurred or not.</returns>
        private bool SingleIntersect(BellRing bellRing)
        {
            List<BellRing> bellRingsOfDay = bellRingRepo.GetBellRingsForDay(bellRing.BellRingTime).Where(x => x.Id != bellRing.Id).OrderBy(x => x.BellRingTime).ToList();
            // checks if bellrign's inetrval "sticks into" the next bellring in the period
            for (int i = 0; i < bellRingsOfDay.Count(); i++)
            {
                if (bellRingsOfDay[i].BellRingTime > bellRing.BellRingTime)
                {
                    if (bellRingsOfDay[i].BellRingTime <= bellRing.BellRingTime || bellRing.BellRingTime + bellRing.Interval >= bellRingsOfDay[i].BellRingTime)
                    {
                        return true;
                    }
                }
                else
                {
                    if (bellRing.BellRingTime >= bellRingsOfDay[i].BellRingTime && bellRing.BellRingTime <= bellRingsOfDay[i].BellRingTime + bellRingsOfDay[i].Interval)
                    {
                        return true;
                    }
                }
            }
            // checks if bellring sticks into a special bellring
            List<BellRing> specials = bellRingsOfDay.Where(x => x.Type.Equals(BellRingType.Special) && x.Id != bellRing.Id).OrderBy(x => x.BellRingTime).ToList();
            for (int i = 0; i < specials.Count(); i++)
            {
                if (specials[i].BellRingTime <= bellRing.BellRingTime && specials[i].BellRingTime + specials[i].Interval >= bellRing.BellRingTime)
                {
                    return true;
                }
            }
            return false;
        }

        private bool LessonIntersects(BellRing startBellRing, BellRing endBellring)
        {
            // checks if bellring sticks in between a lesson's bellring
            List<BellRing> starts = bellRingRepo.GetAll().Where(x => x.Type.Equals(BellRingType.Start) && x.Id != startBellRing.Id).OrderBy(x => x.BellRingTime).ToList();
            List<BellRing> ends = bellRingRepo.GetAll().Where(x => x.Type.Equals(BellRingType.End) && x.Id != endBellring.Id).OrderBy(x => x.BellRingTime).ToList();
            if (starts.Count() != ends.Count())
            {
                throw new Exception($"This date : {startBellRing.BellRingTime:D} is not initialized properly, all start types must have a corresponding end type.");
            }
            for (int i = 0; i < starts.Count(); i++)
            {
                if (starts[i].BellRingTime <= startBellRing.BellRingTime && startBellRing.BellRingTime <= ends[i].BellRingTime + ends[i].Interval
                    || (starts[i].BellRingTime <= endBellring.BellRingTime && endBellring.BellRingTime <= ends[i].BellRingTime + ends[i].Interval))
                {
                    return true;
                }
            }
            return false;
        }

        public void InsertHoliday(Holiday holiday)
        {
            holiday.Id = Guid.NewGuid().ToString();
            holidayRepo.InsertOne(holiday);
        }

        public void InsertTemplate(Template template)
        {
            template.Id = Guid.NewGuid().ToString();
            int i = 1;
            while (!VerifyTemplateName(template.Name))
            {
                template.Name += i;
                i++;
            }
            templateRepo.InsertOne(template);
        }

        public void InsertSequencedBellRings(BellRing bellRing, List<OutputPath> outputPaths)
        {
            if (bellRing == default || outputPaths == default)
            {
                throw new Exception("All parameters must be declared in the body.");
            }
            if (bellRing.Description == null)
            {
                throw new Exception("All sequenced bellrings must have a description to describe their purpose.");
            }
            if (outputPaths.Count() == 0)
            {
                throw new Exception("There are no outputs setup for this sequenced bellring.");
            }
            if (!outputPaths.All(output => output.SequenceID > 0))
            {
                throw new Exception("Indexing of the seuqence must start from 1.");
            }
            if (outputPaths.Select(output => output.SequenceID).Distinct().Count() != outputPaths.Count())
            {
                throw new Exception("All of the sequence ID-s must be unique starting from 1.");
            }
            outputPaths = outputPaths.OrderBy(output => output.SequenceID).ToList();
            bellRing.BellRingTime = new DateTime(1, 1, 1); // Assings a static time value to achive the "storing" of a reusable Sequenced bellring
            bellRing.Id = Guid.NewGuid().ToString();
            bellRing.Type = BellRingType.Special;
            bellRingRepo.InsertOne(bellRing);
            int i = 1;
            foreach (OutputPath outputPath in outputPaths)
            {
                outputPath.Id = Guid.NewGuid().ToString();
                outputPath.BellRingId = bellRing.Id;
                outputPath.SequenceID = i++;
            }
            outputPathRepo.InsertMultiple(outputPaths.ToList());
            SetBellRingIntervalByPath(bellRing.Id);
        }

        public void AssignTimeToSequencedBellRing(BellRing bellRing, DateTime time)
        {
            if (bellRing == default)
            {
                throw new Exception("All parameters must be declared in the body.");
            }
            if (bellRing.Description == null)
            {
                throw new Exception("All sequenced bellrings must have a description to describe their purpose.");
            }
            List<OutputPath> outputs = readlogic.GetOutputsForBellRing(bellRing.Id).ToList();
            if (outputs.Count() == 0)
            {
                throw new Exception("There are no outputs setup for this sequenced bellring.");
            }
            if (!outputs.All(output => output.SequenceID > 0))
            {
                throw new Exception("Indexing of the seuqence must start from 1.");
            }
            if (outputs.Select(output => output.SequenceID).Distinct().Count() != outputs.Count())
            {
                throw new Exception("All of the sequence ID-s must be unique starting from 1.");
            }
            BellRing newlyAssignedBellRing = new BellRing();
            newlyAssignedBellRing.Interval = bellRing.Interval;
            newlyAssignedBellRing.IntervalSeconds = bellRing.IntervalSeconds;
            newlyAssignedBellRing.Description = bellRing.Description;
            newlyAssignedBellRing.Type = BellRingType.Special;
            newlyAssignedBellRing.BellRingTime = time;
            newlyAssignedBellRing.Id = Guid.NewGuid().ToString();
            bellRingRepo.InsertOne(newlyAssignedBellRing);
            List<OutputPath> outputsToInsert = new List<OutputPath>();
            outputs.ForEach(x =>
            {
                OutputPath oneOutput = new OutputPath
                {
                    Id = Guid.NewGuid().ToString(),
                    BellRingId = newlyAssignedBellRing.Id,
                    FilePath = x.FilePath,
                    SequenceID = x.SequenceID,
                };
                outputsToInsert.Add(oneOutput);
            });
            outputPathRepo.InsertMultiple(outputsToInsert);
            if (SingleIntersect(newlyAssignedBellRing) || BetweenLessonsIntersect(newlyAssignedBellRing))
            {
                bellRingRepo.Delete(newlyAssignedBellRing);
                throw new Exception($"A bellring must not intersect with any other bellrigns during date {newlyAssignedBellRing.BellRingTime:d}");
            }
        }

        public void UpdateAssignedSequencedBellRing(BellRing bellRing, List<OutputPath> outputPaths)
        {
            if (bellRing == default || outputPaths == default)
            {
                throw new Exception("All parameters must be declared in the body.");
            }
            if (bellRing.Description == null)
            {
                throw new Exception("All sequenced bellrings must have a description to describe their purpose.");
            }
            if (bellRing.Id == null)
            {
                throw new Exception("Update method must get an Id to prefrom the updation.");
            }
            if (outputPaths.Count() == 0)
            {
                throw new Exception("There are no outputs setup for this sequenced bellring.");
            }
            if (!outputPaths.All(output => output.SequenceID > 0))
            {
                throw new Exception("Indexing of the seuqence must start from 1.");
            }
            if (outputPaths.Select(output => output.SequenceID).Distinct().Count() != outputPaths.Count())
            {
                throw new Exception("All of the sequence ID-s must be unique starting from 1.");
            }
            BellRing formerBellring = bellRingRepo.GetOne(bellRing.Id);
            List<OutputPath> formerOutputs = outputPathRepo.GetAll().Where(x => x.BellRingId == bellRing.Id).ToList();
            foreach (var item in formerOutputs)
            {
                outputPathRepo.Delete(item);
            }
            bellRing.Type = BellRingType.Special;
            bellRing.IntervalSeconds = 0;
            bellRingRepo.Update(bellRing.Id, bellRing);
            int i = 1;
            foreach (OutputPath outputPath in outputPaths)
            {
                outputPath.Id = Guid.NewGuid().ToString();
                outputPath.BellRingId = bellRing.Id;
                outputPath.SequenceID = i++;
            }
            outputPathRepo.InsertMultiple(outputPaths.ToList());
            SetBellRingIntervalByPath(bellRing.Id);
            if (SingleIntersect(bellRing) || BetweenLessonsIntersect(bellRing))
            {
                bellRingRepo.Update(formerBellring.Id, formerBellring);
                foreach (var item in outputPaths)
                {
                    outputPathRepo.Delete(item);
                }
                outputPathRepo.InsertMultiple(formerOutputs);
                throw new Exception($"A bellring must not intersect with any other bellrigns during date {bellRing.BellRingTime:d}");
            }
        }

        public void UpdateSequencedBellRing(BellRing bellRing, List<OutputPath> outputPaths)
        {
            if (bellRing == default || outputPaths == default)
            {
                throw new Exception("All parameters must be declared in the body.");
            }
            if (bellRing.Description == null)
            {
                throw new Exception("All sequenced bellrings must have a description to describe their purpose.");
            }
            if (bellRing.Id == null)
            {
                throw new Exception("Update method must get an Id to prefrom the updation.");
            }
            if (readlogic.GetSequencedBellring(bellRing.Id) == null)
            {
                throw new Exception($"Bellring with the description {bellRing.Description} is not a sequenced bellring");
            }
            if (outputPaths.Count() == 0)
            {
                throw new Exception("There are no outputs setup for this sequenced bellring.");
            }
            if (!outputPaths.All(output => output.SequenceID > 0))
            {
                throw new Exception("Indexing of the seuqence must start from 1.");
            }
            if (outputPaths.Select(output => output.SequenceID).Distinct().Count() != outputPaths.Count())
            {
                throw new Exception("All of the sequence ID-s must be unique starting from 1.");
            }
            BellRing formerBellring = bellRingRepo.GetOne(bellRing.Id);
            List<OutputPath> formerOutputs = outputPathRepo.GetAll().Where(x => x.BellRingId == bellRing.Id).ToList();
            foreach (var item in formerOutputs)
            {
                outputPathRepo.Delete(item);
            }
            bellRing.BellRingTime = new DateTime(1, 1, 1); // Assings a static time value to achive the "storing" of a reusable Sequenced bellring
            bellRing.Type = BellRingType.Special;
            bellRing.IntervalSeconds = 0;
            bellRingRepo.Update(bellRing.Id, bellRing);
            int i = 1;
            foreach (OutputPath outputPath in outputPaths)
            {
                outputPath.Id = Guid.NewGuid().ToString();
                outputPath.BellRingId = bellRing.Id;
                outputPath.SequenceID = i++;
            }
            outputPathRepo.InsertMultiple(outputPaths.ToList());
            SetBellRingIntervalByPath(bellRing.Id);
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

        public void InsertTemplateElement(TemplateElement templateElement)
        {
            templateElement.Id = Guid.NewGuid().ToString();
            if (templateElement.FilePath == null)
            {
                templateElement.FilePath = "default.mp3";
            }
            if (templateElement.IntervalSeconds == 0)
            {
                throw new Exception("This parameter should never be zero, it's required.");
            }
            templateElement.Interval = new TimeSpan(0, 0, templateElement.IntervalSeconds);
            templateElementRepo.InsertOne(templateElement);
        }

        public void InsertOutputPath(OutputPath outputPath)
        {
            outputPath.Id = Guid.NewGuid().ToString();
            outputPathRepo.InsertOne(outputPath);
        }

        // Update
        public void UpdateBellRing(string oid, BellRing bellRing)
        {
            bellRingRepo.Update(oid, bellRing);
        }

        // Delete
        public void DeleteBellring(BellRing bellRing)
        {
            switch (bellRing.Type)
            {
                case BellRingType.Start:
                    BellRing nextBellRinging = (from x in bellRingRepo.GetBellRingsForDay(bellRing.BellRingTime.Date)
                                                where x.BellRingTime > bellRing.BellRingTime
                                                orderby x.BellRingTime ascending
                                                select x).FirstOrDefault();
                    if (nextBellRinging != null)
                    {
                        bellRingRepo.Delete(nextBellRinging);
                    }
                    break;

                case BellRingType.End:
                    BellRing previousBellRinging = (from x in bellRingRepo.GetBellRingsForDay(bellRing.BellRingTime.Date)
                                                    where x.BellRingTime < bellRing.BellRingTime
                                                    orderby x.BellRingTime descending
                                                    select x).FirstOrDefault();
                    if (previousBellRinging != null)
                    {
                        bellRingRepo.Delete(previousBellRinging);
                    }
                    break;
            }
            bellRingRepo.Delete(bellRing);
        }

        public void DeleteHoliday(Holiday holiday)
        {
            holidayRepo.Delete(holiday);
        }

        public void DeleteTemplate(Template template)
        {
            templateRepo.Delete(template);
        }

        public void DeleteTemplateElement(TemplateElement templateElement)
        {
            templateElementRepo.Delete(templateElement);
        }

        public void DeleteOutputPath(OutputPath outputPath)
        {
            outputPathRepo.Delete(outputPath);
        }

        public void SetBellRingIntervalByPath(string id)
        {
            if (bellRingRepo.GetOne(id).IntervalSeconds > 0)
            {
                // No exception should be thrown this method should be automated
                // throw new Exception("This Bellring has a static interval, so it shall not be changed.");
                return;
            }
            TimeSpan t = new TimeSpan(0, 0, 0, 0);
            IQueryable<OutputPath> outputPaths = readlogic.GetOutputsForBellRing(id);
            foreach (var item in outputPaths)
            {
                string path =
                Path.Combine(Environment.CurrentDirectory + @"\Output\", item.FilePath);
                if (File.Exists(path))
                {
                    string ct;
                    var x = new FileExtensionContentTypeProvider().TryGetContentType(path, out ct);
                    if (ct == "text/plain")
                    {
                        // TODO: For txt files we should implement a mathematical function to assume an interval. done
                        char[] delims = { '.', '!', '?', ',', '(', ')', '\t', '\n', '\r', ' ' };

                        string[] words = File.ReadAllText(path)
                            .Split(delims, StringSplitOptions.RemoveEmptyEntries);
                        TimeSpan avrageWordsDuration = new TimeSpan(0, 0, 0, 0, words.Length * 650);
                        t += avrageWordsDuration;
                    }
                    else if (ct == "audio/mpeg")
                    {
                        var tfile = TagLib.File.Create(path);
                        TimeSpan duration = tfile.Properties.Duration;
                        t += duration.Add(new TimeSpan(0, 0, 0, 1));
                    }
                }
                else
                {
                    throw new Exception($"Couldn't get file with the name: {item}");
                }
            }
            if (t > new TimeSpan(0, 0, 0, 0))
            {
                BellRing b = bellRingRepo.GetOne(id);
                b.Interval = t;
                bellRingRepo.Update(id, b);
            }
        }

        public void ModifyByTemplate(DateTime dayDate, Template template)
        {
            IQueryable<BellRing> BellringsOfDay = bellRingRepo.GetBellRingsForDay(dayDate);
            IQueryable<TemplateElement> ElementsOfTemplate = readlogic.GetElementsForTemplate(template.Id);
            if (BellringsOfDay == null || ElementsOfTemplate == null)
            {
                throw new Exception("There is no data set for this template and/or day.");
            }
            foreach (var item in BellringsOfDay)
            {
                if (item.Type.Equals(BellRingType.Start) || item.Type.Equals(BellRingType.End))
                {
                    bellRingRepo.Delete(item);
                }
            }

            foreach (var item in ElementsOfTemplate)
            {
                AssignNewBellRingByTemplateElement(item, dayDate);
            }
        }

        public void RemoveAllHolidays()
        {
            IQueryable<Holiday> holidays = holidayRepo.GetAll().Where(x => x.Type == HolidayType.Break || x.Type == HolidayType.Manual);
            if (holidays == null)
            {
                return;
            }
            foreach (var item in holidays)
            {
                IQueryable<BellRing> bellRingsDuringHoliday =
                    bellRingRepo.GetAll().Where(bellring => bellring.BellRingTime >= item.StartTime && item.EndTime >= bellring.BellRingTime);
                if (bellRingsDuringHoliday != null)
                {
                    foreach (var BellRingToBeRemoved in bellRingsDuringHoliday)
                    {
                        bellRingRepo.Delete(BellRingToBeRemoved);
                    }
                }
            }
            IQueryable<Holiday> holidayswithtypeholiday = holidayRepo.GetAll().Where(x => x.Type == HolidayType.Holiday);
            foreach (var item in holidays)
            {
                IQueryable<BellRing> bellRingsDuringHoliday =
                    bellRingRepo.GetAll().Where(bellring =>
                    (bellring.BellRingTime.Month >= item.StartTime.Month &&
                    bellring.BellRingTime.Day >= item.StartTime.Day &&
                    bellring.BellRingTime.Hour >= item.StartTime.Hour &&
                    bellring.BellRingTime.Minute >= item.StartTime.Minute
                    )
                    &&
                    (item.EndTime.Month >= bellring.BellRingTime.Month &&
                    item.EndTime.Day >= bellring.BellRingTime.Day &&
                    item.EndTime.Month >= bellring.BellRingTime.Month &&
                    item.EndTime.Hour >= bellring.BellRingTime.Hour &&
                    item.EndTime.Minute >= bellring.BellRingTime.Minute)
                    );
                if (bellRingsDuringHoliday != null)
                {
                    foreach (var BellRingToBeRemoved in bellRingsDuringHoliday)
                    {
                        bellRingRepo.Delete(BellRingToBeRemoved);
                    }
                }
            }
        }

        public void RemoveByHoliday(string id)
        {
            Holiday h = holidayRepo.GetOne(id);
            if (h == null)
            {
                throw new Exception("Holiday was not found");
            }
            if (h.Type == HolidayType.Holiday)
            {
                IQueryable<BellRing> bellRingsDuringHoliday =
                        bellRingRepo.GetAll().Where(bellring => (
                        bellring.BellRingTime.Month >= h.StartTime.Month &&
                        bellring.BellRingTime.Day >= h.StartTime.Day &&
                        bellring.BellRingTime.Hour >= h.StartTime.Hour &&
                        bellring.BellRingTime.Minute >= h.StartTime.Minute
                    )
                    &&
                    (h.EndTime.Month >= bellring.BellRingTime.Month &&
                    h.EndTime.Day >= bellring.BellRingTime.Day &&
                    h.EndTime.Month >= bellring.BellRingTime.Month &&
                    h.EndTime.Hour >= bellring.BellRingTime.Hour &&
                    h.EndTime.Minute >= bellring.BellRingTime.Minute)
                    );
                if (bellRingsDuringHoliday != null)
                {
                    foreach (var BellRingToBeRemoved in bellRingsDuringHoliday)
                    {
                        bellRingRepo.Delete(BellRingToBeRemoved);
                    }
                }
            }
            else
            {
                IQueryable<BellRing> bellRingsDuringHoliday =
                        bellRingRepo.GetAll().Where(bellring => bellring.BellRingTime >= h.StartTime && h.EndTime >= bellring.BellRingTime);
                if (bellRingsDuringHoliday != null)
                {
                    foreach (var BellRingToBeRemoved in bellRingsDuringHoliday)
                    {
                        bellRingRepo.Delete(BellRingToBeRemoved);
                    }
                }
            }
        }

        // this method should be "locked" from the frontend site to only allow years to be filled by a template with a name like "normal"
        public void ApplyTemplateWithoutFileAssign(Template template, DateTime startDate, DateTime endDate)
        {
            if (template == null)
            {
                throw new Exception($"Template element must be set to fill the range between {startDate} and {endDate}.");
            }
            List<BellRing> bellRingsForADay = new List<BellRing>();
            IList<TemplateElement> ElementsOfTemplate = readlogic.GetElementsForTemplate(template.Id).ToList();
            DeleteBellRingsInRange(startDate, endDate);
            foreach (DateTime day in EachDay(startDate, endDate)) // loops through an entire school year
            {
                if ((int)day.DayOfWeek < 6) // checks if it's a workday Monday->Friday
                {
                    foreach (var item in ElementsOfTemplate)
                    {
                        AssignNewBellRingByTemplateElement(item, day);
                        //InsertBellRing(b);
                    }
                }
            }
        }

        public void ApplyTemplateWithFileAssign(Template template, DateTime startDate, DateTime endDate, string fileName)
        {
            if (template == null)
            {
                throw new Exception($"Template element must be set to fill the range between {startDate} and {endDate}.");
            }
            string path =
               Path.Combine(Environment.CurrentDirectory + @"\Output\", fileName);
            if (!File.Exists(path))
            {
                throw new Exception($"This file: {fileName} does not exist on the server please reupload it.");
            }

            List<BellRing> bellRingsForADay = new List<BellRing>();
            IList<TemplateElement> ElementsOfTemplate = readlogic.GetElementsForTemplate(template.Id).ToList();
            DeleteBellRingsInRange(startDate, endDate);
            foreach (DateTime day in EachDay(startDate, endDate)) // loops through an entire school year
            {
                if ((int)day.DayOfWeek < 6) // checks if it's a workday Monday->Friday
                {
                    foreach (var item in ElementsOfTemplate)
                    {
                        AssignNewBellRingByTemplateElementWithFile(item, day, fileName);
                        //InsertBellRing(b);
                    }
                }
            }
        }

        public void DeleteBellRingsInRange(DateTime startDate, DateTime endDate)
        {
            List<BellRing> inRange = bellRingRepo.GetAll()
                .Where(x => (x.BellRingTime >= startDate && x.BellRingTime <= endDate))
                .ToList();

            BellRing first = inRange.FirstOrDefault();
            if (first != null && first.Type.Equals(BellRingType.End))
            {
                DeleteBellring(first); // If a template deletes a lesson this deletes the start of it when its out of range.
                inRange.Remove(first);
            }
            BellRing last = inRange.LastOrDefault();
            if (last != null && last.Type.Equals(BellRingType.Start))
            {
                DeleteBellring(last); // If a template deletes a lesson this deletes the end of it when its out of range.
                inRange.Remove(last);
            }
            foreach (BellRing bellRing in inRange)
            {
                bellRingRepo.Delete(bellRing);
            }
        }

        private void AssignNewBellRingByTemplateElement(TemplateElement templateElement, DateTime dayDate)
        {
            BellRing b = new BellRing()
            {
                Id = Guid.NewGuid().ToString(),
                Interval = templateElement.Interval,
                Type = templateElement.Type,
                BellRingTime = new DateTime(dayDate.Year, dayDate.Month, dayDate.Day, templateElement.BellRingTime.Hour, templateElement.BellRingTime.Minute, templateElement.BellRingTime.Second),
            };
            bellRingRepo.InsertOne(b);
            OutputPath outputPath = new OutputPath()
            {
                Id = Guid.NewGuid().ToString(),
                FilePath = templateElement.FilePath,
                BellRingId = b.Id,
            };
            outputPathRepo.InsertOne(outputPath);
        }

        private void AssignNewBellRingByTemplateElementWithFile(TemplateElement templateElement, DateTime dayDate, string fileName)
        {
            BellRing b = new BellRing()
            {
                Id = Guid.NewGuid().ToString(),
                Interval = templateElement.Interval,
                Type = templateElement.Type,
                BellRingTime = new DateTime(dayDate.Year, dayDate.Month, dayDate.Day, templateElement.BellRingTime.Hour, templateElement.BellRingTime.Minute, templateElement.BellRingTime.Second),
            };
            bellRingRepo.InsertOne(b);
            OutputPath outputPath = new OutputPath()
            {
                Id = Guid.NewGuid().ToString(),
                FilePath = fileName,
                BellRingId = b.Id,
            };
            outputPathRepo.InsertOne(outputPath);
        }

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}