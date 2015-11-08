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

        public string LanguageName { get { return "C++ (1998)"; } }

        const string SourceFileExtension = ".cpp";
        const string ExecutableFileExtension = ".exe";

        public CompilationResult Compile (string sourceCode)
        {

            if(GetFullFilePath("g++.exe")==null)
            { 
                throw new CompilatorDependencyNotFulfilledException("MiniGW g++.exe not found. Please add the MiniGW/bin folder path to your system's PATH variable.");
            }

            string srcFile = Path.GetTempFileName().Replace(".tmp", SourceFileExtension);
            string executablePath = Path.GetTempFileName().Replace(".tmp", ExecutableFileExtension);
            if (!srcFile.EndsWith(SourceFileExtension)) srcFile += SourceFileExtension;
            if (!executablePath.EndsWith(ExecutableFileExtension)) executablePath += ExecutableFileExtension;

            File.WriteAllText(srcFile, sourceCode);

            ProcessStartInfo psi = new ProcessStartInfo("g++.exe", string.Format("{0} -o {1}", srcFile, executablePath));
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
                throw new CompilationErrorException(compilerOutput);
            }

            string s = p.StandardOutput.ReadToEnd();

            return new CompilationResult(executablePath, compilerOutput);
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
            try
            {
                gpp.Compile(cppHelloWorldWithError);
            }
            catch(CompilationErrorException e)
            {
                Debug.WriteLine(e.CompilerOutput);
                return;
            }
            throw new Exception("Shoud've yelded an error.");
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
