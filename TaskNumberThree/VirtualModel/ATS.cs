using System;
using System.Collections.Generic;
using System.Linq;
using TaskNumberThree.RealModel;
using TaskNumberThree.AnyEventArgs;
using TaskNumberThree.Interfaces;
using TaskNumberThree.VirtualModel.Auxiliary;

namespace TaskNumberThree.VirtualModel
{
    public class ATS: ICanCall
    {
        private readonly IDictionary<int, Tuple<IAgreement, Port>> _users;
        private readonly IDictionary<int, int> _onlineList;
        private readonly ICollection<CallInfo> _callInfo;

        public ICollection<CallInfo> GetCallInfo()
        {
            return _callInfo;
        }

        public IDictionary<int, Tuple<IAgreement, Port>> GetUsersInfo()
        {
            return _users;
        }
        public ATS()
        {
            _users = new Dictionary<int, Tuple<IAgreement, Port>>();
            _onlineList = new Dictionary<int, int>();
            _callInfo = new List<CallInfo>();
        }

        public int CanICall(int mobileNumber, int targetMobileNumber)
        {
            if (_users[mobileNumber].Item2.Status == PortStatus.Blocked)
            {
                return 1;
            }
            if (_users[targetMobileNumber].Item2.Status == PortStatus.Blocked)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }

        public void ChangeTariffPlan(object sender, TariffEventArgs e)
        {
            if (_users[e.MobileNumber].Item1.DateOfLastChange.Year < DateTime.Now.Year || _users[e.MobileNumber].Item1.DateOfLastChange.Month <= (DateTime.Now.Month - 1))
            {
                Console.WriteLine("---ATS : Your tariff plan {0} was changed successfully to {1} !"
                    , _users[e.MobileNumber].Item1.TariffPlan.Name, e.TariffPlan.Name);
                _users[e.MobileNumber].Item1.TariffPlan = e.TariffPlan;
                _users[e.MobileNumber].Item1.DateOfLastChange = DateTime.Now;
            }
            else
            {
                Console.WriteLine("---ATS : Dear subscriber {0}, please, wait a month after last change of your tariff plan!", e.MobileNumber);
            }
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
            defaultPort.ChangeTariffEvent += ChangeTariffPlan;
            return newTerminal;
        }

        public void CreateCall(object sender, ICreateCall e)
        {
            int mobileNumber = e.MobileNumber;
            int targetMobileNumber = e.TargetMobileNumber;
            Port targetPort = _users[targetMobileNumber].Item2;
            #region CallEventArgs
            if (e is CallEventArgs)
            {
                Console.WriteLine("---ATS : New call from {0} to {1}", mobileNumber, targetMobileNumber);
                if (CanICall(e.MobileNumber, e.TargetMobileNumber) == 0)
                {
                    if (targetPort.Status == PortStatus.Enabled)
                    {
                        _onlineList.Add(mobileNumber, targetMobileNumber);
                        targetPort.CallFromAts(mobileNumber, targetMobileNumber);
                    }
                    else if (targetPort.Status == PortStatus.Busy)
                    {
                        Console.WriteLine("---ATS : Target number is busy!");
                    }
                    else if (targetPort.Status == PortStatus.Disabled)
                    {
                        Console.WriteLine("---ATS : Target number is disabled!");
                    }
                }
                else if (CanICall(e.MobileNumber, e.TargetMobileNumber) == 1)
                {
                    Console.WriteLine("---ATS : You do not have enough money to make a call!");
                }
                if (CanICall(e.MobileNumber, e.TargetMobileNumber) == 2)
                {
                    Console.WriteLine("---ATS : The called party is disconnected on the debt!");
                }
            }
            #endregion
            #region AnswerEventArgs
            if (e is AnswerEventArgs)
            {
                CallInfo callInfo = new CallInfo(mobileNumber, targetMobileNumber, DateTime.Now);
                _callInfo.Add(callInfo);
                CallStatus status = ((AnswerEventArgs)e).Status;
                if (status == CallStatus.Successfuly)
                {
                    Console.WriteLine("---ATS : {0} successfully phoned to {1}", e.MobileNumber, e.TargetMobileNumber);
                }
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
                Console.WriteLine("---ATS : The call was completed by the calling subscriber {0}", e.MobileNumber);
                endFrom = e.MobileNumber;
                endTo = _onlineList[endFrom];
                targetPort = _users[endTo].Item2;
                ToWriteOff(endFrom);
                _onlineList.Remove(endFrom);
                targetPort.GetAnswer(endFrom, endTo, CallStatus.NotSuccessfuly);
            }
            // if (_onlineList.Any(x => x.Value.Equals(e.MobileNumber)))
            // если завершил звонок вызываемый абонент
            else
            {
                Console.WriteLine("---ATS : The call was completed by the called subscriber {0}", e.MobileNumber);
                endFrom = e.MobileNumber;
                endTo = GetKey(e.MobileNumber);
                targetPort = _users[endTo].Item2;
                ToWriteOff(endTo);
                targetPort.GetAnswer(endFrom, endTo, CallStatus.NotSuccessfuly);
                _onlineList.Remove(endTo);
            }
        }

        public void ToWriteOff(int mobileNumber)
        {
            if (_callInfo.ElementAt(_callInfo.Count - 1).MobileNumber == mobileNumber)
            {
                CallInfo callinfo = _callInfo.Last(x => x.MobileNumber.Equals(mobileNumber));
                callinfo.FinishCall = DateTime.Now;
                double toWriteOff = (TimeSpan.FromTicks((callinfo.FinishCall - callinfo.StartCall).Ticks).TotalSeconds / 60) * _users[mobileNumber].Item1.TariffPlan.CostPerMinute;
                toWriteOff += (TimeSpan.FromTicks((callinfo.FinishCall - callinfo.StartCall).Ticks).TotalSeconds % 60) * (_users[mobileNumber].Item1.TariffPlan.CostPerMinute / 60);
                callinfo.ToWriteOff = Math.Round(toWriteOff, 2);
                _users[mobileNumber].Item1.User.RemoveBalance(callinfo.ToWriteOff);
            }
            else
            {
                    Console.WriteLine("---ATS : The call was dropped by the called party on a call from {0}", mobileNumber);
            }
        }

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
