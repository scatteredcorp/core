using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using BGC.Blockchain;

namespace BGC.ProofOfWork {
    public class PoW {
        public const int Difficulty = 20;

        public Block Block;
        public byte[] Target;

        public PoW(Block block) {
            Block = block;
            Target = Consensus.Mining.Target();
        }

        private byte[] InitData(uint nonce) {
            List<byte> data = new List<byte>();
            data.AddRange(Block.BlockHeader.PreviousHash);
            data.AddRange(Block.HashContracts());
            data.AddRange(BitConverter.GetBytes(nonce));
            data.AddRange(Target);

            return data.ToArray();
        }

        public (uint, byte[]) Run() {
            byte[] hash = new byte[32];

            uint nonce = 0;
            while (nonce < UInt32.MaxValue) {
                byte[] data = InitData(nonce);
                
                // Compute hash of block
                SHA256 sha = SHA256.Create();
                hash = sha.ComputeHash(data);
                
                // SHA256 returns an unsigned byte array
                // But BigInteger only accepts signed byte arrays
                // Append zero-byte to make it positive
                BigInteger intHash = new BigInteger(hash, true);
                BigInteger target = new BigInteger(Target, true);
                // If block hash is below target, it is valid
                if (intHash.CompareTo(target) == -1) {
                    break;
                }

                // Compute a different hash by changing the nonce
                nonce++;
            }
            return (nonce, hash);
        }


    }
}