using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Exhys.ExecutionCore.Contracts;

namespace Exhys.ExecutionCore.Tests
{
    [TestClass]
    public class CompilerFactoryTests
    {
        [TestMethod]
        public void TestGetCompiler()
        {
            ICompiler compiler = CompilerFactory.Get("c++");
            Assert.IsNotNull(compiler);
        }
    }
}
