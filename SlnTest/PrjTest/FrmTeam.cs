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
    public partial class FrmTeam : Form
    {
        public FrmTeam()
        {
            InitializeComponent();
        }

        BasketBallEntities1 dbconect = new BasketBallEntities1();

        #region 查看
        private void button4_Click(object sender, EventArgs e)
        {
            //List<TeamInformation1> q = this.dbconect.TeamInformations.ToList();
            //var q = this.dbconect.TeamInformations.ToList();
            var q = from n in this.dbconect.TeamInformations
                    select new
                    {
                        n.TeamID,
                        n.TeamName,
                        n.City,
                        n.AreaID,
                        n.CreatYear,
                        n.Picture
                    };


            this.dataGridView1.DataSource = q.ToList();
        }
        #endregion

        #region 新增
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                this.pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                byte[] bytes = ms.GetBuffer();

                TeamInformation team = new TeamInformation()
                {
                TeamName = this.textBox1.Text,
                City = this.textBox2.Text,
                AreaID = int.Parse(this.textBox3.Text),
                CreatYear = this.textBox4.Text,
                Picture = bytes
                };
                this.dbconect.TeamInformations.Add(team);

                this.dbconect.SaveChanges();

                this.dataGridView1.DataSource = this.dbconect.TeamInformations.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("請輸入完整資料");
            }
            
        }
        #endregion

        #region Browse
        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            if(fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.pictureBox1.Image = Image.FromFile(fileDialog.FileName);
            }
        }
        #endregion
        
        #region 修改
        private void button2_Click(object sender, EventArgs e)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            this.pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            byte[] bytes = ms.GetBuffer();
            var q = (from n in this.dbconect.TeamInformations
                    where n.TeamName.Contains(this.textBox1.Text)
                    select n).FirstOrDefault();
                    

            if (q == null)
                return;
            else q.Picture = bytes;

            this.dbconect.SaveChanges();
           
        }
        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            var q = (from n in this.dbconect.TeamInformations
                    where n.TeamName.Contains(this.textBox1.Text)
                    select n).FirstOrDefault();
            
            this.dbconect.TeamInformations.Remove(q);

            this.dbconect.SaveChanges();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FrmPlayerIn f = new FrmPlayerIn();
            f.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FrmPlayer f = new FrmPlayer();
            f.Show();
        }
    }
}
