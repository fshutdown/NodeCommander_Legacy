using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using NLog;
using Stratis.CoinmasterClient.Git;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinMasterAgent.Git
{
    public class GitRepository
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public GitRepositoryInfo Info { get; set; }

        public GitRepository()
        {

        }

        public GitRepository(string codeDirectoryFullName) : this()
        {
            Info = new GitRepositoryInfo(codeDirectoryFullName);
            RefreshGitRepositoryInfo();
        }


        public void ChangeBranch()
        {

        }

        public void Fetch()
        {
            RunProcess("git", "fetch");
            RefreshGitRepositoryInfo();
        }

        public void Pull()
        {

        }

        public void Patch()
        {

        }

        public void Revert()
        {

        }

        private string RunProcess(string processName, string arguments)
        {
            logger.Trace($"Calling \"{processName} {arguments}\"");
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.FileName = processName;
            startInfo.WindowStyle = ProcessWindowStyle.Minimized;
            startInfo.Arguments = arguments;
            startInfo.WorkingDirectory = Info.RepositoryFullName;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;

            try
            {
                Process process = Process.Start(startInfo);
                string processOutput = process.StandardOutput.ReadToEnd();
                process.StandardOutput.Close();

                return processOutput;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error while calling \"{processName} {arguments}\": {ex.Message}");
                return string.Empty;
            }
        }

        public void RefreshGitRepositoryInfo()
        {
            SetCurrentBranchName();
            SetRepositoryUrl();
            SetLatestLocalCommit();
            SetCommitDifference();
        }

        private void SetRepositoryUrl()
        {
            string gitCommandOutput = string.Empty;
            try
            {
                gitCommandOutput = RunProcess("git", "config --get remote.origin.url");
                Info.RepositoryUrl = gitCommandOutput.Trim();
            }
            catch (Exception ex)
            {
                logger.Warn(ex, $"Cannot get repository URL: {gitCommandOutput}");
            }
        }

        private void SetLatestLocalCommit()
        {
            string gitCommandOutput = string.Empty;
            try
            {
                gitCommandOutput = RunProcess("git", "log -n 1 master --format=format:\"%an||%ci\"");
                string[] gitCommandOutputParts = gitCommandOutput.Split("||");

                Info.LatestLocalCommitAuthor = gitCommandOutputParts[0];
                Info.LatestLocalCommitDateTime = DateTime.ParseExact(gitCommandOutputParts[1].Trim(), "yyyy-MM-dd HH:mm:ss K", null);
            }
            catch (Exception ex)
            {
                logger.Warn(ex, $"Cannot get date of the latest local commit: {gitCommandOutput}");
            }
        }

        private void SetCommitDifference()
        {
            string gitCommandOutput = string.Empty;
            try
            {
                gitCommandOutput = RunProcess("git", "git for-each-ref --format=\"%(push:track)\" refs/heads");
                Info.CommitDifference = gitCommandOutput.Trim();
            }
            catch (Exception ex)
            {
                logger.Warn(ex, $"Cannot get number of comits behind the remote branch: {gitCommandOutput}");
            }
        }

        private void SetCurrentBranchName()
        {
            string gitCommandOutput = string.Empty;
            try
            {
                gitCommandOutput = RunProcess("git", "rev-parse --abbrev-ref HEAD");
                Info.CurrentBranchName = gitCommandOutput.Trim();
            }
            catch (Exception ex)
            {
                logger.Warn(ex, $"Cannot get the name of the current branch: {gitCommandOutput}");

            }
        }
    }
}
