using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Repository
{
    public static class ConstConfig
    {
        private static string filepath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "setup.xml");
        public static string DomainAddress { get
            {
                return XDocument.Load(filepath).Element("setup").Element("domain").Value+":"+ XDocument.Load(filepath).Element("setup").Element("port").Value;
            }
        }
        public static string NtpAddress{ get
            {
                return XDocument.Load(filepath).Element("setup").Element("ntp").Value;
            } 
        }

        public static double Width = 500;
        public static double Height = 500;
    }
}
