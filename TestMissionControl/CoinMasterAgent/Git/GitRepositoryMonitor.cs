using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using NLog;
using Stratis.CoinmasterClient.Git;
using Stratis.CoinMasterAgent.Agent;

namespace Stratis.CoinMasterAgent.Git
{
    public class GitRepositoryMonitor
    {
        public Dictionary<String, GitRepository> GitRepositories { get; set; }
        public const int RefreshRepositoryInterval = 60000;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private Timer _jobScheduler;


        public GitRepositoryMonitor(List<String> codeDirecotryList)
        {
            GitRepositories = new Dictionary<string, GitRepository>();
            _jobScheduler = new Timer(RefreshRepositoryInterval);
            _jobScheduler.Elapsed += RefreshRepository;
            _jobScheduler.AutoReset = true;

            foreach (string codeDirectoryFullName in codeDirecotryList)
            {
                GitRepository repository = new GitRepository(codeDirectoryFullName);
                GitRepositories.Add(codeDirectoryFullName, repository);
            }
        }

        public void Start()
        {
            _jobScheduler.Interval = 100;
            _jobScheduler.Start();

        }

        public void Stop()
        {
            _jobScheduler.Enabled = false;
            _jobScheduler.Stop();
        }

        public void UpdateRepositoryList(List<String> codeDirecotryList)
        {
            foreach (string codeDirectoryFullName in GitRepositories.Keys.ToList())
            {
                if (!codeDirecotryList.Contains(codeDirectoryFullName))
                {
                    GitRepositories.Remove(codeDirectoryFullName);
                }
            }

            foreach (string codeDirectoryFullName in codeDirecotryList)
            {
                if (!GitRepositories.ContainsKey(codeDirectoryFullName))
                {
                    GitRepository repository = new GitRepository(codeDirectoryFullName);
                    GitRepositories.Add(codeDirectoryFullName, repository);
                }
            }
        }

        private void RefreshRepository(object sender, ElapsedEventArgs e)
        {
            _jobScheduler.Interval = RefreshRepositoryInterval;
            _jobScheduler.Stop();

            foreach (GitRepository gitRepository in GitRepositories.Values)
            {
                gitRepository.Fetch();
            }

            _jobScheduler.Start();
        }

        public List<GitRepositoryInfo> GetGitRepositoryInformation()
        {
            List<GitRepositoryInfo> repositoryInfoList = (from r in GitRepositories
                select r.Value.Info).ToList();
            return repositoryInfoList;
        }
    }
}
