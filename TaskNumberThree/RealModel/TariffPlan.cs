using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskNumberThree.RealModel.Auxiliary;

namespace TaskNumberThree.RealModel
{
    public class TariffPlan
    {
        public string Name { get; private set; }
        public int CostPerMinute { get; private set; }
        public int SubscriptionFee { get; private set; }

        public TariffPlan(TariffList anyTariff)
        {
            switch (anyTariff)
            {
                case TariffList.Child:
                {
                    Name = "Детский";
                    CostPerMinute = 5;
                    SubscriptionFee = 12;
                    break;
                }
                case TariffList.Guest:
                {
                    Name = "Гостевой";
                    CostPerMinute = 8;
                    SubscriptionFee = 25;
                    break;
                }
                case TariffList.Onliner:
                {
                    Name = "Onliner.BY";
                    CostPerMinute = 2;
                    SubscriptionFee = 15;
                    break;
                }
                case TariffList.Unlim:
                {
                    Name = "Безлимитище";
                    CostPerMinute = 1;
                    SubscriptionFee = 50;
                    break;
                }
            }
        }
    }
}
