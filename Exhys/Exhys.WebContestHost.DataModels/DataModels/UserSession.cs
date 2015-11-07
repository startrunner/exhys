using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    public class UserSession
    {
        [Key]
        public Guid Id { get; set; }

        public string UserAgentString { get; set; }

        public string BrowserName { get; set; }

        public string IPAdress { get; set; }

        [Required]
        public virtual UserAccount UserAccount { get; set; }
    }
}
