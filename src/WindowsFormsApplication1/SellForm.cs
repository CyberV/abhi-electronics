using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class SellForm : Form
    {

        public delegate void EventDeligate(InventoryItem item);
        public event EventDeligate Sold;

        InventoryItem order;

        int stockCount;

        public SellForm()
        {
            InitializeComponent();
        }

        public void ShowForm(InventoryItem item)
        {
            this.Show();
            stockCount = item.Quantity;
            order = item;
            numQuantity.Maximum = stockCount;
            ResetFields();

            lblCount.Text = item.Quantity.ToString();
            lblProduct.Text = item.ProductName;
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            order.Date = txtDate.Value;
            order.BillNo = txtBill.Text;
            order.sellCount = (int)numQuantity.Value;
            order.Price = float.Parse(txtPrice.Text);
            order.UpdateCount(order.Quantity - (int)numQuantity.Value);
            Sold.Invoke(order);
            this.Hide();
        }

        private void ResetFields()
        {
            txtPrice.Text = "";
            numQuantity.Value = 0;
            btnSell.Visible = false;
            txtBill.Text = "";
            txtDate.Value = DateTime.Now;
                
        }

        private void field_Changed(object sender, EventArgs e)
        {
            if ((txtPrice.Text.Length > 0) && (numQuantity.Value > 0 && numQuantity.Value <= stockCount))
            {
                btnSell.Visible = true;
            }
            else
            {
                btnSell.Visible = false;
            }
        }

        private void field_Changed(object sender, KeyEventArgs e)
        {
            this.field_Changed(null, EventArgs.Empty);
        }

        private void SellForm_Load(object sender, EventArgs e)
        {

        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }
}
