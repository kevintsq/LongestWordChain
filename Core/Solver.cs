using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    internal class Solver
    {
        private Graph graph;
        private Queue<Node> topoNodes;

        public bool JudgeCircle()
        {
            Dictionary<string, Node> tempNodes = new Dictionary<string, Node>();

            foreach (var name in graph.nodeSet.Keys)
            {
                Node node = graph.nodeSet[name];
                tempNodes.Add(name, node.DeepCloneNode());
            }

            topoNodes = new Queue<Node>();

            while (true)
            {
                bool hasZeroInDeg = false;
                foreach (var name in tempNodes.Keys)
                {
                    Node node = tempNodes[name];
                    if (node.inDeg == 0)
                    {
                        topoNodes.Enqueue(graph.nodeSet[name]);
                        foreach (var edge in graph.edges[graph.nodeSet[name]])
                        {
                            tempNodes[edge.to.word].inDeg--;
                        }

                        tempNodes.Remove(name);
                        hasZeroInDeg = true;
                        break;
                    }
                }

                if (tempNodes.Keys.Count == 0)
                {
                    return false;
                }

                if (!hasZeroInDeg)
                {
                    return true;
                }
            }
        }

        private Dictionary<Node, bool> isVisit;

        private readonly List<Path> paths = new List<Path>();

        // GET_PATH_ALL
        public void FindPath(Node nowNode, Path nowPath)
        {
            isVisit[nowNode] = true;
            if (nowNode.Equals(graph.EndNode))
            {
                if (nowPath.edgeList.Count > 2)
                {
                    paths.Add(nowPath);
                }
            }
            else
            {
                foreach (var edge in graph.edges[nowNode])
                {
                    if (isVisit[edge.to])
                    {
                        continue;
                    }

                    Path nextPath = nowPath.ClonePath();
                    nextPath.edgeList.Add(edge);
                    nextPath.weight += edge.weight;
                    FindPath(edge.to, nextPath);
                }
            }

            isVisit[nowNode] = false;
            return;
        }

        public List<string> SolveGenerateAll(List<string> words)
        {
            List<string> results = new List<string>();
            List<List<Edge>> chains = new List<List<Edge>>();

            // foreach (string word in words)
            // {
            //     Console.WriteLine(word);
            // }

            graph = new Graph();
            graph.BuildGraghForAllWords(words);
            // graph.debugOutput();

            bool isDAG = JudgeCircle();

            // Console.WriteLine(isDAG);
            //
            // Console.WriteLine(topoNodes.Count);

            if (isDAG)
            {
                throw new CircleDetected();
            }

            // foreach (var node in topoNodes)
            // {
            //     Console.WriteLine(node.debugOutput());
            // }

            isVisit = new Dictionary<Node, bool>();
            foreach (var node in graph.nodeSet.Values)
            {
                isVisit.Add(node, false);
            }

            // foreach (var key in isVisit.Keys)
            // {
            //     Console.WriteLine($"{key.word}: {isVisit[key]}");
            // }

            FindPath(graph.StartNode, new Path());
            foreach (var path in paths)
            {
                results.Add(path.OutputPath());
            }

            return results;
        }

        // GET_PATH_UNIQUE
        public void FindPathUnique(Node nowNode, Path nowPath, HashSet<char> usedAlphabet)
        {
            isVisit[nowNode] = true;
            if (nowNode.Equals(graph.EndNode))
            {
                if (nowPath.edgeList.Count > 2)
                {
                    paths.Add(nowPath);
                }
            }
            else
            {
                foreach (var edge in graph.edges[nowNode])
                {
                    if (isVisit[edge.to] || usedAlphabet.Contains(edge.to.begin))
                    {
                        continue;
                    }

                    Path nextPath = nowPath.ClonePath();

                    HashSet<char> nextUsedAlphabet = new HashSet<char>(usedAlphabet)
                    {
                        edge.to.begin
                    };

                    nextPath.edgeList.Add(edge);
                    nextPath.weight += 1;
                    FindPathUnique(edge.to, nextPath, nextUsedAlphabet);
                }
            }

            isVisit[nowNode] = false;
            return;
        }

        public List<string> SolveGenerateUnique(List<string> words)
        {
            List<string> results = new List<string>();
            List<List<Edge>> chains = new List<List<Edge>>();

            // foreach (string word in words)
            // {
            //     Console.WriteLine(word);
            // }

            graph = new Graph();
            graph.BuildGraghForAllWords(words);
            // graph.debugOutput();

            bool isDAG = JudgeCircle();

            // Console.WriteLine(isDAG);
            //
            // Console.WriteLine(topoNodes.Count);

            if (isDAG)
            {
                throw new CircleDetected();
            }

            // foreach (var node in topoNodes)
            // {
            //     Console.WriteLine(node.debugOutput());
            // }

            isVisit = new Dictionary<Node, bool>();
            foreach (var node in graph.nodeSet.Values)
            {
                isVisit.Add(node, false);
            }

            // foreach (var key in isVisit.Keys)
            // {
            //     Console.WriteLine($"{key.word}: {isVisit[key]}");
            // }

            FindPathUnique(graph.StartNode, new Path(), new HashSet<char>());

            Path candidatePath = new Path();

            foreach (var path in paths)
            {
                if (path.weight > candidatePath.weight)
                {
                    candidatePath = path;
                }
            }

            results.AddRange(candidatePath.OutputPathByWord());

            return results;
        }

        // GET_PATH_MOST_OR_LONGEST
        public void FindPathMostOrLongest(Node nowNode, Path nowPath, char begin, char tail, bool isMost)
        {
            isVisit[nowNode] = true;
            if (nowNode.Equals(graph.EndNode))
            {
                if (nowPath.edgeList.Count > 2)
                {
                    if (tail == '\0' || nowPath.edgeList[nowPath.edgeList.Count - 1].@from.end == tail)
                    {
                        paths.Add(nowPath);
                    }
                }
            }
            else
            {
                foreach (var edge in graph.edges[nowNode])
                {
                    if (isVisit[edge.to])
                    {
                        continue;
                    }

                    if (begin != '\0' && nowNode == graph.StartNode && edge.to.begin != begin)
                    {
                        continue;
                    }

                    Path nextPath = nowPath.ClonePath();
                    nextPath.edgeList.Add(edge);
                    if (isMost)
                    {
                        nextPath.weight += 1;
                    }
                    else
                    {
                        nextPath.weight += edge.weight;
                    }
                    FindPathMostOrLongest(edge.to, nextPath, begin, tail, isMost);
                }
            }

            isVisit[nowNode] = false;
            return;
        }

        public List<string> SolveGenerateMostOrLongest(List<string> words, char begin, char tail, bool canLoop, bool isMost)
        {
            List<string> results = new List<string>();
            List<List<Edge>> chains = new List<List<Edge>>();

            // foreach (string word in words)
            // {
            //     Console.WriteLine(word);
            // }

            graph = new Graph();
            graph.BuildGraghForAllWords(words);
            // graph.debugOutput();

            bool isDAG = JudgeCircle();

            // Console.WriteLine(isDAG);
            //
            // Console.WriteLine(topoNodes.Count);

            if (isDAG && !canLoop)
            {
                throw new CircleDetected();
            }

            // foreach (var node in topoNodes)
            // {
            //     Console.WriteLine(node.debugOutput());
            // }


            isVisit = new Dictionary<Node, bool>();
            foreach (var node in graph.nodeSet.Values)
            {
                isVisit.Add(node, false);
            }

            // foreach (var key in isVisit.Keys)
            // {
            //     Console.WriteLine($"{key.word}: {isVisit[key]}");
            // }

            FindPathMostOrLongest(graph.StartNode, new Path(), begin, tail, isMost);

            Path candidatePath = new Path();

            foreach (var path in paths)
            {
                if (path.weight > candidatePath.weight)
                {
                    candidatePath = path;
                }
            }

            results.AddRange(candidatePath.OutputPathByWord());

            return results;
        }
    }
}