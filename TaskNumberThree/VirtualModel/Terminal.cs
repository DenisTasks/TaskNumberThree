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
        private Port _defaultPort;

        public Terminal(int mobileNumber, Port port)
        {
            _mobileNumber = mobileNumber;
            _defaultPort = port;
        }

        public event EventHandler<CallEventArgs> NewCall;

        // виртуальный защищенный метод, проверяющий наличие подписчиков(290)
        protected virtual void OnNewCall(CallEventArgs e)
        {
            EventHandler<CallEventArgs> temp = NewCall;
            if (temp != null)
            {
                temp(this, e);
            }
        }

        // метод, принимающий некоторую информацию и генерирующий событие (через проверку)(292)
        public void CreateCall(int targetMobileNumber)
        {
            if (_defaultPort.GetStatus(this))
            {
                //
                OnNewCall(new CallEventArgs(_mobileNumber, targetMobileNumber));
            }
        }

    }
}
