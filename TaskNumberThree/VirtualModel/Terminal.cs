using System;
using TaskNumberThree.AnyEventArgs;
using TaskNumberThree.RealModel;
using TaskNumberThree.VirtualModel.Auxiliary;

namespace TaskNumberThree.VirtualModel
{
    public class Terminal
    {
        private readonly Port _defaultPort;

        public int MobileNumber { get; }

        public Terminal(int mobileNumber, Port port)
        {
            MobileNumber = mobileNumber;
            _defaultPort = port;
        }

        public event EventHandler<CallEventArgs> NewCallEvent;
        public event EventHandler<AnswerEventArgs> AnswerEvent;
        public event EventHandler<EndEventArgs> EndEvent;
        public event EventHandler<TariffEventArgs> ChangeTariffEvent;

        protected virtual void OnNewCall(CallEventArgs e)
        {
            EventHandler<CallEventArgs> temp = NewCallEvent;
            if (temp != null)
            {
                temp(this, e);
            }
        }
        protected virtual void OnAnswer(AnswerEventArgs e)
        {
            EventHandler<AnswerEventArgs> temp = AnswerEvent;
            if (temp != null)
            {
                temp(this, e);
            }
        }
        protected virtual void OnEnd()
        {
            EventHandler<EndEventArgs> temp = EndEvent;
            if (temp != null)
            {
                temp(this, new EndEventArgs(MobileNumber));
            }
        }
        protected virtual void OnChangeTariffPlan(TariffEventArgs e)
        {
            EventHandler<TariffEventArgs> temp = ChangeTariffEvent;
            if (temp != null)
            {
                temp(this, e);
            }
        }

        public void TurnOn()
        {
            if (_defaultPort.TurnOn(this))
            {
                _defaultPort.GetCallFromAtsEvent += GetCallFromAts;
                _defaultPort.GetAnswerEvent += GetAnswer;
                Console.WriteLine("-Terminal is ON at the {0}", MobileNumber);
            }
        }
        public void TurnOff()
        {
            if (_defaultPort.TurnOff(this))
            {
                _defaultPort.GetCallFromAtsEvent -= GetCallFromAts;
                _defaultPort.GetAnswerEvent -= GetAnswer;
                Console.WriteLine("-Terminal is OFF at the {0}", MobileNumber);
            }
        }

        public void DontHaveMoney()
        {
            _defaultPort.Status = PortStatus.Blocked;
        }
        public void ChangeTariffPlan(TariffPlan tariffPlan)
        {
            OnChangeTariffPlan(new TariffEventArgs(MobileNumber, tariffPlan));
        }
        public void CreateCall(int targetMobileNumber)
        {
            Console.WriteLine("-Terminal {0} : try call to {1}", MobileNumber, targetMobileNumber);
            OnNewCall(new CallEventArgs(MobileNumber, targetMobileNumber));
        }
        public void GetAnswer(object sender, AnswerEventArgs e)
        {
            if (e.Status == CallStatus.Successfuly)
            {
                Console.WriteLine("-Terminal {0} : connection established with the target number {1}", e.MobileNumber, e.TargetMobileNumber);
            }
            else
            {
                Console.WriteLine("-Terminal {0} : connection disconnected at the initiative from {1}", e.TargetMobileNumber, e.MobileNumber);
            }
        }
        public void GetCallFromAts(object sender, CallEventArgs e)
        {
            Console.WriteLine("-Terminal {1} : request accepted from {0}", e.MobileNumber, e.TargetMobileNumber);
            Console.WriteLine("-To answer a call? Press <y> for answer");
            if (Console.ReadKey().KeyChar == 'y')
            {
                Console.WriteLine();
                AnswerToCallFromAts(e, CallStatus.Successfuly);
            }
            else
            {
                RejectedCall();
            }
        }
        public void AnswerToCallFromAts(CallEventArgs e, CallStatus status)
        {
            Console.WriteLine("-Terminal {0} : answered the call from {1}", e.TargetMobileNumber, e.MobileNumber);
            OnAnswer(new AnswerEventArgs(e.MobileNumber, e.TargetMobileNumber, status));
        }
        public void RejectedCall()
        {
            Console.WriteLine("-Terminal {0}: I ended the call", MobileNumber);
            OnEnd();
        }


    }
}
