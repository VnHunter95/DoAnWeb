/*
Created		7/14/2016
Modified		7/16/2016
Project		
Model			
Company		
Author		
Version		
Database		MS SQL 2005 
*/

create database PHUKIENANIME
GO
use PHUKIENANIME
Create table [SANPHAM]
(
	[Idsp] Char(5) NOT NULL,
	[Idloai] Char(5) NOT NULL,
	[Tensanpham] Nvarchar(50) NOT NULL,
	[Thongtin] Nvarchar(150) NULL,
	[Soluongcon] Bigint Default 0 NOT NULL,
	[Nhasanxuat] Nvarchar(15) NULL,
	[Dongia] Decimal(18,0) NULL,
	[Hinhanh] Varchar(50) NOT NULL,
Primary Key ([Idsp])
) 
go

Create table [KHACHHANG]
(
	[Username] Char(16) NOT NULL,
	[Password] Char(16) NOT NULL,
	[Hoten] Nvarchar(25) NOT NULL,
	[Sdt] Char(11) NULL,
	[Diachi] Nvarchar(150) NULL,
	[Email] Char(50) NULL,
Primary Key ([Username])
) 
go

Create table [LOAISANPHAM]
(
	[Idloai] Char(5) NOT NULL,
	[Tenloai] Nvarchar(15) NOT NULL,
Primary Key ([Idloai])
) 
go

Create table [DONDATHANG]
(
	[SoHD] Bigint NOT NULL,
	[Username] Char(16) NOT NULL,
	[Ngaylap] Datetime NOT NULL,
	[Tongtien] Decimal(18,0) NULL,
	[Ngaythanhtoan] Datetime NULL,
Primary Key ([SoHD])
) 
go

Create table [QUANTRI]
(
	[User] Char(16) NOT NULL,
	[Password] Char(16) NOT NULL,
Primary Key ([User])
) 
go

Create table [CT_DDH]
(
	[Idsp] Char(5) NOT NULL,
	[SoHD] Bigint NOT NULL,
	[Soluong] Integer NOT NULL,
	[Size] Char(5) NULL,
	[Giaban] Numeric(18,0) NOT NULL,
Primary Key ([Idsp],[SoHD])
) 
go

Create table [PHIEUNHAP]
(
	[SoPN] Bigint NOT NULL,
	[Ngaylap] Datetime NOT NULL,
	[Tongchi] Decimal(18,0) NULL,
Primary Key ([SoPN])
) 
go

Create table [CT_PN]
(
	[Idsp] Char(5) NOT NULL,
	[SoPN] Bigint NOT NULL,
	[Soluong] Bigint NOT NULL,
	[Giatien] Decimal(18,0) NOT NULL,
Primary Key ([Idsp],[SoPN])
) 
go

Create table [THONGTIN]
(
	[Tencuahang] Nvarchar(50) NULL,
	[Diachi] Nvarchar(1) NULL,
	[Sdt1] Char(11) NULL,
	[sdt2] Char(11) NULL,
	[Email1] Char(50) NULL,
	[Email2] Char(50) NULL,
	[Facebook] Char(50) NULL
) 
go

Create table [GIOHANG]
(
	[Sogh] Bigint NOT NULL,
	[Username] Char(16) NOT NULL,
Primary Key ([Sogh],[Username])
) 
go

Create table [CT_GIOHANG]
(
	[Idsp] Char(5) NOT NULL,
	[Sogh] Bigint NOT NULL,
	[Username] Char(16) NOT NULL,
	[Soluong] Bigint NULL,
	[Thanhtien] Numeric(18,0) NULL,
Primary Key ([Idsp],[Sogh],[Username])
) 
go


Alter table [CT_DDH] add  foreign key([Idsp]) references [SANPHAM] ([Idsp])  on update no action on delete no action 
go
Alter table [CT_PN] add  foreign key([Idsp]) references [SANPHAM] ([Idsp])  on update no action on delete no action 
go
Alter table [CT_GIOHANG] add  foreign key([Idsp]) references [SANPHAM] ([Idsp])  on update no action on delete no action 
go
Alter table [DONDATHANG] add  foreign key([Username]) references [KHACHHANG] ([Username])  on update no action on delete no action 
go
Alter table [GIOHANG] add  foreign key([Username]) references [KHACHHANG] ([Username])  on update no action on delete no action 
go
Alter table [SANPHAM] add  foreign key([Idloai]) references [LOAISANPHAM] ([Idloai])  on update no action on delete no action 
go
Alter table [CT_DDH] add  foreign key([SoHD]) references [DONDATHANG] ([SoHD])  on update no action on delete no action 
go
Alter table [CT_PN] add  foreign key([SoPN]) references [PHIEUNHAP] ([SoPN])  on update no action on delete no action 
go
Alter table [CT_GIOHANG] add  foreign key([Sogh],[Username]) references [GIOHANG] ([Sogh],[Username])  on update no action on delete no action 
go


Set quoted_identifier on
go


Set quoted_identifier off
go


/* Roles permissions */


/* Users permissions */


