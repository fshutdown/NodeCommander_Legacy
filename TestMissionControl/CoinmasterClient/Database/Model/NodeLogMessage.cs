using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Stratis.CoinmasterClient.Database.Model
{
    public class NodeLogMessage
    {
        public long Id { get; set; }
        public DateTime Timestamp { get; set; }
        public String FullNodeName { get; set; }

        public int Thread { get; set; }
        public String Level { get; set; }
        public String CallSite { get; set; }
        public String Message { get; set; }
        public String Exception { get; set; }
        public String Stacktrace { get; set; }

        public override string ToString()
        {
            return Message;
        }
    }
}
