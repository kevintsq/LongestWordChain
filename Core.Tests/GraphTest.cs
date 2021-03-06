// <copyright file="GraphTest.cs">Copyright ©  2022</copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Tests
{
    /// <summary>此类包含 Graph 的参数化单元测试</summary>
    [PexClass(typeof(Graph))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class GraphTest
    {
        /*
        /// <summary>测试 BuildGraphForFilteredWords(List`1&lt;String&gt;, Boolean) 的存根</summary>
        
        [PexMethod]
        internal void BuildGraphForFilteredWordsTest(
            [PexAssumeUnderTest] Graph target,
            List<string> words,
            bool isMost
        )
        {
            target.BuildGraphForFilteredWords(words, isMost);
            // TODO: 将断言添加到 方法 GraphTest.BuildGraphForFilteredWordsTest(Graph, List`1<String>, Boolean)
            if (!isMost)
            {
                foreach (string word in words)
                {
                    foreach (Node node in target.nodeSet.Values)
                    {
                        if (node.begin == word[0] && node.end == word[word.Length - 1])
                        {
                            Assert.IsTrue(node.word.Length >= word.Length);
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (string word in words)
                {
                    bool canFindRepresent = false;

                    foreach (Node node in target.nodeSet.Values)
                    {
                        if (node.begin == word[0] && node.end == word[word.Length - 1])
                        {
                            canFindRepresent = true;
                            break;
                        }
                    }
                    Assert.IsTrue(canFindRepresent);
                }
            }
        }
        */

        /// <summary>测试 BuildGraghForAllWords(List`1&lt;String&gt;) 的存根</summary>
        [PexMethod]
        internal void BuildGraghForAllWordsTest([PexAssumeUnderTest] Graph target, List<string> words)
        {
            target.BuildGraghForAllWords(words);
            // TODO: 将断言添加到 方法 GraphTest.BuildGraghForAllWordsTest(Graph, List`1<String>)
            
            foreach (string word in words)
            {
                target.nodeSet.ContainsKey(word);
                target.nodeSet[word].outDeg = 1 + words.Where(a => a[0]==word[word.Length-1]).ToList().Count;
                target.nodeSet[word].inDeg = 1 + words.Where(a => a[a.Length - 1] == word[0]).ToList().Count;
            }
        }
    }
}
