﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace BGC.Network
{
    class Utils
    {
        public static byte[] CreateTextMessage(string text, Message.MAGIC targetNetwork = Message.MAGIC.TestNetwork)
        {
            byte[] txt_bytes = Encoding.Unicode.GetBytes(text);
            byte[] r = new byte[Message.MessageStructureSize + txt_bytes.Length]; // utf-16 -> 2 bytes / char

            // Not flexible
            r[0] = (byte) targetNetwork;
            r[1] = (byte) Message.COMMAND.TextMessage;

            WriteUInt(r, 2, (uint) txt_bytes.Length);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(txt_bytes);
                for (int i = 0; i < 4; i++)
                {
                    r[i + 6] = hash[i];
                }
            }

            for (int i = 0; i < txt_bytes.Length; i++)
            {
                r[i + 10] = txt_bytes[i];
            }

            return r;
        }

        private static void WriteUInt(byte[] target, int startIndex, uint value)
        {
            byte[] n = BitConverter.GetBytes(value);
            for (int i = 0; i < n.Length; i++)
                target[startIndex + i] = n[i];
        }
    }
}
