using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskNumberThree.VirtualModel;
using TaskNumberThree.Interfaces;
using TaskNumberThree.VirtualModel.Auxiliary;

namespace TaskNumberThree.AnyEventArgs
{
    public class AnswerEventArgs : EventArgs, ICreateCall
    {
        public int MobileNumber { get; private set; }
        public int TargetMobileNumber { get; private set; }
        public CallStatus Status;

        public AnswerEventArgs(int mobileNumber, int targetMobileNumber, CallStatus status)
        {
            MobileNumber = mobileNumber;
            TargetMobileNumber = targetMobileNumber;
            Status = status;
        }
    }
}
