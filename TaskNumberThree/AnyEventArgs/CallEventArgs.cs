using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskNumberThree.Interfaces;

namespace TaskNumberThree.AnyEventArgs
{
    public class CallEventArgs : EventArgs, ICreateCall
    {
        public int MobileNumber { get; private set; }
        public int TargetMobileNumber { get; private set; }

        public CallEventArgs(int mobileNumber, int targetMobileNumber)
        {
            MobileNumber = mobileNumber;
            TargetMobileNumber = targetMobileNumber;
        }
    }
}
