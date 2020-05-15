using System;

namespace BGC.Consensus {
	public class Block {

		// Check if block size is <= BlockSize
		public static bool ValidateSize(Blockchain.Block block) {
            byte[] serialized = block.Serialize();
            return serialized.Length <= Consensus.BlockSize;
        }
    }
}
