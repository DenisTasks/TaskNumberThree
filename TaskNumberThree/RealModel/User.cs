using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskNumberThree.RealModel
{
    public class User
    {
        public string Name { get; private set; }
        public string LastName { get; private set; }
        public double Balance { get; private set; }

        public User(string name, string lastName)
        {
            Name = name;
            LastName = lastName;
            Balance = 0;
        }
        public User(string name, string lastName, double balance)
        {
            Name = name;
            LastName = lastName;
            Balance = Math.Round(balance, 2);
        }
        public void AddBalance(double balance)
        {
            Balance = Balance + balance;
        }
        public void RemoveBalance(double balance)
        {
            Balance = Balance - balance;
        }
    }
}
