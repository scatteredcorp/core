using System;
using System.Collections.Generic;
using BGC.Marbles;

namespace BGC.Contracts {
    public class Placement {
        private List<PlacementMarble> marbles = new List<PlacementMarble>();

        public void Add(byte type, ulong amount) {
            for (byte i = 0; i < marbles.Count; i++) {
                if (marbles[i].Type == type) {
                    marbles[i].Amount += amount;
                    return;
                }
            }
            
            marbles.Add(new PlacementMarble(type, amount));
        }

        public List<PlacementMarble> GetMarbles() {
            return marbles;
        }
        
        public byte[] Serialize() {
            var serialized = new List<byte> {(byte) marbles.Count};
            
            for (byte i = 0; i < marbles.Count; i++) {
                serialized.Add(marbles[i].Type);
                
                byte[] amount = BitConverter.GetBytes(marbles[i].Amount);
                serialized.AddRange(amount);
            }
            
            return serialized.ToArray();
        }
    }
    
    
    public class PlacementMarble {
        public byte Type;
        public ulong Amount;

        public PlacementMarble(byte type, ulong amount) {
            Type = type;
            Amount = amount;
        }
    }
}
