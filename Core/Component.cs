using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    internal class Edge
    {
        public int from;
        public int to;
        public string word;

        public int length()
        {
            return word.Length;
        }
    }

    internal class Graph
    {
        private Dictionary<int, List<Edge>> edges;

    }
}
