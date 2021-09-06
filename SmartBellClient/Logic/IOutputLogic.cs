using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public interface IOutputLogic
    {
        void TTS(string txt, int intervalSeconds);
        void MP3(string auido,int intervalSeconds);
    }
}
