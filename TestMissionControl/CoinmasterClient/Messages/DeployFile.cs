using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.CoinmasterClient.Messages
{
    public class DeployFile
    {
        public String Path { get; set; }
        public String FileName { get; set; }
        public long Size { get; set; }
        public Byte[] Data { get; set; }
    }
}
