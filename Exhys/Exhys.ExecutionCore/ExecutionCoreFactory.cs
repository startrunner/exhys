using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Exhys.ExecutionCore.Contracts;

namespace Exhys.ExecutionCore
{
    public static class ExecutionCoreFactory
    {
        public static IExecutionCore Generate()
        {
            return new ExecutionCore();
        }
    }
}
