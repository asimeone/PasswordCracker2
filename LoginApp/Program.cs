using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LoginApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\creds.creds";
            
            var contents = File.ReadAllText(path);

            Dictionary<string, string> users = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(contents))
            {
                string[] fileContents = contents.Split(Environment.NewLine);
                
                foreach(string u in fileContents)
                {
                    string[] userAndPass = u.Split('|');

                    if (!string.IsNullOrEmpty(userAndPass[0]) && !string.IsNullOrEmpty(userAndPass[1]))
                    {
                        users.Add(userAndPass[0], userAndPass[1]);
                    }
                }
            }
            
            var md5 = System.Security.Cryptography.MD5.Create();

            bool done = false;
            while(!done)
            {
                string userName = string.Empty;
                                
                Console.WriteLine("Enter username:");
                userName = Console.ReadLine();

                var userExists = users.ContainsKey(userName);
                
                while(!userExists)
                {
                    Console.WriteLine("Username does not exist, try again");
                    userName = Console.ReadLine();
                    userExists = users.ContainsKey(userName);
                }
                

                Console.WriteLine("Enter password:");
                string password = Console.ReadLine();
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                var hash = md5.ComputeHash(passwordBytes);
                string base64Pass = Convert.ToBase64String(hash);
                string savedPass = string.Empty;
                users.TryGetValue(userName, out savedPass);


                bool isPasswordValid = savedPass.Equals(base64Pass);
                
                while (!isPasswordValid)
                {
                    Console.WriteLine("Invalid password, try again");
                    password = Console.ReadLine();
                    passwordBytes = Encoding.UTF8.GetBytes(password);
                    hash = md5.ComputeHash(passwordBytes);
                    base64Pass = Convert.ToBase64String(hash);
                    isPasswordValid = savedPass.Equals(base64Pass);
                }

                Console.WriteLine("User Logged in! Done? Y or N");
                if(Console.ReadLine().Equals("Y"))
                {
                    done = true;
                }
            }
        }
    }
}
