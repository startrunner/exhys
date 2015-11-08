using System;
using System.Diagnostics;
using Exhys.ExecutionCore.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniGWCpp11.Tests
{
    [TestClass]
    public class MiniGWCpp11Tests
    {
        [TestMethod]
        public void TestGnuGppHelloWorld ()
        {
            Debug.WriteLine("Shit");
            ICompiler gpp = new MiniGW();

            CompilationResult helloWorldProgram = gpp.Compile(cppHelloWorld);

            Process testProcess = Process.Start(new ProcessStartInfo(helloWorldProgram.ExecutablePath) { UseShellExecute = false, RedirectStandardOutput = true });
            testProcess.WaitForExit();

            string programOut = testProcess.StandardOutput.ReadToEnd();
            if (!programOut.Contains("H")) throw new Exception("Compiler Failed");
        }

        [TestMethod]
        public void TestGnuGppHelloWorldWithError ()
        {
            ICompiler gpp = new MiniGW();
            var program = gpp.Compile(cppHelloWorldWithError);

            if (program.IsSuccessful) throw new Exception("Shoud've yelded an error.");
        }

        const string cppHelloWorld = @"
        #include <iostream>
        #warning some warning
        using namespace std;
        int main()
        {
            cout<<'H'<<endl;;;
            return 0;
        }";

        const string cppHelloWorldWithError = @"
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
