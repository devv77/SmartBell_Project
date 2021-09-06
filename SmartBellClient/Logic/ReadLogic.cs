using Data;
using Newtonsoft.Json;
using Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Logic
{
    public class ReadLogic : IReadLogic
    {
        private static string outputFolder = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName+@"\Output");
        public void GetAllFiles(IList<OutputPath> outputs)
        {
            foreach (var item in outputs.GroupBy(test => test.FilePath).Select(grp => grp.First()))
            {
                string url = ConstConfig.DomainAddress + @$"/File/{item.FilePath}";
                WebClient wc = new WebClient();
                if (!File.Exists(Path.Combine(outputFolder, item.FilePath)))
                {
                    wc.DownloadFile(url, Path.Combine(outputFolder, item.FilePath));
                }
            }   
        }

        public IList<BellRing> GetBellRingsForDay(DateTime dayDate)
        {
            var jsonDate = dayDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");

            string url = ConstConfig.DomainAddress + $"/Client/GetBellRingsForDay/{jsonDate}";
            WebClient wc = new WebClient();
            string jsonContent = wc.DownloadString(url);
            return JsonConvert.DeserializeObject<List<BellRing>>(jsonContent);

        }

        public IList<OutputPath> GetAllOutputPathsForDay(DateTime dayDate)
        {
            string url = ConstConfig.DomainAddress + @$"/Client/GetAllOutputPathsForDay/{dayDate}";
            WebClient wc = new WebClient();
            string jsonContent = wc.DownloadString(url);
            return JsonConvert.DeserializeObject<List<OutputPath>>(jsonContent);
        }
        public IList<OutputPath> GetOutputPathsForBellRing(string id, List<OutputPath> outputPaths)
        {
            return outputPaths.Where(x => x.BellRingId == id).OrderBy(x=>x.SequenceID).ToList();
        }
    }
}
