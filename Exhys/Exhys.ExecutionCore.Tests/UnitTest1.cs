using System;
using System.Diagnostics;
using System.IO;
using Exhys.ExecutionCore.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Environment;

namespace Exhys.ExecutionCore.Tests
{
    class GnuGpp : ICompiler
    {
        public string LanguageAlias { get { return "c++"; } }

        public string LanguageName { get { return "C++ (1998)"; } }

        const string gppPath = @"C:\MiniGW\bin\g++.exe";
        const string objFile = @"C:\\Users\\Alexander\\Desktop\\cppCompilerTest.o";

        public string Compile (string sourceCode)
        {
            string srcFile = Path.GetTempFileName();
            File.WriteAllText(srcFile, sourceCode);

            ProcessStartInfo psi = new ProcessStartInfo(gppPath, string.Format("-c {0} -o {1}", srcFile, objFile));
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;


            Process p = Process.Start(psi);

            string s = p.StandardOutput.ReadToEnd();

            return "";
        }

        public GnuGpp()
        {

        }
    }

    [TestClass]
    public class ExecutionCore_Tests
    {
        [TestMethod]
        public void CompilerTest1 ()
        {
            Debug.WriteLine("Shit");
            ICompiler gpp = new GnuGpp();
            string f = gpp.Compile(cppHelloWorld);
        }

        const string cppHelloWorld = @"
        #include <iostream>
        using namespace std;
        int main()
        {
            cout<<'H'<<endl;
            return 0;
        }";
    };
}
