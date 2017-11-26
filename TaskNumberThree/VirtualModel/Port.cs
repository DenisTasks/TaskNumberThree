using System;
using TaskNumberThree.AnyEventArgs;
using TaskNumberThree.VirtualModel.Auxiliary;

namespace TaskNumberThree.VirtualModel
{
    public class Port
    {
        public PortStatus Status;

        public Port()
        {
            Status = PortStatus.Disabled;
        }

        public event EventHandler<CallEventArgs> NewCallEvent;
        public event EventHandler<CallEventArgs> GetCallFromAtsEvent;
        public event EventHandler<AnswerEventArgs> NewAnswerEvent;
        public event EventHandler<AnswerEventArgs> GetAnswerEvent;
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
        protected virtual void OnGetAnswer(AnswerEventArgs e)
        {
            EventHandler<AnswerEventArgs> temp = GetAnswerEvent;
            if (temp != null)
            {
                temp(this, e);
            }
        }
        protected virtual void OnNewCallFromAts(CallEventArgs e)
        {
            EventHandler<CallEventArgs> temp = GetCallFromAtsEvent;
            if (temp != null)
            {
                temp(this, e);
            }
        }
        protected virtual void OnNewAnswer(AnswerEventArgs e)
        {
            EventHandler<AnswerEventArgs> temp = NewAnswerEvent;
            if (temp != null)
            {
                temp(this, e);
            }
        }
        protected virtual void OnEnd(EndEventArgs e)
        {
            EventHandler<EndEventArgs> temp = EndEvent;
            if (temp != null)
            {
                temp(this, e);
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
        public bool TurnOn(Terminal terminal)
        {
            if (Status == PortStatus.Disabled)
            {
                Status = PortStatus.Enabled;
                terminal.NewCallEvent += CreateCall;
                terminal.AnswerEvent += CreateAnswer;
                terminal.EndEvent += CreateEnd;
                terminal.ChangeTariffEvent += CreateChange;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool TurnOff(Terminal terminal)
        {
            if (Status == PortStatus.Enabled || Status == PortStatus.Busy)
            {
                Status = PortStatus.Disabled;
                terminal.NewCallEvent -= CreateCall;
                terminal.AnswerEvent -= CreateAnswer;
                terminal.EndEvent -= CreateEnd;
                terminal.ChangeTariffEvent -= CreateChange;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreateChange(object sender, TariffEventArgs e)
        {
            OnChangeTariffPlan(e);
        }
        public void CreateCall(object sender, CallEventArgs e)
        {
            Console.WriteLine("--Port {0} : try call to {1}", e.MobileNumber, e.TargetMobileNumber);
            if (Status != PortStatus.Blocked)
            {
                Status = PortStatus.Busy;
                OnNewCall(new CallEventArgs(e.MobileNumber, e.TargetMobileNumber));
            }
            else
            {
                OnNewCall(new CallEventArgs(e.MobileNumber, e.TargetMobileNumber));
            }
        }
        public void GetAnswer(int mobileNumber, int targetMobileNumber, CallStatus status)
        {
            if (status == CallStatus.NotSuccessfuly)
            {
                Status = PortStatus.Enabled;
            }
            OnGetAnswer(new AnswerEventArgs(mobileNumber, targetMobileNumber, status));
        }
        public void CallFromAts(int mobileNumber, int targetMobileNumber)
        {
            Console.WriteLine("--Port: received a request from ATS from {0} to {1}", mobileNumber, targetMobileNumber);
            Status = PortStatus.Busy;
            OnNewCallFromAts(new CallEventArgs(mobileNumber, targetMobileNumber));
        }
        public void CreateAnswer(object sender, AnswerEventArgs e)
        {
            Console.WriteLine("--Port: {0} successfully phoned to {1}", e.MobileNumber, e.TargetMobileNumber);
            OnNewAnswer(e);
        }
        public void CreateEnd(object sender, EndEventArgs e)
        {
            Console.WriteLine("--Port: {0} ended the call", e.MobileNumber);
            Status = PortStatus.Enabled;
            OnEnd(e);
        }
    }
}
