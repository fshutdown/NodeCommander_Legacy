using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;

namespace Stratis.NodeCommander.Tools
{
    public static class TortoiseGit
    {
        public static string TortoiseGitProc = @"C:\Program Files\TortoiseGit\bin\TortoiseGitProc.exe";
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static void ShowSwitchBranchDialog(string path)
        {
            RunCommnd(path, "switch");
        }

        private static void RunCommnd(string path, string command)
        {
            if (!File.Exists(TortoiseGitProc))
            {
                MessageBox.Show($"It looks like the TortoiseGIT is not installed.\n\nFile {TortoiseGitProc} has not been found", "TortoiseGit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            logger.Trace($"Executing {command} TortoiseGit Utility");
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = false;
            startInfo.FileName = TortoiseGitProc;
            startInfo.WindowStyle = ProcessWindowStyle.Minimized;
            startInfo.Arguments = $"/path:\"{path}\" /command:{command}";
            startInfo.WorkingDirectory = path;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;

            try
            {
                Process process = Process.Start(startInfo);
                string processOutput = process.StandardOutput.ReadToEnd();
                process.StandardOutput.Close();

                if (!string.IsNullOrEmpty(processOutput))
                {
                    MessageBox.Show($"{TortoiseGitProc} returned\n\n{processOutput}", "TortoiseGit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error while executing {TortoiseGitProc} with arguments \"{startInfo.Arguments} \": {ex.Message}");
            }
        }
    }
}
