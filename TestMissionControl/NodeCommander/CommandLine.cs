using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Stratis.NodeCommander
{
    public static class CommandLine
    {
        public static void RunCommand(string processName, string arguments)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = true;
            startInfo.FileName = processName;
            startInfo.WindowStyle = ProcessWindowStyle.Minimized;
            startInfo.Arguments = arguments;

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
