using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exhys.ExecutionCore.Contracts;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace Exhys.ExecutionCore
{
    public class MefCompilerFactory : ICompilerFactory
    {
        Dictionary<string,ICompiler> compilers;
        public MefCompilerFactory()
        {
            LoadAllCompilers();
        }

        private void LoadAllCompilers()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(".\\compilers"));
            CompositionContainer container = new CompositionContainer(catalog);
            IEnumerable<Lazy<ICompiler>> loadedLazyCompilers = container.GetExports<ICompiler>();
            compilers = loadedLazyCompilers.ToDictionary(x => x.Value.LanguageAlias, x=>x.Value);
        }

        public ICompiler Get(string languageAlias)
        {
            if (compilers.ContainsKey(languageAlias))
            {
                return compilers[languageAlias];
            }
            else
            {
                return null;
            }
        }
    }
}
