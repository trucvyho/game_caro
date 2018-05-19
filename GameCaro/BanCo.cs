using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCaro
{
    class BanCo
    {
        private int _SoDong;
        private int _SoCot;
        public int SoDong
        {
            get { return _SoDong; }
        }
        public int SoCot
        {
            get { return _SoCot; }
        }
        public BanCo()
        {
            _SoDong = 0;
            _SoCot = 0;
        }
        public BanCo(int SoDong,int SoCot)
        {
            _SoDong = SoDong;
            _SoCot = SoCot;
        }
        public void VeBanCo(Graphics g)
        {
            for(int i = 0;i<=_SoCot; i++)
            {
                g.DrawLine(CaroChess.pen, i * OCo._ChieuRong, 0, i * OCo._ChieuRong, _SoDong * OCo._chieuCao);
            }
            for (int j = 0; j <= _SoDong; j++)
            {
                g.DrawLine(CaroChess.pen, 0, j * OCo._chieuCao, _SoCot * OCo._ChieuRong, j * OCo._chieuCao);
            }
        }
        public void VeQuanCo(Graphics g,Point point, Pen pen)
        {
            g.DrawEllipse(pen, point.X+4, point.Y+4 , OCo._ChieuRong-8, OCo._chieuCao-8);
            //g.DrawEllipse(pen, point.X + 2, point.Y + 2, OCo._ChieuRong - 4, OCo._chieuCao - 4);
            // comment
        }
        public void XoaQuanCo(Graphics g, Point point,SolidBrush sb)
        {
            g.FillRectangle(sb, point.X + 1 , point.Y +1, OCo._ChieuRong - 2, OCo._chieuCao - 2);
        }
    }
}
