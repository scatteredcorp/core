using System;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using BGC.Contracts;
using Microsoft.VisualBasic;

namespace BGC.Blockchain {
    public static class Mempool {
        private static List<IContract> Pool { get; } = new List<IContract>();
        
        public static void AddContract(IContract contract) {
            Pool.Add(contract);
        }

        public static void RemoveContracts(IEnumerable<IContract> contracts) {
            foreach (IContract c in contracts) {
                Pool.Remove(c);
            }
        }

        public static List<IContract> GetBestContracts(int count)
        {
            // Minus: decreasing order
            Pool.Sort((contract, contract1) => - contract.Fee.TotalValue().CompareTo(contract1.Fee.TotalValue()));

            List<IContract> contracts = new List<IContract>();

            for (int i = 0; i < count && i < Pool.Count; ++i)
            {
                contracts.Add(Pool[i]);
            }

            return contracts;
        }
    }
}