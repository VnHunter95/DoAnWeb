using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhuKien.Models;

namespace WebPhuKien.Controllers
{
    public class NguoidungController : Controller
    {
        //
        // GET: /Nguoidung/
        DataClasses1DataContext data = new DataClasses1DataContext();
       
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult UserCP()
        {
            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            else 
            {
                KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
                ViewData["Loipass"]=TempData["Loipass"];
                ViewData["Loipassmoi"] = TempData["Loipass2"];
                ViewData["Loipassmoi2"] = TempData["Loipass3"];
                ViewData["Ketqua"] = TempData["Ketqua"];
                return View(kh);
            }
        }
        [HttpPost]
        public ActionResult UserCP(KHACHHANG kh, FormCollection collection)
        { 
            KHACHHANG khmoi = data.KHACHHANGs.FirstOrDefault(n=>n.Username==kh.Username);
            if(khmoi==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            khmoi.Hoten = kh.Hoten;
            khmoi.Diachi = kh.Diachi;
            khmoi.Sdt = kh.Sdt;
            khmoi.Email = kh.Email;
            UpdateModel(khmoi);
            data.SubmitChanges();
            ViewBag.Thongbao = "Cập Nhật Thành Công";
            return View(khmoi);
            
        }
        [HttpPost]
        public ActionResult Changepass(string user, FormCollection collection)
        {
            string pass = collection["pass"];
            string passmoi = collection["passmoi"];
            string passmoi2 = collection["passmoi2"];
            if (String.IsNullOrEmpty(pass) || pass.Length > 16)
            {
                TempData["Loipass"] = "Nhập Mật Khẫu 16 Ký Tự !";
                return RedirectToAction("UserCP");  
            }
            if (String.IsNullOrEmpty(passmoi) || passmoi.Length > 16)
            {
                TempData["Loipass2"] = "Nhập Mật Khẩu Mới Không Quá 16 Ký Tự !";
                return RedirectToAction("UserCP");     
            }
            if (String.IsNullOrEmpty(passmoi2) || passmoi2.Length > 16)
            {
                TempData["Loipass3"] = "Nhập Lại Mật Khẩu Mối Không Quá 16 Ký Tự !";
                if (passmoi.CompareTo(passmoi2) != 0)
                {

                    TempData["Loipass3"] = "Sai Mật Khẩu Nhập Lại !";
                }
                return RedirectToAction("UserCP");  
            }
            KHACHHANG kh = data.KHACHHANGs.FirstOrDefault(n => n.Username == user);
                if(kh==null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
            if(kh.Password == pass)
            {
                kh.Password = passmoi;
                UpdateModel(kh);
                data.SubmitChanges();
                TempData["Ketqua"] = "Cập Nhật Mật Khẩu Thành Công ";
                return RedirectToAction("UserCP");   
            }

            TempData["Ketqua"] = "Sai Mật Khẩu ";
            return RedirectToAction("UserCP");   
        }   
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }

        public ActionResult Hientenuser()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.Username == tendn && n.Password == matkhau);
                if (kh != null)
                {
                    ViewBag.Thongbao = "Đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index","Phukien");
                }
                else
                {
                    ViewBag.Thongbao = "Sau tên đăng nhập hoặc mật khẩu";
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KHACHHANG kh)
        {
            var user = collection["Username"];
            var pass = collection["Password"];
            var pass2 = collection["Password2nd"];
            var ten = collection["Name"];
            var sdt = collection["Sdt"];
            var diachi = collection["Diachi"];
            var email = collection["Email"];
            
            if (String.IsNullOrEmpty(user) || user.Length>16)
            {
                ViewData["Loi1"] = "Vui lòng kiểm tra lại! Không được để trống hoặc đặt hơn 16 ký tự!";
            }
            if (String.IsNullOrEmpty(pass) || pass.Length>16)
            {
                ViewData["Loi2"] = "Password không được để trống hoặc dài hơn 16 ký tự!";
            }
            else if (pass.CompareTo(pass2) != 0)
            {
                    ViewData["Loi2"] = "Mật khẩu nhập lại không đúng !";
            }
            if (String.IsNullOrEmpty(pass2))
            {
                ViewData["Loi3"] = "Nhập lại mật khẩu !";
            }
            if (String.IsNullOrEmpty(ten))
            {
                ViewData["Loi4"] = "Vui lòng nhập tên !";
            }
            else
            {
                kh.Username = user;
                kh.Password = pass;
                kh.Hoten = ten;
                kh.Diachi = diachi;
                kh.Sdt = sdt;
                kh.Email = email;
                data.KHACHHANGs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("Dangnhap");


            }
            return this.Dangky();
        }

        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index", "PhuKien");
        }

	}
}