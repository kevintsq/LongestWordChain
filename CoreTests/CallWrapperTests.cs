using Microsoft.VisualStudio.TestTools.UnitTesting;
using CLI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Tests
{
    [TestClass()]
    public class CallWrapperTests
    {
        [TestMethod()]
        public void CallCoreByOptionsTest()
        {
            List<string> toWrite =
                    CallWrapper.CallCoreByOptions(
                        @"C:\Users\tanta\Documents\Personal\Studies\Undergraduate\C#\LongestWordChain\TestCases\1.txt",
                        OperationType.All, '\0', '\0', false);
            var results = new HashSet<string>();
            foreach (string line in toWrite)
            {
                results.Add(line.Trim());
            }
            var real = new HashSet<string>()
            {
                "6",
                "woo oom",
                "moon noox",
                "oom moon",
                "woo oom moon",
                "oom moon noox",
                "woo oom moon noox"
            };
            Assert.IsTrue(real.SetEquals(results));
        }

        [TestMethod()]
        public void CallCoreByOptionsTest2()
        {
            List<string> toWrite =
                    CallWrapper.CallCoreByOptions(
                        @"C:\Users\tanta\Documents\Personal\Studies\Undergraduate\C#\LongestWordChain\TestCases\2.txt",
                        OperationType.Most, '\0', '\0', false);
            var results = new HashSet<string>();
            for (int i = 1; i < toWrite.Count; i++)
            {
                results.Add(toWrite[i].Trim());
            }
            var real = new HashSet<string>()
            {
                "algebra",
                "apple",
                "elephant",
                "trick"
            };
            Assert.IsTrue(real.SetEquals(results));
        }

        [TestMethod()]
        public void CallCoreByOptionsTest3()
        {
            List<string> toWrite =
                    CallWrapper.CallCoreByOptions(
                        @"C:\Users\tanta\Documents\Personal\Studies\Undergraduate\C#\LongestWordChain\TestCases\3.txt",
                        OperationType.Unique, '\0', '\0', false);
            var results = new HashSet<string>();
            for (int i = 1; i < toWrite.Count; i++)
            {
                results.Add(toWrite[i].Trim());
            }
            var real = new HashSet<string>()
            {
                "apple",
                "elephant",
                "trick"
            };
            Assert.IsTrue(real.SetEquals(results));
        }

        [TestMethod()]
        public void CallCoreByOptionsTest4()
        {
            List<string> toWrite =
                    CallWrapper.CallCoreByOptions(
                        @"C:\Users\tanta\Documents\Personal\Studies\Undergraduate\C#\LongestWordChain\TestCases\4.txt",
                        OperationType.Longest, '\0', '\0', false);
            var results = new HashSet<string>();
            for (int i = 1; i < toWrite.Count; i++)
            {
                results.Add(toWrite[i].Trim());
            }
            var real = new HashSet<string>()
            {
                "pseudopseudohypoparathyroidism",
                "moon"
            };
            Assert.IsTrue(real.SetEquals(results));
        }

        [TestMethod()]
        public void CallCoreByOptionsTest5()
        {
            List<string> toWrite =
                    CallWrapper.CallCoreByOptions(
                        @"C:\Users\tanta\Documents\Personal\Studies\Undergraduate\C#\LongestWordChain\TestCases\5.txt",
                        OperationType.Most, 'e', '\0', false);
            var results = new HashSet<string>();
            for (int i = 1; i < toWrite.Count; i++)
            {
                results.Add(toWrite[i].Trim());
            }
            var real = new HashSet<string>()
            {
                "elephant",
                "trick"
            };
            Assert.IsTrue(real.SetEquals(results));
        }

        [TestMethod()]
        public void CallCoreByOptionsTest6()
        {
            List<string> toWrite =
                    CallWrapper.CallCoreByOptions(
                        @"C:\Users\tanta\Documents\Personal\Studies\Undergraduate\C#\LongestWordChain\TestCases\6.txt",
                        OperationType.Most, '\0', 't', false);
            var results = new HashSet<string>();
            for (int i = 1; i < toWrite.Count; i++)
            {
                results.Add(toWrite[i].Trim());
            }
            var real = new HashSet<string>()
            {
                "algebra",
                "apple",
                "elephant"
            };
            Assert.IsTrue(real.SetEquals(results));
        }

        [TestMethod()]
        public void CallCoreByOptionsTest7()
        {
            List<string> toWrite =
                    CallWrapper.CallCoreByOptions(
                        @"C:\Users\tanta\Documents\Personal\Studies\Undergraduate\C#\LongestWordChain\TestCases\7.txt",
                        OperationType.Most, '\0', '\0', true);
            var results = new HashSet<string>();
            for (int i = 1; i < toWrite.Count; i++)
            {
                results.Add(toWrite[i].Trim());
            }
            var real = new HashSet<string>()
            {
                "table",
                "element",
                "teach",
                "heaven"
            };
            Assert.IsTrue(real.SetEquals(results));
        }

        [TestMethod()]
        public void CallCoreByOptionsTest8()
        {
            try
            {
                List<string> toWrite =
                    CallWrapper.CallCoreByOptions(
                        @"C:\Users\tanta\Documents\Personal\Studies\Undergraduate\C#\LongestWordChain\TestCases\7.txt",
                        OperationType.Most, '\0', '\0', false);
            }
            catch (Exception ex)
            {
                ex.Equals(new Core.CircleDetected());
            }
        }
    }
}