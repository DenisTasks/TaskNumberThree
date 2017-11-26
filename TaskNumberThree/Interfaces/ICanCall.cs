using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskNumberThree.VirtualModel;
using TaskNumberThree.AnyEventArgs;
using TaskNumberThree.RealModel;
using TaskNumberThree.VirtualModel.Auxiliary;

namespace TaskNumberThree.Interfaces
{
    public interface ICanCall: ISaveInfo<CallInfo>
    {
        IDictionary<int, Tuple<IAgreement, Port>> GetUsersInfo();
        Terminal NewUserWithTerminal(TariffPlan tariffPlan, User user, int mobileNumber);
        void CreateCall(object sender, ICreateCall e);
        void EndCall(object sender, EndEventArgs e);
    }
}
