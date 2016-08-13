using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhuKien.Models;

namespace WebPhuKien.Controllers
{
    public class GiohangController : Controller
    {
        //
        // GET: /Giohang/
        DataClasses1DataContext data = new DataClasses1DataContext();
        bool KiemTraStringCoSo(string text) //Kt CHuỗi
        {
            foreach (char c in text)
            {
                if (((c >= '0' && c <= '9')))
                {
                    return true;
                }
            }
            return false;
        }
        bool KiemTraStringLaSo(string text) //Kt CHuỗi
        {
            foreach (char c in text)
            {
                if (((c >= '0' && c <= '9')))
                {

                }
                else
                    return false;
            }
            return true;
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "Phukien");
            }

            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();

            return View(lstGiohang);


        }
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "Phukien");
            }
            Boolean coloi = false;
            string tennguoinhan = collection["TenNguoiNhan"];
            string diachinhan = collection["Diachinguoinhan"];
            string sdtnhan = collection["Sdtnguoinhan"];
            if (String.IsNullOrEmpty(tennguoinhan) || tennguoinhan.Length > 25 || KiemTraStringCoSo(tennguoinhan))
            {
                ViewData["LoiTen"] = "Nhập tên người nhận tối đa 25 ký tự !";
                coloi = true;
            }
            if (String.IsNullOrEmpty(diachinhan)||diachinhan.Length>150)
            {
                ViewData["LoiDiachi"] = "Nhập địa chỉ người nhận tối đa 150 ký tự !";
                coloi = true;
            }
            if (String.IsNullOrEmpty(sdtnhan) || sdtnhan.Length > 11 || sdtnhan.Length < 10 || !KiemTraStringLaSo(sdtnhan))
            {
                ViewData["LoiSdt"] = "Sai Số Điện Thoại !";
                coloi = true;
            }
            DateTime ngaygiao = DateTime.Now;
            if (collection["Ngaygiao"] != null)
            {
                ngaygiao = DateTime.Parse(String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]));
                if (ngaygiao <= DateTime.Today)
                {
                    ViewData["LoiNgayGiao"] = "Lỗi Ngày Giao !";
                    coloi = true;
                }
            }
            if (coloi)
            {
                return this.DatHang();
            }

            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();
            ddh.Emailnguoinhan = collection["Emailnhan"];
            ddh.TenNguoiNhan = tennguoinhan;
            ddh.Diachinguoinhan = diachinhan;
            ddh.Sdtnguoinhan = sdtnhan;
            ddh.Username = kh.Username;
            ddh.Ngaydat = DateTime.Now;
            ddh.Tinhtranggiaohang = false;
            ddh.Ngaygiao = ngaygiao;
            ddh.Dathanhtoan = false;
            data.DONDATHANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                CT_DDH cthd = new CT_DDH();
                cthd.SoHD = ddh.SoHD;
                cthd.Idsp = item.sMaSP;
                cthd.Soluong = item.iSoluong;
                cthd.Dongia = (decimal)item.dDongia;
                data.CT_DDHs.InsertOnSubmit(cthd);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }

        public ActionResult Xacnhandonhang()
        {
            return View();
        }
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }

        public ActionResult ThemGiohang(string Masp)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.Find(n => n.sMaSP == Masp);
            if (sanpham == null)
            {
                sanpham = new Giohang(Masp);
                lstGiohang.Add(sanpham);
                return RedirectToAction("Giohang", "Giohang");

            }
            else
            {
                sanpham.iSoluong++;
                return RedirectToAction("Giohang", "Giohang");
            }
        }
        public ActionResult XoaGiohang(string Masp)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.sMaSP == Masp);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.sMaSP == Masp);
                return RedirectToAction("GioHang");
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "PhuKien");
            }
            return RedirectToAction("GioHang");

        }
        public ActionResult XoaTatcaGiohang()
        {
            List<Giohang> listGiohang = Laygiohang();
            listGiohang.Clear();
            return RedirectToAction("Index", "PhuKien");
        }
        public ActionResult CapnhatGiohang(string MaSP, FormCollection f)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.sMaSP == MaSP);
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongSoLuong;

        }

        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhtien);
            }
            return iTongTien;
        }

        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }

        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "PhuKien");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
	}
}