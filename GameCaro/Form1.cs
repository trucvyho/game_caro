using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCaro
{
    public partial class GameCaro : Form
    {
        private CaroChess CaroChess;
        private Graphics grs;
        public GameCaro()   
        {
            InitializeComponent();
            CaroChess = new CaroChess();
            grs = pnlBanCo.CreateGraphics();
            CaroChess.KhoiTaoMangOCo();
            btnPlayervsPlayer.Click +=  PvsP;
            btnUndo.Click += Undo_Click;
            btnRedo.Click += Redo_Click;
        }
        private void GameCaro_Load(object sender, EventArgs e)
        { 
            label1.Text = "\n\n\n Người chơi có đúng 5 quân \n trong cùng một hàng \n sẽ thắng!";
        
        }

        private void pnlBanCo_Paint(object sender, PaintEventArgs e)
        {
            CaroChess.VeBanCo(grs);
            CaroChess.VeLaiQuanCo(grs);
        }

        private void pnlBanCo_MouseClick(object sender, MouseEventArgs e)
        {
            if (!CaroChess.SanSang)
                return;
            CaroChess.DanhCo(e.X, e.Y, grs);
            if (CaroChess.KiemTraChienThang())
                CaroChess.KetThucTroChoi();
        }

        private void PvsP(object sender, EventArgs e)
        {
            grs.Clear(pnlBanCo.BackColor);
            CaroChess.StartPlayervsPlayer(grs);
        }

        private void Undo_Click(object sender, EventArgs e)
        {
            CaroChess.Undo(grs);
        }

        private void Redo_Click(object sender, EventArgs e)
        {
            CaroChess.Redo(grs);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }


    }
}
