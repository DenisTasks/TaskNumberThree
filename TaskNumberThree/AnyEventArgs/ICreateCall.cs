using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskNumberThree.AnyEventArgs
{
    public interface ICreateCall
    {
        int MobileNumber { get; }
        int TargetMobileNumber { get; }
    }
}
