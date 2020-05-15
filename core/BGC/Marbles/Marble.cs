using System;

namespace BGC.Marbles {

    public enum Type : byte {
        Earth,
        Elastic,
        Layers,
        Ribbon,
        Stripes,
        Whirlwind,
        Soccer
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

        public static Marble Earth  = new Marble("Earth", Type.Earth, 256, true);
        public static Marble Elastic   = new Marble("Elastic", Type.Elastic, 24);
        public static Marble Layers    = new Marble("Layers", Type.Layers, 8);
        public static Marble Ribbon    = new Marble("Ribbon", Type.Ribbon, 8);
        public static Marble Stripes   = new Marble("Stripes", Type.Stripes, 2);
        public static Marble Whirlwind = new Marble("Whirlwind", Type.Whirlwind, 1);
        public static Marble Soccer    = new Marble("Soccer", Type.Soccer, 1, true);

        public static int Value(Type type)
        {
            switch (type)
            {
                case Type.Earth:
                    return 39;
                case Type.Elastic:
                    return 69;
                case Type.Layers:
                    return 208;
                case Type.Ribbon:
                    return 208;
                case Type.Stripes:
                    return 833;
                case Type.Whirlwind:
                    return 1667;
                case Type.Soccer:
                    return 10000;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
        // Define all marbles in an array
        public static Marble[] All = new Marble[]{
            Earth,
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

        public uint InitialQuantity;
        public bool Special;

        // If special == true : only one marble 
        // Otherwise, there exists the 6 colors
        public Marble(string name, Type type, uint quantity, bool special = false) {
            Name = name;
            Type = type;
            InitialQuantity = quantity;
            Special = special;
        }

        public uint Quantity() {

            uint height = Blockchain.Blockchain.Height;
            uint divide = (uint) Math.Pow(2, height / Consensus.Consensus.HalvingCycle); 

            // Halve reward
            return InitialQuantity / divide;
        }
    }
}
