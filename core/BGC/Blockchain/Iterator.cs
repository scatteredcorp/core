using LevelDB;

namespace BGC.Blockchain {
    public class Iterator {
        public byte[] CurrentHash { get; private set; }
        public DB DB { get; }

        public Iterator() {
            CurrentHash = Blockchain.LastHash;
            DB = Blockchain.DB;
        }

        public Block Next() {
            byte[] blockBytes = DB.Get(Utils.BuildKey("b", CurrentHash));
            Block block = Blockchain.DeserializeBlock(blockBytes);

            CurrentHash = block.BlockHeader.PreviousHash;

            return block;
        }
    }
}