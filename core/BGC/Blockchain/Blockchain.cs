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
        private const string dbLocation = "./tmp";

        public static byte[] LastHash { get; private set; }
        public static uint Height { get; private set; }
        public static DB DB { get; private set; } = null;

        public static void RedindexWorldState() {
            
            return;
        }
        
        public static byte[] LastTarget() {

            if (Height < Consensus.Mining.AdjustDifficultyBlocks) {
                return Consensus.Mining.MaxTarget;
            }
            
            byte[] blockBytes = DB.Get(Utils.BuildKey("b", LastHash));
            Block block = DeserializeBlock(blockBytes);
            return block.BlockHeader.Target;
        }
        
        public static void Resume() {
            // Check if blockchain exists
            try {
                Options options = new Options{CreateIfMissing = false};
                DB = new DB(options, dbLocation);
                LastHash = DB.Get(Utils.BuildKey("lh"));
                byte[] h = DB.Get(Utils.BuildKey("h"));

                Height = BitConverter.ToUInt32(h);
                
                byte[] blockBytes = DB.Get(Utils.BuildKey("b", LastHash));
                Block block = DeserializeBlock(blockBytes);

            } catch(Exception e) {
                Console.Write(e);
                Console.WriteLine("Blockchain has not been initialized.");
                System.Environment.Exit(1);
            }
        }
        
        public static void Init(byte[] rewardAddress) {
            // Check if blockchain has already been initialized
            Options options = new Options{CreateIfMissing = true};
            DB = new DB(options, dbLocation);
            byte[] lastHash = DB.Get(Utils.BuildKey("lh"));
            
            if (lastHash != null) {
                Console.WriteLine("Blockchain is already initialized.");
                System.Environment.Exit(1);
            }

            TransactionContract coinbase = TransactionHelper.CoinbaseTransaction(rewardAddress);
            Block genesisBlock = BlockHelper.Genesis(coinbase);
            
            // Compute block hash
            PoW pow = new PoW(genesisBlock);
            (uint nonce, byte[] hash) = pow.Run();
            
            // Set nonce (solution)
            genesisBlock.BlockHeader.Nonce = nonce;

            // Set last hash
            LastHash = hash;
            
            // Store block data
            genesisBlock.Push();
        }

        // Add block to blockchain
        public static bool PushBlock(Block block) {
            byte[] hash = block.BlockHeader.Hash();
            byte[] key = Utils.BuildKey("b", hash);


            byte[] bytes = block.Serialize();
            // Save block
            DB.Put(key, bytes);
            
            // Define LastHash
            DB.Put(Utils.BuildKey("lh"), hash);
            LastHash = hash;
            Height++;
            
            // Increment height
            DB.Put(Utils.BuildKey("h"), BitConverter.GetBytes(Height));
            Blockchain.RedindexWorldState();
            
            return true;
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

            uint offset = 105; // header size

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
            byte[] target = DeserializeHash(bytes, ref offset);
            uint nonce = DeserializeUint(bytes, ref offset);

            return new BlockHeader(version, previousHash, merkleRoot, target, timestamp, nonce);
        }
    }
}
