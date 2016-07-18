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
        public ActionResult Index()
        {
            return View();
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