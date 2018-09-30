using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFMVVM.Models
{
    public class WPFMVVMModel : IWPFMVVMModel
    {
    }

    public class SliderDataContext
    {
        public double CoreMax { get; set; }
        public double CoreMin { get; set; }
        public double CoreTick { get; set; }
        public string CoreTitle { get; set; }
        public double CoreValue { get; set; }
    }
}
