using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskNumberThree.VirtualModel;

namespace TaskNumberThree.AnyEventArgs
{
    public class EndEventArgs: EventArgs, ICreateCall
    {
        public int MobileNumber { get; private set; }
        public int TargetMobileNumber { get; private set; }

        public EndEventArgs(int mobileNumber)
        {
            MobileNumber = mobileNumber;
        }
    }
}
