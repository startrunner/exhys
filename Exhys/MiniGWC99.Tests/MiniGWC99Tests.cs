using System;
using System.Diagnostics;
using Exhys.ExecutionCore;
using Exhys.ExecutionCore.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniGWC99.Tests
{
    [TestClass]
    public class MiniGWC99Tests
    {
        [TestMethod]
        public void TestGnuGccHelloWorld ()
        {
            Debug.WriteLine("Shit");
            CompilerFactory.Initialize("");
            ICompiler gpp = CompilerFactory.Instance.Get("c");

            CompilationResult helloWorldProgram = gpp.Compile(cHelloWorld);

            Process testProcess = Process.Start(new ProcessStartInfo(helloWorldProgram.ExecutablePath) { UseShellExecute = false, RedirectStandardOutput = true });
            testProcess.WaitForExit();

            string programOut = testProcess.StandardOutput.ReadToEnd();
            if (!programOut.Contains("H")) throw new Exception("Compiler Failed");
        }

        [TestMethod]
        public void TestGnuGccHelloWorldWithError ()
        {
            ICompiler gpp = new MiniGWC();
            var program = gpp.Compile(cHelloWorldWithError);

            if (program.IsSuccessful) throw new Exception("Shoud've yelded an error.");
        }

        const string cHelloWorld = @"
        #include <stdio.h>
        #warning some warning
        int main()
        {
            putchar('H');
            putchar('\n');
            return 0;
        }";

        const string cHelloWorldWithError = @"
        #include <iostream>
        #warning some warning
        //using namespace std;
        int main()
        {
            cout<<'H'<<endl;
            return 0;
        }";
    }
}
