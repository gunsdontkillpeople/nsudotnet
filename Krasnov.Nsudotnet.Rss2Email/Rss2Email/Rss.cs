using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Rss2Email
{
    class Rss
    {
        public class ChannelClass
        {
            private string _title;
            public string Title
            {
                get { return _title; }
                set { _title = value; }
            }

            private string _description;
            public string Description
            {
                get { return _description; }
                set { _description = value; }
            }


            private string _link;
            public string Link
            {
                    get { return _link; }
                    set { _link = value; }
            }
        }

        public class Item
        {
            private string _title;
            public string Title
            {
                get { return _title; }
                set { _title = value; }
            }

            private string _description;
            public string Description
            {
                get { return _description; }
                set { _description = value; }
            }

            private string _link;
            public string Link
            {
                get { return _link; }
                set { _link = value; }
            }

            private string _copyright;
            public string CopyRight
            {
                get { return _copyright; }
                set { _copyright = value; }
            }


        }

        private Item[] _articles;
        public Item[] Articles
        {
            get { return _articles;}
            set { _articles = value; }
        }

        private ChannelClass _channel;
        public ChannelClass Channel
        {
            get { return Channel; }
            set { Channel = value; }
        }

        private DateTime _lastMessageDate;
        private int _numberNewArticles;

        public Rss()
        {
            _channel = new ChannelClass();
            _lastMessageDate = DateTime.Now;
            _numberNewArticles = 0;
            _articles = new Item[3];
        }


        public void ParseArticles(string sourceURL)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(sourceURL);
                XmlNode mainNode = xml.DocumentElement;
                XmlNodeList nodesList = mainNode.ChildNodes;

                foreach (XmlNode channel in nodesList)
                {
                    foreach (XmlNode channelItem in channel)
                    {
                        switch (channelItem.Name)
                        {
                            case "title":
                                _channel.Title = channelItem.InnerText;
                                break;
                            case "description":
                                _channel.Description = channelItem.InnerText;
                                break;
                            case "link":
                                _channel.Link = channelItem.InnerText;
                                break;
                            case "item":
                                XmlNodeList itemList = channelItem.ChildNodes;
                                _articles[_numberNewArticles] = new Item();
                                foreach (XmlNode item in itemList)
                                {
                                    switch (item.Name)
                                    {
                                        case "title":
                                            _articles[_numberNewArticles].Title = item.InnerText;
                                            break;
                                        case "description":
                                            _articles[_numberNewArticles].Description = item.InnerText;
                                            break;
                                        case "link":
                                            _articles[_numberNewArticles].Link = item.InnerText;
                                            break;
                                        case "copyright":
                                            _articles[_numberNewArticles].CopyRight = item.InnerText;
                                            break;
                                        case "pubDate":
                                            if (DateTime.Parse(item.InnerText) > _lastMessageDate)
                                            {
                                                _lastMessageDate = DateTime.Parse(item.InnerText);
                                                _numberNewArticles++;
                                                if (_numberNewArticles == 3)
                                                {
                                                    return;
                                                }
                                            }
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public string CreateMessage()
        {
            StringBuilder sendString = new StringBuilder();
            sendString.Append("RSS: " + _channel.Title + "\n");
            sendString.Append("Содержание: " + _channel.Description + "\n");
            sendString.Append("Ссылка на ресурс: " + _channel.Link + "\n");

            foreach (Item article in _articles)
            {
                sendString.Append("\n" + "____________________________________________" + "\n");
                sendString.Append("Тема: " + article.Title + "\n");
                sendString.Append("Содержание: " + article.Description + "\n");
                sendString.Append("Автор: " + article.CopyRight + "\n");
                sendString.Append("Ссылка на новость: " + article.Link + "\n\n\n\n" + "____________________________________________");

            }

            return sendString.ToString();
        }

        public void SendRss(string sourceURL, string recieverMail)
        {

            while (true)
            {
                if (_numberNewArticles == 3)
                {

                    Mail.SendMail(CreateMessage(), recieverMail);
                    _articles = new Item[3];
                    _numberNewArticles = 0;
                    Console.WriteLine("Three new articles was sent");
                }
                else
                {
                    System.Threading.Thread.Sleep(10000);
                    ParseArticles(sourceURL);
                }
            }
        }
    }
}
