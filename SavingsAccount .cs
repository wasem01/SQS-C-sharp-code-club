using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQSBankingApplicationNew.Bank
{
    class SavingsAccount : Account
    {

        public SavingsAccount(double balance) : base(balance)
        {
            Name = "SavingsAccount";
        }

        public override double Withdraw(double amount)
        {
            // Withdraw depending on balance and the amount requested.
            if (amount <= 0)
            {
                throw new AmountLessEqualsZeroException();
            }
            else if (Balance <= amount)
            {
                throw new InsufficientFundsException();
            }
            else
            {
                Balance -= amount;
            }

            return Balance;
        }
    }
}

