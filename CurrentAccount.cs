using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SQSBankingApplicationNew.Bank
{
    class CurrentAccount : Account
    {

        public int Overdraft
        {
            get
            {
                return 200;
            }
        }

        public CurrentAccount(double balance) : base (balance)
        {
            Name = "Current Account";
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
