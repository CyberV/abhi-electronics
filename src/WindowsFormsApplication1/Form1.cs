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
    public partial class Form1 : Form
    {

        private AddForm frmAdd;
        private SellForm frmSell;
        private RemoveForm frmRemove;

        private List<InventoryItem> items;
        private List<InventoryItem> all;
        private List<Order> orders;
        private List<Order> allOrders;

        private int currentIndex;

        public Form1()
        {
            InitializeComponent();
            
            items = new List<InventoryItem>();
            orders = new List<Order>();

            frmAdd = new AddForm();
            frmSell = new SellForm();
            frmRemove = new RemoveForm();

            //frm.MdiParent = this;
            frmAdd.InfoAdded += frm_InfoAdded;
            frmAdd.InfoUpdated += frmAdd_InfoUpdated;
            frmSell.Sold += frmSell_Sold;
            frmRemove.Removed += frmRemove_Removed;

            DB.onEvent += DB_onEvent;

            items = all;
            orders = allOrders;

            ReadRecords();
            
        }

        void frmAdd_InfoUpdated(InventoryItem item)
        {
            frmAdd.Hide();

            Order ordr = new Order("update", item.Date.ToShortDateString(), item.Values[2], item.Quantity, float.Parse(item.Values[5]), item.BillNo);
            orders.Add(ordr);
            allOrders.Add(ordr);
            lstHistory.Items.Add(ordr.ListItem);

            InventoryItem fndItem = items.Find(itm => ((itm.Values[2] == item.Values[2])));
            if (fndItem != null)
            {
                int replaceIndex = items.IndexOf(fndItem);
                //fndItem.UpdateCount(item.Quantity + fndItem.Quantity);
                items[replaceIndex] = item;
                //all[all.IndexOf(item)] = item;
                lstResults.Items[replaceIndex] = item.ListItem;
                Debug.Print("Updating item to List.Total : " + all.Count.ToString());
            }

            PopulateList();
            WriteRecords();
            
        }

        void frmRemove_Removed(InventoryItem item, bool removeStock, bool removeHistory)
        {
            if (removeStock)
            {
                items.Remove(item);
                PopulateList();
            }

            if (removeHistory)
            {
                RemoveFromHistory(item);
                PopulateHistory();
            }

            WriteRecords();
        }

        void frmSell_Sold(InventoryItem item)
        {
            Order ordr = new Order("sell", item.Date.ToShortDateString(), item.Values[2], item.sellCount, float.Parse(item.Values[5]), item.BillNo);
            orders.Add(ordr);
            allOrders.Add(ordr);
            lstHistory.Items.Add(ordr.ListItem);

            //item.UpdateCount(items[currentIndex].Quantity - item.Quantity);

            if (item.Quantity > 0)
            {
                items[currentIndex] = item;
                all[all.IndexOf(item)] = item;
                lstResults.Items[currentIndex] = item.ListItem;
            }
            else
            {
                items.RemoveAt(currentIndex);
                //Debugger.Break();
                //all.RemoveAt(all.IndexOf(item));
                lstResults.Items.RemoveAt(currentIndex);
            }
            WriteRecords();
            //throw new NotImplementedException();
        }

        void DB_onEvent(string status)
        {
            //throw new NotImplementedException();
            lblStatus.Text = status;
        }

        ~Form1()
        {
            frmAdd.Close();
        }

        private void NavItem_MouseEnter(object sender, EventArgs e)
        {
            Panel pnl = (Panel)sender;
            pnl.BackColor = Color.Orange;
        }

        private void NavItem_MouseLeave(object sender, EventArgs e)
        {
            Panel pnl = (Panel)sender;
            pnl.BackColor = Color.Wheat;
        }

        private void lbl_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            processLabelEvent(lbl, Color.Orange);
        }

        private void processLabelEvent(Label lbl, Color clr)
        {
            switch (lbl.Name.Replace("lbl", ""))
            {
                case "Stock": pnlStock.BackColor = clr; break;
                case "Recent": pnlRecent.BackColor = clr; break;
                case "Low": pnlLow.BackColor = clr; break;
                case "Add": pnlAdd.BackColor = clr; break;
            }
        }

        private void lbl_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            processLabelEvent(lbl, Color.Wheat);
        }

        private void groupBox1_MouseHover(object sender, EventArgs e)
        {
            GroupBox grp = (GroupBox)sender;

            grp.BackColor = Color.Orange;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pnlAdd_MouseEnter(object sender, EventArgs e)
        {
            Panel pnl = (Panel)sender;
            pnl.BackColor = Color.MediumSeaGreen;
        }

        private void pnlAdd_MouseLeave(object sender, EventArgs e)
        {
            Panel pnl = (Panel)sender;
            pnl.BackColor = Color.SeaGreen;
        }

        private void pnlAdd_Click(object sender, EventArgs e)
        {
            //InventoryItem itm = new InventoryItem();
            if (!frmAdd.IsDisposed)
            {
                frmAdd.Dispose();
            }

            frmAdd = new AddForm();
            frmAdd.InfoAdded += frm_InfoAdded;
            frmAdd.ShowForm();
            //
        }

        void frm_InfoAdded(InventoryItem item)
        {
            frmAdd.Hide();

            Order ordr = new Order("buy", item.Date.ToShortDateString(), item.Values[2], item.Quantity, float.Parse(item.Values[5]),item.BillNo);
            orders.Add(ordr);
            allOrders.Add(ordr);
            lstHistory.Items.Add(ordr.ListItem);

            InventoryItem fndItem = items.Find(itm => ((itm.Values[2] == item.Values[2])));
            if (fndItem != null)
            {
                int replaceIndex = items.IndexOf(fndItem);
                fndItem.UpdateCount(item.Quantity + fndItem.Quantity);
                items[replaceIndex] = fndItem;
                all[all.IndexOf(fndItem)] = fndItem;
                lstResults.Items[replaceIndex] = fndItem.ListItem;
                Debug.Print("Updating item to List.Total : " + all.Count.ToString());
            }
            else
            {
                
                items.Add(item);
                Debug.Print("Adding new item to Item List.Total : " + items.Count.ToString());
                //all.Add(item);
                lstResults.Items.Add(item.ListItem);
                Debug.Print("Adding new item to ALL List.Total : " + all.Count.ToString());
            }
            WriteRecords();
        }


        public void ReadRecords()
        {

            items = DB.ReadStocks();
            all = items;
            InventoryItem[] itms = items.ToArray<InventoryItem>();
            foreach (InventoryItem itm in itms)
            {
                lstResults.Items.Add(itm.ListItem);
            }

            orders = DB.ReadHistory();
            allOrders = DB.ReadHistory(); 
            foreach (Order ord in orders)
            {
                lstHistory.Items.Add(ord.ListItem);
            }



        }

        public void WriteRecords()
        {
            DB.Write(all.ToArray<InventoryItem>(), allOrders.ToArray());
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            int wid = (this.Width )/ 8;
            for (int i = 0; i < 6; i++)
            {
                lstResults.Columns[i].Width = wid;
                lstHistory.Columns[i].Width = wid;
            }
                
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (frmSell.IsDisposed)
            {
                frmSell = new SellForm();
                frmSell.Sold += frmSell_Sold;
            }

            currentIndex = lstResults.SelectedIndices[0];

            frmSell.ShowForm(items[currentIndex]);
        }
        
        //Add to Stock
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            if (!frmAdd.IsDisposed)
            {
                frmAdd.Dispose();
            }

            frmAdd = new AddForm();
            frmAdd.InfoAdded += frm_InfoAdded;
            currentIndex = lstResults.SelectedIndices[0];

            frmAdd.ShowForm(items[currentIndex]);
        }

        private void label5_MouseEnter(object sender, EventArgs e)
        {
            processLabelEvent((Label)sender, Color.MediumSeaGreen);
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            processLabelEvent((Label)sender, Color.SeaGreen);
        }

        private void pnlLow_Click(object sender, EventArgs e)
        {

            pnlLow.MouseLeave -= NavItem_MouseLeave;
            lblLow.MouseLeave -= lbl_MouseLeave;

            pnlLow.BackColor = Color.Orange;
            pnlStock.BackColor = Color.Wheat;

            pnlStock.MouseLeave += NavItem_MouseLeave;
            lblStock.MouseLeave += lbl_MouseLeave;



            List<InventoryItem> lowItems = all.FindAll(itm => itm.Quantity <= 3);
            items = lowItems;
            Debug.Print("Adding items " + items.Count.ToString());
            PopulateList();
            tabControl1.SelectedIndex = 0;
        }

        void PopulateList()
        {
            lstResults.Items.Clear();
            foreach (InventoryItem itm in items)
            {
                lstResults.Items.Add(itm.ListItem);
            }
        }

        void PopulateHistory()
        {
            lstHistory.Items.Clear();
            foreach (Order ord in orders)
            {
                lstHistory.Items.Add(ord.ListItem);
            }
        }

        void RemoveFromHistory(InventoryItem itm)
        {
            orders.RemoveAll(p => { return p.ProductID == itm.ProductID; });
            allOrders.RemoveAll(p => { return p.ProductID == itm.ProductID; });
        }

        private void pnlStock_Click(object sender, EventArgs e)
        {
            lstResults.Items.Clear();

            pnlStock.MouseLeave -= NavItem_MouseLeave;
            lblStock.MouseLeave -= lbl_MouseLeave;

            pnlStock.BackColor = Color.Orange;
            pnlLow.BackColor = Color.Wheat;

            pnlLow.MouseLeave += NavItem_MouseLeave;
            lblLow.MouseLeave += lbl_MouseLeave;

            items = all;
            Debug.Print("Adding items " + items.Count.ToString());
            foreach (InventoryItem itm in items)
            {
                lstResults.Items.Add(itm.ListItem);
            }

            tabControl1.SelectedIndex = 0;
        }

        private void cntxItems_Opening(object sender, CancelEventArgs e)
        {
            if (lstResults.SelectedItems.Count > 0)
            {
                cntxItems.Enabled = true;
            }
            else
            {
                cntxItems.Enabled = false;
            }
        }

        private void searchChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text.Length > 0)
            {
                items = FindItems(txtSearch.Text.ToLower());
            }
            else
            {
                items = all;
            }

            PopulateList();
        }

        List<InventoryItem> FindItems(string srch)
        {
            List<InventoryItem> srchItems = new List<InventoryItem>();

            for (int i = 0; i < items.Count; i++)
            {
                for (int j = 0; j < items[i].Values.Length; j++)
                {
                    if (items[i].Values[j].ToLower().Contains(srch))
                    {
                        srchItems.Add(items[i]);
                        Debug.Print("Adding Search Item with " + items[i].Values[j]);
                        break;
                    }
                }
            }

            return srchItems;

        }

        List<Order> FindOrders(string srch)
        {
            List<Order> srchItems = new List<Order>();

            for (int i = 0; i < orders.Count; i++)
            {
                for (int j = 0; j < orders[i].Values.Length; j++)
                {
                    if (orders[i].Values[j].ToLower().Contains(srch))
                    {
                        srchItems.Add(orders[i]);
                        Debug.Print("Adding Search Item with " + orders[i].Values[j]);
                        break;
                    }
                }
            }

            return srchItems;

        }

        private void searchHistoryChanged(object sender, EventArgs e)
        {
            if (txtHistorySearch.Text.Length > 0)
            {
                orders = FindOrders(txtHistorySearch.Text.ToLower());
            }
            else
            {
                orders = allOrders;
            }

            PopulateHistory();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentIndex = lstResults.SelectedIndices[0];
            frmRemove.ShowForm(items[currentIndex]);
            
            
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentIndex = lstResults.SelectedIndices[0];
            if (!frmAdd.IsDisposed)
            {
                frmAdd.Dispose();  
            }

            frmAdd = new AddForm();

            frmAdd.InfoUpdated += frmAdd_InfoUpdated;
            frmAdd.ShowEditForm(items[currentIndex]);
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string status = Export.ToFile("ExportData.csv",all.ToArray());
            if (status == null)
            {
                MessageBox.Show("Data exported to ExportData.csv");
            }
            else
            {
                MessageBox.Show(status);
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Export.ReadTest();
        }

        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (all.Count < 1)
            {
                exportToolStripMenuItem.Enabled = false;
            }
            else
            {
                exportToolStripMenuItem.Enabled = true;
            }

            
        }

        private void cyberVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Developed by CyberV\nvikrant.siwach@gmail.com\n9560879722\n\nHope you sincerely enjoy using the application. If not, drop me a message above and I'll get right to it. :)");
        }

        



    }
} 
