using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskNumberThree.VirtualModel;
using TaskNumberThree.AnyEventArgs;
using TaskNumberThree.RealModel;
using TaskNumberThree.Interfaces;

namespace TaskNumberThree.Interfaces
{
    public interface IATS: ISaveInfo<CallInfo>
    {
        Terminal NewUserWithTerminal(TariffPlan tariffPlan, User user, int mobileNumber);
        void CreateCall(object sender, ICreateCall e);
        void EndCall(object sender, EndEventArgs e);
    }
}
