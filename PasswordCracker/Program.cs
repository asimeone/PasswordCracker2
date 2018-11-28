using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PasswordCracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> users = new Dictionary<string, string>();
            CreateUserAndPasswordDictionary(users);

            List<string> dictionaryWords = new List<string>();
            dictionaryWords = GetWordSubset();

            var md5 = System.Security.Cryptography.MD5.Create();
            Dictionary<string, string> type2s = Type2Passwords.CreatePermuationsAndHashes(md5);

            bool done = false;
            bool passwordCracked = false;
            while (!done && !passwordCracked)
            {
                foreach(KeyValuePair<string, string> userNameAndBase64Pass in users)
                {
                    Console.WriteLine($"User: {userNameAndBase64Pass.Key} ... starting timer...");
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    string base64Hash = userNameAndBase64Pass.Value;

                    passwordCracked = false;

                    foreach(string word in dictionaryWords)
                    {
                        var wordBytes = Encoding.UTF8.GetBytes(word);
                        var hash = md5.ComputeHash(wordBytes);
                        var base64WordHash = Convert.ToBase64String(hash);

                        if(base64Hash.Equals(base64WordHash))
                        {
                            passwordCracked = true;
                            Console.WriteLine($"Password found from dictionary subset: {word}");
                            stopwatch.Stop();
                            Console.WriteLine($"time it took in seconds: {stopwatch.Elapsed.Seconds}");
                            Console.WriteLine("Check next user? Y or N");
                            if (Console.ReadLine().Equals("Y"))
                            {
                                break;
                            }
                            else
                            {
                                return;
                            }

                        }
    
                    }

                    if(passwordCracked)
                    {
                        continue;
                    }
                                                           
                    Console.WriteLine($"checking permutations of type2 passwords (a) for user: {userNameAndBase64Pass.Key}");
                    if (type2s.ContainsKey(base64Hash))
                    {
                        Console.WriteLine($"type 2 password found");
                        Console.WriteLine($"password: {type2s[base64Hash]}");
                        passwordCracked = true;
                        break;
                    }

                    Console.WriteLine($"Password not cracked for user {userNameAndBase64Pass}");
                    Console.WriteLine("Continue to next user? Y or N");
                    if (Console.ReadLine().Equals("Y"))
                    {
                        done = true;
                        break;
                    }
                    
                }
            }

            Console.WriteLine("Press enter to close");
            Console.ReadLine();//close app

        }

        private static List<string> GetWordSubset()
        {
            List<string> dictionaryWords;
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "PasswordCracker.dictionary.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                dictionaryWords = result.Split(Environment.NewLine).ToList();
            }

            return dictionaryWords;
        }

        private static void CreateUserAndPasswordDictionary(Dictionary<string, string> users)
        {
            string path = @"C:\creds.creds";

            var contents = File.ReadAllText(path);

            if (!string.IsNullOrEmpty(contents))
            {
                string[] fileContents = contents.Split(Environment.NewLine);

                foreach (string u in fileContents)
                {
                    string[] userAndPass = u.Split('|');

                    if (!string.IsNullOrEmpty(userAndPass[0]) && !string.IsNullOrEmpty(userAndPass[1]))
                    {
                        users.Add(userAndPass[0], userAndPass[1]);
                    }
                }
            }
        }
    }
}
