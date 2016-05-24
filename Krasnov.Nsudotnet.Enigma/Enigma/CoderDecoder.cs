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
            using (_algorithm = ChooseEncryptAlg(parser.Algorithm))
            {
                _algorithm.GenerateIV();
                _algorithm.GenerateKey();
                using (FileStream inputFileStream = new FileStream(parser.InputFileName, FileMode.Open, FileAccess.Read)
                    )
                {
                    using (
                        FileStream outputFileStream = new FileStream(parser.OutputFileName, FileMode.Create,
                            FileAccess.Write))
                    {
                        using (ICryptoTransform encryptor = _algorithm.CreateEncryptor())
                        {
                            using (
                                CryptoStream cryptoStream = new CryptoStream(outputFileStream, encryptor,
                                    CryptoStreamMode.Write))
                            {
                                inputFileStream.CopyTo(cryptoStream);
                            }
                        }
                        using (
                            BinaryWriter keyWriter =
                                new BinaryWriter(File.Open(parser.KeyFileName, FileMode.Create, FileAccess.Write)))
                        {
                            keyWriter.Write(Convert.ToBase64String(_algorithm.Key));
                            keyWriter.Write(Convert.ToBase64String(_algorithm.IV));
                        }
                    }
                }
            }

        }

        private void Decrypt(CommandLineParser parser)
        {
            using (_algorithm = ChooseEncryptAlg(parser.Algorithm))
            {
                using (FileStream inputFileStream = new FileStream(parser.InputFileName, FileMode.Open, FileAccess.Read)
                    )
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
                        using (
                            CryptoStream cryptoStream = new CryptoStream(inputFileStream, decryptor,
                                CryptoStreamMode.Read))
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
        }

        public void ChooseMode(CommandLineParser parser)
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

        private SymmetricAlgorithm ChooseEncryptAlg(string algorithm)
        {
            switch (algorithm)
            {
                case "aes":
                    return new AesManaged();
                case "des":
                    _algorithm = new DESCryptoServiceProvider();
                    break;
                case "rc2":
                    return new RC2CryptoServiceProvider();
                case "rijndael":
                    return new RijndaelManaged();
            }
            return null;
        }
    }
}
