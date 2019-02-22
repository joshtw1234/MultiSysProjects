using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioControlLib.Structures
{
    class EQSTATE
    {
        // Filter #1 (Low band)

        public double lf;       // Frequency
        public double f1p0;     // Poles ...
        public double f1p1;
        public double f1p2;
        public double f1p3;

        // Filter #2 (High band)

        public double hf;       // Frequency
        public double f2p0;     // Poles ...
        public double f2p1;
        public double f2p2;
        public double f2p3;

        // Sample history buffer

        public double sdm1;     // Sample data minus 1
        public double sdm2;     //                   2
        public double sdm3;     //                   3

        // Gain Controls

        public double lg;       // low  gain
        public double mg;       // mid  gain
        public double hg;       // high gain
    }
}
