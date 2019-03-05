using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQSBankingApplicationNew.Datasource;
using System.Threading;

namespace SQSBankingApplicationNew.Bank
{
    class Client
    {
        public string FirstName { get; private set; }
        public string SecondName { get; private set; }

        public string username { get; private set; }

        public List<Account> Accounts { get; private set; }

        public int Age { get; private set; }

        public string Password { get; private set; }

        public Client(string username, string Password)//, String SecondName,  string age, string username, string password)
        {
            // Create a local instance of DatasourceFactory.
            var dbFactory = new DatasourceFactory();

            // Retrieve the account information from the datasource.
            List<string> accountInfo = new List<string>();
            accountInfo = dbFactory.getAccountData(username, Password);

            // Check if data has been returned.


            if (accountInfo.Count> 0) 
            {
               this.FirstName  = accountInfo[0];
               this.SecondName = accountInfo[1];
               this.Age = Convert.ToInt32(accountInfo[2]);
                this.username = accountInfo[3];
                this.Password = accountInfo[4];
                Accounts = new List<Account>();
                Accounts.Add(new CurrentAccount (Convert.ToDouble(accountInfo[5])));
                Accounts.Add(new SavingsAccount(Convert.ToDouble(accountInfo[6])));
            }
            else
            {
                Console.WriteLine("ERROR: Invalid username or password.");
                Thread.Sleep(5000);
                Environment.Exit(0);
            }
        }

       //public Client(string username, string password)
       //{
       //   this.username = username;
       //   Password = password;

       //}

        public void SetPassword(string NewPassword)
        {
            Password = NewPassword;
        }

        public void SetFirstName(string NewFirstName)   
        {
                FirstName = NewFirstName;            

        }

        public void SetSecondName(string NewSecondName)
        {
            SecondName = NewSecondName;

        }
        public void SetUserName(string NewUserName)
        {
            username = NewUserName;

        }


        public Client(string _FirstName,string _SecondName, String _age, string _UserName, string _Password) 
        {
            this.Age = Convert.ToInt32(_age);
            this.username = _UserName;
            this.Password = _Password;
            this.FirstName = _FirstName;
            this.SecondName = _SecondName;
            Accounts = new List<Account>();
            Accounts.Add(new CurrentAccount(Convert.ToDouble(0)));
            Accounts.Add(new SavingsAccount(Convert.ToDouble(0)));
        }


        /// <summary>
        /// This will commit all the changes back into the source file.
        /// </summary>
        public void SaveAllData()
        {
            var dbFactory = new DatasourceFactory();
            //changed in exercise 3 to add Savings account Balance
            var AccountInfo = new List<string>() { FirstName, SecondName, Age.ToString(), username, Password, Accounts[0].Balance.ToString(), Accounts[1].Balance.ToString()};
            dbFactory.setAccountData(AccountInfo);
        }

    }
}
