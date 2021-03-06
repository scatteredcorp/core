﻿using System.Linq;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using BGC.Base58;
using LevelDB;
using Secp256k1Net;
using System.Numerics;

namespace BGC {
	public static class Utils {

		public static bool ValidateAddress(string address) {
			if (address.Length < 26 || address.Length > 35) return false;
			return ValidateAddress(Base58Encode.Decode(address));
		}

		public static bool ValidateAddress(byte[] address) {
			SHA256 sha = SHA256.Create();
			byte[] d1 = sha.ComputeHash(Helper.SubArray(address, 0, 21));
			byte[] d2 = sha.ComputeHash(d1);

			if (!Helper.SubArray(address, 21, 4).SequenceEqual(Helper.SubArray(d2, 0, 4))) return false;
			return true;
		}

		public static byte[] StringToBytes(string data) {
			return Encoding.UTF8.GetBytes(data);
		}
		
		public static string ToHex(byte[] bytes) {
			return String.Join(" ", bytes.Select(b => b.ToString("x2")).ToArray()).Replace(" ", "").ToUpper();
		}
		
		public static byte[] FromHex(string value) {
			byte[] result = new byte[value.Length / 2];

			value = value.ToUpper();

			for (int i = 0; i < value.Length; i++) {
				result[i / 2] <<= 4;
				switch (value[i]) {
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
		public static byte[] EncryptBytes(byte[] plainBytes, byte[] passkey, byte[] iv) {
			SHA256 sha = new SHA256Managed();
			byte[] key = sha.ComputeHash(passkey);

			// Instantiate a new Aes object to perform string symmetric encryption
			Aes encryptor = Aes.Create();

			encryptor.Mode = CipherMode.CBC;

			// Set key and IV
			byte[] aesKey = new byte[32];
			Array.Copy(key, 0, aesKey, 0, 32);
			encryptor.Key = aesKey;
			encryptor.IV = iv;

			// Instantiate a new MemoryStream object to contain the encrypted bytes
			MemoryStream memoryStream = new MemoryStream();

			// Instantiate a new encryptor from our Aes object
			ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();

			// Instantiate a new CryptoStream object to process the data and write it to the 
			// memory stream
			CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

			// Encrypt the input plaintext string
			cryptoStream.Write(plainBytes, 0, plainBytes.Length);

			// Complete the encryption process
			cryptoStream.FlushFinalBlock();

			// Convert the encrypted data from a MemoryStream to a byte array
			byte[] cipherBytes = memoryStream.ToArray();

			// Close both the MemoryStream and the CryptoStream
			memoryStream.Close();
			cryptoStream.Close();

			return cipherBytes;
		}

		public static byte[] DecryptBytes(byte[] cipherBytes, byte[] passkey, byte[] iv) {
			SHA256 sha = new SHA256Managed();
			byte[] key = sha.ComputeHash(passkey);
			// Instantiate a new Aes object to perform string symmetric encryption
			Aes encryptor = Aes.Create();

			encryptor.Mode = CipherMode.CBC;

			// Set key and IV
			byte[] aesKey = new byte[32];
			Array.Copy(key, 0, aesKey, 0, 32);
			encryptor.Key = aesKey;
			encryptor.IV = iv;

			// Instantiate a new MemoryStream object to contain the encrypted bytes
			MemoryStream memoryStream = new MemoryStream();

			// Instantiate a new encryptor from our Aes object
			ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();

			// Instantiate a new CryptoStream object to process the data and write it to the 
			// memory stream
			CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

			// Will contain decrypted plaintext
			string plainText = String.Empty;

			try {
				// Decrypt the input ciphertext string
				cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

				// Complete the decryption process
				cryptoStream.FlushFinalBlock();

				// Convert the decrypted data from a MemoryStream to a byte array
				byte[] plainBytes = memoryStream.ToArray();
				return plainBytes;
			} finally {
				// Close both the MemoryStream and the CryptoStream
				memoryStream.Close();
				cryptoStream.Close();
			}
		}

		public static (byte[], bool) SignData(byte[] hash, byte[] privateKey) {
			using (Secp256k1 secp256k1 = new Secp256k1()) {
				byte[] sig = new byte[Secp256k1.UNSERIALIZED_SIGNATURE_SIZE];
				bool valid = secp256k1.SignRecoverable(sig, hash, privateKey);
                return (sig, valid);
			}
		}

        public static byte[] ConcatBytes(byte[] b1, byte[] b2) {
            if (b2 == null) return b1;
            byte[] result = new byte[b2.Length + b2.Length];
            uint i = 0;
            for (; i < b1.Length; i++) {
                result[i] = b1[i];
            }
            for (uint j = 0; j < b2.Length; j++) {
                result[i] = b2[j];
                i++;
            }

            return result;
        }

        public static byte[] BuildKey(string prefix, byte[] hash = null) {
            return ConcatBytes(Encoding.ASCII.GetBytes(prefix), hash);
        }

		public static (BigInteger numerator, BigInteger denominator) Fraction(decimal d) {
			int[] bits = decimal.GetBits(d);
			BigInteger numerator = (1 - ((bits[3] >> 30) & 2)) *
								unchecked(((BigInteger)(uint)bits[2] << 64) |
											((BigInteger)(uint)bits[1] << 32) |
											(BigInteger)(uint)bits[0]);
			BigInteger denominator = BigInteger.Pow(10, (bits[3] >> 16) & 0xff);
			return (numerator, denominator);
		}

    }
}