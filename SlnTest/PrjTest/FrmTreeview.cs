using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrjTest
{
    public partial class FrmTreeview : Form
    {
        public FrmTreeview()
        {
            InitializeComponent();
        }
        BasketBallEntities1 dbconect = new BasketBallEntities1();

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            var q = from p in this.dbconect.PlayerInformations
                    from t in this.dbconect.TeamInformations
                    where p.TeamID== t.TeamID
                    select new { p.Name,t.TeamName,p.Country };//t.TeamName ;
            this.treeView1.Nodes.Clear();

            foreach (var n in q)
            {
                int i = 0;
                int s = 0;
                string teamname = n.TeamName;
                string name = n.Name;
                TreeNode team = new TreeNode(teamname);
                TreeNode player = new TreeNode(name);
                if (treeView1.Nodes.Count == 0)
                {
                    treeView1.Nodes.Add(team);
                    team.Nodes.Add(player);
                }
                else
                {
                    while (treeView1.Nodes.Count > i)
                    {
                        string a = treeView1.Nodes[i].Text;
                        if (a == teamname)
                        {
                            treeView1.Nodes[i].Nodes.Add(player);
                            break;
                        }
                        i++;
                    }
                }
                if (i == treeView1.Nodes.Count)
                {
                    treeView1.Nodes.Add(team);
                    team.Nodes.Add(player);
                }
            }

        }
        #region 選取treeview
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //var player = from h in this.dbconect.PlayerInformations
            //             from t in this.dbconect.TeamInformations
            //             where h.Name == e.Node.Text && h.TeamID == t.TeamID
            //             select new
            //             {
            //                 h.Name,
            //                 h.Position,
            //                 h.Height,
            //                 h.Weight,
            //                 t.TeamName,
            //                 h.Country,
            //                 h.Picture
            //            }; 

            //this.dataGridView1.DataSource = player.ToList();

            var q = from p in this.dbconect.PlayerInformations
                    from t in this.dbconect.TeamInformations
                    where p.TeamID == t.TeamID
                    select new 
                    {
                        p.Name,
                        p.Position,
                        p.Height,
                        p.Weight,
                        t.TeamName,
                        p.Country,
                        p.Picture
                    };

            this.pictureBox1.Image = null;
            foreach (var n in q)
            {
                if(n.Name == e.Node.Text)
                {
                    var player = from h in q
                    where h.Name == e.Node.Text
                    select h;
                    
                    var p = from h in q
                            where h.Name == e.Node.Text
                            select h.Picture;

                    byte[] bytes = p.First();
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                    this.pictureBox1.Image = Image.FromStream(ms);

                    this.dataGridView1.DataSource = player.ToList();
                //this.dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells); //每一欄(行) (一)的寬
                this.dataGridView1.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders); //每一列(|) 的寬

                }
                if(n.TeamName == e.Node.Text)
                {
                    var player = from h in q
                                 where h.TeamName == e.Node.Text
                                 select h;
                    var p = from h in this.dbconect.TeamInformations
                            where h.TeamName == e.Node.Text
                            select h.Picture;

                    byte[] bytes = p.First();
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                    this.pictureBox1.Image = Image.FromStream(ms);

                    this.dataGridView1.DataSource = player.ToList();
                    //this.dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    this.dataGridView1.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);
                    this.chart1.DataSource = player.ToList();
                    this.chart1.Series[0].Name = "身高體重";
                    this.chart1.Series[0].XValueMember = "Height";
                    this.chart1.Series[0].YValueMembers = "Weight";
                    this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                }
                
            }

        }
        #endregion
        public static Image BytesToImage(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            Image image = System.Drawing.Image.FromStream(ms);
            return image;
        }




            //private void button1_Click(object sender, EventArgs e)
            //{
            //    var q = from p in this.dbconect.PlayerInformations
            //            from t in this.dbconect.TeamInformations
            //            where p.TeamID == t.TeamID
            //            orderby p.Height descending
            //            select new
            //            {
            //                p.PlayerID,
            //                p.Name,
            //                p.Position,
            //                p.Height,
            //                p.Weight,
            //                t.TeamName,
            //                p.Country,
            //                p.Picture
            //            };
            //    foreach (var team in q)
            //    {
            //        string s = $"{team.TeamName}";
            //        TreeNode node = this.treeView1.Nodes.Add(team.TeamName.ToString());
            //        foreach (var positon in team.TeamName)
            //        {
            //            node.Nodes.Add(positon.ToString());
            //        }
            //    }
            //    this.dataGridView1.DataSource = q.ToList();

            //    this.chart1.DataSource = q.ToList();
            //    this.chart1.Series[0].XValueMember = "Height";
            //    this.chart1.Series[0].YValueMembers = "Weight";
            //    this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            //}
        }
}
