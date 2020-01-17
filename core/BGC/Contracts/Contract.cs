using BGC.Marbles;

namespace BGC.Contracts {

    public interface IContract {
        byte[] Serialize();
    }
    
    public class BaseContract {
        public byte Version;
        public byte Type;
        
        public Placement Fee;
        public ulong Nonce;

        protected BaseContract(Placement fee, ulong nonce) {
            Fee = fee;
            Nonce = nonce;
        }
    }
}
