using System;

namespace Rss2Email
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter your rss url:");
            string rssURL = Console.ReadLine();
            Rss rssReader = new Rss();

            Console.WriteLine("Please enter reciever mail:");
            try
            {
                rssReader.SendRss(rssURL, Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
