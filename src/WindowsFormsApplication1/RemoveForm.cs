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
    public partial class RemoveForm : Form
    {
        public delegate void EventDeligate(InventoryItem item, bool removeStock, bool removeHistory);
        public event EventDeligate Removed;

        private InventoryItem currentItem;

        public RemoveForm()
        {
            InitializeComponent();
        }

        private void RemoveForm_Load(object sender, EventArgs e)
        {
            
            this.Hide();
        }

        public void ShowForm(InventoryItem itm)
        {
            chkHistory.Checked = true;
            chkStock.Checked = true;
            currentItem = itm;
            this.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Removed(currentItem, chkStock.Checked, chkHistory.Checked);

        }
    }
}
