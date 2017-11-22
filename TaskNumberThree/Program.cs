using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TaskNumberThree.RealModel;
using TaskNumberThree.VirtualModel;
using TaskNumberThree.Billing;

namespace TaskNumberThree
{
    class Program
    {
        static void Main(string[] args)
        {
            ATS mts = new ATS();
            Billing.Billing billing = new Billing.Billing(mts);
            Terminal myTerminal = mts.NewUserWithTerminal(
                new TariffPlan("Unlim", 11, 4, 20), 
                new User("Denis", "Tarasevich", 20), 298666683);
            Terminal yourTerminal = mts.NewUserWithTerminal(
                new TariffPlan("Unlim", 10, 4, 20),
                new User("Sergey", "Tarasevich", 20), 298840666);
            Terminal yourTerminal2 = mts.NewUserWithTerminal(
                new TariffPlan("Unlim", 10, 4, 20),
                new User("Viachaslau", "Tarasevich", 20), 295399992);
            myTerminal.TurnOn();
            yourTerminal.TurnOn();
            yourTerminal2.TurnOn();
            myTerminal.CreateCall(yourTerminal.MobileNumber);
            Thread.Sleep(1000);
            myTerminal.RejectedCall();
            myTerminal.CreateCall(yourTerminal.MobileNumber);
            Thread.Sleep(1000);
            yourTerminal.RejectedCall();
            yourTerminal2.CreateCall(myTerminal.MobileNumber);
            Thread.Sleep(1000);
            myTerminal.RejectedCall();
            myTerminal.CreateCall(yourTerminal.MobileNumber);
            yourTerminal2.CreateCall(myTerminal.MobileNumber);
            yourTerminal2.TurnOff();
            myTerminal.RejectedCall();
            myTerminal.CreateCall(yourTerminal2.MobileNumber);
            foreach (var item in billing.WithoutSort(myTerminal.MobileNumber))
            {
                Console.WriteLine(item.CallInOut + " " + item.MyMobileNumber + " " + item.AnyMobileNumber + " " + (item.Duration/60) + " min. " + (item.Duration%60) + " s.");
            }
            Console.WriteLine("======================");
            foreach (var item in billing.SortByDate(myTerminal.MobileNumber))
            {
                Console.WriteLine(item.CallInOut + " " + item.MyMobileNumber + " " + item.AnyMobileNumber + " " + item.DateTime);
            }
            Console.WriteLine("======================");
            foreach (var item in billing.SortByToWriteOff(myTerminal.MobileNumber))
            {
                Console.WriteLine(item.CallInOut + " " + item.MyMobileNumber + " " + item.AnyMobileNumber + " " + item.ToWriteOff);
            }
            Console.WriteLine("======================");
            foreach (var item in billing.SortByAnyNumber(myTerminal.MobileNumber))
            {
                Console.WriteLine(item.CallInOut + " " + item.MyMobileNumber + " " + item.AnyMobileNumber + " " + item.AnyMobileNumber);
            }
            Console.WriteLine("======================");
            Console.ReadLine();
        }
    }
}
