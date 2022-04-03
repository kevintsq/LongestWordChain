using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CLI
{
    internal class Parser
    {
        public static unsafe int Parse(string inputFile, char*[] words)
        {
            // Console.WriteLine("Now parsing.");
            int wordNum = 0;
            byte[] text = System.IO.File.ReadAllBytes(inputFile); 
            
            StringBuilder sb = new StringBuilder();
            bool inWord = false;

            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    sb.Append(c);
                    inWord = true;
                }
                else
                {
                    if (inWord)
                    {
                        string word = sb.ToString();
                        // Console.WriteLine(word);
                        word = word.ToLower();
                        words[wordNum++] = (char*)Marshal.StringToHGlobalUni(word);
                        sb.Clear();
                    }

                    inWord = false;
                }
            }

            if (inWord)
            {
                string word = sb.ToString();
                // Console.WriteLine(word);
                word = word.ToLower();
                words[wordNum++] = (char*)Marshal.StringToHGlobalUni(word);
            }

            return wordNum;
        }
    }
}
