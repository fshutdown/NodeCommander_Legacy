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
                DBreezeDataFolderName = Path.Combine(ClientConfigReader.NodeCommanderDataDirectory, "dBreeze"),
            };
            Engine = new DBreezeEngine(EngineConfiguration);

            CustomSerializator.ByteArraySerializator = (object o) => { return NetJSON.NetJSON.Serialize(o).To_UTF8Bytes(); };
            CustomSerializator.ByteArrayDeSerializator = (byte[] bt, Type t) => { return NetJSON.NetJSON.Deserialize(t, bt.UTF8_GetString()); };

            Engine.Scheme.DeleteTable("BlockchainHeight");
            Engine.Scheme.DeleteTable("TS_BlockchainHeight");
            Engine.Scheme.DeleteTable("Mining");
            Engine.Scheme.DeleteTable("TS_Mining");
            Engine.Scheme.DeleteTable("Reorg");
            Engine.Scheme.DeleteTable("TS_Reorg");
        }

        public void Persist(BlockchainHeight blockchainHeight)
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

        public void Persist(BlockchainMining miningEntry)
        {
            try
            {
                using (var t = Engine.GetTransaction())
                {
                    t.SynchronizeTables("Mining");

                    bool newEntity = miningEntry.Id == 0;
                    if (newEntity)
                        miningEntry.Id = t.ObjectGetNewIdentity<long>("Mining");

                    t.ObjectInsert("Mining", new DBreezeObject<BlockchainMining>
                    {
                        NewEntity = newEntity,
                        Entity = miningEntry,
                        Indexes = new List<DBreezeIndex>
                        {
                            new DBreezeIndex(1, miningEntry.Id) { PrimaryIndex = true },
                            new DBreezeIndex(2, miningEntry.Timestamp),
                            new DBreezeIndex(3, miningEntry.FullNodeName),
                        }
                    }, false);

                    t.TextInsert("TS_Mining", miningEntry.Id.ToBytes(), miningEntry.FullNodeName);

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Persist(BlockchainReorg reorgEntry)
        {
            try
            {
                using (var t = Engine.GetTransaction())
                {
                    t.SynchronizeTables("Reorg");

                    bool newEntity = reorgEntry.Id == 0;
                    if (newEntity)
                        reorgEntry.Id = t.ObjectGetNewIdentity<long>("Reorg");

                    t.ObjectInsert("Reorg", new DBreezeObject<BlockchainReorg>
                    {
                        NewEntity = newEntity,
                        Entity = reorgEntry,
                        Indexes = new List<DBreezeIndex>
                        {
                            new DBreezeIndex(1, reorgEntry.Id) { PrimaryIndex = true },
                            new DBreezeIndex(2, reorgEntry.Timestamp),
                            new DBreezeIndex(3, reorgEntry.FullNodeName),
                        }
                    }, false);

                    t.TextInsert("TS_Reorg", reorgEntry.Id.ToBytes(), reorgEntry.FullNodeName);

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

        public int GetMinedBlockCount(string fullNodeName)
        {
            int count = 0;
            try
            {
                using (var t = Engine.GetTransaction())
                {
                    foreach (byte[] doc in t.TextSearch("TS_Mining").BlockAnd(fullNodeName).GetDocumentIDs())
                    {
                        DBreezeObject<BlockchainMining> obj = t.Select<byte[], byte[]>("Mining", 1.ToIndex(doc)).ObjectGet<BlockchainMining>();
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

            return count;
        }

        public int GetReorgCount(string fullNodeName)
        {
            int count = 0;
            try
            {
                using (var t = Engine.GetTransaction())
                {
                    foreach (byte[] doc in t.TextSearch("TS_Reorg").BlockAnd(fullNodeName).GetDocumentIDs())
                    {
                        DBreezeObject<BlockchainReorg> obj = t.Select<byte[], byte[]>("Reorg", 1.ToIndex(doc)).ObjectGet<BlockchainReorg>();
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

            return count;
        }

    }
}


