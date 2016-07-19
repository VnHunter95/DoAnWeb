using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhuKien.Models;

namespace WebPhuKien.Controllers
{
    public class PhuKienController : Controller
    {
        //
        // GET: /PhuKien/
        DataClasses1DataContext data = new DataClasses1DataContext();

        private List<SANPHAM> Laysanpham(int count)
        {
            return data.SANPHAMs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }

        public ActionResult Index()
        {
            var spmoi = Laysanpham(3);
            return View(spmoi);
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
        public ActionResult SPTheoLoai(string id)
        {
            var sach = data.SANPHAMs.Where(a => a.Idloai == id);
            return View(sach);
        }
        public ActionResult SPTheoNSX(string id)
        {
            var sach = data.SANPHAMs.Where(a => a.Idnsx == id);
            return View(sach);
        }


	}
}