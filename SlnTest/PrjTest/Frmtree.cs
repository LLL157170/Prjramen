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
    public partial class Frmtree : Form
    {
        public Frmtree()
        {
            InitializeComponent();
        }
        BasketBallEntities1 dbconect = new BasketBallEntities1();
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var q = from p in this.dbconect.PlayerInformations
                    from t in this.dbconect.TeamInformations
                    select new {p.Name,t.TeamName };
            this.treeView1.Nodes.Clear();
            //List<string> team = new List<string>();
            //List<string> player = new List<string>();
            foreach (var n in q)
            {
                int i = 0;
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
    }
}
