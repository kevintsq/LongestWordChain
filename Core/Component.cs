using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;


namespace Core
{
    internal class Node
    {
        public string word;
        public int id;

        public char begin;
        public char end;

        public int inDeg = 0;
        public int outDeg = 0;

        public int length()
        {
            return word.Length;
        }

        public Node(string word, int id, char begin, char end)
        {
            this.word = word;
            this.id = id;
            this.begin = begin;
            this.end = end;
        }

        public Node DeepCloneNode()
        {
            Node clone = new Node(word, id, begin, end)
            {
                inDeg = inDeg,
                outDeg = outDeg
            };
            return clone;
        }

        public string DebugOutput()
        {
            return $"{word}: {id},{inDeg},{outDeg}.";
        }
    }

    internal class Edge
    {
        public int id;
        public Node from;
        public Node to;
        public int weight;

        public Edge(int id, Node from, Node to, int weight)
        {
            this.id = id;
            this.from = from;
            this.to = to;
            this.weight = weight;
        }
    }

    internal class Path
    {
        public List<Edge> edgeList = new List<Edge>();
        public int weight = 0;

        public Path ClonePath()
        {
            Path clone = new Path();
            clone.edgeList.AddRange(edgeList);
            clone.weight = weight;
            return clone;
        }

        public string OutputPath()
        {
            string result = "";
            foreach (var edge in edgeList)
            {
                if (edge.to.outDeg != 0)
                {
                    result = result + edge.to.word + " ";
                }
            }

            return result;
        }

        public List<string> OutputPathByWord()
        {
            List<string> result = new List<string>();

            foreach (var edge in edgeList)
            {
                if (edge.to.outDeg != 0)
                {
                    result.Add(edge.to.word);
                }
            }

            return result;
        }
    }

    internal class Graph
    {
        public Dictionary<char, HashSet<Node>> wordDictionaryByBegin = new Dictionary<char, HashSet<Node>>();
        public Dictionary<char, HashSet<Node>> wordDictionaryByEnd = new Dictionary<char, HashSet<Node>>();
        public Dictionary<Node, HashSet<Edge>> edges = new Dictionary<Node, HashSet<Edge>>();
        public Dictionary<int, Edge> edgeSet = new Dictionary<int, Edge>();
        public Dictionary<string, Node> nodeSet = new Dictionary<string, Node>();

        public Node StartNode = new Node("___Start___", 0, '[', ']'); // Abstract Node Start
        public Node EndNode = new Node("___End___", -1, '{', '}'); // Abstract Node End


        private int nodeID = 1;
        private int edgeID = 1;

        // Build a standard graph for all word chains search.
        public void BuildGraghForAllWords(List<string> words)
        {
            nodeSet.Add("___Start___", StartNode);
            nodeSet.Add("___End___", EndNode);


            edges.Add(StartNode, new HashSet<Edge>());
            edges.Add(EndNode, new HashSet<Edge>());


            foreach (string word in words)
            {
                if (nodeSet.Keys.Contains(word))
                {
                    continue;
                }

                Node newWord = new Node(word, nodeID, word[0], word[word.Length - 1]);

                edges.Add(newWord, new HashSet<Edge>());
                nodeSet.Add(word, newWord);
                nodeID++;

                // Handle StartEdge.
                Edge newStartEdge = new Edge(edgeID, StartNode, newWord, newWord.length());
                edgeSet.Add(edgeID, newStartEdge);
                StartNode.outDeg++;
                newWord.inDeg++;
                edgeID++;
                edges[StartNode].Add(newStartEdge);

                // Handle EndEdge.
                Edge newEndEdge = new Edge(edgeID, newWord, EndNode, 0);
                edgeSet.Add(edgeID, newEndEdge);
                newWord.outDeg++;
                EndNode.inDeg++;
                edgeID++;
                edges[newWord].Add(newEndEdge);


                // Add edges.
                if (wordDictionaryByBegin.ContainsKey(newWord.end))
                {
                    foreach (Node wordNode in wordDictionaryByBegin[newWord.end])
                    {
                        Edge tempEdge = new Edge(edgeID, newWord, wordNode, wordNode.length());
                        edgeSet.Add(edgeID, tempEdge);
                        newWord.outDeg++;
                        wordNode.inDeg++;
                        edgeID++;
                        edges[newWord].Add(tempEdge);
                    }
                }

                if (wordDictionaryByEnd.ContainsKey(newWord.begin))
                {
                    foreach (Node wordNode in wordDictionaryByEnd[newWord.begin])
                    {
                        Edge tempEdge = new Edge(edgeID, wordNode, newWord, newWord.length());
                        edgeSet.Add(edgeID, tempEdge);
                        wordNode.outDeg++;
                        newWord.inDeg++;
                        edgeID++;
                        edges[wordNode].Add(tempEdge);
                    }
                }

                // Register new word.
                if (!wordDictionaryByBegin.ContainsKey(newWord.begin))
                {
                    wordDictionaryByBegin.Add(newWord.begin, new HashSet<Node>());
                }

                if (!wordDictionaryByEnd.ContainsKey(newWord.end))
                {
                    wordDictionaryByEnd.Add(newWord.end, new HashSet<Node>());
                }

                wordDictionaryByBegin[word[0]].Add(newWord);
                wordDictionaryByEnd[word[word.Length - 1]].Add(newWord);
            }
        }

        private class WordIdentifier
        {
            public char begin;
            public char end;

            public WordIdentifier(char begin, char end)
            {
                this.begin = begin;
                this.end = end;
            }
        }

        public void BuildGraphForFilteredWords(List<string> words, bool isMost)
        {
            Dictionary<WordIdentifier, string> wordDictionary = new Dictionary<WordIdentifier, string>();
            WordIdentifier wordIdentifier;
            foreach (string word in words)
            {
                wordIdentifier = new WordIdentifier(word[0], word[word.Length - 1]);
                if (!wordDictionary.ContainsKey(wordIdentifier))
                {
                    wordDictionary.Add(wordIdentifier, word);
                } 
                else
                {
                    if (!isMost)
                    {
                        if (word.Length > wordDictionary[wordIdentifier].Length)
                        {
                            wordDictionary[wordIdentifier] = word;
                        }
                    }
                }
            }
            BuildGraghForAllWords(new List<string>(wordDictionary.Values));
        }
        public void DebugOutput()
        {
            foreach (var node in nodeSet.Values)
            {
                Console.WriteLine(node.DebugOutput());
            }

            foreach (var edge in edgeSet.Values)
            {
                Console.WriteLine($"{edge.@from.word}->{edge.@to.word}:{edge.weight}");
            }
        }
    }
}