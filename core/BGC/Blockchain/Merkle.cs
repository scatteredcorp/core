using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace BGC.Blockchain {
    public class MerkleTree {
        public MerkleNode Root;

        public MerkleTree(byte[][] data) {
            List<MerkleNode> nodes = new List<MerkleNode>();
            
            // If tree branches is odd, duplicate last branch
            if (data.Length % 2 != 0) {
                data.Append(data[data.Length - 1]);
            }

            for (uint i = 0; i < data.Length; i++) {
                MerkleNode node = new MerkleNode(null, null, data[i]);
                nodes.Add(node);
            }

            for (uint i = 0; i < data.Length / 2; i++) {
                List<MerkleNode> level = new List<MerkleNode>();
                for (int j = 0; i < nodes.Count; j++) {
                    MerkleNode node = new MerkleNode(nodes[j], nodes[j + 1], null);
                    level.Add(node);
                }

                nodes = level;
            }

            Root = nodes[0];
        }
    }

    public class MerkleNode {
        public MerkleNode Left;
        public MerkleNode Right;
        public byte[] Data;

        public MerkleNode(MerkleNode left, MerkleNode right, byte[] data) {
            if (left == null && right == null) {
                SHA256 sha = SHA256.Create();
                byte[] hash = sha.ComputeHash(data);
                Data = hash;
            } else {
                List<byte> previous = new List<byte>();
                previous.AddRange(left.Data);
                previous.AddRange(right.Data);
                
                SHA256 sha = SHA256.Create();
                byte[] hash = sha.ComputeHash(previous.ToArray());
                Data = hash;
            }

            Left = left;
            Right = right;
        }
    }
    
    
}