using GalaSoft.MvvmLight;
using System;

namespace Data
{
    public enum BellRingType
    {
        Start,
        End,
        Special
    }
    public class BellRing:ObservableObject
    {
        private string id;
        private string description;
        private DateTime bellRingTime;
        private TimeSpan interval;
        private int intervalSeconds;
        private BellRingType type;
        public string Id
        { 
            get { return this.id; } set { this.Set(ref this.id, value); }
        }

        public string Description
        {
            get { return this.description; }
            set { this.Set(ref this.description, value); }
        }
        public DateTime BellRingTime
        {
            get { return this.bellRingTime; }
            set { this.Set(ref this.bellRingTime, value); }
        }
        public TimeSpan Interval
        {
            get { return this.interval; }
            set { this.Set(ref this.interval, value); }
        }
        public int IntervalSeconds
        {
            get { return this.intervalSeconds; }
            set { this.Set(ref this.intervalSeconds, value); }
        }
        public BellRingType Type
        {
            get { return this.type; }
            set { this.Set(ref this.type, value); }
        }
    }
}
