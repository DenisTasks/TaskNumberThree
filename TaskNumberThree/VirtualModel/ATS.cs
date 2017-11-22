using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskNumberThree.RealModel;
using TaskNumberThree.AnyEventArgs;
using TaskNumberThree.Interfaces;

namespace TaskNumberThree.VirtualModel
{
    public class ATS: IATS
    {
        private readonly IDictionary<int, Tuple<IAgreement, Port>> _users;
        private readonly IDictionary<int, int> _onlineList;
        private readonly ICollection<CallInfo> _callInfo;

        public ICollection<CallInfo> GetInfo()
        {
            return _callInfo;
        }

        public ATS()
        {
            _users = new Dictionary<int, Tuple<IAgreement, Port>>();
            _onlineList = new Dictionary<int, int>();
            _callInfo = new List<CallInfo>();
        }

        public Terminal NewUserWithTerminal(TariffPlan tariffPlan, User user, int mobileNumber)
        {
            Agreement newAgreement = new Agreement(tariffPlan, user, mobileNumber);
            Port defaultPort = new Port();
            Terminal newTerminal = new Terminal(mobileNumber, defaultPort);
            _users.Add(mobileNumber, new Tuple<IAgreement, Port>(newAgreement, defaultPort));
            defaultPort.NewCallEvent += CreateCall;
            defaultPort.NewAnswerEvent += CreateCall;
            defaultPort.EndEvent += EndCall;
            return newTerminal;
        }

        public void CreateCall(object sender, ICreateCall e)
        {
            int mobileNumber = e.MobileNumber;
            int targetMobileNumber = e.TargetMobileNumber;
            Port port = _users[mobileNumber].Item2;
            Port targetPort = _users[targetMobileNumber].Item2;
            CallInfo callInfo;
            #region CallEventArgs
            if (e is CallEventArgs)
            {
                Console.WriteLine("АТС: Новый звонок с {0} на {1}", mobileNumber, targetMobileNumber);
                Console.ReadLine();
                if (_users[mobileNumber].Item1.User.Balance > 0)
                {
                    if (targetPort.Status == PortStatus.Enabled) // если у вызываемого абонента телефон включен
                    {
                        _onlineList.Add(mobileNumber, targetMobileNumber);
                        callInfo = new CallInfo(mobileNumber, targetMobileNumber, DateTime.Now);
                        _callInfo.Add(callInfo);
                        targetPort.CallFromAts(mobileNumber, targetMobileNumber);
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
                targetPort.GetAnswer(e.MobileNumber, e.TargetMobileNumber, status);
            }
            #endregion
        }

        public void EndCall(object sender, EndEventArgs e)
        {
            int endFrom;
            int endTo;
            Port targetPort;
            // если завершил звонок вызывающий абонент
            if (_onlineList.Any(x => x.Key.Equals(e.MobileNumber)))
            {
                Console.WriteLine("Звонок завершил вызывающий абонент {0}", e.MobileNumber);
                endFrom = e.MobileNumber;
                endTo = _onlineList[endFrom];
                targetPort = _users[endTo].Item2;

                ToWriteOff(endFrom);
                //callinfo = _callInfo.Last(x => x.MobileNumber.Equals(endFrom));
                //callinfo.FinishCall = DateTime.Now;
                //double toWriteOff = TimeSpan.FromTicks((callinfo.FinishCall - callinfo.StartCall).Ticks).TotalMinutes
                //                    * _users[endFrom].Item1.TariffPlan.CostPerMinute;
                //callinfo.ToWriteOff = Math.Round(toWriteOff, 2);
                //_users[endFrom].Item1.User.RemoveBalance(callinfo.ToWriteOff);

                _onlineList.Remove(endFrom);
                targetPort.GetAnswer(endFrom, endTo, CallStatus.NotSuccessfuly);
            }
            // if (_onlineList.Any(x => x.Value.Equals(e.MobileNumber)))
            // если завершил звонок вызываемый абонент
            else
            {
                Console.WriteLine("Звонок завершил вызываемый абонент {0}", e.MobileNumber);
                endFrom = e.MobileNumber;
                endTo = GetKey(e.MobileNumber);
                targetPort = _users[endTo].Item2;

                ToWriteOff(endTo);
                //callinfo = _callInfo.Last(x => x.MobileNumber.Equals(endTo));
                //callinfo.FinishCall = DateTime.Now;
                //double toWriteOff = TimeSpan.FromTicks((callinfo.FinishCall - callinfo.StartCall).Ticks).TotalMinutes
                //                    * _users[endTo].Item1.TariffPlan.CostPerMinute;
                //callinfo.ToWriteOff = toWriteOff;
                //_users[endTo].Item1.User.RemoveBalance(callinfo.ToWriteOff);

                targetPort.GetAnswer(endFrom, endTo, CallStatus.NotSuccessfuly);
                _onlineList.Remove(endTo);
            }
        }

        public void ToWriteOff(int mobileNumber)
        {
            CallInfo callinfo;
            callinfo = _callInfo.Last(x => x.MobileNumber.Equals(mobileNumber));
            callinfo.FinishCall = DateTime.Now;
            double toWriteOff = (TimeSpan.FromTicks((callinfo.FinishCall - callinfo.StartCall).Ticks).TotalSeconds / 60) * _users[mobileNumber].Item1.TariffPlan.CostPerMinute;
            toWriteOff += (TimeSpan.FromTicks((callinfo.FinishCall - callinfo.StartCall).Ticks).TotalSeconds % 60) * (_users[mobileNumber].Item1.TariffPlan.CostPerMinute / 60);
            callinfo.ToWriteOff = Math.Round(toWriteOff, 2);
            _users[mobileNumber].Item1.User.RemoveBalance(callinfo.ToWriteOff);
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
