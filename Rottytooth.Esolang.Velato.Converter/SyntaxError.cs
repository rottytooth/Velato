using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rottytooth.Esolang.Velato.Converter
{
    public class SyntaxError : Exception
    {
        public SyntaxError(string error, int line, string command)
            : base(error + " at line " + line.ToString() + " | " + command)
        { }
    }
}
