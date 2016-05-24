using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
    
    class CommandLineParser
    {
        private string _encrypt;
        public string Encrypt
        {
            get { return  _encrypt; }
            set { _encrypt = value;  }
        }

        private string _algorithm;
        public string Algorithm
        {
            get { return _algorithm; }
            set { _algorithm = value; }
        }

        private string _inputFileName;
        public string InputFileName
        {
            get { return _inputFileName; }
            set { _inputFileName = value; }
        }

        private string _outputFileName;
        public string OutputFileName
        {
            get { return _outputFileName; }
            set { _outputFileName = value; }
        }

        private string _keyFileName;
        public string KeyFileName
        {
            get { return _keyFileName; }
            set { _keyFileName = value; }
        }

        public CommandLineParser()
        {
            _encrypt = null;
            _inputFileName = null;
            _outputFileName = null;
            _keyFileName = null;
            _algorithm = null;
        }
        public bool GetArgs(string[] args)
        {
            switch (args[0])
            {
                case "encrypt":
                    _encrypt = args[0];
                    break;
                case "decrypt":
                    _encrypt = args[0];
                    break;
                default:
                    Console.WriteLine("Wrong firs argument: {0}", args[0]);
                    return false;
            }

            _inputFileName = args[1];

            switch (args[2])
            {
                case "aes":
                    _algorithm = args[2];
                    break;
                case "des":
                    _algorithm = args[2];
                    break;
                case "rc2":
                    _algorithm = args[2];
                    break;
                case "rijndael":
                    _algorithm = args[2];
                    break;
                default:
                    Console.WriteLine("The {0} is not algorithm of list: \" rc2, aes, des,rijndael \" ", args[2]);
                    return false;
            }
            if (_encrypt == "encrypt")
            {
                _outputFileName = args[3];
                _keyFileName = String.Concat(args[1].Substring(0, args[1].LastIndexOf('.')) + ".key.txt");
            }
            else
            {
                _outputFileName = args[4];
                _keyFileName = args[3];
            }
            return true;
        }
    }
}
