using DBreeze;

namespace Stratis.CoinmasterClient.Database
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
