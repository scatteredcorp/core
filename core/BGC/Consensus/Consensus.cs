namespace BGC.Consensus {
    
    public class Consensus {
        public const uint BlockTime = 1; 			// 30s
        public const uint TargetAdjustment = 10;    // every 10 blocks adjust mining target
        public const uint BlockSize = 64 * 1024; 	// 64 KB
        public const uint HalvingCycle = 10;   // every 2.1M blocks
    }
}
