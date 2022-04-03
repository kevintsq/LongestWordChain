using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Launcher
    {
        public static int MAX_RESULT_AMOUNT = 20000;
        
        public static unsafe int gen_chain_word(char** words, int len, char** result, char head, char tail, bool enable_loop)
        {
            Solver solver = new Solver();
            List<string> results;
            results = solver.SolveGenerateMostOrLongest(Utility.ConvertCharArrayToStringList(words, len), head, tail, enable_loop, true);
            if (results.Count > MAX_RESULT_AMOUNT)
            {
                return -1;
            }
            else
            {
                for (int i = 0; i < results.Count; i++)
                {
                    result[i] = (char*)Marshal.StringToHGlobalUni(results[i]);
                }
                return results.Count;
            }
        }
        public static unsafe int gen_chains_all(char** words, int len, char** result)
        {
            Solver solver = new Solver();
            List<string> results;
            results = solver.SolveGenerateAll(Utility.ConvertCharArrayToStringList(words, len));
            if (results.Count > MAX_RESULT_AMOUNT)
            {
                return -1;
            }
            else
            {
                for (int i = 0; i < results.Count; i++)
                {
                    result[i] = (char*)Marshal.StringToHGlobalUni(results[i]);
                }
                return results.Count;
            }
        }
        public static unsafe int gen_chain_word_unique(char** words, int len, char** result)
        {
            Solver solver = new Solver();
            List<string> results;
            results = solver.SolveGenerateUnique(Utility.ConvertCharArrayToStringList(words, len));
            if (results.Count > MAX_RESULT_AMOUNT)
            {
                return -1;
            }
            else
            {
                for (int i = 0; i < results.Count; i++)
                {
                    result[i] = (char*)Marshal.StringToHGlobalUni(results[i]);
                }
                return results.Count;
            }
        }
        public static unsafe int gen_chain_char(char** words, int len, char** result, char head, char tail, bool enable_loop)
        {
            Solver solver = new Solver();
            List<string> results;
            results = solver.SolveGenerateMostOrLongest(Utility.ConvertCharArrayToStringList(words, len), head, tail, enable_loop, false);
            if (results.Count > MAX_RESULT_AMOUNT)
            {
                return -1;
            }
            else
            {
                for (int i = 0; i < results.Count; i++)
                {
                    result[i] = (char*)Marshal.StringToHGlobalUni(results[i]);
                }
                return results.Count;
            }
        }
    }
}
