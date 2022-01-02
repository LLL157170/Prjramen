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
    public partial class FrmItem : Form
    {
        public FrmItem()
        {
            InitializeComponent();
        }
        BasketBallEntities1 dbconect = new BasketBallEntities1();
        //新增
        private void button1_Click(object sender, EventArgs e)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            this.pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            byte[] bytes = ms.GetBuffer();

            Iteminformation item = new Iteminformation
            {
                ItemName = this.textBox1.Text,
                ProductId = int.Parse(this.comboBox1.Text),
                TeamID = int.Parse(this.comboBox2.Text),
                price = int.Parse(this.textBox2.Text),
                picture = bytes

            };
            this.dbconect.Iteminformations.Add(item);

            this.dbconect.SaveChanges();
           

            this.dataGridView1.DataSource = this.dbconect.Iteminformations.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            if(openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.pictureBox1.Image = Image.FromFile(openFile.FileName);
            }
        }

        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            var q = from t in this.dbconect.Products
                    select new {t.ProductId };

            this.comboBox1.Items.Clear();
            foreach(var n in q)
            {
                string s = n.ProductId.ToString();
                this.comboBox1.Items.Add(s);
            }
        }

        private void comboBox2_MouseClick(object sender, MouseEventArgs e)
        {
            var q = from n in this.dbconect.TeamInformations
                    select new { n.TeamID };
            this.comboBox2.Items.Clear();
            foreach(var n in q)
            {
                string s = n.TeamID.ToString();
                this.comboBox2.Items.Add(s);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var q = from n in this.dbconect.Products
                    select n;
            foreach(var n in q)
            {
                if (this.comboBox1.Text == n.ProductId.ToString())
                    this.label1.Text = n.ProductName;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var q = from n in this.dbconect.TeamInformations
                    select n;

            foreach(var n in q)
            {
                if (this.comboBox2.Text == n.TeamID.ToString())
                    this.label2.Text = n.TeamName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var q = from n in this.dbconect.Iteminformations
                    select n;
            
            this.dataGridView1.DataSource = q.ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            
            
                this.pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                byte[] bytes = ms.GetBuffer();
                var q = from n in this.dbconect.Iteminformations
                        where n.ItemName.Contains(this.textBox1.Text)
                        select n;

                foreach (var n in q)
                {
                   
                    n.picture = bytes;
                }
            

            this.dbconect.SaveChanges();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //System.IO.MemoryStream ms = new System.IO.MemoryStream();


            //this.pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            //byte[] bytes = ms.GetBuffer();

            //var q = from n in this.dbconect.Iteminformations
            //        where 
            //        select n;
            //foreach(var n in q)
            //{
            //    n.ItemName = this.textBox1.Text;
            //}
        }
    }
}
