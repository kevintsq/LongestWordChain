// <copyright file="SolverTest.cs">Copyright ©  2022</copyright>
using System;
using Core;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Tests
{
    /// <summary>此类包含 Solver 的参数化单元测试</summary>
    [PexClass(typeof(Solver))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class SolverTest
    {
        /// <summary>测试 FindPath(Node, Path) 的存根</summary>
        [PexMethod]
        internal void FindPathTest(
            [PexAssumeUnderTest]Solver target,
            Node nowNode,
            Path nowPath
        )
        {
            target.FindPath(nowNode, nowPath);
            // TODO: 将断言添加到 方法 SolverTest.FindPathTest(Solver, Node, Path)
        }
    }
}
