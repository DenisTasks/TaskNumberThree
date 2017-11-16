using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskNumberThree.AnyEventArgs
{
    public class CallEventArgs : EventArgs
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
