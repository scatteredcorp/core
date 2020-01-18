using System;
using System.Diagnostics;
using System.Transactions;
using BGC.Contracts;
using BGC.ProofOfWork;
using LevelDB;
using StackExchange.Redis;

namespace BGC.Blockchain {
    public class Blockchain {
        private const string dbLocation = "./tmp";

        public byte[] LastHash;
        public uint Height;

        public void Resume() {
        }

        public void Init(byte[] rewardAddress) {
            Options options = new Options {CreateIfMissing = true};
            DB db = new DB(options, dbLocation);

            TransactionContract coinbase = TransactionHelper.CoinbaseTransaction(rewardAddress, 0);
            Console.WriteLine(coinbase.Type);
            Block genesisBlock = BlockHelper.Genesis(coinbase);
            
            PoW pow = new PoW(genesisBlock);
            (uint nonce, byte[] hash) = pow.Run();
            Console.WriteLine(BitConverter.ToString(hash));
        }
    }
}
