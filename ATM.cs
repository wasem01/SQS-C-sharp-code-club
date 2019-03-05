using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;



namespace SQSBankingApplicationNew.Bank
{
    internal class ATM
    {
        Client client = null;

        public ATM()
        {
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine(@"
______________________    ________              ______  
__  ___/_  __ \_  ___/    ___  __ )_____ __________  /__
_____ \_  / / /____ \     __  __  |  __ `/_  __ \_  //_/
____/ // /_/ /____/ /     _  /_/ // /_/ /_  / / /  ,<   
/____/ \___\_\/____/      /_____/ \__,_/ /_/ /_//_/|_|  
                                                        
                            ");
        }
        internal void SelectOperation()
        {
            string option = "";
            int optionAttempts = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter one of the following options:(1) Deposit, " +
                    "(2) Withdraw," +
                    " (3) Check Balance," +
                    " (4) BalanceTransfer," +
                    " (5) ChangePassword, " +
                    "(6) Logout, " +
                    "(7) ChangeDetails,");
                option = Console.ReadLine();


                switch (option)
                {
                    case "1":
                        Deposit();
                        break;
                    case "2":
                        Withdraw();
                        break;
                    case "3":
                        Balance();
                        break;
                    case "4":
                        BalanceTransfer();
                        break;
                    case "5":
                        ChangePassword();
                        break;
                    case "6":
                        Logout();
                        break;
                    case "7":
                        ChangeDetails();
                        break;
                        default:
                        if (optionAttempts == 3)
                        {
                            Console.WriteLine("Maximum attempts exceeded.");
                            Thread.Sleep(5000);
                            Environment.Exit(0);
                        }
                        Console.WriteLine("Invalid Input");
                        Thread.Sleep(5000);
                        optionAttempts++;
                        Console.Clear();
                        SelectOperation();
                        break;
                }

            }
            while (option != "0");
        }

       
        private void ChangeDetails()
        {

            Console.WriteLine("please enter NewFirstName");
            string NewFirstName = Console.ReadLine();
            client.SetFirstName(NewFirstName);
            Console.WriteLine("please enter NewSecondName");
            string NewSecondName = Console.ReadLine();
            client.SetSecondName(NewSecondName);
            Console.WriteLine("please enter NewUserName");
            string NewUserName = Console.ReadLine();
            client.SetUserName(NewUserName);
            Console.WriteLine("please eneter your OldPassword");
            string oldpassword = Console.ReadLine();

            if (oldpassword == client.Password)

            {
                Console.WriteLine("please eneter your NewPassword");
                string NewPassword = Console.ReadLine();
                client.SetPassword(NewPassword);

            }
            client.SaveAllData();
            Thread.Sleep(3000);
            Console.WriteLine("changeDetails Successfull");

        }

        private void ChangePassword()
        {
            Console.WriteLine("please eneter your OldPassword");
            string oldpassword = Console.ReadLine();

            if (oldpassword == client.Password)

            {
                Console.WriteLine("please eneter your NewPassword");
                string NewPassword = Console.ReadLine();
                client.SetPassword(NewPassword);
                client.SaveAllData();
                Thread.Sleep(3000);
            }
            else
            {
                Console.WriteLine("Please Start Again");
            }



            Console.WriteLine("changePassword Successfull");
        }

        private void BalanceTransfer()
        {
            Console.WriteLine("please enter the amount to Transfer");
            Console.ReadLine();
            client.SaveAllData();



        }

        private void Logout()
        {
            Environment.Exit(0);
        }

        private void Balance()
        {
            Console.Clear();
            Console.WriteLine("Here are your balances:");
            for (int i = 0; i < client.Accounts.Count; i++)
            {
                Console.WriteLine(i + ": " + client.Accounts[i].Name + ": " + client.Accounts[i].Balance.ToString("C"));
            }
            Thread.Sleep(3000);
        }
        private void Withdraw()

        {
            Console.Clear();
            var accountID = SelectAccount();
            Console.WriteLine("Please enter the amount you wish to withdraw: ");

            // Check if the user's input was valid.
            string k = Console.ReadLine();
            double n;
            if (double.TryParse(k, out n) && accountID >= 0 && accountID < client.Accounts.Count)
            {
                double amount = Convert.ToInt32(k);
                // wihtdraw selected denominations £5, £10, £20, £50.                   


                try
                {
                    if (amount % 5 == 0)

                    {
                        var newBalance = client.Accounts[accountID].Withdraw(amount);
                        client.SaveAllData();

                        //ok good. so this is where withdraw takes aplce. This is only point where our data points converge - i.e. we know user, trans type and  the other one..
                        //so we call our new method  - and fill in the variabels as per it's definitino
                        WriteToTransactionFile(client.username, "Wthdrawl", amount.ToString());

                        // soorry is there a reason u wrote withdrawal?
                        // just so you know it;s a sting i'm specifying at this point - i want you to see if write my explicit text, then come back and fix it :-)
                        //ok 

                        Console.WriteLine("please collect your amount");
                        Console.WriteLine("your withdrawl request is completed");
                        Thread.Sleep(3000);

                    }
                    else
                    {
                        Console.WriteLine("please select the correct amount");
                        Thread.Sleep(3000);
                        Withdraw();
                    }

                }

                catch (InsufficientFundsException)
                {
                    Console.WriteLine("ERROR: Not enough funds in the account.");
                    Thread.Sleep(3000);
                    Withdraw();
                }
                catch (AmountLessEqualsZeroException)
                {
                    Console.WriteLine("ERROR: The amount needs to be higher than 0.");
                    Thread.Sleep(3000);
                    Withdraw();
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR: Technical Error appeared, please ask the Service Desk. ;-)");
                    Thread.Sleep(3000);
                    Withdraw();



                }

            }
            else
            {
                Console.WriteLine("ERROR: Invalid input, input must be a numeric value and a correct account selected");
                Thread.Sleep(3000);
                Withdraw();

                Console.WriteLine("your withdrawl request is completed");
            }



        }

        //this is the withdraw method, this chunk of code is how we write to our transactions file
        //however - we will do this for deposit, withdraw and transfer.... so... we don't want to keep copying and pastying blocks of code
        //if we do we triple thw maitnenance work.. so it needs to be a method - a  helper method.
        //....what does it do?  - it writes to a transaction file - let's call it that  
        //what does it need (the quesiton i asked about system gnerated vs user inputs)
        // it needs 3 strings - username of our current user from client. transaction type and amount. all stuff we have
        //so...  - obviously we can't createa a method here - because i'm in the middle of antoher method. so i'll move it to the class
        // OK so... my note in skype said....
        //[‎12/‎10/‎2018 12:06]  
        //that helper  does: 1.Convert to a list  2.puts your data in correct order for the headers 3.gerneates date stamp and time stamp  4.passes information to that write funciton
        //this is what we need tod o....

        //we have 3 inputs. but we need to write 5 items in the correct order.

        //1. convert to a list... this is the next line
        //2. puts data int he correct order.  Whatever order the list is, is what i's written as.
        //this line was taken from your function to write the data. this is the "correct" order you specified:  header = "transactiontype,amounts,dates,times,username"; 
        //
        //we need these to map our 3 inputsl; string username, string transactionType, string amount
        //This is them being added to the list.  The helper method means the user (that calls this method) doesn't ened to care about  the order  - jsut the data

        public void WriteToTransactionFile(string username, string transactionType, string amount)
        {

            List<String> WithdrawStringList = new List<string>();

            WithdrawStringList.Add(transactionType); //transactiontype
            WithdrawStringList.Add(amount); //amounts
            WithdrawStringList.Add(DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"));//dates  - these are the last 2. and are trickiest. I know it's datetime but i've not used it for a while.

            //WithdrawStringList.Add(); // times // going yo leave as a placeholder string for a moment, im here reading your comments
            WithdrawStringList.Add(username); // username
            // WithdrawStringList.Add("string 6");  // only 5 vatiables now so we can remove this

            //one more thing to do. Let's re-knit this back into the wthdrawl...............

            //4. write the data..... That's this bit so we already have it.
            var transHistory = new TransactionHistory();
            transHistory.setAccountData(WithdrawStringList); //this needs a LIST?
        }

        private void Deposit()
        {
            Console.Clear();
            short accountID;
            accountID = SelectAccount();

            Console.WriteLine("Please enter the amount you wish to deposit: ");

            // Check if the user's input was valid.
            string k = Console.ReadLine();
            double n;
            if (double.TryParse(k, out n) && accountID >= 0 && accountID < client.Accounts.Count)
            {
                try
                {
                    double amount = Convert.ToDouble(k);
                    var newBalance = client.Accounts[accountID].Deposit(amount);
                    client.SaveAllData();
                }



                catch (AmountLessEqualsZeroException)
                {
                    Console.WriteLine("ERROR: The amount needs to be higher than 0.");
                    Thread.Sleep(3000);
                    Deposit();

                }


            }
            else
            {
                Console.WriteLine("ERROR: Invalid input, input must be a numeric value and an existing account must be selected.");
                Thread.Sleep(3000);
                Deposit();

            }

        }

        private short SelectAccount()
        {
            string option;
            short result;
            if (client.Accounts.Count == 1)
            {
                return 0;
            }
            else
            {
                Console.WriteLine("Here are your accounts. Which one do you want to deposit into:");
                for (int i = 0; i < client.Accounts.Count; i++)
                {
                    Console.WriteLine(i + ": " + client.Accounts[i].Name + ": " + client.Accounts[i].Balance.ToString("C"));
                }
                option = Console.ReadLine();
                if (short.TryParse(option, out result))
                {
                    return result;
                }
                else
                {
                    Console.WriteLine("ERROR: not a valid input.");
                    Thread.Sleep(3000);
                    Console.Clear();
                    return SelectAccount();
                }
            }
        }


        internal void login()
        {
            Console.WriteLine(" please select (1) NewUser,  (2) ExistingUser");
            string options = Console.ReadLine();
            switch (options)
            {
                case "1":
                    NewUser();
                    break;

                case "2":
                    ExistingUser();
                    break;
            }

        }
        private void ExistingUser()

        {
            Console.WriteLine("Please enter your username: ");
            string username = Console.ReadLine();
            Console.WriteLine("Please enter your password: ");
            string password = Console.ReadLine();
            client = new Client(username, password);
            Console.Clear();
        }

        public void NewUser()
        {
            Console.WriteLine("Eneter your Details");
            Console.WriteLine("Eter your FirstName");
            string FirstName = Console.ReadLine();
            Console.WriteLine("Enter your SecondName");
            string SecondName = Console.ReadLine();
            Console.WriteLine("Age");
            string Age = Console.ReadLine();
            Console.WriteLine("Enter UserName");
            String UserName = Console.ReadLine();
            Console.WriteLine("Password");
            String Password = Console.ReadLine();
            client = new Client(FirstName, SecondName, Age, UserName, Password);
            client.SaveAllData();
            Console.WriteLine("you are Successfully Registered");




        }









    }
}




    



