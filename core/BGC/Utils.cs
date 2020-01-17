using System;
using System.Collections;

public static class Utils {
	public static byte[] AppendByte(byte[] bArray, byte newByte) {
		byte[] newArray = new byte[bArray.Length + 1];
		bArray.CopyTo(newArray, 1);
		newArray[0] = newByte;
		return newArray;
	}
}