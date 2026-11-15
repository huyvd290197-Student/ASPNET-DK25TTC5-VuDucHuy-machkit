# MạchKit — Website bán linh kiện điện tử

**Sinh viên:** Vũ Đức Huy  
**Lớp:** DK25TTC5  
**MSSV:** 170125378  
**Đề tài:** Xây dựng website bán linh kiện điện tử hỗ trợ mua theo bộ thực hành  
**Thời hạn thực hiện:** 15/12/2026

## 1. Giới thiệu dự án

MạchKit là website bán linh kiện điện tử dành cho người học điện tử, sinh viên và người cần chuẩn bị linh kiện cho các bài thực hành.

Điểm nổi bật của đề tài là hỗ trợ chuẩn bị bộ thực hành: người dùng chọn số bộ cần làm, hệ thống tính số lượng linh kiện cần thiết, chi phí dự kiến và những linh kiện còn thiếu so với tồn kho.

### Bộ thực hành mẫu: Đèn LED đôi trên breadboard

| Linh kiện | Số lượng / bộ | Tồn kho mẫu |
|---|---:|---:|
| LED đỏ | 2 | 10 |
| Điện trở 330 Ω | 2 | 7 |
| Breadboard mini | 1 | 5 |

Kết quả kiểm tra dự kiến: chuẩn bị 3 bộ thì đủ hàng; chuẩn bị 4 bộ thì thiếu 1 điện trở 330 Ω.

## 2. Công nghệ sử dụng

- ASP.NET Core MVC
- C# và .NET 8
- Entity Framework Core
- SQL Server Express
- ASP.NET Core Identity
- Bootstrap, HTML, CSS và JavaScript
- Git và GitHub

## 3. Môi trường phát triển

| Thành phần | Cấu hình |
|---|---|
| Hệ điều hành | Windows 10 Pro |
| Trình soạn thảo | Visual Studio Code |
| .NET SDK | .NET 8 |
| Cơ sở dữ liệu | SQL Server Express |
| SQL Server instance | `.\SQLEXPRESS` |
| Database | `MachKitDb` |
| Quản lý mã nguồn | GitHub / GitHub Desktop |

Project nằm trong thư mục `src/MachKit`, file project là `MachKit.csproj`.

Chạy ứng dụng từ thư mục chứa file `.csproj`:

```powershell
dotnet run --launch-profile http
```

Địa chỉ local đã sử dụng:

```text
http://localhost:5178
```

## 4. Cơ sở dữ liệu

Các bảng nghiệp vụ đã được tạo trong database `MachKitDb`:

- `DanhMuc`
- `ThuongHieu`
- `LinhKien`
- `ThongSoKyThuat`
- `BoThucHanh`
- `ThanhPhanBo`

ASP.NET Core Identity cũng đã được tích hợp để quản lý tài khoản và vai trò, sử dụng các bảng chuẩn như `AspNetUsers`, `AspNetRoles`, `AspNetUserRoles` và các bảng liên quan.

## 5. Kết quả thực hiện đến hết Tuần 02

### Công việc đã thực hiện

- Tạo repository GitHub và đưa project lên GitHub.
- Thiết lập quy trình cập nhật mã nguồn theo từng tuần.
- Cấu hình project sử dụng .NET 8.
- Kết nối ứng dụng với SQL Server Express.
- Tạo database `MachKitDb` và các mô hình dữ liệu nghiệp vụ chính.
- Xây dựng, cải thiện giao diện trang chủ MạchKit.
- Tích hợp ASP.NET Core Identity.
- Thiết lập khu vực quản trị và xác nhận đăng nhập Admin thành công.
- Xử lý lỗi thêm mới Danh mục liên quan đến trường `Slug` bắt buộc.

### Giao diện trang chủ

Trang chủ đã được cải thiện với các thành phần:
- Thanh menu điều hướng.
- Banner giới thiệu MạchKit.
- Hình ảnh minh họa linh kiện.
- Khu vực giới thiệu bộ thực hành.
- Form tính số bộ thực hành.
- Bảng linh kiện và thông tin đủ/thiếu hàng.
- Các thẻ nội dung linh kiện.
- Bố cục hỗ trợ màn hình nhỏ.

### Quản trị và xác thực

ASP.NET Core Identity đã được tích hợp. Người dùng xác nhận đã đăng nhập Admin thành công.

