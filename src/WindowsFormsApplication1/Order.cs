using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class Order
    {
        public string ProductID { get; set; }
        public string BillNo { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string Date { get; set; }

        public string Type { get; set; }

        private string[] items;
        string[] keys;

        public ListViewItem ListItem
        {
            get
            {
                return new ListViewItem(items);
            }
        }

        public String[] Keys
        {
            get
            {
                return keys;
            }
        }

        public String[] Values
        {
            get
            {
                return items;
            }
        }

        public Order(string bs, string date, string id, int quantity, float price, string bill)
        {
            ProductID = id;
            Quantity = quantity;
            Price = price;
            BillNo = bill;
            Date = date;

            if (bs.ToLower().Contains('b') || bs.ToLower().Contains("buy"))
            {
                Type = "Buy";
            }
            else if (bs.ToLower().Contains('s') || bs.ToLower().Contains("sell"))
                Type = "Sell";
            else
                Type = "Update";

            this.keys = new string[] { "type", "date", "productId", "quantity", "price", "bill" };
            this.items = new string[] { Type, date, ProductID, Quantity.ToString(), Price.ToString(), BillNo };
        }
    }
}
