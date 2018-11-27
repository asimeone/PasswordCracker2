using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordCracker
{
    public static class Type2Passwords
    {
        public static HashSet<string> CreatePermuationsAndHashes(System.Security.Cryptography.MD5 md5)
        {
            string a = "a";
            List<string> permutations = new List<string>();

            for (int i = 0; i <= 9; i++)
            {
                char[] oneNumberPerm = $"{a}{i}".ToCharArray();
                permute(oneNumberPerm, 0, oneNumberPerm.Length - 1, permutations);

                for (int j = 0; j <= 9; j++)
                {
                    char[] twoNumberPerm = $"{a}{i}{j}".ToCharArray();
                    permute(twoNumberPerm, 0, twoNumberPerm.Length - 1, permutations);

                    for (int k = 0; k <= 9; k++)
                    {
                        char[] threeNumberPerm = $"{a}{i}{j}".ToCharArray();
                        permute(threeNumberPerm, 0, threeNumberPerm.Length - 1, permutations);
                    }
                }
            }

            char[] special = { '!', '@', '#', '$', '%', '*' };

            foreach (char c in special)
            {
                for (int i = 0; i <= 9; i++)
                {
                    //test.Add($"{a}{i}{c}");
                    char[] oneNumberAndCharPerm = $"{a}{i}{c}".ToCharArray();
                    permute(oneNumberAndCharPerm, 0, oneNumberAndCharPerm.Length - 1, permutations);

                    for (int j = 0; j <= 9; j++)
                    {
                        char[] twoNumberAndCharPerm = $"{a}{i}{j}{c}".ToCharArray();
                        permute(twoNumberAndCharPerm, 0, twoNumberAndCharPerm.Length - 1, permutations);
                        //test.Add($"{a}{i}{j}{c}");
                    }
                }
            }

            foreach (char c in special)
            {
                char[] oneCharPerm = $"{a}{c}".ToCharArray();
                permute(oneCharPerm, 0, oneCharPerm.Length - 1, permutations);

                for (int i = 0; i <= special.Length - 1; i++)
                {
                    //test.Add($"{a}{i}{c}");
                    char[] twoCharPerm = $"{a}{special[i]}{c}".ToCharArray();
                    permute(twoCharPerm, 0, twoCharPerm.Length - 1, permutations);

                    for (int j = 0; j <= special.Length - 1; j++)
                    {
                        char[] threeCharPerm = $"{a}{special[i]}{special[j]}{c}".ToCharArray();
                        permute(threeCharPerm, 0, threeCharPerm.Length - 1, permutations);
                        //test.Add($"{a}{i}{j}{c}");
                    }
                }
            }
            HashSet<string> type2PasswordHashes = new HashSet<string>();
            //shortcut to not doing permutations correctly
            foreach(string permutation in permutations)
            {
                var wordBytes = Encoding.UTF8.GetBytes(permutation);
                var hash = md5.ComputeHash(wordBytes);
                var base64WordHash = Convert.ToBase64String(hash);
                if(!type2PasswordHashes.Contains(base64WordHash))
                {
                    type2PasswordHashes.Add(base64WordHash);
                }
            }
            return type2PasswordHashes;
        }

        static void permute(char[] arry, int i, int n, List<string> s)
        {
            int j;
            if (i == n)
                s.Add(new string(arry));
            //Console.WriteLine(arry);
            else
            {
                for (j = i; j <= n; j++)
                {
                    swap(ref arry[i], ref arry[j]);
                    permute(arry, i + 1, n, s);
                    swap(ref arry[i], ref arry[j]); //backtrack
                }
            }
        }

        static void swap(ref char a, ref char b)
        {
            char tmp;
            tmp = a;
            a = b;
            b = tmp;
        }
    }
}
