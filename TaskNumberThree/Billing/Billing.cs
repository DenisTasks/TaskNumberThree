using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        public ICollection<CallString> GetDetalisation(int mobileNumber)
        {
            ICollection<CallString> callStrings = new List<CallString>();
            var getMyCalls = _callInfo.GetInfo().
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
