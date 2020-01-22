using System;
using System.Globalization;
using System.Numerics;
using Xunit;
using Wallet = BGC.Wallet;

namespace BGC.Tests.Wallet {
    public class WalletTest {
        [Fact]
        public void TestComputePubKey() {
            BigInteger privateKey = BigInteger.Parse("0" + "B455EB71D558989F738D80FDCC599B9BFD708CF0862F4B02574CDCCDA4F0A157",
                NumberStyles.AllowHexSpecifier);
            BigInteger publicKey =
                BigInteger.Parse(
                    "0" + "040DFCBABA41CF5087A07E964DCC5D58A4AF5BD25CDBD96963E8470BE90E39A0870CD7CEF647809870E6691DF0D36A4E621DA59C9AB1D7008655CC9778744A3BAD",
                    NumberStyles.AllowHexSpecifier);

            BGC.Wallet.Wallet wallet = new BGC.Wallet.Wallet(privateKey);
            Assert.Equal(publicKey.ToByteArray(), wallet.PublicKey);
        }
    }
}