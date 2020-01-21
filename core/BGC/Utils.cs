using System;
using System.Collections;
using System.Linq;
using System.Security.Cryptography;
using BGC.Base58;

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
}