using Rottytooth.Esolang.Velato.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rottytooth.Esolang.Velato
{
    public class Note
    {
        public int RawNumber { get; set; }

        public string Name { get; set; }

        public double Pitch { get; set; }

        public string Var { get
            {
                string name = this.Name + this.Number.ToString();
                name = name.Replace("#", "s");
                return name;
            } 
        }

        public double PitchCorrection { get; set; }

        public int PitchCorrectionUnits { get; set; }

        public double Number
        {
            get
            {
                return Convert.ToDouble(RawNumber) + PitchCorrection;
            }
        }
    }
}
