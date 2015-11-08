using System;
using System.Diagnostics;
using System.IO;
using Exhys.ExecutionCore.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Environment;

namespace Exhys.ExecutionCore.Tests
{
    class MiniGW : ICompiler
    {
        public string LanguageAlias { get { return "c++"; } }

        public string LanguageName { get { return "C++ 2011"; } }

        const string SourceFileExtension = ".cpp";
        const string ExecutableFileExtension = ".exe";

        public CompilationResult Compile (string sourceCode)
        {
            if(GetFullFilePath("g++.exe")==null)
            {
                return new CompilationResult("Compiler Dependency requirement not fulfulled");
            }

            string srcFile = Path.GetTempFileName().Replace(".tmp", SourceFileExtension);
            string executablePath = Path.GetTempFileName().Replace(".tmp", ExecutableFileExtension);
            if (!srcFile.EndsWith(SourceFileExtension)) srcFile += SourceFileExtension;
            if (!executablePath.EndsWith(ExecutableFileExtension)) executablePath += ExecutableFileExtension;

            File.WriteAllText(srcFile, sourceCode);

            ProcessStartInfo psi = new ProcessStartInfo("g++.exe", string.Format("{0} -std=c++11 -O3 -o {1}", srcFile, executablePath));
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            Process p = Process.Start(psi);
            p.WaitForExit();

            string compilerOutput = string.Format("{0}\n{1}", p.StandardOutput.ReadToEnd(), p.StandardError.ReadToEnd());

            //Dem peasants shall not try to hack our file system
            compilerOutput = compilerOutput.Replace(srcFile, "[src file]").Replace(executablePath, "[exe file]");

            if (!File.Exists(executablePath))
            {
                return new CompilationResult(compilerOutput);
            }

            return new CompilationResult(executablePath, compilerOutput, true);
        }

        private static string GetFullFilePath (string fileName)
        {
            if (File.Exists(fileName))return Path.GetFullPath(fileName);
            foreach (var path in GetEnvironmentVariable("PATH").Split(';'))
            {
                var fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))return fullPath;
            }
            return null;
        }

        public MiniGW () { }
    }

    [TestClass]
    public class ExecutionCore_Tests
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
        public void TestGnuGppHelloWorldWithError()
        {
            ICompiler gpp = new MiniGW();
            var program=gpp.Compile(cppHelloWorldWithError);

            if(program.IsSuccessful)throw new Exception("Shoud've yelded an error.");
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
    };
}
