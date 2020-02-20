using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using BGC.Contracts;

namespace BGC.Blockchain {
    public static class Mempool {
        private static List<IContract> Pool { get; }
        
        public static void AddContract(IContract contract) {
            Pool.Add(contract);
        }

        public static void RemoveContracts(IEnumerable<IContract> contracts) {
            foreach (IContract c in contracts) {
                Pool.Remove(c);
            }
        }
    }
}