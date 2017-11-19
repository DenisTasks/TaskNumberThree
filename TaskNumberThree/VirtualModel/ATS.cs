using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskNumberThree.RealModel;
using TaskNumberThree.AnyEventArgs;
using TaskNumberThree.Interfaces;

namespace TaskNumberThree.VirtualModel
{
    public class ATS
    {
        private IDictionary<int, Tuple<IAgreement, Port>> _usersMts;
        private IDictionary<int, int> _onlineList;
        public ATS()
        {
            _usersMts = new Dictionary<int, Tuple<IAgreement, Port>>();
            _onlineList = new Dictionary<int, int>();
        }
        public Terminal NewUserWithTerminal(TariffPlan tariffPlan, User user, int mobileNumber)
        {
            Agreement newAgreement = new Agreement(tariffPlan, user, mobileNumber);
            Port defaultPort = new Port();
            Terminal newTerminal = new Terminal(mobileNumber, defaultPort);
            _usersMts.Add(mobileNumber, new Tuple<IAgreement, Port>(newAgreement, defaultPort));
            defaultPort.NewCallEvent += CreateCall;
            defaultPort.NewAnswerEvent += CreateCall;
            defaultPort.EndEvent += EndCall;
            return newTerminal;
        }

        public void CreateCall(object sender, ICreateCall e)
        {
            int mobileNumber = e.MobileNumber;
            int targetMobileNumber = e.TargetMobileNumber;
            Port port = _usersMts[mobileNumber].Item2;
            Port targetPort = _usersMts[targetMobileNumber].Item2;
            #region CallEventArgs
            if (e is CallEventArgs)
            {
                Console.WriteLine("АТС: Новый звонок с " + mobileNumber + " на " + targetMobileNumber);
                Console.ReadLine();
                if (_usersMts[mobileNumber].Item1.User.Balance > 0)
                {
                    if (targetPort.Status == PortStatus.Enabled) // если у вызываемого абонента телефон включен
                    {
                        _onlineList.Add(mobileNumber, targetMobileNumber);
                        Console.WriteLine("OnlineList add " + _onlineList.ElementAt(0));
                        targetPort.CallFromATS(mobileNumber, targetMobileNumber);
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
                else
                {
                    Console.WriteLine("У Вас недостаточно средств для совершения звонка!");
                }
            }
            #endregion

            #region AnswerEventArgs
            if (e is AnswerEventArgs)
            {
                CallStatus status = ((AnswerEventArgs)e).Status;
                targetPort.GetAnswer(mobileNumber, targetMobileNumber, status);
            }
            #endregion
        }

        public void EndCall(object sender, EndEventArgs e)
        {
            int mobileNumber;
            int targetMobileNumber;
            Port port;
            Port targetPort;
            if (_onlineList.Any(x => x.Key.Equals(e.MobileNumber)))
            {
                mobileNumber = e.MobileNumber;
                targetMobileNumber = GetKey(e.MobileNumber);
                port = _usersMts[mobileNumber].Item2;
                targetPort = _usersMts[targetMobileNumber].Item2;
            }
            // if (_onlineList.Any(x => x.Value.Equals(e.MobileNumber)))
            else
            {
                targetMobileNumber = e.MobileNumber;
                mobileNumber = GetKey(e.MobileNumber);
                targetPort = _usersMts[mobileNumber].Item2;
                port = _usersMts[targetMobileNumber].Item2;
            }
            Console.WriteLine("OnlineList remove " + _onlineList.ElementAt(0));
            _onlineList.Remove(mobileNumber);
            targetPort.GetAnswer(mobileNumber, targetMobileNumber, CallStatus.NotSuccessfuly);
        }

        // поиск ключа по значению
        public int GetKey(int x)
        {
            foreach (var item in _onlineList)
            {
                if (item.Value.Equals(x))
                {
                    return item.Key;
                }
            }
            return -1;
        }
    }
}
