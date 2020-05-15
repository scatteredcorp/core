using System;
using System.Security.Cryptography;
using System.Text;
using BGC.Contracts;
using BGC.Wallet;
using Xunit;
using Xunit.Abstractions;

namespace BGC.Tests.TestContracts {
	public class TestThrowContract {
		private readonly ITestOutputHelper _testOutputHelper;

		public TestThrowContract(ITestOutputHelper testOutputHelper) {
			_testOutputHelper = testOutputHelper;
		}

		[Fact]
		public void TestSerializeStartContract() {
			 // Create dummy contract
            Placement fee = new Placement();
            fee.Add(Marbles.Type.Ribbon, Marbles.Color.Dark, 12);
            fee.Add(Marbles.Type.Elastic, Marbles.Color.Dark, 15);

            // Dummy wallets
            Wallet.Wallet wallet = new Wallet.Wallet(WalletHelper.GeneratePrivateKey());
            
            SHA256 sha = new SHA256Managed();
            byte[] gameHash = sha.ComputeHash(Encoding.UTF8.GetBytes("BGC"));

            byte x = 50;
            byte z = 32;
            byte throwNonce = 0; // first throw of the game
            byte playerNonce = 12; // 13th contract of the player's wallet
            
            // Create contract
            ThrowContract throwContract = new ThrowContract(fee, gameHash, x, z, throwNonce, 0, null);

            // Serialize it (without signatures)
            byte[] bytes = throwContract.Serialize(ContractHelper.SerializationType.NoSig);
            
            // Deserialize it
            ThrowContract resultContract = (ThrowContract) Contracts.ContractHelper.Deserialize(bytes);
    
            // Check if versions match
            Assert.Equal(bytes, resultContract.Serialize(ContractHelper.SerializationType.NoSig));

            // Player signs the contract
            throwContract.Sign(wallet.PrivateKey, playerNonce);

            // Serialize it
            bytes = throwContract.Serialize();
            
            // Deserialize it
            resultContract = (ThrowContract) Contracts.ContractHelper.Deserialize(bytes);
            
            // Check if versions match
            Assert.Equal(bytes, resultContract.Serialize());
            
             _testOutputHelper.WriteLine(Utils.ToHex(bytes));
		}
	}
}
