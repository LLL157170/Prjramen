using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrjTest
{
    public partial class FrmItemSerch : Form
    {
        public FrmItemSerch()
        {
            InitializeComponent();
            LoadPictureBig();

        }

        public void LoadPictureBig()
        {
            this.dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            this.dataGridView1.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);
        }

        BasketBallEntities1 dbconect = new BasketBallEntities1();

        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            var q = from n in this.dbconect.TeamInformations
                    select new { n.TeamName };
            this.comboBox1.Items.Clear();
            //this.comboBox1.Text = "請選擇";
            foreach (var n in q)
            {
                string s = n.TeamName;
                this.comboBox1.Items.Add(s);
            }
        }

        private void comboBox2_MouseClick(object sender, MouseEventArgs e)
        {
            var q = from n in this.dbconect.Products
                    select new { n.ProductName };
            this.comboBox2.Items.Clear();
            foreach (var n in q)
            {
                string s = n.ProductName;
                this.comboBox2.Items.Add(s);
            }
        }
        #region 查詢
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                var q = from Item in this.dbconect.Iteminformations
                        from p in this.dbconect.Products
                        from t in this.dbconect.TeamInformations
                        where Item.TeamID == t.TeamID && Item.ProductId == p.ProductId
                        select new
                        {
                            Item.ItemID,
                            Item.ItemName,
                            p.ProductName,
                            t.TeamName,
                            Item.price,
                            Item.picture
                        };

                foreach (var n in q)
                {
                    if (n.ProductName == this.comboBox2.Text)
                    {
                        var product = from a in q
                                      where a.ProductName == this.comboBox2.Text
                                      select a;

                        this.bindingSource1.DataSource = product.ToList();
                        this.bindingNavigator1.BindingSource = this.bindingSource1;
                        this.dataGridView1.DataSource = this.bindingSource1;
                        LoadPictureBig();

                    }
                    if (n.TeamName == this.comboBox1.Text)
                    {
                        var Team = from a in q
                                   where a.TeamName == this.comboBox1.Text
                                   select a;
                        
                        this.bindingSource1.DataSource = Team.ToList();
                        this.bindingNavigator1.BindingSource = this.bindingSource1;
                        this.dataGridView1.DataSource = this.bindingSource1;
                        LoadPictureBig();
                    }
                    if (n.ProductName == this.comboBox2.Text && n.TeamName == this.comboBox1.Text)
                    {
                        var TeamProduct = from a in q
                                          where n.ProductName == this.comboBox2.Text && n.TeamName == this.comboBox1.Text
                                          select a;
                        
                        this.bindingSource1.DataSource = TeamProduct.ToList();
                        this.bindingNavigator1.BindingSource = this.bindingSource1;
                        this.dataGridView1.DataSource = this.bindingSource1;
                    }
                    if (this.textBox2.Text != "" && this.textBox3.Text != "")
                    {
                        int b = int.Parse(this.textBox2.Text);
                        int s = int.Parse(this.textBox3.Text);
                        var price = from a in q
                                    where a.price > b && a.price < s
                                    select a;
                        this.bindingSource1.DataSource = price.ToList();
                        this.bindingNavigator1.BindingSource = this.bindingSource1;
                        this.dataGridView1.DataSource = this.bindingSource1;
                        LoadPictureBig();
                    }
                    if (this.textBox1.Text != "")
                    {
                        var name = from a in q
                                   where a.ItemName.Contains(this.textBox1.Text)
                                   select a;
                        this.bindingSource1.DataSource = name.ToList();
                        this.bindingNavigator1.BindingSource = this.bindingSource1;
                        this.dataGridView1.DataSource = this.bindingSource1;
                        LoadPictureBig();
                    }



                }
                this.comboBox1.Text = "請選擇";
                this.comboBox2.Text = "請選擇";
            }
            catch(Exception ex)
            {
                MessageBox.Show("請輸入正確Price");
            }
        }

        #endregion

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            this.bindingSource1.Position = 0;
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            this.bindingSource1.Position -= 1;
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            this.bindingSource1.Position += 1;
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            this.bindingSource1.Position = this.bindingSource1.Count - 1;
        }

        private void FrmItemSerch_Load_1(object sender, EventArgs e)
        {
            var q = from Item in this.dbconect.Iteminformations
                    from p in this.dbconect.Products
                    from t in this.dbconect.TeamInformations
                    where Item.TeamID == t.TeamID && Item.ProductId == p.ProductId
                    select new
                    {
                        Item.ItemID,
                        Item.ItemName,
                        p.ProductName,
                        t.TeamName,
                        Item.price,
                        Item.picture
                    };
            this.bindingSource1.DataSource = q.ToList();
            this.bindingNavigator1.BindingSource = this.bindingSource1;
            this.dataGridView1.DataSource = q.ToList();
        }
    }
}
