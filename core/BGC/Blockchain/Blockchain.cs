using System;
using System.Diagnostics;
using System.Text;
using System.Transactions;
using BGC.Contracts;
using BGC.ProofOfWork;
using LevelDB;
using StackExchange.Redis;

namespace BGC.Blockchain {
    public class Blockchain {
        private static readonly byte[] lh = Encoding.ASCII.GetBytes("lh");
        private const string dbLocation = "./tmp";
        
        public byte[] LastHash;
        public uint Height;
        public DB DB = null;

        public void Resume() {
            // Check if blockchain exists
            try {
                Options options = new Options{CreateIfMissing = false};
                DB = new DB(options, dbLocation);
            } catch (Exception e) {
                Console.WriteLine("Blockchain has not been initialized.");
                System.Environment.Exit(1);
            }
        }

        public void Init(byte[] rewardAddress) {
            // Check if blockchain has already been initialized
            Options options = new Options{CreateIfMissing = true};
            DB = new DB(options, dbLocation);
            byte[] lastHash = DB.Get(lh);
            
            if (lastHash != null) {
                Console.WriteLine("Blockchain is already initialized.");
                System.Environment.Exit(1);
            }

            TransactionContract coinbase = TransactionHelper.CoinbaseTransaction(rewardAddress, 0);
            Console.WriteLine(coinbase.Type);
            Block genesisBlock = BlockHelper.Genesis(coinbase);
            
            // Compute block hash
            PoW pow = new PoW(genesisBlock);
            (uint nonce, byte[] hash) = pow.Run();
            
            // Set nonce (solution)
            genesisBlock.BlockHeader.Nonce = nonce;

            // Set last hash
            LastHash = hash;
            
            // Store block data
            DB.Put(hash, genesisBlock.Serialize());
            
            // Store last hash
            DB.Put(lh, hash);
            
            Console.WriteLine(BitConverter.ToString(hash));
        }
    }
}
