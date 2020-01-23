using System.Linq;
using System;
using System.Security.Cryptography;
using BGC.Base58;

namespace BGC {
	public static class Utils {
		public static bool ValidateAddress(string address) {
			if (address.Length < 26 || address.Length > 35) return false;
			byte[] decoded = Base58Encode.Decode(address);

			SHA256 sha = SHA256.Create();
			byte[] d1 = sha.ComputeHash(Helper.SubArray(decoded, 0, 21));
			byte[] d2 = sha.ComputeHash(d1);

			if (!Helper.SubArray(decoded, 21, 4).SequenceEqual(Helper.SubArray(d2, 0, 4))) return false;
			return true;
		}

        public static byte[] FromHex(string value)
        {
            byte[] result = new byte[value.Length / 2];

            value = value.ToUpper();

            for (int i = 0; i < value.Length; i++)
            {
                result[i / 2] <<= 4;
                switch (value[i])
                {
                    case '0':
                        result[i / 2] |= 0x0;
                        break;
                    case '1':
                        result[i / 2] |= 0x1;
                        break;
                    case '2':
                        result[i / 2] |= 0x2;
                        break;
                    case '3':
                        result[i / 2] |= 0x3;
                        break;
                    case '4':
                        result[i / 2] |= 0x4;
                        break;
                    case '5':
                        result[i / 2] |= 0x5;
                        break;
                    case '6':
                        result[i / 2] |= 0x6;
                        break;
                    case '7':
                        result[i / 2] |= 0x7;
                        break;
                    case '8':
                        result[i / 2] |= 0x8;
                        break;
                    case '9':
                        result[i / 2] |= 0x9;
                        break;
                    case 'A':
                        result[i / 2] |= 0xA;
                        break;
                    case 'B':
                        result[i / 2] |= 0xB;
                        break;
                    case 'C':
                        result[i / 2] |= 0xC;
                        break;
                    case 'D':
                        result[i / 2] |= 0xD;
                        break;
                    case 'E':
                        result[i / 2] |= 0xE;
                        break;
                    case 'F':
                        result[i / 2] |= 0xF;
                        break;
                    default:
                        throw new Exception("Invalid hexadecimal character");
                }
            }

            return result;
        }
	}
}
