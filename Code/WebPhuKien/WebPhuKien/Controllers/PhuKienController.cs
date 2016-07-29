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

        private List<SANPHAM> Laysanpham(int count)
        {
            return data.SANPHAMs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }

        public ActionResult Index(int ? page)
        {
          //  var spmoi = Laysanpham(3);
          //  return View(spmoi);
            int pageSize = 5;
            int pageNum = (page ?? 1);

            var spmoi = Laysanpham(15);
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
        public ActionResult ThongTinShop()
        {
            return View();
        }
        public ActionResult LienHe()
        {
            return View();
        }
	}
}