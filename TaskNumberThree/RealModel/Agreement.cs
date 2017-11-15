using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskNumberThree.Interfaces;

namespace TaskNumberThree.RealModel
{
    public class Agreement : IAgreement
    {
        public TariffPlan TariffPlan { get; private set; }
        public User User { get; private set; }
        public int MobileNumber { get; private set; }
        private DateTime DateOfLastChange;

        public Agreement(TariffPlan tariffPlan, User user)
        {
            TariffPlan = tariffPlan;
            User = user;
            MobileNumber = 111;
            DateOfLastChange = DateTime.Now;
        }
        public bool ChangeTariffPlan(TariffPlan newTariffPlan)
        {
            // не забыть проверить на декабрь(12)-январь(1)
            if (DateOfLastChange.Month <= (DateTime.Now.Month - 1))
            {
                TariffPlan = newTariffPlan;
                DateOfLastChange = DateTime.Now;
                Console.WriteLine("Your tariff plan " + "{0}" + " was changed successfully to " + "{1}" + " !"
                    , TariffPlan.Name, newTariffPlan.Name);
                return true;
            }
            else
            {
                Console.WriteLine("Please, wait a month after last changed tariff plan!");
                return false;
            }
        }

    }
}
