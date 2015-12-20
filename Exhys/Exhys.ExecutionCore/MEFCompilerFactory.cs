using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exhys.ExecutionCore.Contracts;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace Exhys.ExecutionCore
{
    public class CompilerFactory
    {
        CompilerFactory instance;
        static Dictionary<string,ICompiler> compilers;
        string basePath;
        public CompilerFactory(string basePath)
        {
            this.basePath = basePath;
            LoadAllCompilers();
        }

        private void LoadAllCompilers()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(basePath));
            CompositionContainer container = new CompositionContainer(catalog);
            IEnumerable<Lazy<ICompiler>> loadedLazyCompilers = container.GetExports<ICompiler>();
            compilers = loadedLazyCompilers.ToDictionary(x => x.Value.LanguageAlias, x=>x.Value);
        }

        public ICompiler Get(string languageAlias)
        {
            if (compilers.ContainsKey(languageAlias.ToLower()))
            {
                return compilers[languageAlias];
            }
            else
            {
                return null;
            }
        }

        public static CompilerFactory Initialize(string basePath)
        {
            Instance = new CompilerFactory(basePath);
            return Instance;
        }

        public static CompilerFactory Instance { get; private set; }
    }
}
