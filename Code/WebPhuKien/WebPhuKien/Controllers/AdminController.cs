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

        public ActionResult Error()
        { 
                return View();
        }
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");          
            }
            return View();
        }
        //Code Khác
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
        bool KiemTraStringLaChu(string text) //Kt CHuỗi
        {
            foreach (char c in text)
            {
                if (((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c==' '))
                {

                }
                else
                    return false;
            }
            return true;
        }

        //
        //Khach hang
        public ActionResult Khachhang(int? page)
        {
            if (Session["Admin"] == null)
            {
               return RedirectToAction("Error");
            }
            int pageNum = (page ?? 1);
            int pageSize = 30;
            return View(data.KHACHHANGs.OrderBy(n=>n.Username).ToList().ToPagedList(pageNum,pageSize));
        }
        public ActionResult Chitietkh(string user)
        {
            if (Session["Admin"] == null)
            {

               return RedirectToAction("Error");
            }
            return View(data.KHACHHANGs.First(n=>n.Username==user));
        }
        [HttpGet]
        public ActionResult Suakh(string user)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
            var b = data.KHACHHANGs.FirstOrDefault(n => n.Username==user);
            if (b == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(b);
        }
        [HttpPost]
        public ActionResult Suakh(KHACHHANG kh, FormCollection c)
        
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
            KHACHHANG khmoi = data.KHACHHANGs.FirstOrDefault(n=>n.Username==kh.Username);
            if (kh.Password.Length > 16 || string.IsNullOrEmpty(kh.Password))
            { 
                ViewData["Loi1"]= "Vui lòng nhập password ít hơn 16 ký tự !";
                return View(kh);
             }
          
            if (kh.Hoten.Length > 25 || string.IsNullOrEmpty(kh.Hoten))
            {
                ViewData["Loi2"] = "Vui lòng nhập Họ Tên ít hơn 25 ký tự !";
                return View(kh);
            }
         
            if (!string.IsNullOrEmpty(kh.Sdt) && (kh.Sdt.Length > 11 || kh.Sdt.Length < 10 || !KiemTraStringLaSo(kh.Sdt)))
            {
                    ViewData["Loi3"] = "Sai Số Điện Thoại !";
                    return View(kh);
                
            }
            
            if (kh.Diachi.Length > 150 || string.IsNullOrEmpty(kh.Diachi))
            {
                ViewData["Loi4"] = "Vui lòng nhập địa chỉ ít hơn 150 ký tự !";
                return View(kh);
            }
            if (kh.Email.Length > 50 || string.IsNullOrEmpty(kh.Email))
            {
                ViewData["Loi5"] = "Vui lòng nhập email ít hơn 50 ký tự !";
                return View(kh);
            }
            khmoi.Password = kh.Password;
            khmoi.Hoten = kh.Hoten;
            khmoi.Sdt = kh.Sdt;
            khmoi.Email = kh.Email;
            khmoi.Diachi = kh.Diachi;
            UpdateModel(khmoi);
            data.SubmitChanges();
            return RedirectToAction("Khachhang");
        }
        [HttpGet]
        public ActionResult Xoakh(string user)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
            var b = data.KHACHHANGs.FirstOrDefault(n => n.Username == user);
            if (b == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(b);
        }
        [HttpPost]
        public ActionResult Xoakh(string user,FormCollection collection)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
            KHACHHANG b = data.KHACHHANGs.FirstOrDefault(n => n.Username == user);
            var a = data.DONDATHANGs.FirstOrDefault(n => n.Username == user);
            if (a != null)
            {
                ViewData["LoiTrung"] = "Cần phải xóa các Đơn Hàng liên quan Khách Hàng này mới có thể xóa thông tin khách hàng !";
                return View(b);
            }
            if (b == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.KHACHHANGs.DeleteOnSubmit(b);
            data.SubmitChanges();
            return RedirectToAction("Khachhang");
        }
        //
        //HoaDon
        [HttpGet]
        public ActionResult Xemchitiet(int SoHD,int ?page)
        { 
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            ViewBag.SoHD = SoHD;
            return View(data.CT_DDHs.ToList().Where(n=>n.SoHD == SoHD).ToPagedList(pageNumber,pageSize));
        }
        [HttpPost]
        public ActionResult Capnhatdonhang(int SoHD, FormCollection collection, string URL)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
             string Tinhtrang = Request.Form["Tinhtrang"];
             string Thanhtoan = Request.Form["Thanhtoan"];
             
            DONDATHANG hd = data.DONDATHANGs.FirstOrDefault(n => n.SoHD == SoHD);
            if (Tinhtrang == null)
            {
                hd.Tinhtranggiaohang = false;
            }
            else
            {
                hd.Tinhtranggiaohang = true;
            }
            if (Thanhtoan == null)
            {
                hd.Dathanhtoan = false;
            }
            else
            {
                hd.Dathanhtoan = true;
            }
            UpdateModel(hd);
            data.SubmitChanges();
            if (String.IsNullOrEmpty(URL))
            {
                return RedirectToAction("Dondathang");
            }
            return RedirectToAction(URL);
        }
        [HttpGet]
        public ActionResult ThongKeNgay(int? page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
            int pageNum = (page ?? 1);
            int pageSize = 20; 
            decimal Doanhthu = 0;
            var b = data.CT_DDHs.Where(n => n.DONDATHANG.Dathanhtoan == true && n.DONDATHANG.Tinhtranggiaohang == true && n.DONDATHANG.Ngaydat.Value.Date==DateTime.Today).Sum(n => n.Soluong * n.Dongia);
            if (b != null)
            {
                Doanhthu = (decimal)b;
            }
            ViewBag.Doanhthu = Doanhthu;
            return View(data.DONDATHANGs.OrderBy(n => n.SoHD).Where(n => n.Dathanhtoan == true && n.Tinhtranggiaohang == true && n.Ngaydat.Value.Date==DateTime.Today).ToList().ToPagedList(pageNum, pageSize));
        }
        [HttpGet]
        public ActionResult ThongKeThang(int? page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
            int pageNum = (page ?? 1);
            int pageSize = 20;
            decimal Doanhthu = 0;
            var b= data.CT_DDHs.Where(n => n.DONDATHANG.Dathanhtoan == true && n.DONDATHANG.Tinhtranggiaohang == true && n.DONDATHANG.Ngaydat.Value.Month == DateTime.Now.Month && n.DONDATHANG.Ngaydat.Value.Year == DateTime.Now.Year).Sum(n => n.Soluong * n.Dongia);
            if (b != null)
            {
                Doanhthu = (decimal)b;
            }
            ViewBag.Doanhthu = Doanhthu;
            return View(data.DONDATHANGs.OrderBy(n => n.SoHD).Where(n => n.Dathanhtoan == true && n.Tinhtranggiaohang == true && n.Ngaydat.Value.Month==DateTime.Now.Month && n.Ngaydat.Value.Year==DateTime.Now.Year).ToList().ToPagedList(pageNum, pageSize));
        }
        [HttpGet]
        public ActionResult Dondathang(int ?page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
            int pageNum = (page ?? 1);
            int pageSize = 20;
            return View(data.DONDATHANGs.OrderBy(n=>n.SoHD).Where(n=>n.Dathanhtoan == false || n.Tinhtranggiaohang==false).ToList().ToPagedList(pageNum, pageSize));
        }
        [HttpGet]
        public ActionResult HoaDon(int? page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
            int pageNum = (page ?? 1);
            int pageSize = 20;
            return View(data.DONDATHANGs.OrderBy(n => n.SoHD).Where(n=>n.Dathanhtoan==true&&n.Tinhtranggiaohang==true).ToList().ToPagedList(pageNum, pageSize));
        }
        [HttpPost]
        public ActionResult Suattkhcthd(int SoHD, FormCollection collection,String URL)
        {
            if (Session["Admin"] == null)
            {

                return RedirectToAction("Error");
            }
            DONDATHANG hd = data.DONDATHANGs.FirstOrDefault(n=>n.SoHD==SoHD);
            string tennguoinhan = collection["Tennguoinhan"];
            string diachinhan = collection["Diachinhan"];
            string sdtnhan = collection["Sdtnguoinhan"];
            if (String.IsNullOrEmpty(tennguoinhan))
            {
                ViewData["LoiTen"] = "Hãy nhập tên người nhận !";
                return this.Chitiethd(SoHD, null, URL);
            }
            if (String.IsNullOrEmpty(diachinhan))
            {
                ViewData["LoiDiachi"] = "Hãy nhập địa chỉ người nhận !";
                return this.Chitiethd(SoHD, null, URL);
            }
            if (String.IsNullOrEmpty(sdtnhan))
            {
                ViewData["LoiSdt"] = "Hãy nhập số điện thoại người nhận !";
                return this.Chitiethd(SoHD, null, URL);
            }
            DateTime ngaygiao = DateTime.Now;
            if (collection["Ngaygiao"] != null)
            {
                ngaygiao = DateTime.Parse(String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]));
                if (ngaygiao <= DateTime.Today)
                {
                    ViewData["LoiNgayGiao"] = "Lỗi Ngày Giao !";
                    return this.Chitiethd(SoHD, null, URL);
                }
            }
            hd.Emailnguoinhan = collection["Emailnhan"];
            hd.TenNguoiNhan = tennguoinhan;
            hd.Diachinguoinhan = diachinhan;
            hd.Sdtnguoinhan = sdtnhan;
            UpdateModel(hd);
            data.SubmitChanges();
             return this.Chitiethd(SoHD, null, URL);
        }
        public ActionResult Capnhatdonhang(DONDATHANG hd)
        {
            if (Session["Admin"] == null)
            {               
               return RedirectToAction("Error");
            }
          UpdateModel(hd);
          data.SubmitChanges();
          return RedirectToAction("Dondathang");  
        }
        [HttpPost]
        public ActionResult Capnhat1sptrongcthd(int SoHD, string IdSP,FormCollection c,String URL)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
            int soluong = int.Parse(c["soluong"]);
            CT_DDH ct = data.CT_DDHs.SingleOrDefault(n => n.SoHD == SoHD && n.Idsp == IdSP);
            ct.Soluong = soluong;
            UpdateModel(ct);
            data.SubmitChanges();
            return this.Chitiethd(SoHD, null, URL);
        }
        [HttpGet]
        public ActionResult Chitiethd(int SoHD,int ?page,String URL)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            ViewBag.SoHD = SoHD;
            ViewData["url"] = URL;
            return View(data.CT_DDHs.ToList().Where(n=>n.SoHD == SoHD).ToPagedList(pageNumber,pageSize));
        }
        [HttpGet]
        public ActionResult Xoahd(int SoHD,int ?page, string URL)
        { 
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
            ViewData["url"] = URL;
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            ViewBag.SoHD = SoHD;
            return View(data.CT_DDHs.ToList().Where(n=>n.SoHD == SoHD).ToPagedList(pageNumber,pageSize));
        }
        [HttpPost]
        public ActionResult Xoahd(int SoHD, FormCollection collection, string URL)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
            List<CT_DDH> ct = data.CT_DDHs.Where(n => n.SoHD == SoHD).ToList();
            foreach (CT_DDH item in ct)
            {
                data.CT_DDHs.DeleteOnSubmit(item);
            }
            DONDATHANG donhang = data.DONDATHANGs.FirstOrDefault(n => n.SoHD == SoHD);
            data.DONDATHANGs.DeleteOnSubmit(donhang);
            data.SubmitChanges();
            if (String.IsNullOrEmpty(URL))
            {
                return RedirectToAction("Dondathang");
            }
            return RedirectToAction(URL);
        }
        //End HoaDon
        //banner
        [HttpGet]
        public ActionResult Xoabanner(string banner)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
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
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
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
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 3;
            return View(data.BANNERs.ToList().ToPagedList(pageNumber,pageSize));
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Banner(BANNER b, HttpPostedFileBase fileupload)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
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
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
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
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
             SANPHAM sp = data.SANPHAMs.SingleOrDefault(n => n.Idsp == id);
                if (sp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                ViewBag.Masach = sp.Idsp;
                string fullPath = Request.MapPath("~/Hinhsanpham/" + sp.Hinhanh);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                data.SANPHAMs.DeleteOnSubmit(sp);
                data.SubmitChanges();
                return RedirectToAction("Sanpham");
        }
        [HttpGet]
        public ActionResult Suasp(string id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
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
        public ActionResult Suasp(SANPHAM sp, HttpPostedFileBase fileUpdate, FormCollection collection)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
            ViewBag.IdLoai = new SelectList(data.LOAISANPHAMs.ToList().OrderBy(n => n.Tenloai), "Idloai", "Tenloai");
            ViewBag.IdNsx = new SelectList(data.NHASANXUATs.ToList().OrderBy(n => n.Tennsx), "Idnsx", "Tennsx");
            SANPHAM spmoi = data.SANPHAMs.First(n => n.Idsp == sp.Idsp);
            string nsx = collection["IdNsx"];
            string loai = collection["IdLoai"];
            string thongtin = collection["Thongtin"];
            string Hinhanh= spmoi.Hinhanh;
            int sl = int.Parse(collection["Soluong"]);
            decimal dongia = decimal.Parse(collection["Dongia"]);
            if (String.IsNullOrEmpty(sp.Tensanpham) || sp.Tensanpham.Length > 50)
            {
                ViewData["LoiTensp"] = "Nhập Tên Sản Phẩm 50 Ký Tự !";
                return this.Suasp(sp.Idsp);
            }
            if (String.IsNullOrEmpty(sp.Thongtin) ||sp.Thongtin.Length > 300)
            {
                ViewData["Loithongtin"] = "Không Được Quá 300 Ký Tự ( Gồm Những Thẻ HTML )";
                return this.Suasp(sp.Idsp);
            }

            spmoi.Idnsx = nsx;
            spmoi.Idnsx = sp.Idnsx;
            spmoi.Idloai = loai;
            spmoi.Thongtin = thongtin;
            spmoi.Tensanpham = sp.Tensanpham;
            spmoi.Soluongcon = sl;
            spmoi.Dongia = dongia;
            if (collection["Ngaycapnhat"] != null)
            {
                DateTime ngayupdate = DateTime.Parse(String.Format("{0:MM/dd/yyyy}", collection["Ngaycapnhat"]));

                if (ngayupdate.Year < 2000 || ngayupdate.Year > 2100)
                {
                    ViewData["Loi4"] = "Lỗi Ngày !";
                    return this.Suasp(sp.Idsp);
                }
                spmoi.Ngaycapnhat = ngayupdate;
            }
            if (fileUpdate == null)
            {
                UpdateModel(spmoi);
                data.SubmitChanges();
                return RedirectToAction("Sanpham");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpdate.FileName);
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpdate.SaveAs(path);
                        string fullPath = Request.MapPath("~/Hinhsanpham/" + Hinhanh);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                        spmoi.Hinhanh = fileName;
                        UpdateModel(spmoi);
                        data.SubmitChanges();
                    }
                    
                }
            }
            return RedirectToAction("Sanpham");
        }
        public ActionResult Chitietsp(string id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
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
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
            int pageNum = (page ?? 1);
            int pageSize = 7;
            return View(data.SANPHAMs.ToList().OrderBy(n=>n.Idsp).ToPagedList(pageNum,pageSize));
        }
        [HttpGet]
        public ActionResult Themsanpham()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
            ViewBag.IdLoai = new SelectList(data.LOAISANPHAMs.ToList().OrderBy(n => n.Tenloai), "Idloai", "Tenloai");
            ViewBag.IdNsx = new SelectList(data.NHASANXUATs.ToList().OrderBy(n => n.Tennsx), "Idnsx", "Tennsx");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themsanpham(SANPHAM sp, HttpPostedFileBase fileupload,FormCollection collection)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error");
            }
            ViewBag.IdLoai = new SelectList(data.LOAISANPHAMs.ToList().OrderBy(n => n.Tenloai), "Idloai", "Tenloai");
            ViewBag.IdNsx = new SelectList(data.NHASANXUATs.ToList().OrderBy(n => n.Tennsx), "Idnsx", "Tennsx");
            Decimal dongia = decimal.Parse(collection["dongia"]);
            int soluong = int.Parse(collection["soluong"]);
            if (String.IsNullOrEmpty(sp.Idsp) || sp.Idsp.Length > 5)
            {
                ViewData["Loi1"] = "Vui lòng Nhập ID Không Quá 5 Ký Tự";
                return View();
            }

            if(data.SANPHAMs.FirstOrDefault(n=>n.Idsp== sp.Idsp)!= null)
            {
                 ViewData["Loi1"] = "Trùng ID SP Trong Cở Sở Dữ Liệu !";
                return View();
            }
        
        
            if (String.IsNullOrEmpty(sp.Tensanpham) || sp.Tensanpham.Length > 50)
            {
                ViewData["Loi2"] = "Vui lòng Nhập Tên SP Không Quá 50 Ký Tự";
                return View();
            }
            if (String.IsNullOrEmpty(sp.Thongtin) || sp.Thongtin.Length > 300)
            {
                ViewData["Loi3"] = "Vui lòng Nhập Thông Tin SP Không Quá 300 Ký Tự ( Bao gồm các thẻ HTML )";
                return View();
            }


            if (sp.Ngaycapnhat.Value.Year < 2000 || sp.Ngaycapnhat.Value.Year > 2100)
            {
                ViewData["Loi4"] = "Lỗi Ngày !";
                return View();
            }
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
                        return View();
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sp.Hinhanh = fileName;
                    sp.Dongia=dongia;
                    sp.Soluongcon = soluong;
                    data.SANPHAMs.InsertOnSubmit(sp);
                    data.SubmitChanges();

                }
                return RedirectToAction("Sanpham");
            }
        }

        //end Sanpham
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
                if (string.IsNullOrEmpty(ten) || ten.Length >25)
                {
                    ViewData["Loi"] = "Hãy nhập tên loại, không được để trống hoặc vượt quá 25 ký tự!";
                }
                else
                {
                    data.LOAISANPHAMs.InsertOnSubmit(sp2);
                    data.SubmitChanges();
                    return RedirectToAction("LoaiSP", "Admin");
                }

                return this.EditLSP(idloai);
            
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
                else if (string.IsNullOrEmpty(ten) || ten.Length>25)
                {
                    ViewData["Loi2"] = "Hãy nhập tên loại, không được quá 25 ký tự!";
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
                if (string.IsNullOrEmpty(ten)|| ten.Length >25)
                {
                    ViewData["Loi2"] = "Hãy nhập tên Nhà sản xuất! Không được vượt quá 25 ký tự!";
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
                if (string.IsNullOrEmpty(id) || id.Length>5)
                {
                    ViewData["Loi1"] = "Nhập mã Nhà sản xuất! Bao gồm 5 ký tự bao gồm chữ và số!";
                }
                else
                if (string.IsNullOrEmpty(ten) || ten.Length>25)
                {
                    ViewData["Loi2"] = "Vui lòng nhập tên Nhà sản xuất! Bao gồm 25 ký tự!";
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
        [HttpGet]
        public ActionResult EditTTShop()
        {
            var t = data.THONGTINs.First();
            return View(t);
        }
        [HttpPost]
        public ActionResult EditTTShop(FormCollection c,THONGTIN tt)
        {
                ViewBag.Thongbao = null;
                if (string.IsNullOrEmpty(tt.Tencuahang) || tt.Tencuahang.Length > 50)
                {
                    ViewData["Loi1"] = "Tên shop không được bỏ trống hoặc quá 50 ký tự!";
                    return View(tt);
                }
                if (string.IsNullOrEmpty(tt.Diachi) || tt.Diachi.Length > 150)
                {
                    ViewData["Loi2"] = "Phải có địa chỉ Shop! Không được bỏ trống hoặc ghi khống!";
                    return View(tt);
                }
                if (string.IsNullOrEmpty(tt.Sdt1) || tt.Sdt1.Length > 11 || tt.Sdt1.Length < 10)
                {
                    ViewData["Loi3"] = "Phải có số điện thoại! Không được bỏ trống hoặc ghi khống!";
                    return View(tt);
                }
                if (string.IsNullOrEmpty(tt.Email1) || tt.Email1.Length > 50)
                {
                    ViewData["Loi4"] = "Vui lòng điền ghi email shop!";
                    return View(tt);
                }
                //data.THONGTINs.DeleteOnSubmit(tt);
                //THONGTIN tt2 = new THONGTIN();
                THONGTIN tt2 = data.THONGTINs.FirstOrDefault();
                tt2.Tencuahang = tt.Tencuahang;
                    tt2.Diachi = tt.Diachi;
                    tt2.Sdt1 = tt.Sdt1;
                    tt2.sdt2 = tt.sdt2;
                    tt2.Email1 = tt.Email1;
                    tt2.Email2 = tt.Email2;
                    tt2.Facebook = tt.Facebook;
                    UpdateModel(tt2);
                    data.SubmitChanges();
                    ViewBag.Thongbao = "Cập Nhật Thành Công ";
                    return View(tt2);
                
            

        }

        public ActionResult Phanhoi()
        {
            
            var idph = data.LIENHEs.Select(n => n);
            return View(idph);
        }
        public ActionResult DelPhanhoi(int idlh)
        {

            var idph = data.LIENHEs.FirstOrDefault(n => n.Idlienhe == idlh);
            return View(idph);
        }
        [HttpPost]
        public ActionResult DelPhanhoi(int idlh,FormCollection c)
        {
            
                var p = data.LIENHEs.First(n => n.Idlienhe == idlh);
                data.LIENHEs.DeleteOnSubmit(p);
                data.SubmitChanges();
                return RedirectToAction("Phanhoi", "Admin");

        }
        [HttpGet]
        public ActionResult Changepass()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Phukien");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Changepass(FormCollection qt)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Phukien");
            }
            QUANTRI ad = (QUANTRI)Session["Admin"];

            string pass1 = qt["password"];
            string pass2 = qt["pass2"];
            string pass3 = qt["pass3"];

            if(pass1 != ad.Password)
            {
                ViewData["Loi1"] = "Mật khẩu cũ không đúng!";
                return this.Changepass();
            }
            if (pass1 == pass2)
            {
                ViewData["Loi2"] = "Mật khẩu mới không được trùng mật khẩu củ!";
                return this.Changepass();
            }
            if (pass3 != pass2)
            {
                ViewData["Loi3"] = "Mật khẩu nhập lại không trùng nhau!";
                return this.Changepass();
            }
            ad.Password = pass2;
            UpdateModel(ad);
            data.SubmitChanges();
            Session["Admin"] = ad;
            ViewBag.Thongbao = "Đổi Mật Khẩu Thành Công !";
            return this.Changepass();
            
        }
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["Admin"] != null)
            {
                RedirectToAction("Index");
            }
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
                    Session["Admin"] = ad;
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