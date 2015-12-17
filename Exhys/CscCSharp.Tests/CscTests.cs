using System;
using System.Diagnostics;
using Exhys.ExecutionCore;
using Exhys.ExecutionCore.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CscCSharp.Tests
{
    [TestClass]
    public class CscTests
    {
        [TestMethod]
        public void TestCsHelloWorld ()
        {
            Debug.WriteLine("Shit");
            ICompiler csc = CompilerFactory.Get("c#");// new Csc();

            CompilationResult helloWorldProgram = csc.Compile(csHelloWorld);

            Process testProcess = Process.Start(new ProcessStartInfo(helloWorldProgram.ExecutablePath) { UseShellExecute = false, RedirectStandardOutput = true });
            testProcess.WaitForExit();

            string programOut = testProcess.StandardOutput.ReadToEnd();
            if (!programOut.Contains("H")) throw new Exception("Compiler Failed");
        }

        [TestMethod]
        public void TestCsHelloWorldWithError ()
        {
            ICompiler csc = new Csc();
            var program = csc.Compile(csHelloWorldWithError);

            if (program.IsSuccessful) throw new Exception("Shoud've yelded an error.");
        }

        const string csHelloWorld = @"
        namespace HelloWorld
        {
            using System;
            class Program
            {
                static void Main (string[] args)
                {
                    Console.WriteLine('H');
                }
            }
        }
        ";

        const string csHelloWorldWithError = @"
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
