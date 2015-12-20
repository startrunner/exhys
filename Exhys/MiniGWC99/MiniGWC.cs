using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exhys.ExecutionCore.Contracts;

namespace MiniGWC99
{
    [Export(typeof(ICompiler))]
    public class MiniGWC : ICompiler
    {
        public string LanguageAlias { get { return "c"; } }

        public string LanguageName { get { return "C 1999"; } }

        const string SourceFileExtension = ".c";
        const string ExecutableFileExtension = ".exe";

        public CompilationResult Compile (string sourceCode)
        {
            if (GetFullFilePath("gcc.exe") == null)
            {
                return new CompilationResult("Compiler Dependency requirement not fulfulled");
            }

            string srcFile = Path.GetTempFileName().Replace(".tmp", SourceFileExtension);
            string executablePath = Path.GetTempFileName().Replace(".tmp", ExecutableFileExtension);
            if (!srcFile.EndsWith(SourceFileExtension)) srcFile += SourceFileExtension;
            if (!executablePath.EndsWith(ExecutableFileExtension)) executablePath += ExecutableFileExtension;

            File.WriteAllText(srcFile, sourceCode);

            ProcessStartInfo psi = new ProcessStartInfo("gcc.exe", string.Format("{0} -std=c99 -O3 -o {1}", srcFile, executablePath));
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.RedirectStandardInput = true;
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
            if (File.Exists(fileName)) return Path.GetFullPath(fileName);
            foreach (var path in Environment.GetEnvironmentVariable("PATH").Split(';'))
            {
                var fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath)) return fullPath;
            }
            return null;
        }

        public MiniGWC () { }
    }
}
