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
                    CallWrapper.CallCoreByOptions(fileName, type, headLetter ?? '\0', tailLetter ?? '\0', allowingRings);
            Assert.Fail();
        }
    }
}