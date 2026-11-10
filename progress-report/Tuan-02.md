# BÁO CÁO TIẾN ĐỘ – TUẦN 02

| Nội dung | Thông tin |
|---|---|
| Họ và tên | Vũ Đức Huy |
| Lớp | DK25TTC5 |
| MSSV | 170125378 |
| Đề tài | Website bán linh kiện điện tử hỗ trợ mua theo bộ thực hành |
| Tên dự án | MạchKit |
| Kỳ báo cáo | 09/11/2026 – 15/11/2026 |
| Hạn hoàn thành | 15/12/2026 |

## 1. Công việc đã thực hiện
- Kết nối SQL Server Express với ASP.NET Core thông qua Entity Framework Core.
- Tạo CSDL MachKitDb bằng migration.
- Xây dựng các bảng DanhMuc, ThuongHieu, LinhKien, ThongSoKyThuat, BoThucHanh và ThanhPhanBo.
- Chuyển chức năng dự toán bộ thực hành sang lấy dữ liệu từ SQL Server.
- Bổ sung ASP.NET Core Identity, role Admin/NhanVien/KhachHang.
- Xây dựng Area Admin và các chức năng quản lý danh mục, thương hiệu, linh kiện, thông số kỹ thuật và bộ thực hành.
- Bổ sung upload ảnh linh kiện và cơ chế ẩn/hiện linh kiện thay vì xóa cứng.
- Cải thiện giao diện trang chủ MạchKit.

## 2. Kết quả kiểm tra
- [ ] Trang chủ lấy dữ liệu bộ MK-01 từ CSDL.
- [ ] Nhập 3 bộ: đủ hàng.
- [ ] Nhập 4 bộ: thiếu 1 điện trở 330 Ω.
- [ ] Khách chưa đăng nhập không truy cập được /Admin.
- [ ] Admin đăng nhập và truy cập được /Admin.
- [ ] Thêm một linh kiện có ảnh thành công.
- [ ] Thêm ít nhất 3 thông số kỹ thuật cho một linh kiện.

## 3. Mã commit
- `1e344eabdea7ca8158a7103dbaaa552315d57a10`: Tạo CSDL và mô hình dữ liệu.
- `7052970eac9ffa834c577e9b854cdbd554192a6c`: Kết nối dữ liệu thật, Identity và khu quản trị.
- `8c64bffee127f16cf245a8dea25946525e231e13`: Hoàn thiện giao diện và báo cáo tuần 02.

## 4. Khó khăn và cách xử lý
- LocalDB không có trên máy: sử dụng SQL Server Express instance `.\SQLEXPRESS`.
- SDK .NET 10 gây lỗi CET trên máy Windows: cấu hình project dùng SDK .NET 8.
- Giao diện ban đầu không dùng Layout: tạo `_Layout.cshtml`, `_ViewStart.cshtml`, `_ViewImports.cshtml` và chỉnh lại Index.

## 5. Kế hoạch tuần 03
- Xây dựng trang danh sách linh kiện cho khách.
- Tìm kiếm, lọc theo danh mục/thương hiệu/giá.
- Trang chi tiết linh kiện và datasheet.
- Bắt đầu giỏ hàng.
- Viết Chương 2: Nghiên cứu lý thuyết.
