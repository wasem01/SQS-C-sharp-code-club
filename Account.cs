using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SQSBankingApplicationNew.Bank
{
    abstract class Account
    {
        public double Balance { get; protected set; }

        public string Name { get; protected set; }

        public Account (double balance)
        {
            //Hello Waseem!
            this.Balance = balance;
        }

        public abstract double Withdraw(double amount);

        public virtual double Deposit(double amount)
        {
            // Deposit depending on the amount requested.
            if (amount <= 0)
            {
                throw new AmountLessEqualsZeroException();
            }
                
            else
            {
                Balance += amount;
                return Balance;
            }
        }
    }
}
