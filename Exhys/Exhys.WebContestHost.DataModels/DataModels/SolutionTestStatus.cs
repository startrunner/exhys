﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    public class SolutionTestStatus
    {
        [Key]
        public int Id { get; set; }
        
        public string Output { get; set; }

        [Required]
        public double Score { get; set; }

        [Required]
        public TestStatus Status { get; set; }

        [Required]
        public ProblemTest ProblemTest { get; set; }

        public enum TestStatus
        {
            SegmentationFault,
            WrongAnswer,
            CorrectAnswer,
            Timeout
        }
    }

}
