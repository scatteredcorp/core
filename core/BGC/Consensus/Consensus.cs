namespace BGC.Consensus {
    
    public class Consensus {
        public const uint BlockTime         = 60; 			    // 60s
        public const uint TargetAdjustment  = 1440;             // every 1440 blocks adjust mining target
        public const uint BlockSize         = 128  * 1024;    	// 128 KB
        public const uint HalvingCycle      = 10000;            // every 10K blocks
    }
}
