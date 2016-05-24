using System;
/*
 * Программа при запуске запрашивает rss url и мыло пользователя.
 * rss проверяется каждые 10 секунд на наличие новых новостей.
 * отправляются только новости, опубликованные после запуска программы
 * в письме приходит по три новости
 * после каждого отправленного письма в консолько выводится сообщение о доставке
 * как бе все)
 */

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
