using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhuKien.Models;

namespace WebPhuKien.Controllers
{
    public class BannerController : Controller
    {
        //
        // GET: /Banner/
        DataClasses1DataContext data = new DataClasses1DataContext();
        public ActionResult Index()
        {
            var kq = from q in data.BANNERs
                     select q; 
            return PartialView(kq);
        }
	}
}