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
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            LoadArea();
            LoadProduct();
        }

        

        BasketBallEntities1 dbconect = new BasketBallEntities1();
        #region tab(球隊)
        #region 顯示地區(方法)
        private void LoadArea()
        {
            var q = from n in this.dbconect.Areas
                    select new { n.Area1 };

            List<string> b = new List<string>();
            foreach (var a in q)
            {
                b.Add(a.Area1);
            }
            this.linkLabel1.Text = b[0].ToString();
            this.linkLabel2.Text = b[1].ToString();
        }
        #endregion

        #region 顯示照片AREA
        private void LoadPhotoByArea (string Area)
        {
            var q = from n in this.dbconect.Areas
                    from c in n.TeamInformations
                    select new {n.Area1,c.Picture};
            //List<string> ID = new List<string>();

            this.flowLayoutPanel1.Controls.Clear();
            foreach(var p in q)
            {
                if (Area == p.Area1)
                {
                    byte[] bytes = p.Picture;
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                    PictureBox pictureBox = new PictureBox();
                    pictureBox.Image = Image.FromStream(ms);
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox.Size = new Size(200, 200);
                    this.flowLayoutPanel1.Controls.Add(pictureBox);
                }
            }
        }
        #endregion
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoadPhotoByArea(this.linkLabel1.Text);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoadPhotoByArea(this.linkLabel2.Text);
        }

        private void flowLayoutPanel1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmTeam team = new FrmTeam();
            team.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmTreeview treeview = new FrmTreeview();
            treeview.Show();
        }
        #endregion

        private void LoadProduct()
        {
            var q = from n in this.dbconect.Products
                    select n.ProductName;

            List<string> s = new List<string>();
            foreach(var n in q)
            {
                s.Add(n);
            }
            this.linkLabel3.Text = s[0];
            this.linkLabel4.Text = s[1];
            this.linkLabel5.Text = s[2];
            this.linkLabel6.Text = s[3];
            this.linkLabel7.Text = s[4];
            this.linkLabel8.Text = s[5];
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmItemSerch f = new FrmItemSerch();
            f.Show();
        }
        private void LoadPhotoByProduct(string Product)
        {
            var q = from item in this.dbconect.Iteminformations
                    from p in this.dbconect.Products
                    where item.ProductId == p.ProductId
                    select new { p.ProductName, item.picture };

            this.flowLayoutPanel2.Controls.Clear();
            foreach(var n in q)
            {
                if(Product == n.ProductName)
                {
                    byte[] bytes = n.picture;
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                    PictureBox pictureBox = new PictureBox();
                    pictureBox.Image = Image.FromStream(ms);
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox.Size = new Size(100, 100);
                    this.flowLayoutPanel2.Controls.Add(pictureBox);

                }
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoadPhotoByProduct(linkLabel3.Text);
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoadPhotoByProduct(linkLabel4.Text);
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoadPhotoByProduct(linkLabel5.Text);
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoadPhotoByProduct(linkLabel6.Text);
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoadPhotoByProduct(linkLabel7.Text);
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoadPhotoByProduct(linkLabel8.Text);
        }
    }
}
