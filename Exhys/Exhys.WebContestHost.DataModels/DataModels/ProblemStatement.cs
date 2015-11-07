using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    public class ProblemStatement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public byte[] Bytes { get; set; }

        [Required]
        public string Filename { get; set; }

        [Required]
        public virtual Problem Problem { get; set; }
    }
}
