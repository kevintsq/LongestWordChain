using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CLI
{
    public class CallWrapper
        // Wrapper module for calling Core, should be rewritten to call by loading lib(dll) at stage 2.
    {
        public static int MAX_WORD_AMOUNT = 10000;
        public static int MAX_RESULT_AMOUNT = 20000;
        
        public static unsafe List<string> CallCoreByOptions(string inputFileName, OperationType type, char head, char tail, bool loop)
        {
            char** forWords = (char**)Marshal.AllocHGlobal(MAX_WORD_AMOUNT);  // Allocate space for ptr.
            int wordNum = Parser.Parse(inputFileName, forWords);

            char** forResults = (char**)Marshal.AllocHGlobal(MAX_RESULT_AMOUNT); // Allocate space for ptr.
            
            int reNum;

            List<string> re = new List<string>();

            switch (type)
            {
                case OperationType.All:
                    reNum = Core.Launcher.gen_chains_all(forWords, wordNum, forResults);
                    break;
                case OperationType.Unique:
                    reNum = Core.Launcher.gen_chain_word_unique(forWords, wordNum, forResults);
                    break;
                case OperationType.Longest:
                    reNum = Core.Launcher.gen_chain_char(forWords, wordNum, forResults, head, tail, loop);
                    break;
                case OperationType.Most:
                default:
                    reNum = Core.Launcher.gen_chain_word(forWords, wordNum, forResults, head, tail, loop);
                    break;
            }

            for (int i = 0; i < wordNum; i++)
            {
                Marshal.FreeHGlobal((IntPtr)forWords[i]);
            }
            Marshal.FreeHGlobal((IntPtr)forWords);

            if (reNum < 0)
            {
                Marshal.FreeHGlobal((IntPtr)forResults);
                throw new Exception("MAX_RESULT_AMOUNT is exceeded.");
            }
            else
            {
                re.Add(reNum.ToString());
                for (int i = 0; i < reNum; i++)
                {
                    re.Add(new string(forResults[i]));
                    Marshal.FreeHGlobal((IntPtr)forResults[i]);
                }
                Marshal.FreeHGlobal((IntPtr)forResults);
                /*
                 // To test parser.
                re.Add(wordNum.ToString());
                for (int i = 0; i < wordNum; i++)
                {
                    re.Add(new string(forWords[i]));
                }
                */

                return re;
            }
        }
    }
}
