using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exhys.ExecutionCore.Contracts;
using Exhys.SubmissionRouter.Dtos;

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
            SubmissionDto submission = execution.Submission;
            ICompiler compiler = CompilerFactory.Get(submission.LanguageAlias);
            CompilationResult compilationResult = compiler.Compile(submission.SourceCode);
            List<TestResultDto> testResults = null;

            if (compilationResult.IsSuccessful)
            {
                TestRunner testRunner = new TestRunner(compilationResult.ExecutablePath, submission.Tests);
                testResults = testRunner.Run();
            }

            return new ExecutionResultDto()
            {
                ExecutionId = execution.Id,
                TestResults = testResults,
                CompilerOutput = compilationResult.Output,
                IsExecutionSuccessful = compilationResult.IsSuccessful
            };
        }
    }
}
