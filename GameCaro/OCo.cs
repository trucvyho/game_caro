using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCaro
{
    class OCo
    {
        public const int _ChieuRong = 25;
        public const int _chieuCao = 25;
        private int _Dong;

        public int Dong
        {
            get
            {
                return _Dong;
            }

            set
            {
                _Dong = value;
            }
        }
        private int _Cot;
        public int Cot
        {
            get
            {
                return _Cot;
            }

            set
            {
                _Cot = value;
            }
        }

        
        private Point _ViTri;
        public Point ViTri
        {
            get
            {
                return _ViTri;
            }

            set
            {
                _ViTri = value;
            }
        }

        private int _SoHuu;
        public int SoHuu
        {
            get
            {
                return _SoHuu;
            }

            set
            {
                _SoHuu = value;
            }
        }
        public OCo()
        {}
        public OCo(int Dong,int Cot,Point Vitri,int SoHuu)
        {
            _Dong = Dong;
            _Cot = Cot;
            _ViTri = Vitri;
            _SoHuu = SoHuu;
        }
    } 
}
