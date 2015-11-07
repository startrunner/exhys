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
    public static class CompilerFactory
    {
        private static string folderPath = @"C:\ExhysCompilers";
        public static ICompiler Generate(string languageAlias)
        {
            List<string> dllPaths = Directory.GetFiles(folderPath)
                .Where(p => p.EndsWith(".dll"))
                .ToList();
            foreach (string path in dllPaths)
            {
                Assembly asm = null;
                List<TypeInfo> asmCompilerTypes = null;
                try
                {
                    asm = Assembly.LoadFile(path);
                    asmCompilerTypes = asm.DefinedTypes
                        .Where(t => t.ImplementedInterfaces.Contains(typeof(ICompiler)))
                        .ToList();
                    List<ICompiler> asmCompilers = new List<ICompiler>();
                }
                catch
                {
                    //Unable to load assembly... going to next one...
                    continue;
                }

                foreach (TypeInfo ti in asmCompilerTypes)
                {
                    ICompiler compiler = null;
                    try
                    {
                        compiler = ti.GetConstructor(new Type[] { }).Invoke(new object[] { }) as ICompiler;
                        if (compiler.LanguageAlias == languageAlias) return compiler;
                    }
                    catch
                    {
                        //Unable to construct compiler
                        continue;
                    }
                }
            }

            return null;
        }
    }
}
