using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace SQSBankingApplicationNew.Bank
{
    class TransactionHistory
    {
        private string source= "";

        public TransactionHistory()
        {

            // Set the value of source to whats stored in the application App.config file.
            { 

            source = ConfigurationManager.AppSettings["transactionsDataSource"].ToString();

            //Fetch the current value of the project
            var requiredPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)));

            //create the path necessary to access the transactions.csv file
            source= requiredPath.Replace("file:\\", "") + source;

            }
            
        }
        /// Returns the users account data depending if the parsed username 
        /// and password is correct.
        /// </summary>
        
        public List<string> getAccountData(string userName, string password)
        {
            // Declare return variable. 
            List<string> returnData = null;

            try
            {
                // Open file using stream reader.
                using (var reader = new StreamReader(File.OpenRead(source)))
                {
                    // Loop on each line of .csv file until the last
                    // line has been reached.
                    while (!reader.EndOfStream)
                    {
                        // Line & split by the ',' delimiter.
                        string line = reader.ReadLine();
                        string[] vals = line.Split(',');

                        // Ensure that the line contains the userName of 
                        // the account holder.
                        if (line.Contains(userName) || line.Contains(password))
                        {
                            // Store 'vals' to an new instance of 'returnData'.
                            returnData = new List<string>(vals);
                            break;
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("ERROR: Error loading datasource.");
                Console.WriteLine(e.ToString());
            }

            // Return the relevant data.
            return returnData;
        }


        /// <summary>
        /// Returns the users account data depending if the parsed username
        /// is correct.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public List<string> getAccountData(string userName)
        {
            // Declare return variable.
            List<string> returnData = null;

            try
            {
                // Open file using stream reader.
                using (var reader = new StreamReader(File.OpenRead(source)))
                {

                    // Loop on each line of .csv file until the last
                    // line has been reached.
                    while (!reader.EndOfStream)
                    {
                        // Line & split by the ',' delimiter.
                        string line = reader.ReadLine();
                        string[] vals = line.Split(',');

                        // Ensure that the line contains the userName of 
                        // the account holder.
                        if (line.Contains(userName))
                        {
                            // Store 'vals' to an new instance of 'returnData'.
                            returnData = new List<string>(vals);
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: Error loading datasource.");
                Console.WriteLine(e.ToString());
            }

            // Return the relevant data.
            return returnData;
        }


        /// <summary>
        /// Sets the account details to the source file.
        /// </summary>
        /// <param name="accountData"></param>
        /// <returns></returns>
        public bool setAccountData(List<string> accountData)
        {
            // Return variable whether the account details have been
            // saved to the datasource or not.
            bool savedData = false;

            // Set the account details as string.
            string accountDetails = "";
            foreach (var item in accountData)
            {
                if (accountDetails == "") accountDetails = item;
                else accountDetails = accountDetails + "," + item;
            }

            try
            {
                // Check if the file does not exist.
                if (!File.Exists(source))
                {
                    // Create and apppend text to file.
                    string header = "transactiontype,amounts,dates,times,username";
                    var line = string.Format("{0}{1}{2}{3}", header, Environment.NewLine, accountDetails, Environment.NewLine);
                    File.AppendAllText(source, line);
                    savedData = true;
                }
                else 
                {

                    List<string> allLines = new List<string>(File.ReadAllLines(source));
                    var l = accountDetails;

                    // Check to see if there are any existing records
                    // and handle it accordingly.
                    for (int i = 0; i < allLines.Count; i++)
                    {
                        if (allLines[i].Contains(accountData[3]))
                        {
                            allLines.RemoveAt(i);
                            break;
                        }

                    }
                    //Save the data.
                    allLines.Add(l);
                    File.WriteAllLines(source, allLines);
                    savedData = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: Error loading datasource.");
                Console.WriteLine(e.ToString());
                savedData = false;
            }
            // Return the result.
            return savedData;
        }
    }
}


   