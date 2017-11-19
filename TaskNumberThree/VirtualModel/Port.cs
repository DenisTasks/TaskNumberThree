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
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreateCall(object sender, CallEventArgs e)
        {
            OnNewCall(new CallEventArgs(e.MobileNumber, e.TargetMobileNumber));
        }

        public void GetAnswer(int mobileNumber, int targetMobileNumber, CallStatus status)
        {
            Console.WriteLine("ПОРТ1: абонент " + targetMobileNumber + " как-то отреагировал на звонок");
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
            Console.WriteLine("ПОРТ2: Поступил запрос с АТС с номера " + mobileNumber + " на номер " + targetMobileNumber);
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
            Console.WriteLine("ПОРТ2: " + e.MobileNumber + " дозвонился до " + e.TargetMobileNumber);
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
    }
}
