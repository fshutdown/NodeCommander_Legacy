using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.CoinmasterClient.Analysis
{
    public class LogMessagesResource
    {
        public Dictionary<String, int> WarningMessages { get; set; }
        public Dictionary<String, int> ErrorMessages { get; set; }
        public Dictionary<String, int> CriticalMessages { get; set; }
        public Dictionary<String, int> FailMessages { get; set; }


        public LogMessagesResource()
        {
            WarningMessages = new Dictionary<string, int>();
            ErrorMessages = new Dictionary<string, int>();
            CriticalMessages = new Dictionary<string, int>();
            FailMessages = new Dictionary<string, int>();
        }
    }
}
