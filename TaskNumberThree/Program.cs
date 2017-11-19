using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
                new User("Denis", "Tarasevich", 10), 298666683);
            Terminal yourTerminal = mts.NewUserWithTerminal(
                new TariffPlan("Unlim", 10, 4, 20),
                new User("Sergey", "Tarasevich", 10), 298840666);
            Terminal yourTerminal2 = mts.NewUserWithTerminal(
                new TariffPlan("Unlim", 10, 4, 20),
                new User("Viachaslau", "Tarasevich", 10), 295399992);
            myTerminal.TurnOn();
            yourTerminal.TurnOn();
            yourTerminal2.TurnOn();
            myTerminal.CreateCall(yourTerminal.MobileNumber);
            Thread.Sleep(1000);
            myTerminal.RejectedCall();
            yourTerminal2.CreateCall(myTerminal.MobileNumber);
            yourTerminal.CreateCall(myTerminal.MobileNumber);
        }
    }
}
