using System;
using System.Collections.Generic;
using System.Linq;
<<<<<<< HEAD
using System.Threading;
using TaskNumberThree.VirtualModel;
using TaskNumberThree.Interfaces;
using TaskNumberThree.VirtualModel.Auxiliary;
using TaskNumberThree.Billing.Pdf;

namespace TaskNumberThree.Billing
{
    public class Billing : IGetDetalisation
    {
        private readonly ICanCall _callInfo;

        public Billing(ICanCall callInfo)
        {
            _callInfo = callInfo;
            TimerFor25();
        }

        public void TimerFor25()
        {
            uint toNextDay = (uint)(24 - DateTime.Now.Hour);
            uint to25Day;
            if (DateTime.Now.Day < 25)
            {
                to25Day = (uint)(25 - (DateTime.Now.Day + 1));
            }
            else if (DateTime.Now.Day != 31)
            {
                to25Day = (uint)(25 + (31 - (DateTime.Now.Day + 1)));
            }
            else
            {
                to25Day = (uint)(25 + (31 - (DateTime.Now.Day)));
            }
            uint result = ((toNextDay * 3600000) + (to25Day * 86400000));
            TimerCallback timerCallBack = new TimerCallback(CreditMethod);
            Timer timer = new Timer(timerCallBack, _callInfo.GetUsersInfo(), result, 2592000000);
            Console.WriteLine("Billing waiting 25 day of month. Wait at {0} days and {1} hours", to25Day, toNextDay);
            Program.Spaces();
        }
        public void CreditMethod(object obj)
        {
            foreach (var item in (IDictionary<int, Tuple<IAgreement, Port>>)obj)
            {
                if (item.Value.Item1.User.Balance < 0)
                {
                    item.Value.Item2.Status = PortStatus.Blocked;
                }
            }
        }

        public void AddBalance(int mobileNumber, int addMoney)
        {
            var addBalanceUser = _callInfo.GetUsersInfo();
            addBalanceUser[mobileNumber].Item1.User.AddBalance(addMoney);
            Console.WriteLine("Thanks for pay! Your balance is {0}", addBalanceUser[mobileNumber].Item1.User.Balance);
            if (addBalanceUser[mobileNumber].Item1.User.Balance > 0 && addBalanceUser[mobileNumber].Item2.Status == PortStatus.Blocked)
            {
                addBalanceUser[mobileNumber].Item2.Status = PortStatus.Disabled;
                Console.WriteLine("Thanks for pay! You can create call, dear {0}", mobileNumber);
            }
        }

        public void GetPdfSortByDate(int mobileNumber)
        {
            var newList = GetDetalisation(mobileNumber).ToList();
            PdfCreator pdfCreator = new PdfCreator();
            pdfCreator.PdfCreate(newList, String.Format("SortByDate{0}", mobileNumber));
        }
        public void GetPdfSortByToWriteOff(int mobileNumber)
        {
            var newList = GetDetalisation(mobileNumber).OrderBy(x => x.ToWriteOff).ToList();
            IPdfCreator pdfCreator = new PdfCreator();
            pdfCreator.PdfCreate(newList, String.Format("SortByToWriteOff{0}", mobileNumber));
        }
        public void GetPdfSortByAnyNumber(int mobileNumber)
        {
            var newList = GetDetalisation(mobileNumber).OrderBy(x => x.AnyMobileNumber).ToList();
            IPdfCreator pdfCreator = new PdfCreator();
            pdfCreator.PdfCreate(newList, String.Format("SortByAnyNumber{0}", mobileNumber));
=======
using System.Text;
using TaskNumberThree.VirtualModel;
using TaskNumberThree.Interfaces;

namespace TaskNumberThree.Billing
{
    public class Billing
    {
        private readonly ISaveInfo<CallInfo> _callInfo;

        public Billing(ISaveInfo<CallInfo> callInfo)
        {
            _callInfo = callInfo;
>>>>>>> ee681e88e8b7804b0cab92122c87b03b9ed79be4
        }

        public ICollection<CallString> GetDetalisation(int mobileNumber)
        {
            ICollection<CallString> callStrings = new List<CallString>();
<<<<<<< HEAD
            var getMyCalls = _callInfo.GetCallInfo().
=======
            var getMyCalls = _callInfo.GetInfo().
>>>>>>> ee681e88e8b7804b0cab92122c87b03b9ed79be4
                Where(x => x.MobileNumber == mobileNumber || x.TargetMobileNumber == mobileNumber).ToList();
            foreach (var item in getMyCalls)
            {
                CallInOut callInOut;
                int myMobile;
                int anyMobileNumber;
                if (item.MobileNumber == mobileNumber)
                {
                    callInOut = CallInOut.Outgoing;
                    myMobile = item.MobileNumber;
                    anyMobileNumber = item.TargetMobileNumber;
                }
                else
                {
                    callInOut = CallInOut.Incoming;
                    myMobile = item.TargetMobileNumber;
                    anyMobileNumber = item.MobileNumber;
                }
                callStrings.Add(new CallString(myMobile, anyMobileNumber, item.StartCall, TimeSpan.FromTicks((item.FinishCall - item.StartCall).Ticks).TotalSeconds, item.ToWriteOff, callInOut));
            }
            return callStrings;
        }

        public IEnumerable<CallString> WithoutSort(int mobileNumber)
        {
            var newList = GetDetalisation(mobileNumber);
            return newList.ToList();
        }

        public IEnumerable<CallString> SortByDate(int mobileNumber)
        {
            var newList = GetDetalisation(mobileNumber);
            return newList.OrderBy(x => x.DateTime).ToList();
        }

        public IEnumerable<CallString> SortByToWriteOff(int mobileNumber)
        {
            var newList = GetDetalisation(mobileNumber);
            return newList.OrderBy(x => x.ToWriteOff).ToList();
        }

        public IEnumerable<CallString> SortByAnyNumber(int mobileNumber)
        {
            var newList = GetDetalisation(mobileNumber);
            return newList.OrderBy(x => x.AnyMobileNumber).ToList();
        }
    }
}
