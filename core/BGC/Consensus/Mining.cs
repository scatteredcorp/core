using System;
using System.Linq;
using System.Numerics;
using BGC.Blockchain;
using BGC.Contracts;

namespace BGC.Consensus {
	public static class Mining {

		// Difficulty is adjusted every 180 blocks (one hour) 
		public static readonly byte[] MaxTarget = Utils.FromHex("00000000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0F0000");
		
		// Calculate block reward based on height
		public static Placement Reward(uint blockHeight) {
			throw new NotImplementedException();
		}

		public static byte[] Target() {
			// No need to compute if blockchain height is below 180
			// Or height is not modulo 180
			if (Blockchain.Blockchain.Height < Consensus.TargetAdjustment ||
			    Blockchain.Blockchain.Height % Consensus.TargetAdjustment != 0) {
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
				if (blocks == Consensus.TargetAdjustment-1) {
					startTime = block.BlockHeader.Timestamp;
					break;
				}
				
				blocks++;
			}

			uint expectedTime = Consensus.BlockTime * Consensus.TargetAdjustment;

			BigInteger lastTarget = new BigInteger(Blockchain.Blockchain.LastTarget(), true);

			decimal coef = ((decimal) (endTime - startTime)) / (decimal) expectedTime;
			if(coef > (decimal) 1.5) coef = (decimal) 1.5;
			if(coef < (decimal) 0.5) coef = (decimal) 0.5;

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
