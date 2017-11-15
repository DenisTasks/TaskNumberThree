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
        public int Balance { get; private set; }

        public User(string name, string lastName)
        {
            Name = name;
            LastName = lastName;
            Balance = 0;
        }
        public User(string name, string lastName, int balance)
        {
            Name = name;
            LastName = lastName;
            Balance = balance;
        }
        public void AddBalance(int balance)
        {
            Balance = Balance + balance;
        }
        public void RemoveBalance(int balance)
        {
            Balance = Balance - balance;
        }
    }
}
