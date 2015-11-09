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
            SubmissionDto submission = execution.Submission;
            ICompiler compiler = CompilerFactory.Get(submission.LanguageAlias);
            Debug.WriteLine(compiler != null ? compiler.GetType().ToString() : "kompilator na maika ti v putkata kompilator");
            CompilationResult compilationResult = compiler.Compile(submission.SourceCode);
            List<TestResultDto> testResults = null;

            /*if (compilationResult.IsSuccessful)
            {
                TestRunner testRunner = new TestRunner(compilationResult.ExecutablePath, submission.Tests);
                testResults = testRunner.Run();
            }*/
            Debug.WriteLine("maika ti ebah pedal mrusen");
            return new ExecutionResultDto()
            {
                ExecutionId = execution.Id,
                TestResults = null,//testResults,
                CompilerOutput = compilationResult.Output,
                IsExecutionSuccessful = compilationResult.IsSuccessful
            };
        }
    }
}
