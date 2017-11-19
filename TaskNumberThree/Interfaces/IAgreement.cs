using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskNumberThree.RealModel;

namespace TaskNumberThree.Interfaces
{
    public interface IAgreement
    {
        TariffPlan TariffPlan { get; }
        User User { get; }
        int MobileNumber { get; }
        bool ChangeTariffPlan(TariffPlan tariffPlan);
    }
}
