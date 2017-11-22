using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskNumberThree.AnyEventArgs;

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

        protected virtual void OnNewCall(CallEventArgs e)
        {
            EventHandler<CallEventArgs> temp = NewCallEvent;
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
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreateCall(object sender, CallEventArgs e)
        {
            Status = PortStatus.Busy;
            OnNewCall(new CallEventArgs(e.MobileNumber, e.TargetMobileNumber));
        }

        public void GetAnswer(int mobileNumber, int targetMobileNumber, CallStatus status)
        {
            if (status == CallStatus.NotSuccessfuly)
            {
                Status = PortStatus.Enabled;
            }
            OnGetAnswer(new AnswerEventArgs(mobileNumber, targetMobileNumber, status));
        }

        protected virtual void OnGetAnswer(AnswerEventArgs e)
        {
            EventHandler<AnswerEventArgs> temp = GetAnswerEvent;
            if (temp != null)
            {
                temp(this, e);
            }
        }


        //=======================================================================


        public void CallFromAts(int mobileNumber, int targetMobileNumber)
        {
            Console.WriteLine("Порт: поступил запрос с АТС с номера {0} на номер {1}", mobileNumber, targetMobileNumber);
            Status = PortStatus.Busy;
            OnNewCallFromAts(new CallEventArgs(mobileNumber, targetMobileNumber));
        }

        protected virtual void OnNewCallFromAts(CallEventArgs e)
        {
            EventHandler<CallEventArgs> temp = GetCallFromAtsEvent;
            if (temp != null)
            {
                temp(this, e);
            }
        }

        public void CreateAnswer(object sender, AnswerEventArgs e)
        {
            Console.WriteLine("Порт: {0} дозвонился до {1}", e.MobileNumber, e.TargetMobileNumber);
            Console.ReadLine();
            OnNewAnswer(e);
        }

        protected virtual void OnNewAnswer(AnswerEventArgs e)
        {
            EventHandler<AnswerEventArgs> temp = NewAnswerEvent;
            if (temp != null)
            {
                temp(this, e);
            }
        }

        public void CreateEnd(object sender, EndEventArgs e)
        {
            Console.WriteLine("Порт: {0} сбросил звонок ", e.MobileNumber);
            Status = PortStatus.Enabled;
            Console.ReadLine();
            OnEnd(e);
        }

        protected virtual void OnEnd(EndEventArgs e)
        {
            EventHandler<EndEventArgs> temp = EndEvent;
            if (temp != null)
            {
                temp(this, e);
            }
        }
    }
}
