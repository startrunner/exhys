﻿using SubmissionRouterService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionRouterService.Model
{
    public class SubmissionCompletedEventArgs
    {
        public SubmissionResultDto SubmissionResult { get; set; }
    }
}
