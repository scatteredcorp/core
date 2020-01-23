using System;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using BGC;
using Xunit;
using Wallet = BGC.Wallet;

namespace BGC.Tests.Wallet {
    public class WalletTest {
        [Fact]
        public void TestFromHex()
        {
            string hexData = "CB186C327B41EF5F775E531B4FF1C2E555F05C41FBB9CD68F4CC6CBBFD4FB8F1";
            byte[] expectedResult = { 0xCB, 0x18, 0x6C, 0x32, 0x7B, 0x41, 0xEF, 0x5F, 0x77, 0x5E, 0x53, 0x1B, 0x4F, 0xF1, 0xC2, 0xE5, 0x55, 0xF0, 0x5C, 0x41, 0xFB, 0xB9, 0xCD, 0x68,
                0xF4, 0xCC, 0x6C, 0xBB, 0xFD, 0x4F, 0xB8, 0xF1 };

            Assert.Equal(Utils.FromHex(hexData), expectedResult);
        }

        [Fact]
        public void TestComputePubKey()
        {
            byte[] privateKey = Utils.FromHex("CB186C327B41EF5F775E531B4FF1C2E555F05C41FBB9CD68F4CC6CBBFD4FB8F1");
            byte[] expectedResult = Utils.FromHex("041CB0D41D458F5D715899FC68B9AD142F983203A22D06F209576F24F89CD5E5A5A5DF6DE9B4475DD479007FFCE41334448899BCA5ACF8547B378E4369B90B3872");

            Assert.Equal(BGC.Wallet.WalletHelper.ComputePubKey(privateKey), expectedResult);
        }
    }
}
