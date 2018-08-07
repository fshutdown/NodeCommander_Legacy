﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Analysis;

namespace Stratis.CoinmasterClient.Network
{
    public class BlockchainNodeState
    {
        public NodeEndpointName NodeEndpoint { get; set; }
        public NodeProcessState NodeProcessState { get; set; }
        public NodeDeploymentState NodeDeploymentState { get; set; }
        public NodeOperationState NodeOperationState { get; set; }
        public NodeLogState NodeLogState { get; set; }

        public Dictionary<String, Guid> Resources { get; set; }

        public bool Initialized { get; set; }

        public bool OrphanNode { get; set; }

        public BlockchainNodeState(NodeEndpointName nodeEndpoint)
        {
            NodeEndpoint = nodeEndpoint;
            Initialized = false;

            NodeProcessState = new NodeProcessState();
            NodeDeploymentState = new NodeDeploymentState();
            NodeOperationState = new NodeOperationState();
            NodeLogState = new NodeLogState();

            Resources = new Dictionary<string, Guid>();
        }

    }
}
