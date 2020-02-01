using System;
using BGC.Blockchain;

namespace BGC {
    class BGC {
        static void Main(string[] args) {
            CLI.CLI.Run(args);
            
            Blockchain.Blockchain.Resume();
            
            Logger.Debug(string.Join(" ", Blockchain.Blockchain.LastHash));
        }
    }
}