using System;
using System.Threading;
using TaskNumberThree.RealModel;
using TaskNumberThree.VirtualModel;
using TaskNumberThree.RealModel.Auxiliary;

namespace TaskNumberThree
{
    class Program
    {
        static void Main(string[] args)
        {
            ATS mts = new ATS();
            Billing.Billing billing = new Billing.Billing(mts);
            Terminal myTerminal = mts.NewUserWithTerminal(
                new TariffPlan(TariffList.Onliner), 
                new User("Denis", "Tarasevich", -5), 291111111);
            Terminal yourTerminal = mts.NewUserWithTerminal(
                new TariffPlan(TariffList.Child),
                new User("Sergey", "Tarasevich", 20), 292222222);
            Terminal herTerminal = mts.NewUserWithTerminal(
                new TariffPlan(TariffList.Unlim),
                new User("Ina", "Tarasevich", 20), 293333333);
            myTerminal.TurnOn();
            yourTerminal.TurnOn();
            herTerminal.TurnOn();
            myTerminal.DontHaveMoney();

            myTerminal.CreateCall(yourTerminal.MobileNumber);
            billing.AddBalance(myTerminal.MobileNumber, 20);

            myTerminal.CreateCall(yourTerminal.MobileNumber);
            Thread.Sleep(1100);
            myTerminal.RejectedCall();
            Spaces();

            myTerminal.CreateCall(yourTerminal.MobileNumber);
            Thread.Sleep(800);
            yourTerminal.RejectedCall();
            Spaces();

            yourTerminal.CreateCall(myTerminal.MobileNumber);
            Thread.Sleep(1200);
            myTerminal.RejectedCall();
            Spaces();

            herTerminal.CreateCall(myTerminal.MobileNumber);
            Thread.Sleep(1200);
            herTerminal.RejectedCall();
            Spaces();

            // dont answer
            herTerminal.CreateCall(myTerminal.MobileNumber);
            Spaces();

            myTerminal.CreateCall(yourTerminal.MobileNumber);
            herTerminal.CreateCall(myTerminal.MobileNumber);
            herTerminal.TurnOff();
            myTerminal.RejectedCall();
            myTerminal.CreateCall(herTerminal.MobileNumber);
            Spaces();

            myTerminal.ChangeTariffPlan(new TariffPlan(TariffList.Guest));
            Spaces();

            billing.GetPdfSortByDate(myTerminal.MobileNumber);
            billing.GetPdfSortByToWriteOff(myTerminal.MobileNumber);
            billing.GetPdfSortByAnyNumber(myTerminal.MobileNumber);
            billing.GetPdfSortByDate(yourTerminal.MobileNumber);
            billing.GetPdfSortByDate(herTerminal.MobileNumber);

            Console.ReadLine();
        }

        public static void Spaces()
        {
            Console.WriteLine();
            Console.WriteLine("=================================");
            Console.WriteLine();
        }
    }
}
