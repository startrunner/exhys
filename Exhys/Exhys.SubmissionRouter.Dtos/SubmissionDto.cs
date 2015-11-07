using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.SubmissionRouter.Dtos
{
    public class SubmissionDto
    {
        public string LanguageAlias { get; set; }
        public string SourceCode { get; set; }
        public IList<TestDto> Tests { get; set; }
    }
}
