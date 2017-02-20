using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rottytooth.Esolang.Velato.Tokens
{
    public enum CommandType
    {
        Let,
        Declare,
        While,
        If,
        Print,
        Input,
        EndWhile,
        EndIf,
        Else
    }
}
