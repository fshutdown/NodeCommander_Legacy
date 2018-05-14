using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.CoinmasterClient.Messages
{
    public class Resource
    {
        public Guid ResourceId { get; set; }
        public String FullName { get; set; }
        public long Size { get; set; }
        public Byte[] Data { get; set; }
        public int Length { get; set; }
        public bool EndOfData { get; set; }
        public string FullNodeName { get; set; }

        public Resource()
        {
            ResourceId = Guid.NewGuid();
            EndOfData = false;
        }
    }
}
