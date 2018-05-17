using System;
using System.Collections.Generic;
using System.IO;
using DBreeze;
using DBreeze.Objects;
using DBreeze.Utils;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Database.Model;

namespace Stratis.CoinmasterClient.Database
{
    public class DatabaseConnection
    {
        public DBreezeEngine Engine = null;
        public DBreezeConfiguration EngineConfiguration = null;

        public DatabaseConnection()
        {
            EngineConfiguration = new DBreezeConfiguration()
            {
                DBreezeDataFolderName = Path.Combine(NodeCommanderConfig.NodeCommanderDataDirectory, "dBreeze"),
            };
            Engine = new DBreezeEngine(EngineConfiguration);

            CustomSerializator.ByteArraySerializator = (object o) => { return NetJSON.NetJSON.Serialize(o).To_UTF8Bytes(); };
            CustomSerializator.ByteArrayDeSerializator = (byte[] bt, Type t) => { return NetJSON.NetJSON.Deserialize(t, bt.UTF8_GetString()); };
        }

        public void PersistHeight(BlockchainHeight blockchainHeight)
        {
            try
            {
                using (var t = Engine.GetTransaction())
                {
                    t.SynchronizeTables("BlockchainHeight");

                    bool newEntity = blockchainHeight.Id == 0;
                    if (newEntity)
                        blockchainHeight.Id = t.ObjectGetNewIdentity<long>("BlockchainHeight");

                    t.ObjectInsert("BlockchainHeight", new DBreezeObject<BlockchainHeight>
                    {
                        NewEntity = newEntity,
                        Entity = blockchainHeight,
                        Indexes = new List<DBreezeIndex>
                        {
                            new DBreezeIndex(1, blockchainHeight.Id) { PrimaryIndex = true },
                            new DBreezeIndex(2, blockchainHeight.Timestamp),
                            new DBreezeIndex(3, blockchainHeight.FullNodeName),
                        }
                    }, false);

                    t.TextInsert("TS_BlockchainHeight", blockchainHeight.Id.ToBytes(), blockchainHeight.FullNodeName);

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String GetRecordCount(string fullNodeName)
        {
            int count = 0;
            try
            {
                using (var t = Engine.GetTransaction())
                {
                    foreach (byte[] doc in t.TextSearch("TS_BlockchainHeight").BlockAnd(fullNodeName).GetDocumentIDs())
                    {
                        DBreezeObject<BlockchainHeight> obj = t.Select<byte[], byte[]>("BlockchainHeight", 1.ToIndex(doc)).ObjectGet<BlockchainHeight>();
                        if (obj != null)
                        {
                            count++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return count.ToString();
        }
    }
}


