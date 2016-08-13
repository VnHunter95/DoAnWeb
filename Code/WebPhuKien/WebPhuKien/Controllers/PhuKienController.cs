using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhuKien.Models;
using PagedList;
using PagedList.Mvc;


namespace WebPhuKien.Controllers
{
    public class PhuKienController : Controller
    {
        //
        // GET: /PhuKien/
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

        private List<SANPHAM> Laysanpham(int count)
        {
            return data.SANPHAMs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }

        public ActionResult Index(int ? page)
        {
          //  var spmoi = Laysanpham(3);
          //  return View(spmoi);
            int pageSize = 3;
            int pageNum = (page ?? 1);

            var spmoi = Laysanpham(9);
            return View(spmoi.ToPagedList(pageNum, pageSize));

        }
        public ActionResult LoaiSP()
        {

            return PartialView(data.LOAISANPHAMs.Select(a => a));
        }

        public ActionResult NhaSX()
        {
            return PartialView(data.NHASANXUATs.Select(a => a));
        }
        public ActionResult ChiTiet(string id)
        {
           
            var sp = data.SANPHAMs.Where(a => a.Idsp == id);
            return View(sp.Single());
        }
        public ActionResult SPTheoLoai(string id, int? page)
        {
            int pageSize = 9;
            int pageNum = (page ?? 1);
            var sach = data.SANPHAMs.Where(a => a.Idloai == id);
            return View(sach.ToPagedList(pageNum,pageSize));
        }
        public ActionResult SPTheoNSX(string id, int? page)
        {

            int pageSize = 9;
            int pageNum = (page ?? 1);
            var sach = data.SANPHAMs.Where(a => a.Idnsx == id);
            return View(sach.ToPagedList(pageNum,pageSize));
        }
        public ActionResult ThongTinShop()
        {
            var thongtin = data.THONGTINs.Select(n=>n).First();
            return View(thongtin);
        }
        [HttpGet]
        public ActionResult LienHe()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LienHe(FormCollection collection,LIENHE LH)
        {
            string user = collection["Hoten"];            
            string Email = collection["Email"];
            string SDT = collection["Sdt"];
            string Tittle = collection["Tieude"];
            string Content = collection["Noidung"];
            Boolean coloi = false;
            if (String.IsNullOrEmpty(user)||user.Length>25||KiemTraStringCoSo(user))
            {
                ViewData["Loi1"] = "Nhập Tên Tối Đa 25 Ký Tự !";
                coloi = true;
            }
            if (String.IsNullOrEmpty(Email)||Email.Length>50)
            {
                ViewData["Loi2"] = "Nhập Email Tối Đa 50 Ký Tự !";
                coloi = true;
            }
            
            if (!String.IsNullOrEmpty(SDT)&&(SDT.Length>11||SDT.Length<10||!KiemTraStringLaSo(SDT)))
            {
                ViewData["Loi3"] = "Sai Số Điện Thoại ! ";
                coloi = true;
            }
            if (String.IsNullOrEmpty(Tittle)||Tittle.Length>150)
            {
                ViewData["Loi4"] = "Nhập tiêu đề tối đa 150 ký tự !";
                coloi = true;
            }
            if (String.IsNullOrEmpty(Content)||Content.Length>500)
            {
                ViewData["Loi5"] = "Nhập nội dung cần gửi ! Tối đa 500 ký tự ( Bao Gồm Các Thẻ Định Dạng HTML )";
                coloi = true;
            }
            if (coloi)
            {

                return this.LienHe();
            }
                LH.Hoten = user;
                LH.Email = Email;
                LH.Tieude = Tittle;
                LH.Noidung = Content;
                LH.Sdt = SDT;
                data.LIENHEs.InsertOnSubmit(LH);
                data.SubmitChanges();
                return RedirectToAction("Sent");

        }
        public ActionResult Sent()
        {
            return View();
        }
	}
}