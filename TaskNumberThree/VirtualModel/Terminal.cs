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
        public int MobileNumber
        {
            get { return _mobileNumber; }
        }
        private Port _defaultPort;

        public Terminal(int mobileNumber, Port port)
        {
            _mobileNumber = mobileNumber;
            _defaultPort = port;
        }

        public event EventHandler<CallEventArgs> NewCallEvent;
        public event EventHandler<AnswerEventArgs> AnswerEvent;

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
            if (_defaultPort.GetStatus(this))
            {
                _defaultPort.GetCallFromATSEvent += GetCallFromATS;
                _defaultPort.GetAnswerEvent += GetAnswer;
            }
        }

        // метод, принимающий некоторую информацию и генерирующий событие (через проверку)(292)
        public void CreateCall(int targetMobileNumber)
        {
            OnNewCall(new CallEventArgs(_mobileNumber, targetMobileNumber));
        }

        public void GetAnswer(object sender, AnswerEventArgs e)
        {
            if (e.Status == CallStatus.Successfuly)
            {
                Console.WriteLine("ТЕРМИНАЛ1: соединение установлено с абонентом " + e.TargetMobileNumber);
                Console.ReadLine();
            }
            if (e.Status == CallStatus.NotSuccessfuly)
            {
                Console.WriteLine("ТЕРМИНАЛ1: соединение НЕ установлено с абонентом " + e.TargetMobileNumber);
                Console.ReadLine();
            }
        }






        // =======================================================================================







        // Terminal 2 (for example)
        public void GetCallFromATS(object sender, CallEventArgs e)
        {
            Console.WriteLine("ТЕРМИНАЛ2: Звонок от " + "{0}" + " принят на терминале " + "{1}", e.MobileNumber, e.TargetMobileNumber);
            Console.ReadLine();
            AnswerToCallFromATS(e, CallStatus.Successfuly);
            // или отклонить(то же, что и закончить разговор) - списание денег, отключение, занесение информации в билинг
        }

        public void AnswerToCallFromATS(CallEventArgs e, CallStatus status)
        {
            Console.WriteLine("ТЕРМИНАЛ2: " + e.MobileNumber + " дозвонился до " + e.TargetMobileNumber);
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

    }
}
