﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Linq;

using CAPNet.Models;

namespace CAPNet.Cmd
{
    class Program
    {
        static void Main()
        {
            ReadNWS();
        }

        static void ReadNWS()
        {
            List<Alert> alerts;

            using (var reader = XmlReader.Create("http://alerts.weather.gov/cap/us.php?x=0"))
            {
                var feed = SyndicationFeed.Load(reader);

                alerts = (from item in feed.Items
                          from alert in GetAlerts(item.Id)
                          select alert)
                         .ToList();
            }

            if (!alerts.Any())
                Console.WriteLine("No alerts");
            else if(alerts.Count == 1)
                Console.WriteLine("1 alert");
            else
                Console.WriteLine("{0} alerts", alerts.Count);

            foreach (var alert in alerts.Where(a => a.Info.Any(i => i.Severity <= Severity.Severe)))
            {
                Console.WriteLine("------------------------------");
                Console.WriteLine("Sender: " + alert.Sender);
                Console.WriteLine("Note: " + alert.Note);

                foreach (var info in alert.Info)
                {
                    Console.WriteLine("  *******");
                    Console.WriteLine("  " + info.Headline);
                    Console.WriteLine("  Effective: " + info.Effective);
                    Console.WriteLine("  Expires: " + info.Expires);
                    Console.WriteLine("  Severity: " + info.Severity);

                    foreach (var area in info.Areas)
                    {
                        Console.WriteLine("  Area: " + area.Description);
                    }

                }
            }

            Console.WriteLine("End of process.");
            Console.ReadLine();
        }

        public static IEnumerable<Alert> GetAlerts(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/xml";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    string message = String.Format("GET failed. Received HTTP {0}", response.StatusCode);
                    throw new ApplicationException(message);
                }
                else
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        XDocument doc = XDocument.Load(responseStream);
                        var alertList = XmlParser.Parse(doc);
                        return alertList;
                    }
                }
            }
        }
    }
}
