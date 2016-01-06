using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rottytooth.Esolang.Velato.Tokens
{
    public class CommandToken : Token
    {
        public CommandToken() : base()
        {
            ChildCommands = new List<CommandToken>();
        }

        public Note VariableName { get; set; } // used for Declare and Let

        public CommandType CommandType { get; set; } // required for all command types

        public List<CommandToken> ChildCommands { get; set; } // used for block commands

        public Type Type { get; set; } // used for Declare
    }
}
