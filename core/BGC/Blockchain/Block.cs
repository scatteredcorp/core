using System;
using BGC.Contracts;

namespace BGC.Blockchain {
    public struct Block {
        public byte[] Hash;
        public byte[] PreviousHash;
        public Contract Contracts;
        public UInt64 Height;
        public UInt64 Nonce;
        public UInt32 Timestamp;

        public Block Genesis() {
            Block block = new Block();
            block.PreviousHash = null;
            block.Height = 0;
            block.Nonce = 0;
            
            // TODO: Include genesis block reward
            block.Contracts = null;
            
            UInt32 unixTimestamp = (UInt32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            block.Timestamp = unixTimestamp;

            return block;
        }
        
    };
}
