using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Transactions;
using BGC.Contracts;
using BGC.ProofOfWork;
using LevelDB;
using Secp256k1Net;

namespace BGC.Blockchain {
    public static class Blockchain {
        private static readonly byte[] lh = Encoding.ASCII.GetBytes("lh");
        private static readonly byte[] blockPrefix = Encoding.ASCII.GetBytes("b");
        
        private const string dbLocation = "./tmp";

        public static byte[] LastHash { get; private set; }
        public static uint Height { get; private set; }
        public static DB DB { get; private set; } = null;

        public static void Resume() {
            // Check if blockchain exists
            try {
                Options options = new Options{CreateIfMissing = false};
                DB = new DB(options, dbLocation);
            } catch {
                Console.WriteLine("Blockchain has not been initialized.");
                System.Environment.Exit(1);
            }
        }

        public static void Init(byte[] rewardAddress) {
            // Check if blockchain has already been initialized
            Options options = new Options{CreateIfMissing = true};
            DB = new DB(options, dbLocation);
            byte[] lastHash = DB.Get(lh);
            
            if (lastHash != null) {
                Console.WriteLine("Blockchain is already initialized.");
                System.Environment.Exit(1);
            }

            TransactionContract coinbase = TransactionHelper.CoinbaseTransaction(rewardAddress, 0);
            Block genesisBlock = BlockHelper.Genesis(coinbase);
            
            // Compute block hash
            PoW pow = new PoW(genesisBlock);
            (uint nonce, byte[] hash) = pow.Run();
            
            // Set nonce (solution)
            genesisBlock.BlockHeader.Nonce = nonce;

            // Set last hash
            LastHash = hash;
            
            // Store block data
            DB.Put(MakeKey(blockPrefix, hash), genesisBlock.Serialize());
            
            // Store last hash
            DB.Put(lh, hash);
        }

        public static bool SaveBlock(Block block) {
            byte[] hash = block.BlockHeader.Hash();
            byte[] key = MakeKey(blockPrefix, hash);
            
            DB.Put(key, block.Serialize());
            return true;
        }

        private static byte[] MakeKey(byte[] keyPrefix, byte[] hash) {
            byte[] key = new byte[hash.Length + keyPrefix.Length];
            uint i = 0;
            for (; i < keyPrefix.Length; i++) {
                key[i] = keyPrefix[i];
            }
            for (uint j = 0; j < hash.Length; j++) {
                key[i] = hash[j];
                i++;
            }

            return key;
        }
        
        private static uint DeserializeUint(byte[] data, ref uint offset) {
            byte[] bytes = new byte[4];
            Array.Copy(data, offset, bytes, 0, 4);
            
            offset += 4;
            return BitConverter.ToUInt32(bytes);;
        }

        private static byte[] DeserializeHash(byte[] data, ref uint offset) {
            byte[] hash = new byte[32];
            Array.Copy(data, offset, hash, 0, 32);
            offset += 32;
            return hash;
        }

        public static Block DeserializeBlock(byte[] bytes) {
            BlockHeader blockHeader = DeserializeBlockHeader(bytes);
            uint offset = 77; // header size

            uint numContracts = DeserializeUint(bytes, ref offset);

            byte[] contractsBytes = new byte[bytes.Length - offset];
            Array.Copy(bytes, offset, contractsBytes, 0, bytes.Length - offset);

            IContract[] contracts = ContractHelper.DeserializeMany(numContracts, contractsBytes);
            
            return new Block(blockHeader, contracts.ToArray());
        }
        
        private static BlockHeader DeserializeBlockHeader(byte[] bytes) {
            uint offset = 0;
            byte version = bytes[offset];
            offset++;
            
            byte[] previousHash = DeserializeHash(bytes, ref offset);
            byte[] merkleRoot = DeserializeHash(bytes, ref offset);
            uint timestamp = DeserializeUint(bytes, ref offset);
            uint target = DeserializeUint(bytes, ref offset);
            uint nonce = DeserializeUint(bytes, ref offset);

            return new BlockHeader(version, previousHash, merkleRoot, timestamp, target, nonce);
        }
    }
}
