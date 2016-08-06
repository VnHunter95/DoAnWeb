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
            var nv = data.LOAISANPHAMs.First(n => n.Idloai == idloai);
            return View(nv);
        }
        [HttpPost]
        public ActionResult EditLSP(string idloai, FormCollection c)
        {
            
            
                var nv = data.LOAISANPHAMs.First(n => n.Idloai == idloai);
                data.LOAISANPHAMs.DeleteOnSubmit(nv);
                LOAISANPHAM sp2 = new LOAISANPHAM();
                string ten = c["Tenloai"];
                sp2.Idloai = idloai;
                sp2.Tenloai = ten;
                if (string.IsNullOrEmpty(ten))
                {
                    ViewData["Loi"] = "Hãy nhập tên loại, không được để trống!";
                }
                else
                {
                    data.LOAISANPHAMs.InsertOnSubmit(sp2);
                    data.SubmitChanges();
                    return RedirectToAction("LoaiSP", "Admin");
                }

                return this.EditLSP(idloai);
            
        }
        public ActionResult DetailsLSP(string idloai)
        {
            var loai = data.LOAISANPHAMs.First(n => n.Idloai == idloai);
            return View(loai);
        }
        [HttpGet]
        public ActionResult CreateLSP()
        {

            return View();
        }
        [HttpPost]
        public ActionResult CreateLSP(string idloai, FormCollection c)
        {
            LOAISANPHAM a = data.LOAISANPHAMs.SingleOrDefault(n => n.Idloai == idloai);
            if (a != null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else
            {
                var Maloai = c["Idloai"];
                var ten = c["Tenloai"];
                if (string.IsNullOrEmpty(idloai) || idloai.Length>4)
                {
                    ViewData["Loi1"] = "Hãy nhập mã loại, mã loại 4 ký tự bao gồm cả chữ và số!";
                }
                else if (string.IsNullOrEmpty(ten))
                {
                    ViewData["Loi2"] = "Hãy nhập tên loại!";
                }
                else
                {
                    LOAISANPHAM sp = new LOAISANPHAM();
                    sp.Idloai = Maloai;
                    sp.Tenloai = ten;
                    data.LOAISANPHAMs.InsertOnSubmit(sp);
                    data.SubmitChanges();
                    return RedirectToAction("LoaiSP", "Admin");
                }
                return this.CreateLSP();
                
            }
            
        }
        public ActionResult DeleteLSP(string idloai)
        {
            var d = data.LOAISANPHAMs.First(m => m.Idloai == idloai);
            return View(d);
        }
        [HttpPost]
        public ActionResult DeleteLSP(string idloai, FormCollection c)
        {
            SANPHAM a = data.SANPHAMs.SingleOrDefault(n => n.Idloai == idloai);
            if (a != null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else
            {
                var sp = data.LOAISANPHAMs.First(n => n.Idloai == idloai);
                data.LOAISANPHAMs.DeleteOnSubmit(sp);
                data.SubmitChanges();
                return RedirectToAction("LoaiSP", "Admin");
            }
            
        }
        public ActionResult EditNSX(string idnsx)
        {
            var nsx = data.NHASANXUATs.First(n => n.Idnsx == idnsx);
            return View(nsx);
        }
        [HttpPost]
        public ActionResult EditNSX(string idnsx, FormCollection c)
        {
            SANPHAM a = data.SANPHAMs.SingleOrDefault(n => n.Idnsx == idnsx);
            if (a != null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else
            {
                var nsx = data.NHASANXUATs.First(n => n.Idnsx == idnsx);
                data.NHASANXUATs.DeleteOnSubmit(nsx);
                NHASANXUAT nsx2 = new NHASANXUAT();
                string ten = c["Tennsx"];
                string dc = c["Diachi"];
                string sdt = c["Sdtnsx"];
                nsx2.Idnsx = idnsx;
                nsx2.Tennsx = ten;
                nsx2.Diachi = dc;
                nsx2.Sdtnsx = sdt;               
                if (string.IsNullOrEmpty(ten))
                {
                    ViewData["Loi2"] = "Hãy nhập tên Nhà sản xuất!";
                }
                else
                {
                    data.NHASANXUATs.InsertOnSubmit(nsx2);
                    data.SubmitChanges();
                    return RedirectToAction("NSX", "Admin");
                }
                return this.EditNSX(idnsx);
            }
            
        }
        public ActionResult DetailsNSX(string idnsx)
        {
            var nsx = data.NHASANXUATs.First(n => n.Idnsx == idnsx);
            return View(nsx);
        }
        public ActionResult CreateNSX()
        {

            return View();
        }
        [HttpPost]
        public ActionResult CreateNSX(string idnsx,FormCollection c)
        {
            NHASANXUAT a = data.NHASANXUATs.SingleOrDefault(n => n.Idnsx == idnsx);
            if (a != null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else
            {
                var id = c["Idnsx"];
                var ten = c["Tennsx"];
                var dc = c["Diachi"];
                var sdt = c["Sdtnsx"];
                NHASANXUAT nsx = new NHASANXUAT();
                if (string.IsNullOrEmpty(id))
                {
                    ViewData["Loi1"] = "Nhập mã Nhà sản xuất! Bao gồm 5 ký tự bao gồm chữ và số";
                }
                else
                if (string.IsNullOrEmpty(ten))
                {
                    ViewData["Loi2"] = "Vui lòng nhập tên Nhà sản xuất!";
                }
                else
                {
                    nsx.Idnsx = id;
                    nsx.Tennsx = ten;
                    nsx.Diachi = dc;
                    nsx.Sdtnsx = sdt;
                    data.NHASANXUATs.InsertOnSubmit(nsx);
                    data.SubmitChanges();
                    return RedirectToAction("NSX", "Admin");
                }
                return this.CreateNSX();
            }
            
        }
        public ActionResult DeleteNSX(string idnsx)
        {
            var d = data.NHASANXUATs.First(m => m.Idnsx == idnsx);
            return View(d);
        }
        [HttpPost]
        public ActionResult DeleteNSX(string idnsx, FormCollection c)
        {
            SANPHAM a = data.SANPHAMs.SingleOrDefault(n => n.Idnsx == idnsx);
            if (a != null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else
            {
                var nsx = data.NHASANXUATs.First(n => n.Idnsx == idnsx);
                data.NHASANXUATs.DeleteOnSubmit(nsx);
                data.SubmitChanges();
                return RedirectToAction("NSX", "Admin");
            }
            
        }
    //    public ActionResult TTShop()
    //    {
     //      var t = data.THONGTINs.First();
    //        return View(t);
     //   }
        public ActionResult EditTTShop()
        {
            var t = data.THONGTINs.First();
            return View(t);
        }
        [HttpPost]
        public ActionResult EditTTShop(FormCollection c)
        {
                var tt = data.THONGTINs.First();
                data.THONGTINs.DeleteOnSubmit(tt);
                THONGTIN tt2 = new THONGTIN();
                string ten = c["Tencuahang"];
                string dc = c["Diachi"];
                string sdt1 = c["Sdt1"];
                string sdt2 = c["sdt2"];
                string e1 = c["Email1"];
                string e2 = c["Email2"];
                string fb = c["Facebook"];
                tt2.Tencuahang = ten;
                tt2.Diachi = dc;
                tt2.Sdt1 = sdt1;
                tt2.sdt2 = sdt2;
                tt2.Email1 = e1;
                tt2.Email2 = e2;
                tt2.Facebook = fb;
                if (string.IsNullOrEmpty(ten) || ten.Length>50)
                {
                    ViewData["Loi1"] = "Tên shop không được bỏ trống hoặc quá 50 ký tự!";
                }
                if(string.IsNullOrEmpty(dc) || dc.Length>150)
                {
                    ViewData["Loi2"] = "Phải có địa chỉ Shop! Không được bỏ trống hoặc ghi khống!";
                }
                if(string.IsNullOrEmpty(sdt1) || sdt1.Length>11)
                {
                    ViewData["Loi3"] = "Phải có số điện thoại! Không được bỏ trống hoặc ghi khống!";
                }
                if(string.IsNullOrEmpty(e1) || e1.Length>50)
                {
                    ViewData["Loi4"] = "Vui lòng điền ghi email shop!";
                }
                else
                {
                    data.THONGTINs.InsertOnSubmit(tt2);
                    data.SubmitChanges();
                    return RedirectToAction("TTShop", "Admin");
                }
                return this.EditTTShop();
            

        }

        public ActionResult Phanhoi()
        {
            
            var idph = data.LIENHEs.Select(n => n);
            return View(idph);
        }
        public ActionResult QLAdmin()
        {
            return View();
        }
        public ActionResult Changepass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Changepass(FormCollection qt, QUANTRI d)
        {
            var tendn = d.User;
            var mk = d.Password;
            QUANTRI ad = (QUANTRI)Session["Taikhoanadmin"];
            QUANTRI dm = data.QUANTRIs.SingleOrDefault(n => n.User == tendn && n.Password == mk);
            
            data.QUANTRIs.DeleteOnSubmit(ad);
            QUANTRI q = new QUANTRI();
            string pass1 = qt["password"];
            string pass2 = qt["pass2"];
            string pass3 = qt["pass3"];
            ad.Password = pass2;
            if(pass1 != ad.Password)
            {
                ViewData["Loi1"] = "Mật khẩu cũ không đúng!";
            }
            else if (pass3 != pass2)
            {
                ViewData["Loi2"] = "Mật khẩu nhập lại không trùng nhau!";
            }
            else
            {
                data.QUANTRIs.InsertOnSubmit(ad);
                data.SubmitChanges();
                return RedirectToAction("QLAdmin", "Admin");
            }
            return this.Changepass();
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