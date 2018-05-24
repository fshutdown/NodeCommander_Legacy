using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NLog;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Database.Model;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinmasterClient.Resources;

namespace Stratis.CoinmasterClient.Client.Handlers
{
    public class ResourceFromAgentProcessor : RequestProcessorBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public List<Resource> ResourceList { get; set; }

        public ResourceFromAgentProcessor(AgentConnection client) : base(client)
        {
        }

        public override void OpenEnvelope()
        {
            try
            {
                ResourceList = Message.GetPayload<List<Resource>>();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Cannot deserialize Resource List message from agent {Client.Address}");
                return;
            }

            logger.Info($"Received Resource List from agent {Client.Address}");
        }

        public override void Process()
        {
            foreach (Resource resource in ResourceList)
            {
                if (resource.Length == 0) continue;
                string resourcePath = Path.Combine(ClientConfigReader.NodeCommanderDataDirectory, "Data", resource.ResourceId.ToString());

                if (resource.ResourceName == ResourceType.NodeCommanderLog)
                {
                    string lines = Encoding.Default.GetString(resource.Data);
                    foreach (string line in lines.Split(new string[] {"\r", "\n"}, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string[] fields = line.Split(new string[] { "||" }, StringSplitOptions.None);

                        if (fields.Length == 5 && fields[3] == "Stratis.Bitcoin.Features.Miner.PowMining.GenerateBlocks")
                        {
                            Regex miningRegex = new Regex("([0-9]+)-([0-9a-f]+)");
                            Match match = miningRegex.Match(fields[4]);

                            BlockchainMining miningEntry = new BlockchainMining();
                            miningEntry.Timestamp = DateTime.ParseExact(fields[0], "yyyy-MM-dd HH:mm:ss.ffff", null);
                            miningEntry.FullNodeName = resource.FullNodeName;
                            miningEntry.BlockNumber = int.Parse(match.Groups[1].Value);
                            miningEntry.BlockHash = match.Groups[2].Value;

                            Client.Session.Database.Persist(miningEntry);
                        }
                        else if (fields.Length == 5 && fields[3] == "Stratis.Bitcoin.Features.Consensus.ConsensusLoop.RewindCoinViewLockedAsync")
                        {
                            Regex reorgRegex = new Regex("([0-9]+)-([0-9a-f]+)' to '([0-9]+)-([0-9a-f]+)");
                            Match match = reorgRegex.Match(fields[4]);

                            BlockchainReorg reorgEntry = new BlockchainReorg();
                            reorgEntry.Timestamp = DateTime.ParseExact(fields[0], "yyyy-MM-dd HH:mm:ss.ffff", null);
                            reorgEntry.FullNodeName = resource.FullNodeName;
                            reorgEntry.FromBlockNumber = int.Parse(match.Groups[1].Value);
                            reorgEntry.FromBlockHash = match.Groups[2].Value;
                            reorgEntry.ToBlockNumber = int.Parse(match.Groups[3].Value);
                            reorgEntry.ToBlockHash = match.Groups[4].Value;

                            Client.Session.Database.Persist(reorgEntry);
                        }
                    }
                }

                FileStream f = new FileStream(resourcePath, FileMode.Append);
                f.Write(resource.Data, 0, resource.Length);
                f.Close();
            }
        }
    }
}
