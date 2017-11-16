using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskNumberThree.RealModel;
using TaskNumberThree.AnyEventArgs;

namespace TaskNumberThree.VirtualModel
{
    public class ATS
    {
        public Terminal NewUserWithTerminal(TariffPlan tariffPlan, User user, int mobileNumber)
        {
            Agreement newAgreement = new Agreement(tariffPlan, user, mobileNumber);
            Port defaultPort = new Port();
            Terminal newTerminal = new Terminal(mobileNumber, defaultPort);
            defaultPort.NewCall += CreateCall;
            return newTerminal;
        }

        public void CreateCall(object sender, CallEventArgs e)
        {
            Console.WriteLine("Call from " + e.MobileNumber + " to " + e.TargetMobileNumber);
            Console.ReadLine();
        }
    }
}
