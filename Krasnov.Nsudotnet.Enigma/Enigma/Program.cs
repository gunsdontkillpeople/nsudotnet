using System;

namespace Enigma
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Not enough arguments");
            }
            else
            {
                CommandLineParser parser = new CommandLineParser();
                if (parser.GetArgs(args))
                {
                    CoderDecoder coder = new CoderDecoder();
                    coder.ChooseMode(parser);
                }
            }
        }
    }
}