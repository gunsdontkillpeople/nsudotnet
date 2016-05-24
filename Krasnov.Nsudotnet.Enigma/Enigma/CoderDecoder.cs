
using System;
using System.IO;
using System.Security.Cryptography;


namespace Enigma
{
    class CoderDecoder
    {
        private SymmetricAlgorithm _algorithm;
        private void Encrypt(CommandLineParser parser)
        {
            _algorithm.GenerateIV();
            _algorithm.GenerateKey();
            ICryptoTransform encryptor = _algorithm.CreateEncryptor();
            using (FileStream inputFileStream = new FileStream(parser.InputFileName, FileMode.Open, FileAccess.Read))
            {
                using (FileStream outputFileStream = new FileStream( parser.OutputFileName,FileMode.Create,FileAccess.Write))
                {
                    using ( CryptoStream cryptoStream = new CryptoStream(outputFileStream, encryptor , CryptoStreamMode.Write))
                    {
                        inputFileStream.CopyTo(cryptoStream);
                    }
                    using (BinaryWriter keyWriter = new BinaryWriter(File.Open(parser.KeyFileName, FileMode.Create, FileAccess.Write)))
                    {
                        keyWriter.Write(Convert.ToBase64String(_algorithm.Key));
                        keyWriter.Write(Convert.ToBase64String(_algorithm.IV));
                    }
                }
            }

        }

        private void Decrypt(CommandLineParser parser)
        {
            using (FileStream inputFileStream = new FileStream(parser.InputFileName, FileMode.Open, FileAccess.Read))
            {
                using (
                    BinaryReader keyReader =
                        new BinaryReader(File.Open(parser.KeyFileName, FileMode.Open, FileAccess.Read)))
                {
                    _algorithm.Key = Convert.FromBase64String(keyReader.ReadString());
                    _algorithm.IV = Convert.FromBase64String(keyReader.ReadString());
                }
                using (ICryptoTransform decryptor = _algorithm.CreateDecryptor())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(inputFileStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (
                            FileStream outputFileStream = new FileStream(parser.OutputFileName, FileMode.Create,
                                FileAccess.Write))
                        {
                            cryptoStream.CopyTo(outputFileStream);
                        }
                    }
                }
            }
        }

        private void ChooseMode(CommandLineParser parser)
        {
            switch (parser.Encrypt)
            {
                case "encrypt":
                    Encrypt(parser);
                    break;
                case "decrypt":
                    Decrypt(parser);
                    break;
            }
        }

        public void ChooseEncryptAlg(CommandLineParser parser)
        {
            switch (parser.Algorithm)
            {
                case "aes":
                    _algorithm = new AesManaged();
                    break;
                case "des":
                    _algorithm = new DESCryptoServiceProvider();
                    break;
                case "rc2":
                    _algorithm = new RC2CryptoServiceProvider();
                    break;
                case "rijndael":
                    _algorithm = new RijndaelManaged();
                    break;
            }
            ChooseMode(parser);
        }
    }
}
