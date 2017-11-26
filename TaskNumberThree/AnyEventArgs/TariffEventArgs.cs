using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskNumberThree.RealModel;

namespace TaskNumberThree.AnyEventArgs
{
    public class TariffEventArgs: EventArgs
    {
        public int MobileNumber { get; private set; }
        public TariffPlan TariffPlan { get; private set; }

        public TariffEventArgs(int mobileNumber, TariffPlan tariffPlan)
        {
            MobileNumber = mobileNumber;
            TariffPlan = tariffPlan;
        }
    }
}
