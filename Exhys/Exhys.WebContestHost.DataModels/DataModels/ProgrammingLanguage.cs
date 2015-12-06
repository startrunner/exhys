using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels.DataModels
{
    public class ProgrammingLanguage
    {
        [Key]
        //[MinLength(DatabaseLimits.LanguageAlias_MinLength)]
        [MaxLength(DatabaseLimits.LanguageAlias_MaxLength)]
        public string Alias { get; set; }

        [Required]
        [MaxLength(DatabaseLimits.LanguageName_MaxLength)]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
