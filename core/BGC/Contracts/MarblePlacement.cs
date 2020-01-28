using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using BGC.Marbles;

namespace BGC.Contracts {
    
    /// <summary>
    /// Serialization:
    /// 1 byte : Number of placement
    ///
    /// 1 byte : Marble type
    /// 4 bytes : Amount
    /// ... // each placement
    /// 1 byte : Marble type
    /// 4 bytes : Amount
    /// </summary>
    public sealed class Placement {
        public List<PlacementMarble> Marbles { get; } = new List<PlacementMarble>();

        public void Add(byte type, uint amount) {
            // Check if type is already in a placement
            for (byte i = 0; i < Marbles.Count; i++) {
                if (Marbles[i].Type == type) {
                    // If it is, increase it
                    Marbles[i].Amount += amount;
                    return;
                }
            }

            // Otherwise, simply add it
            Marbles.Add(new PlacementMarble(type, amount));
        }

        public byte[] Serialize() {
            // Initialize byte list
            // Add number of marble types
            List<byte> serialized = new List<byte> {(byte) Marbles.Count};
            
            // For each type of marbles
            // We append the type byte
            // And also the amount of marbles of this specific type
            for (byte i = 0; i < Marbles.Count; i++) {
                serialized.Add(Marbles[i].Type);
                
                byte[] amount = BitConverter.GetBytes(Marbles[i].Amount);
                serialized.AddRange(amount);
            }
            
            // Return array of bytes
            return serialized.ToArray();
        }

        public void PrettyPrint() {
            for (int i = 0; i < Marbles.Count; i++) {
                Console.WriteLine("\tType: {0}", Marbles[i].Type);
                Console.WriteLine("\tAmount: {0}", Marbles[i].Amount);
                Console.WriteLine("");
            }
        }
    }
    
    
    public class PlacementMarble {
        public byte Type;
        public uint Amount;

        public PlacementMarble(byte type, uint amount) {
            Type = type;
            Amount = amount;
        }
    }
}
