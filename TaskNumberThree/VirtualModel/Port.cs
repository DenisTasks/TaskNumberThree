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
        public event EventHandler<CallEventArgs> GetCallFromATSEvent;
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

        public bool GetStatus(Terminal terminal)
        {
            if (Status == PortStatus.Disabled)
            {
                Status = PortStatus.Enabled;
                terminal.NewCallEvent += CreateCall; // как только произойдёт событие в терминале, вызовется метод in Port
                terminal.AnswerEvent += CreateAnswer; // второй порт (сигнализирует, что вызываемый абонент как-то ответил)
                terminal.EndEvent += CreateEnd; // второй порт сообщает на АТС, что звонок сброшен
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




        // Port2 (for example)
        public void CallFromATS(int mobileNumber, int targetMobileNumber)
        {
            Console.WriteLine("ПОРТ: Поступил запрос с АТС с номера " + mobileNumber + " на номер " + targetMobileNumber);
            Status = PortStatus.Busy;
            OnNewCallFromATS(new CallEventArgs(mobileNumber, targetMobileNumber));
        }

        protected virtual void OnNewCallFromATS(CallEventArgs e)
        {
            EventHandler<CallEventArgs> temp = GetCallFromATSEvent;
            if (temp != null)
            {
                temp(this, e);
            }
        }

        public void CreateAnswer(object sender, AnswerEventArgs e)
        {
            Console.WriteLine("ПОРТ: " + e.MobileNumber + " дозвонился до " + e.TargetMobileNumber);
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
            Console.WriteLine("ПОРТ: " + e.MobileNumber + " сбросил звонок ");
            Status = PortStatus.Enabled;
            Console.WriteLine("ПОРТ: " + Status);
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
