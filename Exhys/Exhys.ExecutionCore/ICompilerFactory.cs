using Exhys.ExecutionCore.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.ExecutionCore
{
    public interface ICompilerFactory
    {
        ICompiler Get(string languageAlias);
    }
}
