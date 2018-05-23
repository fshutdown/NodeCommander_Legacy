using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.CoinmasterClient.Git
{
    public class GitRepositoryInfo
    {
        public string RepositoryFullName { get; set; }
        public string CurrentBranchName { get; set; }
        public string RepositoryUrl { get; set; }
        public DateTime LatestLocalCommitDateTime { get; set; }
        public string CommitDifference { get; set; }
        public string LatestLocalCommitAuthor { get; set; }

        public GitRepositoryInfo()
        {
            CurrentBranchName = "Unknown";
            RepositoryUrl = "Unknown";
            LatestLocalCommitDateTime = DateTime.MinValue;
            LatestLocalCommitAuthor = "Unknown";
            CommitDifference = "Unknown";
        }

        public GitRepositoryInfo(string codeDirectoryFullName) : this()
        {
            RepositoryFullName = codeDirectoryFullName;
        }
    }
}
