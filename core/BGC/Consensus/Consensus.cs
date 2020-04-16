namespace BGC.Consensus {
    
    public class Consensus {
        public const uint BlockTime         = 120; 			    // 2 mins
        public const uint TargetAdjustment  = 1440;             // every 1440 blocks adjust mining target
        public const uint BlockSize         = 256 * 1024;    	// 256 KB
        public const uint HalvingCycle      = 5000;            // every 50K blocks
    }
}
