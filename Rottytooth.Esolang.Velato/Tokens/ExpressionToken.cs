using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rottytooth.Esolang.Velato.Tokens
{
    public class ExpressionToken : Token
    {
        public ExpressionToken() : base() { }

        public Note VariableName { get; set; }

        public ExpressionType ExpressionType { get; set; } // required for all expression types

        public Type Type { get; set; } // used for literals

        public int IntValue { get; set; }

        public double DoubleValue { get; set; }

        public char CharValue { get; set; }
    }
}
