using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CreateCreds
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string path = @"C:\creds.creds";
            
            //create it manually
            //if (!File.Exists(path))
            //{
            //    File.Create(path);
            //}
            
            var contents = File.ReadAllText(path);

            List<string> users = new List<string>();
            if (!string.IsNullOrEmpty(contents))
            {
                string[] fileContents = contents.Split(Environment.NewLine);
                
                foreach(string u in fileContents)
                {
                    string[] userAndPass = u.Split('|');
                    users.Add(userAndPass[0]);
                }
            }
            
            var md5 = System.Security.Cryptography.MD5.Create();

            bool done = false;
            while(!done)
            {
                string userName = string.Empty;
                                
                Console.WriteLine("Enter username:");
                userName = Console.ReadLine();
                bool userAlreadyExists = users.Exists(x => x.Equals(userName));
                
                while(userAlreadyExists)
                {
                    Console.WriteLine("Username already exists! Enter new username:");
                    userName = Console.ReadLine();
                    userAlreadyExists = users.Exists(x => x.Equals(userName));
                }
                

                Console.WriteLine("Enter password:");
                string password = Console.ReadLine();
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                var hash = md5.ComputeHash(passwordBytes);
                string hashedPassword = Convert.ToBase64String(hash);
                contents = $"{contents}{userName}|{hashedPassword}{Environment.NewLine}";

                Console.WriteLine("Done? Y or N");
                if(Console.ReadLine().Equals("Y"))
                {
                    done = true;
                }
            }

            File.WriteAllText(path, contents);
            
        }
        
    }
}
