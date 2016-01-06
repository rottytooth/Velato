using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rottytooth.Esolang.Velato.Tokens
{
    public abstract class Token
    {
        public List<ExpressionToken> ChildExpressions { get; set; }

        protected Token()
        {
            ChildExpressions = new List<ExpressionToken>();
        }
    }
}
