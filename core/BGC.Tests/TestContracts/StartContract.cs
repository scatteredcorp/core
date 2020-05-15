using System;
using BGC.Contracts;
using Xunit;
using BGC.Wallet;
using Xunit.Abstractions;

namespace BGC.Tests.TestContracts {
	public class TestStartContract {
		private readonly ITestOutputHelper _testOutputHelper;

		public TestStartContract(ITestOutputHelper testOutputHelper) {
			_testOutputHelper = testOutputHelper;
		}

		[Fact]
        public void TestSerializeStartContract() {

            // Create dummy contract
            Placement fee = new Placement();
            fee.Add(Marbles.Type.Stripes, Marbles.Color.Dark, 12);
            fee.Add(Marbles.Type.Ribbon, Marbles.Color.Dark, 15);
            
            Placement playerOnePlacement = new Placement();
            playerOnePlacement.Add(Marbles.Type.Elastic, Marbles.Color.Dark, 64);
            playerOnePlacement.Add(Marbles.Type.Ribbon, Marbles.Color.Dark, 32);

            Placement playerTwoPlacement = new Placement();
            playerTwoPlacement.Add(Marbles.Type.Soccer, Marbles.Color.None, 122);
            playerTwoPlacement.Add(Marbles.Type.Whirlwind, Marbles.Color.Dark, 2);
            
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
            
            _testOutputHelper.WriteLine(Utils.ToHex(bytes));
        }
	}
}
