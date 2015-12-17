using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exhys.ExecutionCore.Contracts;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace CscCSharp
{
    [Export(typeof(ICompiler))]
    public class Csc : ICompiler
    {
        public string LanguageAlias { get { return @"c#"; } } 

        public string LanguageName { get { return @"C#"; } }

        public CompilationResult Compile (string sourceCode)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters()
            {
                GenerateExecutable = true
            };
            var results = provider.CompileAssemblyFromSource(parameters, sourceCode);
            //if(results.error)
            StringBuilder outputBuilder = new StringBuilder();
            foreach(var error in results.Errors)
            {
                outputBuilder.AppendLine(error.ToString());
            }
            if(results.Errors.HasErrors)
            {
                return new CompilationResult(outputBuilder.ToString());
            }
            else
            {
                return new CompilationResult(results.PathToAssembly, outputBuilder.ToString(), true);
            }
        }
    }
}
