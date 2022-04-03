using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI
{
    internal class CallWrapper
        // Wrapper module for calling Core, should be rewritten to call by loading lib(dll) at stage 2.
    {
        public static unsafe List<String> CallCoreByOptions(string inputFileName, OperationType type, char head, char tail, bool loop)
        {
            char*[] forWords = new char*[10501]; // Allocate space for ptr.
            int wordNum = 0;

            wordNum = Parser.parse(inputFileName, forWords);

            char*[] forResults = new char*[20501]; // Allocate space for ptr.
            int reNum = 0;

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

            re.Add(reNum.ToString());
            for (int i = 0; i < reNum; i++)
            {
                re.Add(new string(forResults[i]));
            }

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