Lỗi thêm Danh mục được xác định do `Slug` là trường bắt buộc nhưng form không gửi giá trị. Hướng xử lý là sinh `Slug` tự động từ tên danh mục ở phía máy chủ và cập nhật ModelState trước khi lưu.

**Lưu ý tiến độ:** Bản cập nhật quản lý Linh kiện và tải ảnh lên đã được cung cấp trong quá trình hướng dẫn, nhưng cần kiểm thử trực tiếp trên project hiện tại để xác nhận thêm/sửa linh kiện và upload ảnh hoạt động đầy đủ. Không đánh dấu các thao tác này là hoàn tất nếu chưa kiểm thử.

## 6. Một số lỗi đã gặp và cách xử lý

### Lỗi MSB1003 — không tìm thấy project

Nguyên nhân: Terminal đang đứng ở thư mục không chứa file `.csproj` hoặc `.sln`.

Cách xử lý: chuyển đến thư mục `src/MachKit`, sau đó kiểm tra:

```powershell
dir *.csproj
```

Phải nhìn thấy `MachKit.csproj` trước khi chạy lệnh `dotnet`.

### Lỗi file thực thi bị tiến trình khác khóa

Đã gặp lỗi build do `MachKit.exe` đang được một tiến trình chạy website sử dụng.

Cách xử lý:
1. Dừng website bằng `Ctrl + C` trong Terminal đang chạy.
2. Nếu tiến trình vẫn còn, xác định và dừng đúng PID.
3. Chạy lại:

```powershell
dotnet clean
dotnet build
```

### Lỗi đăng nhập Admin

Đã điều chỉnh quá trình seed tài khoản/vai trò Admin và cấu hình mật khẩu riêng bằng User Secrets. Người dùng xác nhận đăng nhập thành công.

Không lưu mật khẩu Admin trong repository hoặc README.

### Lỗi lưu Danh mục

Nguyên nhân: `Slug` bắt buộc nhưng form không gửi trường này.

Hướng xử lý: tự sinh `Slug` từ tên danh mục ở controller và xử lý ModelState tương ứng trước khi lưu.

## 7. Kế hoạch Tuần 03

1. Xây dựng trang danh sách linh kiện dành cho khách truy cập.
2. Lấy dữ liệu linh kiện từ SQL Server.
3. Tìm kiếm theo tên hoặc mã linh kiện.
4. Lọc theo danh mục và thương hiệu.
5. Lọc theo khoảng giá.
6. Sắp xếp và phân trang.
7. Xây dựng trang chi tiết linh kiện theo slug.
8. Hiển thị ảnh, giá, tồn kho, mô tả và thông số kỹ thuật.
9. Bắt đầu xây dựng giỏ hàng.
10. Kiểm tra số lượng thêm vào giỏ không vượt tồn kho.
11. Viết báo cáo tiến độ Tuần 03 và Chương 2.

## 8. Quy tắc cập nhật GitHub

Dự án được cập nhật thường xuyên theo từng tuần.

- Chỉ commit những nội dung đã thực hiện và kiểm tra.
- Đặt thông điệp commit rõ ràng, có nhắc đến tuần và chức năng.
- Push mã nguồn sau mỗi mốc công việc.
- Không đưa mật khẩu, khóa API hoặc thông tin bí mật lên GitHub.

Ví dụ thông điệp commit Tuần 02 (chỉ sử dụng nếu nội dung tương ứng thực sự đã được commit):

```text
Tuan 02: Cai thien giao dien trang chu MachKit
Tuan 02: Tao co so du lieu va mo hinh du lieu
Tuan 02: Tich hop Identity va khu vuc quan tri
Tuan 02: Xu ly loi tao moi danh muc
Tuan 02: Cap nhat README va bao cao tien do
```

## 9. Thông tin repository

- **Tên repository:** `ASPNET-DK25TTC5-VuDucHuy-machkit`
- **Tài khoản GitHub:** `huyvd290197-Student`

## 10. Trạng thái tài liệu

README này tổng hợp tiến độ đến hết Tuần 02 theo thông tin đã được xác nhận trong quá trình hướng dẫn. Những chức năng chưa có xác nhận kiểm thử được ghi chú riêng, không xem là đã hoàn tất.

**Bước tiếp theo:** tiếp tục Tuần 03, bắt đầu từ trang danh sách linh kiện phía khách hàng.
