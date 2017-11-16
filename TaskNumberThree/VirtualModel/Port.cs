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

        public event EventHandler<CallEventArgs> NewCall;

        protected virtual void OnNewCall(CallEventArgs e)
        {
            EventHandler<CallEventArgs> temp = NewCall;
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
                terminal.NewCall += CreateCall; // как только произойдёт событие в терминале, вызовется метод in Port
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
    }
}
