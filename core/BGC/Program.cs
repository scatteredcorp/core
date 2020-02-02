using System;
using System.Threading;
using BGC.Blockchain;
using BGC.Contracts;
using BGC.Wallet;

namespace BGC {
    class BGC {
        static void Main(string[] args) {
            CLI.CLI.Run(args);
            
            Blockchain.Blockchain.Resume();
            
            Wallet.Wallet wallet = new Wallet.Wallet(WalletHelper.GeneratePrivateKey());


            TransactionContract contract = TransactionHelper.CoinbaseTransaction(wallet.Address(), Blockchain.Blockchain.Height);

            BlockHeader blockHeader = new BlockHeader(1, Blockchain.Blockchain.LastHash, new byte[32], 0, 0, 0);
            IContract[] contracts = new IContract[1]{contract};
            Block b = new Block(blockHeader, contracts);
            ProofOfWork.PoW pow = new ProofOfWork.PoW(b);
            (uint nonce, byte[] hash) = pow.Run();
            b.BlockHeader.Nonce = nonce;
            b.Push();
            
            //Thread.Sleep(1000);
            
            Logger.Debug(string.Join(" ", Blockchain.Blockchain.LastHash));
            
            Iterator iterator = new Iterator();
            while (iterator.CanIterate) {
                Block block = iterator.Next();
                Console.WriteLine(Utils.ToHex(block.BlockHeader.Hash()));
            }
        }
    }
}