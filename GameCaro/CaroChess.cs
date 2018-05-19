using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCaro
{
    public enum KetThuc
    {
        HoaCo,
        Player1,
        Player2,
        COM
    }

    class CaroChess
    {
        public static Pen pen;
        public static Pen pBlue;
        public static Pen pRed;
        public static SolidBrush sbWhite;
        public static SolidBrush sbBlack;
        public static SolidBrush sbSnow;
        private int _CheDoChoi;
        private OCo[,] _MangOCo;
        private BanCo _BanCo;
        private int _LuotDi;
        private bool _SanSang;
        private int _Undo = 0;
        private Stack<OCo> stk_CacNuocDaDi;
        private Stack<OCo> stk_Redo;
        private KetThuc _ketThuc;
        public bool SanSang
        {
            get
            {
                return _SanSang;
            }

            set
            {
                _SanSang = value;
            }
        }

        public CaroChess()
        {
            pen = new Pen(Color.Black);
            pBlue = new Pen(Color.Blue,3);
            pRed = new Pen(Color.Red,3);
            sbBlack = new SolidBrush(Color.Red);
            sbWhite = new SolidBrush(Color.Blue);
            sbSnow = new SolidBrush(Color.Snow);
            _BanCo = new BanCo(20, 20);
            _MangOCo = new OCo[_BanCo.SoDong, _BanCo.SoCot];
            stk_CacNuocDaDi = new Stack<OCo>();
            stk_Redo = new Stack<OCo>();
            _LuotDi = 1;
        }
        public void VeBanCo(Graphics g)
        {
            _BanCo.VeBanCo(g);
        }
        public void KhoiTaoMangOCo()
        {
            for(int i = 0; i<_BanCo.SoDong;i++)
            {
                for(int j =0; j<_BanCo.SoCot;j++)
                {
                    _MangOCo[i, j] = new OCo(i,j,new Point(j*OCo._ChieuRong,i*OCo._chieuCao),0);
                }
            }
        }
        public bool DanhCo(int MouseX, int MouseY,Graphics g)
        {
            if (MouseX % OCo._ChieuRong == 0||MouseY%OCo._chieuCao==0)
                return false;
            int Cot = MouseX / OCo._ChieuRong;
            int Dong = MouseY / OCo._chieuCao;
            if (_MangOCo[Dong, Cot].SoHuu != 0)
                return false;
            switch (_LuotDi)
            {
                case 1:
                    _MangOCo[Dong, Cot].SoHuu = 1;
                    _BanCo.VeQuanCo(g, _MangOCo[Dong, Cot].ViTri, pBlue);
                    _LuotDi = 2;
                    break;
                case 2:
                    _MangOCo[Dong, Cot].SoHuu = 2;
                    _BanCo.VeQuanCo(g, _MangOCo[Dong, Cot].ViTri, pRed);
                    _LuotDi = 1;
                    break;
                default:
                    MessageBox.Show("false");
                    break;
            }
            stk_Redo = new Stack<OCo>();
            OCo oco = new OCo(_MangOCo[Dong, Cot].Dong, _MangOCo[Dong, Cot].Cot, _MangOCo[Dong, Cot].ViTri, _MangOCo[Dong, Cot].SoHuu);
            stk_CacNuocDaDi.Push(oco);
            return true;
        }
        public void VeLaiQuanCo(Graphics g)
        {
            foreach(OCo oco in stk_CacNuocDaDi)
            {
                if (oco.SoHuu == 1)
                    _BanCo.VeQuanCo(g, oco.ViTri, pBlue);
                else 
                    if(oco.SoHuu==2)
                    _BanCo.VeQuanCo(g, oco.ViTri, pRed);

            }
        }
        public void StartPlayervsPlayer(Graphics g)
        {
            _SanSang = true;
            stk_CacNuocDaDi = new Stack<OCo>();
            stk_Redo = new Stack<OCo>();
            KhoiTaoMangOCo();
            VeBanCo(g);
            _CheDoChoi = 1;
            _LuotDi = 1;
        }
        public void StartPlayervsCOM(Graphics g)
        {
            _SanSang = true;
            stk_CacNuocDaDi = new Stack<OCo>();
            stk_Redo = new Stack<OCo>();
            KhoiTaoMangOCo();
            VeBanCo(g);
            _CheDoChoi = 2;
            _LuotDi = 1;
        }
        #region undo,redo
        public void Undo(Graphics g)
        {
            if(stk_CacNuocDaDi.Count != 0)
            {
                OCo oco = stk_CacNuocDaDi.Pop();
                stk_Redo.Push(new OCo(oco.Dong,oco.Cot,oco.ViTri,oco.SoHuu));
                _MangOCo[oco.Dong, oco.Cot].SoHuu = 0;
                _BanCo.XoaQuanCo(g, oco.ViTri, sbSnow);
                if (_LuotDi == 1)
                    _LuotDi = 2;
                else
                    _LuotDi = 1;
                _Undo++;
            }
            if(_Undo == 3)
            {
                stk_CacNuocDaDi = new Stack<OCo>();
                _Undo = 0;
            }
         
        }
        public void Redo(Graphics g)
        {
            if(stk_Redo.Count !=0)
            {
                OCo oco = stk_Redo.Pop();
                stk_CacNuocDaDi.Push(new OCo(oco.Dong, oco.Cot, oco.ViTri, oco.SoHuu));
                _MangOCo[oco.Dong, oco.Cot].SoHuu = oco.SoHuu;
                _BanCo.VeQuanCo(g, oco.ViTri, oco.SoHuu == 1 ? pBlue : pRed);
                if (_LuotDi == 1)
                    _LuotDi = 2;
                else
                    _LuotDi = 1;
                _Undo = 0;
            }
        }
        #endregion

        #region xử lý thắng
        public void KetThucTroChoi()
        {
            switch (_ketThuc)
            {
                case KetThuc.HoaCo:
                    MessageBox.Show("Hòa Cờ!");
                    break;
                case KetThuc.Player1:
                    MessageBox.Show("Người Chơi 1 thắng!");
                    break;
                case KetThuc.Player2:
                    MessageBox.Show("Người Chơi 2 thắng!");
                    break;
                case KetThuc.COM:
                    MessageBox.Show("Computer thắng!");
                    break;
            }
            _SanSang = false;
        }
        public bool KiemTraChienThang()
        {
            if (stk_CacNuocDaDi.Count == _BanCo.SoDong * _BanCo.SoCot)
            {
                _ketThuc = KetThuc.HoaCo;
                return true;
            }
            foreach (OCo oco in stk_CacNuocDaDi)
            {
                if (DuyetDoc(oco.Dong,oco.Cot,oco.SoHuu)|| DuyetNgang(oco.Dong,oco.Cot,oco.SoHuu)|| DuyetCheoXuoi(oco.Dong,oco.Cot,oco.SoHuu)||DuyetCheoNguoc(oco.Dong,oco.Cot,oco.SoHuu))
                {
                    _ketThuc = oco.SoHuu == 1 ? KetThuc.Player1 : KetThuc.Player2;
                    return true;
                }
            }
            return false;
        }
        private bool DuyetDoc(int currentDong, int currentCot, int currSohuu)
        {
            if(currentDong > _BanCo.SoDong - 5)
              return false;
            int Dem;
            for (Dem = 1; Dem < 5; Dem++)
            {
                if (_MangOCo[currentDong + Dem, currentCot].SoHuu != currSohuu)
                    return false;
            }
            if (currentDong == 0 || currentDong + Dem == _BanCo.SoDong)
                return true;
            if (_MangOCo[currentDong - 1, currentCot].SoHuu == 0 || _MangOCo[currentDong + Dem, currentCot].SoHuu == 0)
                return true;
            return false;
        }
        private bool DuyetNgang(int currentDong, int currentCot, int currSohuu)
        {
            if (currentDong > _BanCo.SoCot - 5)
                return false;
            int Dem;
            for (Dem = 1; Dem < 5; Dem++)
            {
                if (_MangOCo[currentDong, currentCot + Dem].SoHuu != currSohuu)
                    return false;
            }
            if (currentCot == 0 || currentCot + Dem == _BanCo.SoCot)
                return true;
            if (_MangOCo[currentDong , currentCot - 1].SoHuu == 0 || _MangOCo[currentDong, currentCot + Dem].SoHuu == 0)
                return true;
            return false;
        }
        private bool DuyetCheoXuoi(int currentDong, int currentCot, int currSohuu)
        {
            if (currentDong > _BanCo.SoDong - 5||currentCot>_BanCo.SoCot-5)
                
                return false;
            int Dem;
            for (Dem = 1; Dem < 5; Dem++)
            {
                if (_MangOCo[currentDong+Dem, currentCot + Dem].SoHuu != currSohuu)
                    return false;
            }
            if (currentDong == 0 || currentDong + Dem == _BanCo.SoDong|| currentCot ==0 || currentCot + Dem == _BanCo.SoCot)
                return true;
            if (_MangOCo[currentDong - 1, currentCot - 1].SoHuu == 0 || _MangOCo[currentDong+ Dem, currentCot + Dem].SoHuu == 0)
                return true;
            return false;
        }
        private bool DuyetCheoNguoc(int currentDong, int currentCot, int currSohuu)
        {
            if (currentDong <4||currentCot > _BanCo.SoCot -5)
                return false;
            int Dem;
            for (Dem = 1; Dem < 5; Dem++)
            {
                if (_MangOCo[currentDong-Dem, currentCot + Dem].SoHuu != currSohuu)
                    return false;
            }
            if (currentDong == 4||currentDong == _BanCo.SoDong - 1||currentCot == 0|| currentCot + Dem == _BanCo.SoCot)
                return true;
            if (_MangOCo[currentDong +1, currentCot - 1].SoHuu == 0 || _MangOCo[currentDong-Dem, currentCot + Dem].SoHuu == 0)
                return true;
            return false;
        }
        #endregion

        #region AI
        //KHai bao mang tren c# 
        private long[] MangDiemTanCong = new long[7] { 0, 3, 24, 192, 1536, 12288, 98304 };

        private long[] MangDienPhongNgu = new long[7] { 0, 1, 9, 81, 729, 6561, 59049 };
        public void KhoiDongComputer(Graphics g)
        {
            //quy dinh danh giua ban co
            if(stk_CacNuocDaDi.Count==0)
            {
                DanhCo(_BanCo.SoDong / 2 * OCo._chieuCao + 1, _BanCo.SoCot / 2 * OCo._ChieuRong + 1, g);

            }
            else
            {
                OCo oco = TimKiemNuocDi();
                DanhCo(oco.ViTri.X + 1, oco.ViTri.Y, g);
            }
        }

        private OCo TimKiemNuocDi()
        {
            OCo oCoResult = new OCo();
            //Bat dau 
            long DiemTong = 0;
            //vet can 
            for (int i = 0; i < _BanCo.SoDong; i++)
                for (int j = 0; j < _BanCo.SoCot;j++ )
                {
                    if (_MangOCo[i,j].SoHuu==0)
                    {
                        long DiemTanCong = DiemTC_DuyetDoc(i, j) + DiemTC_DuyetNgang(i, j) + DiemTC_DuyetCheoNguoc(i, j) + DiemTC_DuyetCheoXuoi(i, j);
                        long DiemPhongNgu = DiemPN_DuyetDoc() + DiemPN_DuyetNgang() + DiemPN_DuyetCheoNguoc() + DiemPN_DuyetCheoXuoi(); 

                    }
                }
                    return oCoResult;
        }
        private long DiemTC_DuyetDoc(int currDong, int currCot)
        {
            long DiemTong = 0;
            long DiemTC = 0;
            int SoQuanTa = 0;
            int SoQuanDich = 0;
            //Tren xuong theo chieu doc
            for (int Dem = 0; Dem < 6 && currDong + Dem < _BanCo.SoDong;Dem++ )
            {
                if (_MangOCo[currDong + Dem, currCot].SoHuu == 1) //May danh truoc
                    SoQuanTa++;
                else if (_MangOCo[currDong + Dem, currCot].SoHuu == 2)
                {
                    SoQuanDich++;
                    //DiemTong
                    break; //Thoat ra khi quan dich chan
                }
                else
                    break;
            }
            //Duoi len theo chieu doc
            for (int Dem = 0; Dem < 6 && currDong - Dem >=0; Dem++)
            {
                if (_MangOCo[currDong - Dem, currCot].SoHuu == 1) //May danh truoc
                    SoQuanTa++;
                else if (_MangOCo[currDong - Dem, currCot].SoHuu == 2)
                {
                    SoQuanDich++;
                    //DiemTong
                    break; //Thoat ra khi quan dich chan
                }
                else
                    break;
            }

            return DiemTong;
        }
        private long DiemTC_DuyetNgang(int currDong, int currCot)
        {
            long DiemTong = 0;

            return DiemTong;
        }
        private long DiemTC_DuyetCheoNguoc(int currDong, int currCot)
        {
            long DiemTong = 0;

            return DiemTong;
        }
        private long DiemTC_DuyetCheoXuoi(int currDong, int currCot)
        {
            long DiemTong = 0;

            return DiemTong;
        }
        #endregion 
    }
}
