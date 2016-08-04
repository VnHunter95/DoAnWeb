﻿using System;
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
        //HoaDon
        [HttpPost]
        public ActionResult Capnhatdonhang(int SoHD,FormCollection c)
        {
            var test = c["thanhtoan"];
            DONDATHANG hd = data.DONDATHANGs.FirstOrDefault(n => n.SoHD == SoHD);
            hd.Tinhtranggiaohang = Boolean.Parse(c["Tinhtranggiaohang"]);
            hd.Dathanhtoan = Boolean.Parse(c["Dathanhtoan"]);
            return RedirectToAction("Dondathang");
        }
        public ActionResult Dondathang()
        {
            return View(data.DONDATHANGs.ToList());
        }
        [HttpPost]
        public ActionResult Suattkhcthd(int SoHD, FormCollection collection)
        {
            DONDATHANG hd = data.DONDATHANGs.FirstOrDefault(n=>n.SoHD==SoHD);
            string tennguoinhan = collection["Tennguoinhan"];
            string diachinhan = collection["Diachinhan"];
            string sdtnhan = collection["Sdtnguoinhan"];
            if (String.IsNullOrEmpty(tennguoinhan))
            {
                ViewData["LoiTen"] = "Hãy nhập tên người nhận !";
                return RedirectToAction("Chitiethd", new { SoHD = SoHD });
            }
            if (String.IsNullOrEmpty(diachinhan))
            {
                ViewData["LoiDiachi"] = "Hãy nhập địa chỉ người nhận !";
                return RedirectToAction("Chitiethd", new { SoHD = SoHD });
            }
            if (String.IsNullOrEmpty(sdtnhan))
            {
                ViewData["LoiSdt"] = "Hãy nhập số điện thoại người nhận !";
                return RedirectToAction("Chitiethd", new { SoHD = SoHD });
            }
            if (collection["Ngaygiao"] != null)
            {
                var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
                hd.Ngaygiao = DateTime.Parse(ngaygiao);
            }
            hd.Emailnguoinhan = collection["Emailnhan"];
            hd.TenNguoiNhan = tennguoinhan;
            hd.Diachinguoinhan = diachinhan;
            hd.Sdtnguoinhan = sdtnhan;
            UpdateModel(hd);
            data.SubmitChanges();
            return RedirectToAction("Chitiethd", new { SoHD = SoHD });
        }
        public ActionResult Capnhatdonhang(DONDATHANG hd)
        { 
          UpdateModel(hd);
          data.SubmitChanges();
          return RedirectToAction("Dondathang");  
        }
        [HttpPost]
        public ActionResult Capnhat1sptrongcthd(int SoHD, string IdSP,FormCollection c)
        {
            int soluong = int.Parse(c["soluong"]);
            CT_DDH ct = data.CT_DDHs.SingleOrDefault(n => n.SoHD == SoHD && n.Idsp == IdSP);
            ct.Soluong = soluong;
            UpdateModel(ct);
            data.SubmitChanges();
            return RedirectToAction("Chitiethd", new { SoHD = SoHD });
        }
        [HttpGet]
        public ActionResult Chitiethd(int SoHD)
        {
            return View(data.CT_DDHs.ToList().Where(n=>n.SoHD == SoHD));
        }
        //End HoaDon
        //banner
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
        //End Banner

        //SanPham
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
        //end Sanpham
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