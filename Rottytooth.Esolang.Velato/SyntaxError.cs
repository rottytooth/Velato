using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rottytooth.Esolang.Velato
{
    public class SyntaxError : Exception
    {
        public SyntaxError(string message, int interval, int index) 
            : base(message + " " + Parser.GetIntervalName(interval) +
                       "(found at note #" + (index + 1).ToString() + ")")
        { } // adding one to index for 1-based list

        public SyntaxError(string message, int interval, int index, string note)
            : base(message + " " + Parser.GetIntervalName(interval) +
                       "(found at note #" + (index + 1).ToString() + " note value " + note )
        { } // adding one to index for 1-based list

    }
}
