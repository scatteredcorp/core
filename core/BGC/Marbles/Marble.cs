namespace BGC.Marbles {
    public class Marble : Enumeration {
       
        public static Marble Earth = new Marble(0, "Earth");
        public static Marble Ocean = new Marble(1, "Earth");
        public static Marble Aggie = new Marble(2, "Aggie");
        
        
        public Marble(byte type, string name) : base(type, name) {}
    }
}
