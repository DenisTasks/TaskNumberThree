using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskNumberThree.RealModel;
using TaskNumberThree.VirtualModel;

namespace TaskNumberThree
{
    class Program
    {
        static void Main(string[] args)
        {
            ATS mts = new ATS();
            Terminal myTerminal = mts.NewUserWithTerminal(
                new TariffPlan("Unlim", 10, 4, 20), 
                new User("Denis", "Tarasevich"), 298666683);
            Terminal yourTerminal = mts.NewUserWithTerminal(
                new TariffPlan("Unlim", 10, 4, 20),
                new User("Sergey", "Tarasevich"), 298840666);
            myTerminal.TurnOn();
            yourTerminal.TurnOn();
            myTerminal.CreateCall(yourTerminal.MobileNumber);
        }
    }
}
