using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskNumberThree.AnyEventArgs;
using System.Threading;

namespace TaskNumberThree.VirtualModel
{
    public class Terminal
    {
        private int _mobileNumber;
        private readonly Port _defaultPort;

        public int MobileNumber
        {
            get { return _mobileNumber; }
        }

        public Terminal(int mobileNumber, Port port)
        {
            _mobileNumber = mobileNumber;
            _defaultPort = port;
        }

        public event EventHandler<CallEventArgs> NewCallEvent;
        public event EventHandler<AnswerEventArgs> AnswerEvent;
        public event EventHandler<EndEventArgs> EndEvent;

        // виртуальный защищенный метод, проверяющий наличие подписчиков(290)
        protected virtual void OnNewCall(CallEventArgs e)
        {
            EventHandler<CallEventArgs> temp = NewCallEvent;
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
            }
        }

        public void TurnOff()
        {
            if (_defaultPort.TurnOff(this))
            {
                _defaultPort.GetCallFromAtsEvent -= GetCallFromAts;
                _defaultPort.GetAnswerEvent -= GetAnswer;
                Console.WriteLine("Телефон у {0} выключен", MobileNumber);
            }
        }

        // метод, принимающий некоторую информацию и генерирующий событие (через проверку)(292)
        public void CreateCall(int targetMobileNumber)
        {
            OnNewCall(new CallEventArgs(MobileNumber, targetMobileNumber));
        }

        public void GetAnswer(object sender, AnswerEventArgs e)
        {
            if (e.Status == CallStatus.Successfuly)
            {
                // логика успешного соединения
                Console.WriteLine("Терминал {0} : соединение установлено с вызываемым абонентом {1}", e.MobileNumber, e.TargetMobileNumber);
                Console.ReadLine();
            }
            else
            {
                // логика сброса
                Console.WriteLine("Терминал {0} : сбросил он => {1}", e.TargetMobileNumber, e.MobileNumber);
                Console.ReadLine();
            }
        }

        // =======================================================================================

        public void GetCallFromAts(object sender, CallEventArgs e)
        {
            Console.WriteLine("Терминал: Запрос от {0} принят на терминале {1}", e.MobileNumber, e.TargetMobileNumber);
            Console.ReadLine();
            Random answer = new Random();
            Console.WriteLine("Принять звонок?");
            if (Console.ReadKey().KeyChar == 'y')
            {
                AnswerToCallFromAts(e, CallStatus.Successfuly);
            }
            else
            {
                RejectedCall();
            }
        }

        public void AnswerToCallFromAts(CallEventArgs e, CallStatus status)
        {
            Console.WriteLine("Терминал {0} : поднял трубку на звонок от {1}", e.TargetMobileNumber, e.MobileNumber);
            Console.ReadLine();
            OnAnswer(new AnswerEventArgs(e.MobileNumber, e.TargetMobileNumber, status));
        }

        protected virtual void OnAnswer(AnswerEventArgs e)
        {
            EventHandler<AnswerEventArgs> temp = AnswerEvent;
            if (temp != null)
            {
                temp(this, e);
            }
        }

        public void RejectedCall()
        {
            Console.WriteLine("Терминал {0}: Я сбросил звонок", MobileNumber);
            Console.ReadLine();
            OnEnd();
        }

        protected virtual void OnEnd()
        {
            EventHandler<EndEventArgs> temp = EndEvent;
            if (temp != null)
            {
                temp(this, new EndEventArgs(MobileNumber));
            }
        }
    }
}
