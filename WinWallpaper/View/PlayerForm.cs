using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WinWallpaper.Utils;
namespace WinWallpaper.View
{
    public partial class PlayerForm : Form
    {

        public MCIPlayer p { get; set; }
        Rectangle rect;
        public PlayerForm(string path)
        {
            InitializeComponent();
            rect = new Rectangle(new Point(0, 0), Screen.PrimaryScreen.Bounds.Size);
            this.p = new MCIPlayer(path, "bg", this.Handle, rect);
            p = this.p;
        }

        private void PlayerForm_Load(object sender, EventArgs e)
        {
            this.Size = Screen.PrimaryScreen.Bounds.Size;
            this.Location = new Point(0, 0);
            this.BackColor = Color.White;
            
            p.Post(MCIPlayer.Cmd.play);
            p.Post(MCIPlayer.Cmd.loops);
        }

        
    }
}
