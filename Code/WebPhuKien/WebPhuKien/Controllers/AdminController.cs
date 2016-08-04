using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhuKien.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
namespace WebPhuKien.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        DataClasses1DataContext data = new DataClasses1DataContext();
          
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Xoabanner(string banner)
        {
            var b = data.BANNERs.FirstOrDefault(n => n.Banner1 == banner);
            if (b == null)
            {
                Response.StatusCode = 404;
                return null; 
            }
            return View(b);
        }
        [HttpPost]
        public ActionResult Xoabanner(string banner,FormCollection c)
        {
            var b = data.BANNERs.FirstOrDefault(n => n.Banner1 == banner);
            if (b == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            string fullPath = Request.MapPath("~/Banner/" + banner);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            data.BANNERs.DeleteOnSubmit(b);
            data.SubmitChanges();
            return RedirectToAction("Banner");
        }
        public ActionResult Banner(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 3;
            return View(data.BANNERs.ToList().ToPagedList(pageNumber,pageSize));
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Banner(BANNER b, HttpPostedFileBase fileupload)
        {
            if (fileupload == null)
            {

                ViewBag.Thongbao = "Hảy Chọn File Hình !";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Banner"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "File đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    b.Banner1 = fileName;
                    data.BANNERs.InsertOnSubmit(b);
                    data.SubmitChanges();
                }
            }
            return RedirectToAction("Banner");
        }
        [HttpGet]
        public ActionResult Xoasp(string id)
        { 
            SANPHAM sp = data.SANPHAMs.SingleOrDefault(n => n.Idsp == id);
                
                if (sp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                ViewBag.Masach = sp.Idsp;
                return View(sp);
        }
        [HttpPost,ActionName("Xoasp")]
        public ActionResult Xacnhanxoa(string id)
        {
             SANPHAM sp = data.SANPHAMs.SingleOrDefault(n => n.Idsp == id);
                if (sp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                ViewBag.Masach = sp.Idsp;
                data.SANPHAMs.DeleteOnSubmit(sp);
                data.SubmitChanges();
                return RedirectToAction("Sanpham");
        }
        [HttpGet]
        public ActionResult Suasp(string id)
        {
            SANPHAM sp = data.SANPHAMs.SingleOrDefault(n => n.Idsp == id);
            ViewBag.Masach = sp.Idsp;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            ViewBag.IdLoai = new SelectList(data.LOAISANPHAMs.ToList().OrderBy(n => n.Tenloai), "Idloai", "Tenloai");
            ViewBag.IdNsx = new SelectList(data.NHASANXUATs.ToList().OrderBy(n => n.Tennsx), "Idnsx", "Tennsx");
          
            return View(sp);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasp(SANPHAM sp, HttpPostedFileBase fileupload, FormCollection c)
        {
            var tensp = c["Tensanpham"];
            ViewBag.IdLoai = new SelectList(data.LOAISANPHAMs.ToList().OrderBy(n => n.Tenloai), "Idloai", "Tenloai");
            ViewBag.IdNsx = new SelectList(data.NHASANXUATs.ToList().OrderBy(n => n.Tennsx), "Idnsx", "Tennsx");
            if (fileupload == null)
            {
                sp.Tensanpham = c["Tensanpham"];
                UpdateModel(sp);
                data.SubmitChanges();
                return RedirectToAction("Sanpham");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sp.Hinhanh = fileName;
                    UpdateModel(sp);
                    data.SubmitChanges();
                }
            }
            return RedirectToAction("Sanpham");
        }
        public ActionResult Chitietsp(string id)
        {
            SANPHAM sp = data.SANPHAMs.SingleOrDefault(n => n.Idsp == id);
            ViewBag.Masach = sp.Idsp;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        public ActionResult Sanpham(int ?page)
        {
            int pageNum = (page ?? 1);
            int pageSize = 7;
            return View(data.SANPHAMs.ToList().OrderBy(n=>n.Idsp).ToPagedList(pageNum,pageSize));
        }
        [HttpGet]
        public ActionResult Themsanpham()
        {
            ViewBag.IdLoai = new SelectList(data.LOAISANPHAMs.ToList().OrderBy(n => n.Tenloai), "Idloai", "Tenloai");
            ViewBag.IdNsx = new SelectList(data.NHASANXUATs.ToList().OrderBy(n => n.Tennsx), "Idnsx", "Tennsx");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themsanpham(SANPHAM sp, HttpPostedFileBase fileupload)
        {
            ViewBag.IdLoai = new SelectList(data.LOAISANPHAMs.ToList().OrderBy(n => n.Tenloai), "Idloai", "Tenloai");
            ViewBag.IdNsx = new SelectList(data.NHASANXUATs.ToList().OrderBy(n => n.Tennsx), "Idnsx", "Tennsx");
            if (fileupload == null)
            {
                
                ViewBag.Thongbao = "Hảy Chọn Ảnh Sản Phẩm";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sp.Hinhanh = fileName;
                    data.SANPHAMs.InsertOnSubmit(sp);
                    data.SubmitChanges();
                }
            }
            return RedirectToAction("Sanpham");
        }
        public ActionResult NSX()
        {
            var nsx = data.NHASANXUATs.Select(n => n);
            return View(nsx);
        }
        public ActionResult LoaiSP()
        {
            var loai = data.LOAISANPHAMs.Select(n => n);
            return View(loai);
        }
        public ActionResult EditLSP(string idloai)
        {
            return View();
        }
        [HttpPost]
        public ActionResult EditLSP(string Idloai, FormCollection c)
        {

            var nv = data.LOAISANPHAMs.First(n => n.Idloai == Idloai);
            data.LOAISANPHAMs.DeleteOnSubmit(nv);
            LOAISANPHAM sp2 = new LOAISANPHAM();
            string ten = c["Tenloai"];
            sp2.Idloai = Idloai;
            sp2.Tenloai = ten;           
            data.LOAISANPHAMs.InsertOnSubmit(sp2);
            data.SubmitChanges();
            return RedirectToAction("LoaiSP", "Admin");
        }
        public ActionResult DetailsLSP()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CreateLSP()
        {

            return View();
        }
        [HttpPost]
        public ActionResult CreateLSP(FormCollection c)
        {
            var Maloai = c["Idloai"];
            var ten = c["Tenloai"];
            LOAISANPHAM sp = new LOAISANPHAM();
            sp.Idloai = Maloai;
            sp.Tenloai = ten;
            data.LOAISANPHAMs.InsertOnSubmit(sp);
            data.SubmitChanges();
            return RedirectToAction("LoaiSP", "Admin");
        }
        [HttpPost]
        public ActionResult DeleteLSP(string idloai, FormCollection c)
        {
            var sp = data.LOAISANPHAMs.First(n => n.Idloai == idloai);
            data.LOAISANPHAMs.DeleteOnSubmit(sp);
            data.SubmitChanges();
            return RedirectToAction("LoaiSP", "Admin");
        }
        public ActionResult EditNSX(string idloai)
        {
            return View();
        }
        [HttpPost]
        public ActionResult EditNSX(string Idnsx, FormCollection c)
        {

            var nsx = data.NHASANXUATs.First(n => n.Idnsx == Idnsx);
            data.NHASANXUATs.DeleteOnSubmit(nsx);
            NHASANXUAT nsx2 = new NHASANXUAT();
            string ten = c["Tennsx"];
            string dc = c["Diachi"];
            string sdt = c["Sdtnsx"];
            nsx2.Idnsx = Idnsx;
            nsx2.Tennsx = ten;
            nsx2.Diachi = dc;
            nsx2.Sdtnsx = sdt;
            data.NHASANXUATs.InsertOnSubmit(nsx2);
            data.SubmitChanges();
            return RedirectToAction("NSX", "Admin");
        }
        public ActionResult DetailsNSX()
        {
            return View();
        }
        public ActionResult CreateNSX()
        {

            return View();
        }
        [HttpPost]
        public ActionResult CreateNSX(FormCollection c)
        {
            var id = c["Idnsx"];
            var ten = c["Tennsx"];
            var dc = c["Diachi"];
            var sdt = c["Sdtnsx"];
            NHASANXUAT nsx = new NHASANXUAT();
            nsx.Idnsx = id;
            nsx.Tennsx = ten;
            nsx.Diachi = dc;
            nsx.Sdtnsx = sdt;
            data.NHASANXUATs.InsertOnSubmit(nsx);
            data.SubmitChanges();
            return RedirectToAction("NSX", "Admin");
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login( FormCollection collection)
        {
            var tendn = collection["username"];
            var mk = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải Nhập Tên Đăng Nhập";
            }
            else if (String.IsNullOrEmpty(mk))
            {
                ViewData["Loi2"] = "Phải Nhập Mật Khẩu";
            }
            else
            {
                QUANTRI ad = data.QUANTRIs.SingleOrDefault(n => n.User == tendn && n.Password == mk);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.Thongbao = "Đăng Nhập Không Thành Công";
                }
            }
            return this.Login();

        }
	}
}