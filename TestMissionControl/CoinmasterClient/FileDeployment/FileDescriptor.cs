using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.CoinmasterClient.FileDeployment
{
    public class FileDescriptor
    {
        public ResourceScope Scope { get; set; }
        public String FullNodeName { get; set; }
        public String LocalPath { get; set; }
        public String RemotePath { get; set; }

        public FileDescriptor(ResourceScope scope, string fullNodeName)
        {
            Scope = scope;
            FullNodeName = fullNodeName;
        }
    }
}
