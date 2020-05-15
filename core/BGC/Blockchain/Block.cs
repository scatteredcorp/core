using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using BGC.Contracts;

namespace BGC.Blockchain {

    /// <summary>
    ///  Serialization:
    /// 
    /// Version: 1 byte
    /// Previous Block Header Hash: 32 bytes
    /// Merkle Root Hash: 32 bytes
    /// Timestamp: 4 bytes
    /// nBits: 4 bytes
    /// Nonce: 4 bytes
    /// </summary>
    public struct BlockHeader {
        public byte Version;

        public byte[] PreviousHash;    // 32 bytes
        public byte[] MerkleRoot;      // 32 bytes
        public uint Timestamp; // 4 bytes
        public byte[] Target; // 32 bytes
        public uint Nonce; // 4 bytes

        public BlockHeader(byte version, byte[] previousHash, byte[] merkleRoot, byte[] target, uint timestamp = 0, uint nonce = 0) {
            Version = version;
            
            PreviousHash = previousHash;
            MerkleRoot = merkleRoot;
            if(timestamp == 0) {
                Timestamp = (uint) DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            } else {
                Timestamp = timestamp;
            }
            Target = target;
            Nonce = nonce;
        }

        public byte[] Serialize() {
            List<byte> serialized = new List<byte> {Version};
        
            serialized.AddRange(PreviousHash);
            serialized.AddRange(MerkleRoot);
            serialized.AddRange(BitConverter.GetBytes(Timestamp));
            serialized.AddRange(Target);
            serialized.AddRange(BitConverter.GetBytes(Nonce));
            
            return serialized.ToArray();
        }
        
        public byte[] Hash() {
            byte[] serialized = Serialize();
            SHA256 sha256 = new SHA256Managed();
            return sha256.ComputeHash(serialized);
        }

        public string HashString() {
            return BitConverter.ToString(Hash()).Replace("-", string.Empty);
        }
    }
    
    /// <summary>
    /// Serialization:
    ///
    /// Block Header: 77 bytes
    /// Number of contracts: 4 bytes
    /// Serialized signed contracts: ? bytes
    /// 
    /// </summary>
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

        public bool Push() {
            Network.Network.PropagateBlock(this);
            return Blockchain.PushBlock(this);
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
        public const byte Version = 1;
        
        public static Block Genesis(TransactionContract coinbase) {
            BlockHeader header = new BlockHeader(Version, new byte[32], new byte[32], Consensus.Mining.Target()); 
            Block block = new Block(header, new IContract[]{coinbase});
            
            return block;
        }
    }
}
