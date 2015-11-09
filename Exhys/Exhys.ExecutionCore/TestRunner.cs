using System;
using System.Collections.Generic;
using System.Diagnostics;
using Exhys.SubmissionRouter.Dtos;

namespace Exhys.ExecutionCore
{
    public class TestRunner
    {
        private string ExecutablePath { get; set; }
        private IList<TestDto> Tests { get; set; }

        public TestRunner (string executablePath, IList<TestDto> tests)
        {
            this.ExecutablePath = executablePath;
            this.Tests = tests;
        }

        public List<TestResultDto> Run ()
        {
            List<TestResultDto> rt = new List<TestResultDto>();
            foreach (TestDto test in Tests)
            {
                rt.Add(RunSingleTest(test));
            }
            return rt;
        }

        private TestResultDto RunSingleTest (TestDto test)
        {
            /*
            http://stackoverflow.com/questions/673036/
            The windows error reporting dialog will keep the process 'running'
            until closed. this makes it impossible for the TestRunner to
            distinguish between TimeOut and SegmentationFault status.
            This temporaryly disables Windows Error reporting for the duration
            of this method's execution.
            */
            int oldErrorMode = Kernel32.SetErrorMode(3);

            Process proc = Process.Start(new ProcessStartInfo(ExecutablePath)
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                ErrorDialog = false,
            });

            proc.StandardInput.WriteLine(test.Input);
            proc.WaitForExit((int)(test.TimeLimit * 1000));

            if (proc.HasExited)
            {
                string output = proc.StandardOutput.ReadToEnd();
                double executionTime = (DateTime.Now-proc.StartTime).TotalSeconds;

                if (proc.ExitCode == 0)
                {
                    //The program hasn't crashed
                    if (output.Trim() == test.Solution.Trim())
                    {
                        Kernel32.SetErrorMode(oldErrorMode);
                        return new TestResultDto()
                        {
                            ExecutionTime = executionTime,
                            Output = output,
                            Status = TestResultDto.ResultStatus.CorrectAnswer
                        };
                    }
                    else
                    {
                        Kernel32.SetErrorMode(oldErrorMode);
                        return new TestResultDto()
                        {
                            ExecutionTime = executionTime,
                            Output = output,
                            Status = TestResultDto.ResultStatus.WrongAnswer
                        };
                    }
                }
                else
                {
                    //A non-zero exit code usually means that the program has crashed.
                    Kernel32.SetErrorMode(oldErrorMode);
                    return new TestResultDto()
                    {
                        Output = null,
                        ExecutionTime = executionTime,
                        Status= TestResultDto.ResultStatus.SegmentationFault
                    };
                }
            }
            else
            {
                proc.Kill();
                Kernel32.SetErrorMode(oldErrorMode);
                return new TestResultDto()
                {
                    ExecutionTime = test.TimeLimit,
                    Output = null,
                    Status = TestResultDto.ResultStatus.TimeOut
                };
            }
        }
    }
}
