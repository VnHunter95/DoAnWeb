using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPhuKien.Models
{
    public class Giohang
    {
        DataClasses1DataContext data = new DataClasses1DataContext();
        public string sMaSP { set; get; }
        public string sTenSP { set; get; }
        public string sHinhanh { set; get; }
        public double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }

        public Giohang(string Masp)
        {
            sMaSP = Masp;
            SANPHAM SP = data.SANPHAMs.Single(n => n.Idsp == sMaSP);
            sTenSP = SP.Tensanpham;
            sHinhanh = SP.Hinhanh;
            dDongia = double.Parse(SP.Dongia.ToString());
            iSoluong = 1;
        }
    }
}