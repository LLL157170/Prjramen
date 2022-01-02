using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrjTest
{
    public partial class FrmPlayer : Form
    {
        public FrmPlayer()
        {
            InitializeComponent();
        }
        BasketBallEntities1 dbconect = new BasketBallEntities1();
        

        #region 新增要打完整資料
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                this.pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                byte[] bytes = ms.GetBuffer();

                var q = from n in this.dbconect.PlayerInformations
                        select new {n.Name };

                PlayerInformation player = new PlayerInformation
                {
                Name = this.textBox1.Text,
                Position = this.textBox2.Text,
                Height = int.Parse(this.textBox3.Text),
                Weight = int.Parse(this.textBox4.Text),
                Country = this.textBox5.Text,
                TeamID =int.Parse(this.comboBox1.Text),
                Picture = bytes 
                };
                //foreach(var n in q)
                //{
                //    if(n.Name == player.Name)
                //    {
                //        break;
                //    }
                //    else
                //    {
                        this.dbconect.PlayerInformations.Add(player);
                    //}
                //}
            
                

                this.dbconect.SaveChanges();

                this.dataGridView1.DataSource = this.dbconect.PlayerInformations.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("請輸入完整資料");
            }



        }
        #endregion

        #region Browse
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.pictureBox1.Image = Image.FromFile(fileDialog.FileName);
            }
        }
        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            var q = from n in this.dbconect.PlayerInformations
                    select new 
                    {
                        n.PlayerID,
                        n.Name,
                        n.Position,
                        n.Height,
                        n.Weight,
                        n.TeamID,
                        n.Country,
                        n.Picture
                    };
            this.dataGridView1.DataSource = q.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            if(this.pictureBox1.Image != null)
            {
                this.pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                byte[] bytes = ms.GetBuffer();
                var q = from n in this.dbconect.PlayerInformations
                    where n.Name.Contains(this.textBox1.Text)
                    select n;

                foreach(var n in q)
                {
                if (n.Picture == null) return;
                n.Picture = bytes ;
                }
            }
            
            

            this.dbconect.SaveChanges();
        }

        #region 輸入姓名就刪除
        private void button5_Click(object sender, EventArgs e)
        {
            var q = (from n in this.dbconect.PlayerInformations
                     where n.Name.Contains(this.textBox1.Text)
                     select n).FirstOrDefault();

            this.dbconect.PlayerInformations.Remove(q);
            this.dbconect.SaveChanges();
        }

        #endregion

        #region combox出現編號
        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            var q = from n in this.dbconect.TeamInformations
                    select new {n.TeamID};

            this.comboBox1.Items.Clear();
            foreach (var teamname in q)
            {
                string s = teamname.TeamID.ToString();
                this.comboBox1.Items.Add(s);
            
            }

        }
        #endregion
        
        #region COMBOX選取
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var q = from n in this.dbconect.TeamInformations
                    select new { n.TeamName,n.TeamID };
            foreach(var label in q)
            {
                if(this.comboBox1.Text == label.TeamID.ToString())
                {
                    this.label6.Text = label.TeamName;
                }
            }
        }
        #endregion
    }

}
