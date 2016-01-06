using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rottytooth.Esolang.Velato.Tokens
{
    public enum ExpressionType
    {
        Variable,
        Value,
        Equal,
        GreaterThan,
        LessThan,
        Not,
        And,
        Or,

        OpenParanthesis,
        CloseParanthesis,

        Plus,
        Minus,
        Multiply,
        Divide,
        Mod,

        Power,
        Log
    }
}
