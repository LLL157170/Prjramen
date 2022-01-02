using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrjTest 
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FrmPlayer());
            //Application.Run(new FrmTeam());
            Application.Run(new FrmMain());
            //Application.Run(new FrmPlayerIn());
            //Application.Run(new FrmTreeview());
            //Application.Run(new FrmItem());
            //Application.Run(new FrmItemSerch());
        }
    }
}
