using BGC.Contracts;
using Xunit;

namespace BGC.Tests.Contracts {
    public class TestContracts {
        [Fact]
        public void TestComputePubKey() {

            Placement fee = new Placement();
            fee.Add(0, 12);
            fee.Add(1, 15);

            Placement playerOnePlacement = new Placement();
            playerOnePlacement.Add(2, 64);
            playerOnePlacement.Add(4, 32);

            Placement playerTwoPlacement = new Placement();
            playerTwoPlacement.Add(5, 122);
            playerTwoPlacement.Add(8, 2);
            
            BGC.Wallet.Wallet wallet1 = new BGC.Wallet.Wallet(BGC.Wallet.WalletHelper.GeneratePrivateKey());
            
            
            StartContract startContract = new StartContract(fee, playerOnePlacement, playerTwoPlacement, BGC.Wallet.WalletHelper.)

            foreach ((string priv, string pub) in keys) {
                byte[] privateKey = Utils.FromHex(priv);
                byte[] expectedResult = Utils.FromHex(pub);

                Assert.Equal(BGC.Wallet.WalletHelper.ComputePubKey(privateKey), expectedResult);
            }
        }
    }
}
