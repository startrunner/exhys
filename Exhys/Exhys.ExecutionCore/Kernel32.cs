using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.ExecutionCore
{
    class Kernel32
    {
        /// <summary>
        /// Sets the system's error mode
        /// </summary>
        /// <param name="wMode">new value of error mode</param>
        /// <returns>returns the value of the error mode before invoking this function</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int SetErrorMode (int wMode);
    }
}
