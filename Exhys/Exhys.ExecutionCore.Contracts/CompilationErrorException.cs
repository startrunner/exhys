using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.ExecutionCore.Contracts
{
    public class CompilationErrorException:Exception
    {
        public string CompilerOutput { get; private set; }
        public CompilationErrorException(string compilerOut)
        {
            CompilerOutput = compilerOut;
        }
    }
}
