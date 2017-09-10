using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public class InventoryItem
    {

        
        string brand;
        string category, productId, color;
        int quantity;
        float price;
        string[] items;
        string[] keys;

        public int sellCount;

        public string BillNo
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public float Price
        {
            get { return price; }
            set { price = value; }
        }

        public string ProductName
        {
            get
            {
                return brand + " " + category + " " + productId + " " + color;
            }
        }

        public string ProductID
        {
            get
            {
                return productId;
            }
        }
    
        public int Quantity
        {
            get
            {
                return quantity;
            }
        }

        public ListViewItem ListItem
        {
            get
            {
                return new ListViewItem(Values);
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
                items = new string[] { this.brand, this.category, this.productId, this.color, this.quantity.ToString(), this.price.ToString() };
                return items;
            }
        }

        public InventoryItem(string brand, string category, string productId, string color, int quantity, float price)
        {
            this.brand = brand;
            this.category = category;
            this.productId = productId;
            this.color = color;
            this.quantity = quantity;
            this.price = price;
            this.keys = new string[] { "brand", "category", "productId", "color", "quantity", "price" };
            this.items = new string[] { this.brand, this.category, this.productId, this.color, this.quantity.ToString(), this.price.ToString() };

            
        }

        public void UpdateCount(int cnt)
        {
            quantity = cnt;
            this.items[4] = this.quantity.ToString();
        }
        
    }
}
