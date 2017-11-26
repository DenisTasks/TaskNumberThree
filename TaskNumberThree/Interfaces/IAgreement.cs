using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskNumberThree.RealModel;

namespace TaskNumberThree.Interfaces
{
    public interface IAgreement
    {
        TariffPlan TariffPlan { get; set; }
        User User { get; }
        int MobileNumber { get; }
        DateTime DateOfLastChange { get; set; }
    }
}
