using CommonServiceLocator;
using Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace SmartBellClient.VM
{
    internal class MainViewModel : ViewModelBase
    {
        private TimeSpan autoUpdateTime = new TimeSpan(6, 00, 00);
        private System.Threading.Timer timer;
        private System.Threading.Timer UpdateTimer;
        DispatcherTimer clockTimer;
        private IReadLogic readLogic;
        private ITimeLogic timeLogic;
        private IOutputLogic outputLogic;

        public ICommand UpdateCmd { get; private set; }
        private string bellRingInfo;
        public string BellRingInfo
        {
            get { return bellRingInfo; }
            private set { Set(ref this.bellRingInfo, value); }
        }
        private ObservableCollection<BellRing> bellRings;
        public ObservableCollection<BellRing> BellRings
        {
            get { return bellRings; }
            private set { Set(ref this.bellRings, value); }
        }
        public ObservableCollection<OutputPath> Outputs { get; private set; }
        private BellRing nextBellRing;
        public BellRing NextBellRing
        {
            get { return nextBellRing; }
            private set { Set(ref this.nextBellRing, value); }
        }
        private DateTime clock;
        public DateTime Clock {
            get { return clock; }
            private set { Set(ref this.clock,value); }
        }
        public MainViewModel(IReadLogic readLogic,ITimeLogic timeLogic,IOutputLogic outputLogic)
        {
            this.readLogic = readLogic;
            this.timeLogic = timeLogic;
            this.outputLogic = outputLogic;

            if (!this.IsInDesignMode)
            {
                Clock = timeLogic.GetNetworkTime();
                startClock();
                //BellRingInfo = "Next bellring time:";
                NextBellRing = timeLogic.GetNextBellringFromServer(Clock);
                this.BellRings = new ObservableCollection<BellRing>(readLogic.GetBellRingsForDay(Clock.Date).OrderBy(x => x.BellRingTime));
                this.BellRings = new ObservableCollection<BellRing>(timeLogic.RemoveAllElapsedBellRings(Clock,BellRings.ToList()).ToList());
                this.Outputs = new ObservableCollection<OutputPath>(readLogic.GetAllOutputPathsForDay(Clock.Date));
                readLogic.GetAllFiles(Outputs);
                BellRingUpdate(NextBellRing);
                SetUpTimer(autoUpdateTime);

                this.UpdateCmd = new RelayCommand(() => UpdateEverything(),true);
            }
            else
            {
                this.BellRings = FillWithSamples();
                BellRingInfo = "Next bellring time:";
                NextBellRing = new BellRing(){
                    BellRingTime = new DateTime(2069, 04, 20, 12, 34, 56), //This is the only part wich is displayed for the moment
                };
                Clock = new DateTime(1999, 07, 18, 12, 34, 56);
            }
        }
        public MainViewModel()
            : this(IsInDesignModeStatic ? null : ServiceLocator.Current.GetInstance<IReadLogic>(),
                  IsInDesignModeStatic ? null : ServiceLocator.Current.GetInstance<ITimeLogic>(),
                  IsInDesignModeStatic ? null : ServiceLocator.Current.GetInstance<IOutputLogic>()) // IoC
        {
        }
        private void SetUpTimer(TimeSpan alertTime)
        {
            DateTime current = Clock;
            TimeSpan timeToGo = alertTime - current.TimeOfDay;
            if (timeToGo < TimeSpan.Zero)
            {
                return;//time already passed
            }
            this.UpdateTimer = new System.Threading.Timer(x =>
            {
                this.UpdateEverything();
            }, null, timeToGo, Timeout.InfiniteTimeSpan);
        }

        private void UpdateEverything()
        {
            clockTimer.Stop();
            if (timer != null)
            {
                timer.Dispose();
            }
            Clock = timeLogic.GetNetworkTime();
            clockTimer.Start();
            //BellRingInfo = "Next bellring time:";
            NextBellRing = timeLogic.GetNextBellringFromServer(Clock);
            this.BellRings = new ObservableCollection<BellRing>(readLogic.GetBellRingsForDay(Clock.Date).OrderBy(x => x.BellRingTime));
            this.BellRings = new ObservableCollection<BellRing>(timeLogic.RemoveAllElapsedBellRings(Clock, BellRings.ToList()).ToList());
            this.Outputs = new ObservableCollection<OutputPath>(readLogic.GetAllOutputPathsForDay(Clock.Date));
            readLogic.GetAllFiles(Outputs);
            BellRingUpdate(NextBellRing);
            SetUpTimer(autoUpdateTime);
        }
        private static ObservableCollection<BellRing> FillWithSamples()
        {
            ObservableCollection<BellRing> brings = new ObservableCollection<BellRing>();
            BellRing br1 = new BellRing()
            {
                Description = "Automatic generated description",
                BellRingTime = new DateTime(1, 1, 1, 8, 0, 0),
                Interval = new TimeSpan(0, 0, 10),
                Type = BellRingType.Start,
            };
            BellRing br2 = new BellRing()
            {
                Description = "Automatic generated description",
                BellRingTime = new DateTime(1, 1, 1, 8, 45, 0),
                Interval = new TimeSpan(0, 0, 10),
                Type = BellRingType.End,
            };
            BellRing br3 = new BellRing()
            {
                Description = "Automatic generated description",
                BellRingTime = new DateTime(1, 1, 1, 8, 55, 0),
                Interval = new TimeSpan(0, 0, 10),
                Type = BellRingType.Start,
            };
            BellRing br4 = new BellRing()
            {
                Description = "Automatic generated description",
                BellRingTime = new DateTime(1, 1, 1, 9, 40, 0),
                Interval = new TimeSpan(0, 0, 10),
                Type = BellRingType.End,
            };
            BellRing br5 = new BellRing()
            {
                Description = "Automatic generated description",
                BellRingTime = new DateTime(1, 1, 1, 9, 55, 0),
                Interval = new TimeSpan(0, 0, 10),
                Type = BellRingType.Start,
            };
            BellRing br6 = new BellRing()
            {
                Description = "Automatic generated description",
                BellRingTime = new DateTime(1, 1, 1, 10, 40, 0),
                Interval = new TimeSpan(0, 0, 10),
                Type = BellRingType.End,
            };
            BellRing br7 = new BellRing()
            {
                Description = "Automatic generated description",
                BellRingTime = new DateTime(1, 1, 1, 10, 50, 0),
                Interval = new TimeSpan(0, 0, 10),
                Type = BellRingType.Start,
            };
            BellRing br8 = new BellRing()
            {
                Description = "Automatic generated description",
                BellRingTime = new DateTime(1, 1, 1, 11, 35, 0),
                Interval = new TimeSpan(0, 0, 10),
                Type = BellRingType.End,
            };
            BellRing br9 = new BellRing()
            {
                Description = "Automatic generated description",
                BellRingTime = new DateTime(1, 1, 1, 11, 55, 0),
                Interval = new TimeSpan(0, 0, 10),
                Type = BellRingType.Start,
            };
            BellRing br10 = new BellRing()
            {
                Description = "Automatic generated description",
                BellRingTime = new DateTime(1, 1, 1, 12, 40, 0),
                Interval = new TimeSpan(0, 0, 10),
                Type = BellRingType.End,
            };
            BellRing br11 = new BellRing()
            {
                Description = "Automatic generated description",
                BellRingTime = new DateTime(1, 1, 1, 12, 50, 0),
                Interval = new TimeSpan(0, 0, 10),
                Type = BellRingType.Start,
            };
            BellRing br12 = new BellRing()
            {
                Description = "Automatic generated description",
                BellRingTime = new DateTime(1, 1, 1, 13, 35, 0),
                Interval = new TimeSpan(0, 0, 10),
                Type = BellRingType.End,
            };
            BellRing br13 = new BellRing()
            {
                Description = "Automatic generated description",
                BellRingTime = new DateTime(1, 1, 1, 13, 40, 0),
                Interval = new TimeSpan(0, 0, 10),
                Type = BellRingType.Start,
            };
            BellRing br14 = new BellRing()
            {
                Description = "Automatic generated description",
                BellRingTime = new DateTime(1, 1, 1, 14, 25, 0),
                Interval = new TimeSpan(0, 0, 10),
                Type = BellRingType.End,
            };
            BellRing br15 = new BellRing()
            {
                Description = "Automatic generated description",
                BellRingTime = new DateTime(1, 1, 1, 14, 35, 0),
                Interval = new TimeSpan(0, 0, 10),
                Type = BellRingType.Start,
            };
            BellRing br16 = new BellRing()
            {
                Description = "Automatic generated description",
                BellRingTime = new DateTime(1, 1, 1, 15, 20, 0),
                Interval = new TimeSpan(0, 0, 10),
                Type = BellRingType.End,
            };
            BellRing br17 = new BellRing()
            {
                Description = "Automatic generated description",
                BellRingTime = new DateTime(1, 1, 1, 15, 25, 0),
                Interval = new TimeSpan(0, 0, 10),
                Type = BellRingType.Start,
            };
            BellRing br18 = new BellRing()
            {
                Description = "Automatic generated description",
                BellRingTime = new DateTime(1, 1, 1, 16, 10, 0),
                Interval = new TimeSpan(0, 0, 10),
                Type = BellRingType.End,
            };
            brings.Add(br1);
            brings.Add(br2);
            brings.Add(br3);
            brings.Add(br4);
            brings.Add(br5);
            brings.Add(br6);
            brings.Add(br7);
            brings.Add(br8);
            brings.Add(br9);
            brings.Add(br10);
            brings.Add(br11);
            brings.Add(br12);
            brings.Add(br13);
            brings.Add(br14);
            brings.Add(br15);
            brings.Add(br16);
            brings.Add(br17);
            brings.Add(br18);
            return brings;
        }
        private void BellRingUpdate(BellRing nextbellring)
        {
            BellRingInfo = "Next bellring time:";
            if (nextbellring == null)
            {
                BellRingInfo = "No more bellrings for " + Clock.Date.ToString("yyyy:MM:dd");
                if ((timeLogic.GetNextBellringFromServer(Clock))!=null) // If the server's data is mismatched with local update it.
                {
                    UpdateEverything();
                }
                return;
            }
            DateTime calledForTime = Clock;
            TimeSpan scheduledTime = new TimeSpan
                (nextbellring.BellRingTime.Hour, nextbellring.BellRingTime.Minute,
                nextbellring.BellRingTime.Second)-clock.TimeOfDay;
            this.timer = new System.Threading.Timer(x =>
            {
                this.PlayAudio();
            }, null, scheduledTime, Timeout.InfiniteTimeSpan);
        }
        private void PlayAudio()
        {
            BellRingInfo = "Current bellringing:";
            List<OutputPath> currentOutputs = readLogic.GetOutputPathsForBellRing(NextBellRing.Id, Outputs.ToList()).ToList();
            foreach (var item in currentOutputs)
            {
                //there should be a check whether it's an mp3 or txt 
                /*MediaPlayer mplayer = new MediaPlayer();
                mplayer.Open(new Uri(Directory.GetParent(
                    Environment.CurrentDirectory).Parent.Parent.FullName + @"\Output" + @$"\default.mp3"));
                mplayer.Play();*/
                if (item.FilePath.Substring(item.FilePath.LastIndexOf('.') + 1).ToLower() == "mp3")
                {
                    if (currentOutputs.Count==1)
                    {
                        outputLogic.MP3(item.FilePath, NextBellRing.IntervalSeconds);
                    }
                    else
                    {
                        outputLogic.MP3(item.FilePath,0);
                    }
                }
                else if(item.FilePath.Substring(item.FilePath.LastIndexOf('.') + 1).ToLower() == "txt")
                {
                    if (currentOutputs.Count == 1)
                    {
                        outputLogic.TTS(item.FilePath, NextBellRing.IntervalSeconds);
                    }
                    else
                    {
                        outputLogic.TTS(item.FilePath,0);
                    }
                }
            }
            //outputLogic.MP3(item.FilePath);
            
            BellRings = new ObservableCollection<BellRing>(timeLogic.RemoveElapsedBellRing(NextBellRing.Id, BellRings.ToList()));
            this.NextBellRing = this.timeLogic.GetNextBellringFromList(Clock.AddSeconds(5),BellRings.ToList());
            BellRingUpdate(NextBellRing);
            
        }
        private void startClock()
        {
            clockTimer = new DispatcherTimer();
            clockTimer.Interval = TimeSpan.FromSeconds(1);
            clockTimer.Tick += this.TicksOfClock;
            clockTimer.Start();
        }
        private void TicksOfClock(object sender, EventArgs e)
        {
            this.Clock = this.Clock.AddSeconds(1);
        }

    }
}
