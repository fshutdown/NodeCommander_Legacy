using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.CoinmasterClient.Messages
{
    public enum ActionType
    {
        StartNode,
        StopNode,
        DeleteFile,
        GitPull
    }
}
