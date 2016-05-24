using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rss2Email
{
    [Serializable]
        [XmlRoot("rss")]
        public class RssRoot
        {
            [XmlElement("channel")]
            public ChannelClass Channel { get; set; }
        }

        [Serializable]
        public class ChannelClass
        {
            [XmlElement("item")]
            public List<Item> Items { get; set; }
        }

        [Serializable]
        public class Item : IComparable<Item>
        {
            [XmlElement("title")]
            public string Title { get; set; }
            
            [XmlElement("description")]
            public string Description { get; set; }

            [XmlElement("link")]
            public string Link { get; set; }

            [XmlElement("copyright")]
            public string CopyRight { get; set; }

            [XmlElement("pubDate")]
            public string PubDate { get; set; }

            public int CompareTo(Item other)
            {
                return DateTime.Parse(PubDate).CompareTo(DateTime.Parse(other.PubDate));
            }
        }
    public class Rss
    {
        private XmlSerializer _xmlSerializer;
        private DateTime _lastMessageDate;
        private List<Item> _newMessages;
        private int _checkFirstIter = 0;

        public Rss()
        {

            _newMessages = new List<Item>();
        }


        public void ParseArticles(string sourceURL)
        {
            _xmlSerializer = new XmlSerializer(typeof(RssRoot));
            using (XmlReader rssReader = XmlReader.Create(sourceURL))
            {
                RssRoot xml= (RssRoot) _xmlSerializer.Deserialize(rssReader);
                List<Item> nodesList = xml.Channel.Items;
                nodesList.Sort();
                nodesList.Reverse();
                if (_checkFirstIter == 0)
                {
                    _lastMessageDate = DateTime.Parse(nodesList[nodesList.Count-1].PubDate);
                }
                foreach (var node in nodesList)
                {
                    if (DateTime.Parse(node.PubDate) > _lastMessageDate)
                    {
                        _newMessages.Add(node);
                    }
                }
                if (_checkFirstIter == 0)
                {
                    _newMessages.Add(nodesList[nodesList.Count - 1]);
                    _checkFirstIter = 1;
                }
                _lastMessageDate = DateTime.Parse(nodesList[0].PubDate);
            }
        }

        public string CreateMessage()
        {
            StringBuilder sendString = new StringBuilder();
            sendString.Append("А вот и пачка новых rss новостей\n\n\n");
            foreach (Item article in _newMessages)
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
                if (_newMessages.Count != 0)
                {

                    Mail.SendMail(CreateMessage(), recieverMail);
                    _newMessages.Clear();
                    _newMessages = new List<Item>();
                    Console.WriteLine("New articles was sent");
                }
                else
                {
                    
                    ParseArticles(sourceURL);
                    System.Threading.Thread.Sleep(10000);
                }
            }
        }
    }
}
