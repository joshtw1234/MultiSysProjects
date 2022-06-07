using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentralModule.Models
{
    class LottoryArgsClass
    {
        private static LottoryArgsClass _instance;
        public static LottoryArgsClass Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new LottoryArgsClass();
                }
                return _instance;
            }
        }
        private LottoryArgsClass() { }

        public void LottoryArgsMethod1(Interface.LottoryArgs args)
        {
            switch(args)
            {
                case Interface.LottoryArgs.Args1:
                    break;
            }

        }
    }
}
