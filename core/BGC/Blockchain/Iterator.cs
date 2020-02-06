using System;
using System.Linq;
using LevelDB;

namespace BGC.Blockchain {
    public class Iterator {
        public byte[] CurrentHash { get; private set; }
        public DB DB { get; }
        public bool CanIterate = true; 

        public Iterator() {
            CurrentHash = Blockchain.LastHash;
            DB = Blockchain.DB;
        }

        public Block Next() {
            byte[] blockBytes = DB.Get(Utils.BuildKey("b", CurrentHash));
            Block block = Blockchain.DeserializeBlock(blockBytes);
        
            CurrentHash = block.BlockHeader.PreviousHash;
            
            if (block.BlockHeader.PreviousHash.SequenceEqual(new byte[32])) {
                CanIterate = false;
            }
            
            return block;
        }
    }
}