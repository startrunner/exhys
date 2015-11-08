using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.ExecutionCore.Contracts
{
    public class CompilationResult
    {
        public string ExecutablePath { get; private set; }
        public string Output { get; private set; }
        public bool IsSuccessful { get; private set; }

        public CompilationResult(string exe, string output, bool isSuccessful)
        {
            ExecutablePath = exe;
            Output = output;
            IsSuccessful = isSuccessful;
        }

        public CompilationResult(string output):this(null, output, false) { }
    }
}
