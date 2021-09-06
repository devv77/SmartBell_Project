using Data;
using SpeechLib;
using System;
using System.IO;
using System.Threading;
using System.Windows.Media;

namespace Logic
{
    public class OutputLogic : IOutputLogic
    {
        public void TTS(string txt, int intervalSeconds)
        {
            string path = Directory.GetParent(
                Environment.CurrentDirectory).Parent.Parent.FullName + @"\Output" + @$"\{txt}";
            TimeSpan avrageWordsDuration = new TimeSpan();
            char[] delims = { '.', '!', '?', ',', '(', ')', '\t', '\n', '\r', ' ' };

            string[] words = File.ReadAllText(path)
                .Split(delims, StringSplitOptions.RemoveEmptyEntries);
            SpVoice voice = new SpVoice();
            SpObjectTokenCategory otc = new SpObjectTokenCategory();
            // otc.SetId("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Speech\\Voices"); // the original route
            otc.SetId("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Speech_OneCore\\Voices"); // route to access regional voices
            ISpeechObjectTokens tokenEnum = otc.EnumerateTokens();
            int nTokenCount = tokenEnum.Count;
            int i = -1;
            bool found = false;
            while (i < nTokenCount && !found)
            {
                i++;
                try
                {
                    if (tokenEnum.Item(i).GetDescription() == "Microsoft Szabolcs - Hungarian (Hungary)")
                    {
                        found = true;
                    }
                }
                catch (Exception)
                {
                    break;
                }
            }
            if (found)
            {
                voice.Voice = (SpObjectToken)tokenEnum.Item(i);
                voice.Speak(File.ReadAllText(path));
                avrageWordsDuration = new TimeSpan(0, 0, 0, 0, words.Length * 650);
            }
            else
            {
                voice.Speak(File.ReadAllText(path), SpeechVoiceSpeakFlags.SVSFlagsAsync);
                avrageWordsDuration = new TimeSpan(0, 0, 0, 0, words.Length * 750);
            }
    
            /*if (intervalSeconds == 0)
            {
                Thread.Sleep(avrageWordsDuration);
                return;
            }
            Thread.Sleep(TimeSpan.FromSeconds(intervalSeconds));*/
        }

        public void MP3(string audio,int intervalSeconds)
        {
            var tfile = TagLib.File.Create(Directory.GetParent(
                Environment.CurrentDirectory).Parent.Parent.FullName + @"\Output" + @$"\{audio}");
            TimeSpan duration = tfile.Properties.Duration+TimeSpan.FromSeconds(1);
            //TimeSpan duration = tfile.p
            MediaPlayer mplayer = new MediaPlayer();
            mplayer.Open(new Uri(Directory.GetParent(
                Environment.CurrentDirectory).Parent.Parent.FullName + @"\Output" + @$"\{audio}"));
            mplayer.Play();
            if (intervalSeconds==0)
            {
                Thread.Sleep((int)duration.TotalMilliseconds);
                return;
            }
            Thread.Sleep(TimeSpan.FromSeconds(intervalSeconds));
            mplayer.Stop();
        }
        

    }
}
