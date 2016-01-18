using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphUltra.Parts
{
    public class Mark
    {
        double markValue;

        public double MarkValue
        {
            get { return markValue; }
            set { markValue = value; }
        }

        int previousVertice;

        public int PreviousVertice
        {
            get { return previousVertice; }
            set { previousVertice = value; }
        }

        Sign markSign;

        public Sign MarkSign
        {
            get { return markSign; }
            set { markSign = value; }
        }

        public Mark(int markValue, Sign markSign)
        {
            this.markValue = markValue;
            this.markSign = markSign;
        }
    }

    public enum Sign
    {
        Positive,
        Negative
    }
}
