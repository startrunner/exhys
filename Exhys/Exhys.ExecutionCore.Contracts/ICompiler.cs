using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.ExecutionCore.Contracts
{
    public interface ICompiler
    {
        /// <summary>
        /// The official, descriptive name of the programming language
        /// e.g C#, Java, C++ 11, Objective-C
        /// </summary>
        string LanguageName { get; }

        /// <summary>
        /// The short name of the programming language, in lower case
        /// e.g c++, java, c, c#
        /// </summary>
        string LanguageAlias { get; }
        
        /// <summary>
        /// Invokes the compiler for the language
        /// </summary>
        /// <param name="sourceCode">Source code in supported language</param>
        /// <returns></returns>
        CompilationResult Compile (string sourceCode);
    }
}
