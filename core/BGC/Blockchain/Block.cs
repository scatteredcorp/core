using System;
using System.Collections.Generic;
using BGC.Contracts;

namespace BGC.Blockchain {

    public struct BlockHeader {
        public byte Version;

        public byte[] PreviousHash;    // 32 bytes
        public byte[] MerkleRoot;      // 32 bytes
        public uint Timestamp;
        public uint DifficultyTarget;
        public uint Nonce;

        public BlockHeader(byte version, byte[] previousHash, byte[] merkleRoot, uint timestamp, uint difficultyTarget, uint nonce) {
            Version = version;
            
            PreviousHash = previousHash;
            
            MerkleRoot = merkleRoot;
            Timestamp = timestamp;
            DifficultyTarget = difficultyTarget;
            Nonce = nonce;
        }

        public byte[] Serialize() {
            List<byte> serialized = new List<byte> {Version};

            serialized.AddRange(PreviousHash);
            serialized.AddRange(MerkleRoot);
            serialized.AddRange(BitConverter.GetBytes(Timestamp));
            serialized.AddRange(BitConverter.GetBytes(DifficultyTarget));
            serialized.AddRange(BitConverter.GetBytes(Nonce));
            
            return serialized.ToArray();
        }
    }
    
    public struct Block {
        
        private const byte Version = 1;
        
        public BlockHeader BlockHeader;
        public IContract[] Contracts;

        public Block(BlockHeader blockHeader, IContract[] contracts) {
            BlockHeader = blockHeader;
            Contracts = contracts;
        }
        
        public byte[] Serialize() {
            List<byte> serialized = new List<byte>();
            byte[] header = BlockHeader.Serialize();
            serialized.AddRange(header);

            serialized.AddRange(BitConverter.GetBytes((uint) Contracts.Length));

            for (uint i = 0; i < Contracts.Length; i++) {
                serialized.AddRange(Contracts[i].Serialize(ContractHelper.SerializationType.Complete));
            }

            return serialized.ToArray();
        }

        public byte[] HashContracts() {
            if (Contracts.Length < 1) return null;
            
            List<byte[]> serialized = new List<byte[]>();
            
            for (uint i = 0; i < Contracts.Length; i++) {
                serialized.Add(Contracts[i].Serialize(ContractHelper.SerializationType.Complete));
            }
            
            MerkleTree merkleTree = new MerkleTree(serialized.ToArray());
            return merkleTree.Root.Data;
        }
        
    };

    public static class BlockHelper {
        private const byte Version = 1;
        
        public static Block Genesis(TransactionContract coinbase) {
            uint unixTimestamp = (uint)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            BlockHeader header = new BlockHeader(Version, new byte[32], new byte[32], unixTimestamp, 0, 0); 
            
            Block block = new Block(header, new IContract[]{coinbase});
            
            return block;
        }
    }
}
