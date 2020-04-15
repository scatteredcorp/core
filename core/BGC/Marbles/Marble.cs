using System;

namespace BGC.Marbles {

    public enum Type : byte {
        Elastic,
        Layers,
        Ribbon,
        Stripes,
        Whirlwind,
        Soccer
    }

    public enum Rarity : byte {
        VeryCommon,
        Common,
        Uncommon,
        Rare,
        ExtraRare,
        Legendary
    }

    public enum Color : byte {
        None,
        Blue,
        Green,
        Orange,
        Red,
        Purple,
        Dark
    }

    public static class Marbles {
        // Invididual definition
        public static Marble Whirlwind = new Marble("Whirlwind", Type.Whirlwind, Rarity.VeryCommon);
        public static Marble Elastic   = new Marble("Elastic", Type.Elastic, Rarity.Common);
        public static Marble Layers    = new Marble("Layers", Type.Layers, Rarity.Uncommon);
        public static Marble Ribbon    = new Marble("Ribbon", Type.Ribbon, Rarity.Uncommon);
        public static Marble Stripes   = new Marble("Stripes", Type.Stripes, Rarity.Rare);

        public static Marble Soccer    = new Marble("Soccer", Type.Soccer, Rarity.Legendary, true);
        
        // Define all marbles in an array
        public static Marble[] All = new Marble[]{
            Whirlwind,

            Elastic,
            Layers,
            Ribbon,
            Stripes,
            
            Soccer
        };
    }

    public class Marble {
       

        public string Name;
        public Type Type;
        public Rarity Rarity; 

        public bool Special;

        // If special == true : only one marble 
        // Otherwise, there exists the 6 colors
        public Marble(string name, Type type, Rarity rarity, bool special = false) {
            Name = name;
            Type = type;
            Rarity = rarity;
            Special = special;
        }

        public uint Quantity() {

            uint height = Blockchain.Blockchain.Height;
            uint divide = (uint) Math.Pow(2, height / Consensus.Consensus.HalvingCycle); 

            // Halve reward
            return InitialQuantity() / divide;
        }

        public uint InitialQuantity() {
            switch(Rarity) {
                case Rarity.VeryCommon:
                    return 100;     // 100M
                case Rarity.Common:
                    return 16;      // 10M
                case Rarity.Uncommon:
                    return 8;       // 1M
                case Rarity.Rare:
                    return 4;       // 100K
                case Rarity.ExtraRare:
                    return 2;       // 10K
                case Rarity.Legendary:
                    return 1;       // 1K

                default: 
                    throw new NotImplementedException();           
            }
        }


    }
}
