using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.ExecutionCore.Contracts
{
    public struct CompilationResult
    {
        public string ExecutablePath { get; private set; }
        public string CompilerOutput { get; private set; }
        public CompilationResult(string exe, string compOut)
        {
            ExecutablePath = exe;
            CompilerOutput = compOut;
        }
    }
}
