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
    public partial class FrmPlayerIn : Form
    {
        public FrmPlayerIn()
        {
            InitializeComponent();
            
        }
        BasketBallEntities1 dbconect = new BasketBallEntities1();

        #region 所有
        private void button1_Click(object sender, EventArgs e)
        {
            var q = from p in this.dbconect.PlayerInformations
                    from t in this.dbconect.TeamInformations
                    where p.TeamID == t.TeamID
                    orderby p.Height descending
                    select new
                    {
                        p.PlayerID,
                        p.Name,
                        p.Position,
                        p.Height,
                        p.Weight,
                        t.TeamName,
                        p.Country,
                        p.Picture
                    };
            
            this.dataGridView1.DataSource = q.ToList();
            this.chart1.DataSource = q.ToList();
            this.chart1.Series[0].XValueMember = "Height";
            this.chart1.Series[0].YValueMembers = "Weight";
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
        }
        #endregion

        #region combox1(name)
        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            var q = from n in this.dbconect.PlayerInformations
                    select new { n.Name };
            this.comboBox1.Items.Clear();
            foreach (var n in q)
            {
                string s = n.Name;
                this.comboBox1.Items.Add(s);
            }
        }
        #endregion

        #region combox2(team)
        private void comboBox2_MouseClick(object sender, MouseEventArgs e)
        {
            var q = from n in this.dbconect.TeamInformations
                    select new { n.TeamName };
            this.comboBox2.Items.Clear();
            foreach (var team in q)
            {
                string s = team.TeamName;
                this.comboBox2.Items.Add(s);
            }

        }
        #endregion
        
        #region 單一球員查詢
        private void button2_Click(object sender, EventArgs e)
        {
            var q = from p in this.dbconect.PlayerInformations
                    from t in this.dbconect.TeamInformations
                    where p.Name == this.comboBox1.Text && p.TeamID == t.TeamID
                    select new
                    {
                        p.PlayerID,
                        p.Name,
                        p.Position,
                        p.Height,
                        p.Weight,
                        t.TeamName,
                        p.Country,
                        p.Picture
                    };
            this.dataGridView1.DataSource = q.ToList();
            this.comboBox1.Text = "請選擇";
            
        }

        #endregion

        #region 球隊成員查詢
        private void button3_Click(object sender, EventArgs e)
        {
            var q = from p in this.dbconect.PlayerInformations
                    from t in this.dbconect.TeamInformations
                    where p.TeamID == t.TeamID
                    select new
                    {
                        p.PlayerID,
                        p.Name,
                        p.Position,
                        p.Height,
                        p.Weight,
                        t.TeamName,
                        p.Country,
                        p.Picture
                    };
           
            var a = from n in q
                    where n.TeamName == this.comboBox2.Text
                    select n;
            this.dataGridView1.DataSource = a.ToList();

            this.chart1.DataSource = a.ToList();
            this.chart1.Series[0].XValueMember = "Height";
            this.chart1.Series[0].YValueMembers = "Weight";
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
        }

        #endregion

        #region country 查詢
        private void button4_Click(object sender, EventArgs e)
        {
            var q = from p in this.dbconect.PlayerInformations
                    from t in this.dbconect.TeamInformations
                    where p.TeamID == t.TeamID
                    select new
                    {
                        p.PlayerID,
                        p.Name,
                        p.Position,
                        p.Height,
                        p.Weight,
                        t.TeamName,
                        p.Country,
                        p.Picture
                    };
            var a = from n in q
                    where n.Country == this.comboBox3.Text
                    select n;
            this.dataGridView1.DataSource = a.ToList();
            
             
            this.chart1.DataSource = a.ToList();
            this.chart1.Series[0].XValueMember = "Height";
            this.chart1.Series[0].YValueMembers = "Weight";
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

        }


        #endregion

        #region COMBOX3(country)
        private void comboBox3_MouseClick(object sender, MouseEventArgs e)
        {
            var q = (from b in this.dbconect.PlayerInformations
                    select new {b.Country }).Distinct() ;
            this.comboBox3.Items.Clear();
            foreach(var n in q)
            {
                string s = n.Country;
                this.comboBox3.Items.Add(s);
            }
        }
        #endregion

        #region (group by country)
        private void button5_Click(object sender, EventArgs e)
        {
            var q = from n in this.dbconect.PlayerInformations
                    group n by n.Country into g
                    orderby g.Count() descending
                    select new { country = g.Key, Playercount = g.Count() };
            this.dataGridView1.DataSource = q.ToList();

            this.chart1.DataSource = q.ToList();
            this.chart1.Series[0].XValueMember = "country";
            this.chart1.Series[0].YValueMembers = "Playercount";
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

        }
        #endregion
    }
}
