using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using BGC.Base58;
using BGC.Contracts;
using BGC.Network;
using BGC.Wallet;
using Secp256k1Net;

namespace BGC {
    class BGC {
        static void Main(string[] args) {
            CLI.CLI.Run(args);
        }
    }
}