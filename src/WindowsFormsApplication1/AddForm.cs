using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class AddForm : Form
    {
        public AddForm()
        {
            InitializeComponent();
        }

        public delegate void EventDeligate(InventoryItem item);
        public event EventDeligate InfoAdded;
        public event EventDeligate InfoUpdated;

        private string mode;

        float rs;

        private void label3_Click(object sender, EventArgs e)
        {
            
        }

        protected override void OnShown(EventArgs e)
        {
            
            base.OnShown(e);
        }

        
        public void ShowEditForm(InventoryItem item)
        {
            this.Show();

            txtPrice.Text = item.Price.ToString();
            numQuantity.Value = item.Quantity;

            btnAdd.Visible = false;
            btnUpdate.Visible = true;

            txtBrand.Text = item.Values[0];
            txtCategory .Text = item.Values[1];
            txtProductId.Text = item.Values[2];
            txtColor.Text = item.Values[3];

            mode = "edit";

            txtProductId.Enabled = false;

            lblHeading.Text = mode.ToUpperInvariant() + " INVENTORY";
        }

        public void ShowForm()
        {
            ResetFields();
            btnAdd.Visible = true;
            btnUpdate.Visible = false;
            txtProductId.Enabled = true;

            mode = "add";
            this.Show();
            lblHeading.Text = mode.ToUpperInvariant() + " INVENTORY";
        }

        public void ShowForm(InventoryItem item)
        {
            this.Show();
            mode = "add";
            txtPrice.Text = "";
            numQuantity.Value = 0;
            txtBill.Text = item.BillNo;
            txtBrand.Text = item.Values[0];
            txtCategory .Text = item.Values[1];
            txtProductId.Text = item.Values[2];
            txtColor.Text = item.Values[3];

            btnAdd.Visible = true;
            btnUpdate.Visible = false;
            txtProductId.Enabled = false;

            lblHeading.Text = mode.ToUpperInvariant() + " INVENTORY";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InventoryItem itm = new InventoryItem(txtBrand.Text, txtCategory.Text, txtProductId.Text, txtColor.Text, int.Parse(numQuantity.Value.ToString()), float.Parse(txtPrice.Text.ToString()));

            itm.BillNo = txtBill.Text;
            itm.Date = txtDate.Value;

            this.InfoAdded.Invoke(itm);
            ResetFields();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ResetFields();
        }

        private void ResetFields()
        {
            txtBrand.Text = "";
            txtCategory.Text = "";
            txtColor.Text = "";
            txtPrice.Text = "";
            txtProductId.Text = "";
            numQuantity.Value = 0;
            txtBill.Text = "";
            txtDate.Value = DateTime.Now;

            btnAdd.Visible = false;
            btnUpdate.Visible = false;
            txtBrand.Focus();
        }

        private void field_TextChanged(object sender, EventArgs e)
        {
            
            if ((txtPrice.Text.Length > 0) && (float.TryParse(txtPrice.Text, out rs)) && (txtProductId.Text.Length > 0) && (numQuantity.Value > 0))
            {
                (mode == "add" ? btnAdd : btnUpdate).Visible = true;
            }
            else
            {
                (mode == "add" ? btnAdd : btnUpdate).Visible = false;
            }
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

        private void AddForm_Load(object sender, EventArgs e)
        {

        }

        private void field_Changed(object sender, KeyEventArgs e)
        {
            this.field_TextChanged(null, EventArgs.Empty);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            InventoryItem itm = new InventoryItem(txtBrand.Text, txtCategory.Text, txtProductId.Text, txtColor.Text, int.Parse(numQuantity.Value.ToString()), float.Parse(txtPrice.Text.ToString()));

            itm.BillNo = txtBill.Text;
            itm.Date = txtDate.Value;

            this.InfoUpdated.Invoke(itm);
            ResetFields();
        }
    }
}
