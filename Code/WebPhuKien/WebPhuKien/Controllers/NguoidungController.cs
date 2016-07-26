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
                    Session["Taikhoan"] = kh.Hoten;
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
            
            if (String.IsNullOrEmpty(user))
            {
                ViewData["Loi1"] = "Không được bỏ trống tên đăng nhập";
            }
            if (String.IsNullOrEmpty(pass))
            {
                ViewData["Loi2"] = "Không được bỏ trống mật khẩu";
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

	}
}