using System;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using Xunit;
using Wallet = BGC.Wallet;

namespace BGC.Tests.Wallet {
    public class TestWallet {
        [Fact]
        public void TestComputePubKey() {

            (string, string)[] keys = {
                (
                    "CB186C327B41EF5F775E531B4FF1C2E555F05C41FBB9CD68F4CC6CBBFD4FB8F1",
                    "041CB0D41D458F5D715899FC68B9AD142F983203A22D06F209576F24F89CD5E5A5A5DF6DE9B4475DD479007FFCE41334448899BCA5ACF8547B378E4369B90B3872"
                ),
                (
                    "0195ED8967F1565E686BE9B43B7171FE59094E9468B9D4F2FE8429CF490B7FB4",
                    "04AE94381F932E34A2A45D4E337A89A0B3E665FD2AF3E267179EED7FDF97518229F224BFFD909B79B57BA3B960445D9ECEBBF59AD6EE47D15FBD4AC8626DA5D205"
                ),
                (
                    "72E69EFA1482252D1B189AC74802028D2A7A3DE71BCAA7818DF75ABC3A780CC1",
                    "042209ECF8266FB766805105ACB52392297F3F32756525CF6DD662F7F45A9DB432F5CB078215D20A99443789FC998B44A670B28CAAEEC36309094460B3A4F6E1A8"
                ),
                (
                    "39ADD5B15FC96E665053D18C034949C6613176AC5081ECCAC6BC11F77133478C",
                    "04F654401F7E2821DFC1409C0F9B33E130A0A2F647F787C9484885821D602A1F9DAA4ED2F1671CD697BDC889DCA692692E05622B1D20D51F791F884F9006145309"
                )
            };

            foreach ((string priv, string pub) in keys) {
                byte[] privateKey = Utils.FromHex(priv);
                byte[] expectedResult = Utils.FromHex(pub);

                Assert.Equal(BGC.Wallet.WalletHelper.ComputePubKey(privateKey), expectedResult);
            }
            
        }
    }
}
