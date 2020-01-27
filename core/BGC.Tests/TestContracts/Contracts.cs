using BGC.Contracts;
using Xunit;
using BGC.Wallet;


namespace BGC.Tests.TestContracts {
    public class TestContracts {
        [Fact]
        public void TestSerializeStartContract() {

            // Create dummy contract
            Placement fee = new Placement();
            fee.Add(0, 12);
            fee.Add(1, 15);
            
            Placement playerOnePlacement = new Placement();
            playerOnePlacement.Add(2, 64);
            playerOnePlacement.Add(4, 32);

            Placement playerTwoPlacement = new Placement();
            playerTwoPlacement.Add(5, 122);
            playerTwoPlacement.Add(8, 2);
            
            // Dummy wallets
            Wallet.Wallet wallet1 = new Wallet.Wallet(WalletHelper.GeneratePrivateKey());
            Wallet.Wallet wallet2 = new Wallet.Wallet(WalletHelper.GeneratePrivateKey());
            

            // Create contract
            StartContract startContract = new StartContract(fee, playerOnePlacement, playerTwoPlacement,
                wallet1.Address(), wallet2.Address());

            // Serialize it (without signatures)
            byte[] bytes = startContract.Serialize(ContractHelper.SerializationType.NoSig);
            
            // Deserialize it
            StartContract resultContract = (StartContract) Contracts.ContractHelper.Deserialize(bytes);
    
            // Check if versions match
            Assert.Equal(bytes, resultContract.Serialize(ContractHelper.SerializationType.NoSig));

            // Player one signs the contract
            startContract.PartialSign(wallet1.PrivateKey, 0);

            // Serialize it
            bytes = startContract.Serialize(ContractHelper.SerializationType.Partial);
            
            // Deserialize it
            resultContract = (StartContract) Contracts.ContractHelper.Deserialize(bytes);
            
            // Check if versions match
            Assert.Equal(bytes, resultContract.Serialize(ContractHelper.SerializationType.Partial));

            // Player two sign the contract
            startContract.Sign(wallet2.PrivateKey, 0);

            // Serialize it
            bytes = startContract.Serialize(ContractHelper.SerializationType.Complete);
            
            // Deserialize it
            resultContract = (StartContract) Contracts.ContractHelper.Deserialize(bytes);
            
            // Check if versions match
            Assert.Equal(bytes, startContract.Serialize(ContractHelper.SerializationType.Complete));

        }
    }
}
