using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskNumberThree.Interfaces;

namespace TaskNumberThree.RealModel
{
    public class Agreement : IAgreement
    {
        public TariffPlan TariffPlan { get; set; }
        public User User { get; private set; }
        public int MobileNumber { get; private set; }
        public DateTime DateOfLastChange { get; set; }

        public Agreement(TariffPlan tariffPlan, User user, int mobileNumber)
        {
            TariffPlan = tariffPlan;
            User = user;
            MobileNumber = mobileNumber;
            DateOfLastChange = DateTime.Now;
        }
    }
}
