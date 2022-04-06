using System;
using Core;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Tests
{
    /// <summary>此类包含 Launcher 的参数化单元测试</summary>
    [TestClass]
    [PexClass(typeof(Launcher))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class LauncherTest
    {

        /// <summary>测试 gen_chain_word(Char**, Int32, Char**, Char, Char, Boolean) 的存根</summary>
        [PexMethod]
        public unsafe int gen_chain_wordTest(
            char** words,
            int len,
            char** result,
            char head,
            char tail,
            bool enable_loop
        )
        {
            int result01 = Launcher.gen_chain_word(words, len, result, head, tail, enable_loop);
            return result01;
            // TODO: 将断言添加到 方法 LauncherTest.gen_chain_wordTest(Char**, Int32, Char**, Char, Char, Boolean)
        }

        /// <summary>测试 gen_chains_all(Char**, Int32, Char**) 的存根</summary>
        [PexMethod]
        public unsafe int gen_chains_allTest(
            char** words,
            int len,
            char** result
        )
        {
            int result01 = Launcher.gen_chains_all(words, len, result);
            return result01;
            // TODO: 将断言添加到 方法 LauncherTest.gen_chains_allTest(Char**, Int32, Char**)
        }

        /// <summary>测试 gen_chain_word_unique(Char**, Int32, Char**) 的存根</summary>
        [PexMethod]
        public unsafe int gen_chain_word_uniqueTest(
            char** words,
            int len,
            char** result
        )
        {
            int result01 = Launcher.gen_chain_word_unique(words, len, result);
            return result01;
            // TODO: 将断言添加到 方法 LauncherTest.gen_chain_word_uniqueTest(Char**, Int32, Char**)
        }

        /// <summary>测试 gen_chain_char(Char**, Int32, Char**, Char, Char, Boolean) 的存根</summary>
        [PexMethod]
        public unsafe int gen_chain_charTest(
            char** words,
            int len,
            char** result,
            char head,
            char tail,
            bool enable_loop
        )
        {
            int result01 = Launcher.gen_chain_char(words, len, result, head, tail, enable_loop);
            return result01;
            // TODO: 将断言添加到 方法 LauncherTest.gen_chain_charTest(Char**, Int32, Char**, Char, Char, Boolean)
        }
    }
}
