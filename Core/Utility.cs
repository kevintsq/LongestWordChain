using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    internal class Utility
    {
        public static unsafe List<string> ConvertCharArrayToStringList(char** toConvert, int len)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < len; i++)
            {
                result.Add(new string(toConvert[i]));
            }
            return result;
        }
    }
}
