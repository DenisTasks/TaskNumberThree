using System;
using System.Threading;
using TaskNumberThree.RealModel;
using TaskNumberThree.VirtualModel;
<<<<<<< HEAD
using TaskNumberThree.RealModel.Auxiliary;
=======
using TaskNumberThree.Billing;
>>>>>>> ee681e88e8b7804b0cab92122c87b03b9ed79be4

namespace TaskNumberThree
{
    class Program
    {
        static void Main(string[] args)
        {
            ATS mts = new ATS();
            Billing.Billing billing = new Billing.Billing(mts);
            Terminal myTerminal = mts.NewUserWithTerminal(
<<<<<<< HEAD
                new TariffPlan(TariffList.Onliner), 
                new User("Denis", "Tarasevich", -5), 291111111);
            Terminal yourTerminal = mts.NewUserWithTerminal(
                new TariffPlan(TariffList.Child),
                new User("Sergey", "Tarasevich", 20), 292222222);
            Terminal herTerminal = mts.NewUserWithTerminal(
                new TariffPlan(TariffList.Unlim),
                new User("Ina", "Tarasevich", 20), 293333333);
=======
                new TariffPlan("Unlim", 11, 4, 20), 
                new User("Denis", "Tarasevich", 20), 298666683);
            Terminal yourTerminal = mts.NewUserWithTerminal(
                new TariffPlan("Unlim", 10, 4, 20),
                new User("Sergey", "Tarasevich", 20), 298840666);
            Terminal yourTerminal2 = mts.NewUserWithTerminal(
                new TariffPlan("Unlim", 10, 4, 20),
                new User("Viachaslau", "Tarasevich", 20), 295399992);
>>>>>>> ee681e88e8b7804b0cab92122c87b03b9ed79be4
            myTerminal.TurnOn();
            yourTerminal.TurnOn();
            herTerminal.TurnOn();
            myTerminal.DontHaveMoney();

            myTerminal.CreateCall(yourTerminal.MobileNumber);
            billing.AddBalance(myTerminal.MobileNumber, 20);

            myTerminal.CreateCall(yourTerminal.MobileNumber);
            Thread.Sleep(1100);
            myTerminal.RejectedCall();
<<<<<<< HEAD
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
=======
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
>>>>>>> ee681e88e8b7804b0cab92122c87b03b9ed79be4
        }
    }
}
