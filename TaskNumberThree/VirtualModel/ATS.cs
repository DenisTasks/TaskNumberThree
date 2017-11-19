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
        private IDictionary<int, Port> _usersMts;
        public ATS()
        {
            _usersMts = new Dictionary<int, Port>();
        }
        public Terminal NewUserWithTerminal(TariffPlan tariffPlan, User user, int mobileNumber)
        {
            Agreement newAgreement = new Agreement(tariffPlan, user, mobileNumber);
            Port defaultPort = new Port();
            Terminal newTerminal = new Terminal(mobileNumber, defaultPort);
            _usersMts.Add(mobileNumber, defaultPort);
            defaultPort.NewCallEvent += CreateCall;
            defaultPort.NewAnswerEvent += CreateCall;
            defaultPort.EndEvent += EndCall;
            return newTerminal;
        }

        public void CreateCall(object sender, ICreateCall e)
        {
            int mobileNumber;
            int targetMobileNumber;
            Port port;
            Port targetPort;
            #region CallEventArgs
            if (e is CallEventArgs)
            {
                Console.WriteLine("АТС: Новый звонок с " + e.MobileNumber + " на " + e.TargetMobileNumber);
                Console.ReadLine();
                mobileNumber = e.MobileNumber;
                targetMobileNumber = e.TargetMobileNumber;
                port = _usersMts[e.MobileNumber];
                targetPort = _usersMts[e.TargetMobileNumber];
                if (targetPort.Status == PortStatus.Enabled) // если у вызываемого абонента телефон включен
                {
                    // check for money
                    targetPort.CallFromATS(e.MobileNumber, e.TargetMobileNumber);
                }
                else if (targetPort.Status == PortStatus.Busy)
                {
                    Console.WriteLine("Target number is busy!");
                    Console.ReadLine();
                }
                else if (targetPort.Status == PortStatus.Disabled)
                {
                    Console.WriteLine("Target number is disabled!");
                    Console.ReadLine();
                }
            }
            #endregion
            #region AnswerEventArgs
            if (e is AnswerEventArgs)
            {
                AnswerEventArgs inATS = (AnswerEventArgs)e;
                targetPort = _usersMts[inATS.TargetMobileNumber];
                targetPort.GetAnswer(inATS.MobileNumber, inATS.TargetMobileNumber, inATS.Status);
            }
            #endregion
        }

        public void EndCall(object sender, EndEventArgs e)
        {
            Console.WriteLine("АТС: " + e.TargetMobileNumber + " сбросил звонок от " + e.MobileNumber);
            Console.ReadLine();
        }
    }
}
