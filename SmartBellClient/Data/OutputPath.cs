using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class OutputPath:ObservableObject
    {
        private string id;
        private string filePath;
        private int sequenceId;
        private string bellRingId;
        public string Id
        {
            get { return this.id; }
            set { this.Set(ref this.id, value); }
        }

        public string FilePath
        {
            get { return this.filePath; }
            set { this.Set(ref this.filePath, value); }
        }

        public int SequenceID
        {
            get { return this.sequenceId; }
            set { this.Set(ref this.sequenceId, value); }
        }

        public string BellRingId
        {
            get { return this.bellRingId; }
            set { this.Set(ref this.bellRingId, value); }
        }

    }
}
