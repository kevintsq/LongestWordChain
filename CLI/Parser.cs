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
            string text = System.IO.File.ReadAllText(inputFile);
            string tempString = "";
            bool inWord = false;

            foreach (var c in text)
            {
                if (char.IsLetter(c))
                {
                    tempString += c;
                    inWord = true;
                }
                else
                {
                    if (inWord)
                    {
                        tempString += '\0';
                        // Console.WriteLine($"{tempString}");
                        tempString = tempString.ToLower();
                        words[wordNum] = (char*)Marshal.StringToHGlobalUni(tempString);
                        wordNum++;
                        tempString = "";
                    }

                    inWord = false;
                }
            }

            if (inWord)
            {
                tempString += '\0';
                // Console.WriteLine($"{tempString}");
                tempString = tempString.ToLower();
                words[wordNum] = (char*)Marshal.StringToHGlobalUni(tempString);
                wordNum++;
            }

            return wordNum;
        }
    }
}
