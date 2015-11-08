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
        string LanguageName { get; }

        string LanguageAlias { get; }

        /// <returns>File path to compiled executable</returns>
        //string Compile (string sourceCode);
          
        CompilationResult Compile (string sourceCode);
    }
}
