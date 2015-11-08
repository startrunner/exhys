using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exhys.SubmissionRouter.Dtos;

namespace Exhys.ExecutionCore
{
    public class TestRunner
    {
        private string executablePath;
        private IList<TestDto> tests;

        public TestRunner(string executablePath, IList<TestDto> tests)
        {
            this.executablePath = executablePath;
            this.tests = tests;
        }

        internal static List<TestResultDto> Run()
        {
            throw new NotImplementedException();
        }
    }
}
