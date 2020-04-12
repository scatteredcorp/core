using System;
using System.Linq;
using System.Numerics;
using BGC.Blockchain;
using BGC.Contracts;

namespace BGC.Consensus {
	public static class Mining {

		// Difficulty is adjusted every 180 blocks (one hour) 
		public const uint AdjustDifficultyBlocks = 5; // TODO CHANGE
		public static readonly byte[] MaxTarget = Utils.FromHex("00000000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000");
		
		// Calculate block reward based on height
		public static Placement Reward(uint blockHeight) {
			throw new NotImplementedException();
		}

		public static byte[] Target() {
			// No need to compute if blockchain height is below 180
			// Or height is not modulo 180
			if (Blockchain.Blockchain.Height < AdjustDifficultyBlocks ||
			    Blockchain.Blockchain.Height % AdjustDifficultyBlocks != 0) {
				return Blockchain.Blockchain.LastTarget();
			}
			
			Iterator iterator = new Iterator();
			uint blocks = 0;

			uint startTime = 0;
			uint endTime = 0;

			while (iterator.CanIterate) {
				Blockchain.Block block = iterator.Next();
				if (blocks == 0) {
					endTime = block.BlockHeader.Timestamp;
				}
				if (blocks == AdjustDifficultyBlocks-1) {
					startTime = block.BlockHeader.Timestamp;
					break;
				}
				
				blocks++;
			}

			uint expectedTime = Consensus.BlockTime * AdjustDifficultyBlocks;

			BigInteger lastTarget = new BigInteger(Blockchain.Blockchain.LastTarget(), true);

			decimal coef = ((decimal) (endTime - startTime)) / (decimal) expectedTime;

			var f = Utils.Fraction(coef);
			BigInteger bigTarget = lastTarget * f.numerator;
			bigTarget = bigTarget / f.denominator;

			byte[] target = bigTarget.ToByteArray();
			byte[] padded = new byte[32];
			for(int i = 0; i < target.Length; i++) {
				padded[i] = target[i];
			}

			Console.WriteLine("Adjusted target by " + coef);

			return padded;
		}
	}
	
}
