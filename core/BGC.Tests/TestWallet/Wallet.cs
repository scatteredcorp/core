using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using Xunit;
using Wallet = BGC.Wallet;

namespace BGC.Tests.TestWallet {
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
        
        [Fact]
        public void TestAddress() {
            
            (string, string)[] keys = {
                (
                    "EA9162170FDDEA91F96278C297D7673EC757BD381CBA631BA5293597D8A5CF0F",
                    "1F69zD7XjM4g8nc8vcAF9SnFzrdorYRPgB"
                ),
                (
                    "AED526534B99AED5BD263C86D393237A8313F97C58FE275FE16D71D39CE18B4B",
                    "1K94qBqGA3uyhzSpByfcvUzKsVEXvYXo4o"
                ),
                (
                    "E79C6F1A02D0E79CF46F75CF9ADA6A33CA5AB03511B76E16A824389AD5A8C204",
                    "1BQrx4ymbiiPZ9oeVGdCWWh5Pb7XwTtXUT"
                ),
                (
                    "D2A95A2F37E5D2A9C15A40FAAFEF5F06FF6F850024825B239D110DAFE09DF737",
                    "1PLpvHwHcG7yDWvjhgvuanRGHppBqDukyW"
                )
            };

            foreach ((string priv, string address) in keys) {
                byte[] privateKey = Utils.FromHex(priv);
                BGC.Wallet.Wallet wallet = new BGC.Wallet.Wallet(privateKey);
                wallet.SetVersionByte(0x00);
                byte[] a = wallet.Address();
                
                Assert.Equal(a, Base58.Base58Encode.Decode(address));
            }
        }
        
        [Fact]
        public void TestEncodedAddress() {
            
            (string, string)[] keys = {
                (
                    "EA9162170FDDEA91F96278C297D7673EC757BD381CBA631BA5293597D8A5CF0F",
                    "1F69zD7XjM4g8nc8vcAF9SnFzrdorYRPgB"
                ),
                (
                    "AED526534B99AED5BD263C86D393237A8313F97C58FE275FE16D71D39CE18B4B",
                    "1K94qBqGA3uyhzSpByfcvUzKsVEXvYXo4o"
                ),
                (
                    "E79C6F1A02D0E79CF46F75CF9ADA6A33CA5AB03511B76E16A824389AD5A8C204",
                    "1BQrx4ymbiiPZ9oeVGdCWWh5Pb7XwTtXUT"
                ),
                (
                    "D2A95A2F37E5D2A9C15A40FAAFEF5F06FF6F850024825B239D110DAFE09DF737",
                    "1PLpvHwHcG7yDWvjhgvuanRGHppBqDukyW"
                )
            };

            foreach ((string priv, string address) in keys) {
                byte[] privateKey = Utils.FromHex(priv);
                BGC.Wallet.Wallet wallet = new BGC.Wallet.Wallet(privateKey);
                wallet.SetVersionByte(0x00);

                Assert.Equal(wallet.EncodedAddress(), address);
            }
        }

        [Fact]
        public void TestCreateWallet() {

            BGC.Wallet.WalletHelper.DeleteWallet();
            
            byte[] pass = Encoding.UTF8.GetBytes("password123");
            BGC.Wallet.Wallet wallet = BGC.Wallet.WalletHelper.CreateWallet(pass);
            BGC.Wallet.Wallet w = BGC.Wallet.WalletHelper.LoadWallet(pass);
            
            Assert.Equal(wallet.PrivateKey, w.PrivateKey);
            Assert.Equal(wallet.PublicKey, w.PublicKey);
            
            BGC.Wallet.WalletHelper.DeleteWallet();
        }
    }
}
