using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskNumberThree.RealModel
{
    public class TariffPlan
    {
        public string Name { get; private set; }
        public int CostPerMinute { get; private set; }
        public int CostSMS { get; private set; }
        public int SubscriptionFee { get; private set; }

        public TariffPlan(string name, int costPerMinute, int costSMS, int subscriptionFee)
        {
            Name = name;
            CostPerMinute = costPerMinute;
            CostSMS = costSMS;
            SubscriptionFee = subscriptionFee;
        }
    }
}
