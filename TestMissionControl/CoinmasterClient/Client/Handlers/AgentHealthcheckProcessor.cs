using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Database.Model;
using Stratis.CoinmasterClient.Git;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinmasterClient.Client.Handlers
{
    public class AgentHealthcheckProcessor : RequestProcessorBase
    {
        public AgentHealthMessage AgentHealthMessage { get; set; }
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public AgentHealthcheckProcessor(AgentConnection agent) : base(agent)
        {
        }

        public override void OpenEnvelope()
        {
            try
            {
                AgentHealthMessage = Message.GetPayload<AgentHealthMessage>();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Cannot deserialize Agent Health State message from agent {Agent.Address}");
                return;
            }

            logger.Info($"Received AgentHealthState from agent {Agent.Address}");
        }

        public override void Process()
        {
            foreach (GitRepositoryInfo gitRepositoryInfo in AgentHealthMessage.AgentHealthState.GitRepositoryInfo)
            {
                string codeDirectory = gitRepositoryInfo.RepositoryFullName;
                foreach (BlockchainNode node in Agent.Session.ManagedNodes.Nodes.Values)
                {
                    if (node.NodeConfig.CodeDirectory == codeDirectory)
                    {
                        node.GitRepositoryInfo = gitRepositoryInfo;
                    }
                }
            }

            Agent.Session.OnAgentHealthcheckStatsUpdated(Agent, AgentHealthMessage.AgentHealthState, string.Empty);
        }
    }
}
