using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exhys.ExecutionCore.Contracts;
using Exhys.SubmissionRouter.Dtos;
using System.Diagnostics;

namespace Exhys.ExecutionCore
{
    public class ExecutionCore : IExecutionCore
    {
        public ExecutionResultDto Execute (ExecutionDto execution)
        {
            return ExecuteAsync(execution).GetAwaiter().GetResult();
        }

        public async Task<ExecutionResultDto> ExecuteAsync (ExecutionDto execution)
        {
            SubmissionDto submission;
            ICompiler compiler;
            CompilationResult compilationResult = null;
            List<TestResultDto> testResults = null;
            try
            {
                submission = execution.Submission;
                compiler = CompilerFactory.Instance.Get(submission.LanguageAlias);
                compilationResult = compiler.Compile(submission.SourceCode);

                if (compilationResult.IsSuccessful)
                {
                    TestRunner testRunner = new TestRunner(compilationResult.ExecutablePath, submission.Tests);
                    testResults = testRunner.Run();
                }
            }
            catch { }
            return new ExecutionResultDto()
            {
                ExecutionId = execution.Id,
                TestResults = testResults,
                CompilerOutput = compilationResult?.Output,
                IsExecutionSuccessful = compilationResult==null? false : compilationResult.IsSuccessful
            };
        }
    }
}
