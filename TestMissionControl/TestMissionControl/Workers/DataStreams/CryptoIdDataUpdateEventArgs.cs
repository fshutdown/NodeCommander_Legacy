using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.TestMissionControl.Workers.DataStreams
{
    public class CryptoIdDataUpdateEventArgs : EventArgs
    {
        public int BlockHeight { get; set; }
        public decimal CirculatingCoinsCount { get; set; }
        public decimal Difficulty { get; set; }
        public decimal Hashrate { get; set; }
        public decimal Nethashps { get; set; }
        public decimal Netmhashps { get; set; }
        public decimal Totalbc { get; set; }
        public decimal Totalcoins { get; set; }
    }
}
