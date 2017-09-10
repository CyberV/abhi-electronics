using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace WindowsFormsApplication1
{
    class DB
    {
        public delegate void EventDelegate(string status);
        public static event EventDelegate onEvent;




        public static List<Order> ReadHistory()
        {
            List<Order> items = new List<Order>();
            XmlDocument doc = new XmlDocument();
            
            
                if(File.Exists("Data.xml"))
                {
                    doc.Load("Data.xml");
                }
                else
                {
                   XDocument xmlFile = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("XML File for storing Records"));
                    xmlFile.Add( new XElement( "Root" ) );
                xmlFile.Save("Data.xml");
                doc.Load("Data.xml");
                }
            
            string text = string.Empty;
            XmlNodeList xnl = doc.SelectNodes("Data/StockHistory/History");
            foreach (XmlNode node in xnl)
            {
                

                    items.Add(new Order(
                        node["type"].InnerText,
                        node["date"].InnerText,
                        node["productId"].InnerText,
                        int.Parse(node["quantity"].InnerText),
                        float.Parse(node["price"].InnerText),
                        node["bill"].InnerText));
                    
                
            }

            onEvent.Invoke("Added " + items.Count.ToString() + " Records");
            return items;
        }

        public static List<InventoryItem> ReadStocks()
        {
            List<InventoryItem> items = new List<InventoryItem>();
            XmlDocument doc = new XmlDocument();
            
            
                if(File.Exists("Data.xml"))
                {
                    doc.Load("Data.xml");
                }
                else
                {
                   XDocument xmlFile = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("XML File for storing Records"));
                    xmlFile.Add( new XElement( "Root" ) );
                xmlFile.Save("Data.xml");
                doc.Load("Data.xml");
                }
            
            string text = string.Empty;
            XmlNodeList xnl = doc.SelectNodes("Data/StockListing/Stock");
            string s1,s2,s3,s4,s5,s6;
            foreach (XmlNode node in xnl)
            {
                {
                    s1 = node["brand"].InnerText;
                    s2 = node["category"].InnerText;
                    s3 = node["productId"].InnerText;
                    s4 = node["color"].InnerText;
                    s5 = node["quantity"].InnerText;
                    s6 = node["price"].InnerText;

                    items.Add(new InventoryItem(s1, s2, s3, s4, int.Parse(s5), float.Parse(s6)));
                    
                }
            }

            onEvent.Invoke("Added " + items.Count.ToString() + " Records");
            return items;
        }

        public static void Write(InventoryItem[] records, Order[] orders)
        {
            onEvent.Invoke("Opening File");
            XmlTextWriter textWriter;

            if (File.Exists("Data.xml"))
            {
                

                textWriter = new XmlTextWriter("Data.xml", Encoding.Unicode);
            }
            else
            {
                XDocument xmlFile = new XDocument(
             new XDeclaration("1.0", "utf-8", "yes"),
             new XComment("XML File for storing Records"));
                xmlFile.Add(new XElement("Root"));
                xmlFile.Save("Data.xml");
                textWriter = new XmlTextWriter("Data.xml", Encoding.Unicode);
            }

            
            // Opens the document  
            textWriter.WriteStartDocument();
            // Write comments  
            textWriter.WriteComment("Inventory Data: LG Electronics. All Rights Reserved.");
            // Write first element  
            textWriter.WriteStartElement("Data");
            textWriter.WriteStartElement("StockListing");
            foreach (InventoryItem record in records)
            {
                textWriter.WriteStartElement("Stock");
                // Write next element  
                for (int i = 0; i < record.Keys.Length; i++)
                {
                    textWriter.WriteStartElement(record.Keys[i], "");
                    textWriter.WriteString(record.Values[i]);
                    textWriter.WriteEndElement();
                }

                textWriter.WriteEndElement();  // for Each record
            }

            textWriter.WriteEndElement(); // for StockListing

            textWriter.WriteStartElement("StockHistory");
            foreach (Order record in orders)
            {
                textWriter.WriteStartElement("History");
                // Write next element  
                for (int i = 0; i < record.Keys.Length; i++)
                {
                    textWriter.WriteStartElement(record.Keys[i], "");
                    textWriter.WriteString(record.Values[i]);
                    textWriter.WriteEndElement();
                }

                textWriter.WriteEndElement();  // for Each record
            }

            textWriter.WriteEndElement(); // for StockHistory

            textWriter.WriteEndElement(); // for Data



            // Ends the document.  
            textWriter.WriteEndDocument();
            // close writer  
            textWriter.Close();
            onEvent.Invoke("Write Finished");
        }
    }
}
