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
<<<<<<< HEAD
=======
        public bool ChangeTariffPlan(TariffPlan newTariffPlan)
        {
            // не забыть проверить на декабрь(12)-январь(1)
            if (DateOfLastChange.Month <= (DateTime.Now.Month - 1))
            {
                TariffPlan = newTariffPlan;
                DateOfLastChange = DateTime.Now;
                Console.WriteLine("Your tariff plan {0} was changed successfully to {1} !"
                    , TariffPlan.Name, newTariffPlan.Name);
                return true;
            }
            else
            {
                Console.WriteLine("Please, wait a month after last change of your tariff plan!");
                return false;
            }
        }

>>>>>>> ee681e88e8b7804b0cab92122c87b03b9ed79be4
    }
}
