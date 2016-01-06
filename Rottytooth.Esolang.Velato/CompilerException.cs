using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rottytooth.Esolang.Velato
{
    /// <summary>
    /// Internal compilation exception
    /// </summary>
    public class CompilerException : Exception
    {
        public CompilerException(string message) : base(message)
        { }
    }
}
