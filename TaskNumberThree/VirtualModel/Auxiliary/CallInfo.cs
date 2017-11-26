using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskNumberThree.VirtualModel.Auxiliary
{
    public class CallInfo
    {
        public int MobileNumber { get; set; }
        public int TargetMobileNumber { get; set; }
        public DateTime StartCall { get; set; }
        public DateTime FinishCall { get; set; }
        public double ToWriteOff { get; set; }

        public CallInfo(int mobilenumber, int targetmobilenumber, DateTime startcall)
        {
            MobileNumber = mobilenumber;
            TargetMobileNumber = targetmobilenumber;
            StartCall = startcall;
        }
    }
}
