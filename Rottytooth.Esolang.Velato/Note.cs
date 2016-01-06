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
