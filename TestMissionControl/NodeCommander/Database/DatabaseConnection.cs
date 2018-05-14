using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBreeze;

namespace Stratis.NodeCommander.Database
{
    public class DatabaseConnection
    {
        public static DBreezeEngine Engine = null;

        public DatabaseConnection()
        {
            Engine = new DBreezeEngine("dBreeze");
        }

    }
}
