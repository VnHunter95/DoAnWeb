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
        bool KiemTraStringCoSo(string text) //Kt CHuỗi
        {
            foreach (char c in text)
            {
                if (((c >= '0' && c <= '9')))
                {
                    return true;
                }
            }
            return false;
        }
        bool KiemTraStringLaSo(string text) //Kt CHuỗi
        {
            foreach (char c in text)
            {
                if (((c >= '0' && c <= '9')))
                {

                }
                else
                    return false;
            }
            return true;
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
            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            KHACHHANG khmoi = data.KHACHHANGs.FirstOrDefault(n=>n.Username==kh.Username);
            if(khmoi==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            string hoten = collection["Hoten"];
            string Sdt = collection["Sdt"];
            string Diachi = collection["Diachi"];
            string Email = collection["Email"];
            Boolean coloi = false;
            if (hoten.Length > 25 || string.IsNullOrEmpty(hoten) || KiemTraStringCoSo(hoten))
            {
                ViewData["Loi2"] = "Vui lòng nhập Họ Tên ít hơn 25 ký tự !";
                coloi = true;
            }

            if (string.IsNullOrEmpty(Sdt) || Sdt.Length > 11 || Sdt.Length < 10 || !KiemTraStringLaSo(Sdt))
            {
                ViewData["Loi3"] = "Sai Số Điện Thoại !";
                coloi = true;
            }

            if (Diachi.Length > 150 || string.IsNullOrEmpty(Diachi))
            {
                ViewData["Loi4"] = "Vui lòng nhập địa chỉ ít hơn 150 ký tự !";
                coloi = true;
            }
            if (Email.Length > 50 || string.IsNullOrEmpty(Email))
            {
                ViewData["Loi5"] = "Vui lòng nhập email ít hơn 50 ký tự !";
                coloi = true;
            }
            if (coloi)
            {
                return View(kh);
            }
            khmoi.Hoten = hoten;
            khmoi.Diachi = Diachi;
            khmoi.Sdt = Sdt;
            khmoi.Email = Email;
            UpdateModel(khmoi);
            data.SubmitChanges();
            ViewBag.Thongbao = "Cập Nhật Thành Công";
            return View(khmoi);
            
        }
        [HttpPost]
        public ActionResult Changepass(string user, FormCollection collection)
        {
            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            Boolean coloi = false;
            string pass = collection["pass"];
            string passmoi = collection["passmoi"];
            string passmoi2 = collection["passmoi2"];

            KHACHHANG kh = data.KHACHHANGs.FirstOrDefault(n => n.Username == user);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            if (String.IsNullOrEmpty(pass) || pass.Length > 16)
            {
                TempData["Loipass"] = "Nhập Mật Khẩu 16 Ký Tự !";
                coloi = true;
            }
            else if (kh.Password != pass)
                {
                    TempData["Ketqua"] = "Sai Mật Khẩu ";
                    coloi = true;
                }
            if (String.IsNullOrEmpty(passmoi) || passmoi.Length > 16)
            {
                TempData["Loipass2"] = "Nhập Mật Khẩu Mới Không Quá 16 Ký Tự !";
                coloi = true;   
            }else if (pass == passmoi)
            {
                TempData["Loipass2"] = "Không Được Trùng Mật Khẩu Hiện Tại !";
                coloi = true;      
            }
            if (passmoi != passmoi2)
            {
                TempData["Loipass3"] = "Sai Mật Khẩu Nhập Lại !";
                coloi = true;  
            }
            if (!coloi)
            {
                kh.Password = passmoi;
                UpdateModel(kh);
                data.SubmitChanges();
                TempData["Ketqua"] = "Cập Nhật Mật Khẩu Thành Công ";
            }
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
            if (Session["Taikhoan"] != null)
            {
                return RedirectToAction("Index","Phukien");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Dangky(FormCollection collection,KHACHHANG KH)
        {
            if (Session["Taikhoan"] != null)
            {
                return RedirectToAction("Index","Phukien");
            }
            Boolean coloi = false ;
            var user = collection["Username"];
            var pass = collection["Password"];
            var pass2 = collection["Password2nd"];
            var ten = collection["Hoten"];
            var sdt = collection["Sdt"];
            var diachi = collection["Diachi"];
            var email = collection["Email"];
            KHACHHANG b = data.KHACHHANGs.FirstOrDefault(n => n.Username==user);
            if (String.IsNullOrEmpty(user) || user.Length > 16)
            {
                ViewData["Loi1"] = "Nhập Username Tối Đa 16 Ký Tự!";
                coloi = true;
            }else if (b != null)
                {
                    ViewData["Loi1"] = "Tên Tài Khoản Này Đã Được Đăng Ký !";
                    coloi = true;
                }
            if (String.IsNullOrEmpty(pass) || pass.Length > 16)
            {
                ViewData["Loi2"] = "Nhập Mật Khẩu Tối Đa 16 Ký Tự!";
                coloi = true;
            }
            if (pass.CompareTo(pass2) != 0)
            {
                ViewData["Loi3"] = "Mật khẩu nhập lại không đúng !";
                coloi = true;
            }
            if (String.IsNullOrEmpty(ten) || ten.Length > 25 || KiemTraStringCoSo(ten))
            {
                ViewData["Loi4"] = "Nhập Tên không quá 25 ký tự!";
                coloi = true;
            }
            if (string.IsNullOrEmpty(sdt) || sdt.Length > 11 || sdt.Length < 10 || !KiemTraStringLaSo(sdt))
            {
                ViewData["Loidt"] = "Sai Số Điện Thoại !";
                coloi = true;

            }
            if (diachi.Length > 150 || string.IsNullOrEmpty(diachi))
            {
                ViewData["Loidc"] = "Nhập địa chỉ ít hơn 150 ký tự !";
                coloi = true;
            }
            if (email.Length > 50 || string.IsNullOrEmpty(email))
            {
                ViewData["Loiemail"] = "Nhập email ít hơn 50 ký tự !";
                coloi = true;
            }
            if (coloi)
            {
                return this.Dangky();
            }
                KHACHHANG kh = new KHACHHANG();
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

        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index", "PhuKien");
        }

	}
}