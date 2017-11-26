using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskNumberThree.VirtualModel.Auxiliary
{
    public class CallString
    {
        public int MyMobileNumber { get; private set; }
        public int AnyMobileNumber { get; private set; }
        public DateTime DateTime { get; private set; }
        public int Duration { get; private set; }
        public double ToWriteOff { get; private set; }
        public CallInOut CallInOut { get; private set; }

        public CallString(int myMobileNumber, int anyMobileNumber, DateTime dateTime, double duration, double toWriteOff, CallInOut callInOut)
        {
            MyMobileNumber = myMobileNumber;
            AnyMobileNumber = anyMobileNumber;
            DateTime = dateTime;
            Duration = (int)duration;
            ToWriteOff = toWriteOff;
            CallInOut = callInOut;
        }
    }
}
