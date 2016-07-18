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
            return View(data.LOAISANPHAMs.Select(a => a));
        }

        public ActionResult NhaSX()
        {
            return View(data.NHASANXUATs.Select(a => a));
        }



	}
}