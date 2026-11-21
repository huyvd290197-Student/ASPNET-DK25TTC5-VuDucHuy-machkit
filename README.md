# MạchKit – Website bán linh kiện điện tử theo bộ thực hành

> **Đề tài:** Xây dựng website bán linh kiện điện tử hỗ trợ mua theo bộ thực hành  
> **Tiến độ cập nhật:** Tuần 03 (16/11–22/11/2026)  
> **Hạn hoàn thành đồ án:** 15/12/2026

## 1. Thông tin sinh viên

| Nội dung | Thông tin |
|---|---|
| **Họ và tên** | **Vũ Đức Huy** |
| **Lớp** | DK25TTC5 |
| **Mã số sinh viên** | 170125378 |
| **Email** | Huyvd290197@tvu-onschool.edu.vn |
| **Số điện thoại** | 0935075821 |
| **Tài khoản GitHub** | [huyvd290197-Student](https://github.com/huyvd290197-Student) |
| **Repository** | [ASPNET-DK25TTC5-VuDucHuy-machkit](https://github.com/huyvd290197-Student/ASPNET-DK25TTC5-VuDucHuy-machkit) |

## 2. Giới thiệu và bài toán

**MạchKit** là website bán linh kiện điện tử phục vụ học sinh, sinh viên và người thực hành lắp ráp mạch điện tử. Ngoài việc quản lý và tìm mua linh kiện, điểm khác biệt của đề tài là **hỗ trợ chuẩn bị linh kiện theo bộ thực hành**:

1. Người dùng chọn số bộ thực hành cần chuẩn bị.
2. Hệ thống tính số lượng cần dùng cho từng linh kiện dựa trên định mức của bộ.
3. Hệ thống tính tổng chi phí dự kiến và đối chiếu với số lượng tồn kho.
4. Nếu thiếu hàng, hệ thống hiển thị linh kiện và số lượng còn thiếu.

### Bộ thực hành minh họa: Đèn LED đôi trên breadboard

| Linh kiện | Số lượng / bộ | Tồn kho của dữ liệu mẫu ban đầu |
|---|---:|---:|
| LED đỏ 5mm | 2 | 10 |
| Điện trở 330 Ω | 2 | 7 |
| Breadboard mini | 1 | 5 |

Với **dữ liệu mẫu ban đầu**: chuẩn bị **3 bộ** thì đủ hàng; chuẩn bị **4 bộ** thì thiếu **1 điện trở 330 Ω**. Đây là tình huống kiểm thử minh họa, **không phải số tồn kho cố định**: khi nhân viên cập nhật hàng trong SQL Server, kết quả tính toán sẽ thay đổi theo dữ liệu thực tế.

## 3. Công nghệ sử dụng

| Thành phần | Công nghệ |
|---|---|
| Ngôn ngữ và framework | C#, .NET 8, ASP.NET Core MVC |
| Truy cập cơ sở dữ liệu | Entity Framework Core |
| Cơ sở dữ liệu | Microsoft SQL Server Express |
| Xác thực và phân quyền | ASP.NET Core Identity |
| Giao diện | Razor Views, Bootstrap, HTML, CSS, JavaScript |
| Quản lý mã nguồn | Git, GitHub, GitHub Desktop |
| Công cụ phát triển | Visual Studio Code, SQL Server Management Studio |
| Hệ điều hành phát triển | Windows 10 Pro |

Repository có `global.json` để cố định phiên bản .NET SDK phù hợp với project.

## 4. Các chức năng và tiến độ thực hiện

### Tuần 01 (02/11–08/11/2026) – Khởi tạo dự án

- Tạo và kết nối repository GitHub; thống nhất quy trình commit theo tuần.
- Tạo project `MachKit`, cấu hình .NET SDK 8 và chạy bản website khởi đầu.
- Xây dựng chức năng dự toán bộ thực hành ban đầu.
- Cập nhật tài liệu giới thiệu dự án và khắc phục các lỗi cấu hình môi trường.

[Báo cáo tiến độ Tuần 01](progress-report/Tuan-01.md)

### Tuần 02 (09/11–15/11/2026) – Cơ sở dữ liệu, quản trị và giao diện

- Kết nối ASP.NET Core MVC với SQL Server Express qua Entity Framework Core.
- Tạo database `MachKitDb` cùng các bảng nghiệp vụ chính: `DanhMuc`, `ThuongHieu`, `LinhKien`, `ThongSoKyThuat`, `BoThucHanh`, `ThanhPhanBo`.
- Chuyển dữ liệu bộ thực hành và linh kiện sang SQL Server.
- Tích hợp ASP.NET Core Identity và các vai trò `Admin`, `NhanVien`, `KhachHang`.
- Xây dựng khu vực quản trị (`Areas/Admin`) và các chức năng quản lý dữ liệu.
- Bổ sung quản lý linh kiện, tải ảnh lên thư mục `wwwroot/uploads/` và trạng thái ngừng bán/mở bán.
- Cải thiện giao diện trang chủ: banner, menu, dự toán bộ thực hành, bảng đủ/thiếu hàng, card linh kiện và bố cục responsive.
- Xử lý vấn đề trường `Slug` bắt buộc khi lưu danh mục; đăng nhập tài khoản Admin đã được xác nhận hoạt động.

[Báo cáo tiến độ Tuần 02](progress-report/Tuan-02.md)

### Tuần 03 (16/11–22/11/2026) – Trang khách hàng và hoàn thiện chức năng mua sắm

**Trạng thái ghi nhận:** Người thực hiện xác nhận đã hoàn thành **11 bước của Tuần 03** trên môi trường phát triển. Các mục dưới đây phản ánh kế hoạch 11 bước đã được báo hoàn thành; việc đồng bộ và đối chiếu từng chức năng với nhánh `main` trên GitHub được thực hiện khi Push các commit Tuần 03.

1. Xây dựng trang **danh sách linh kiện dành cho khách hàng**.
2. Hiển thị dữ liệu linh kiện lấy từ **SQL Server**.
3. Tìm kiếm linh kiện theo **tên hoặc mã**.
4. Lọc linh kiện theo **danh mục và thương hiệu**.
5. Lọc theo **khoảng giá**.
6. **Sắp xếp và phân trang**, giữ điều kiện tìm kiếm/lọc khi chuyển trang.
7. Xây dựng trang **chi tiết linh kiện theo slug**.
8. Trình bày **ảnh, giá, tồn kho, mô tả, thông số kỹ thuật** và các thông tin liên quan của linh kiện.
9. Bắt đầu xây dựng **chức năng giỏ hàng**.
10. Kiểm tra **số lượng thêm vào giỏ không vượt quá tồn kho**.
11. Cập nhật **báo cáo tiến độ Tuần 03** và **Chương 2 – Nghiên cứu lý thuyết**.

**Chặng 3A – Danh sách linh kiện:** Trang khách hàng được triển khai độc lập với Area Admin. Các file chính ở môi trường phát triển gồm:

```text
src/MachKit/Controllers/LinhKienController.cs
src/MachKit/ViewModels/LinhKienDanhSachViewModel.cs
src/MachKit/Views/LinhKien/Index.cshtml
```

Trang danh sách chỉ hiển thị linh kiện **đang bán**; tìm kiếm, lọc, sắp xếp và phân trang được thực hiện qua truy vấn Entity Framework Core.

**Chặng 3B – Chi tiết linh kiện:** Tìm sản phẩm theo slug; trình bày thông tin chi tiết, thông số kỹ thuật và tài liệu datasheet (nếu có).

**Chặng 3C – Giỏ hàng:** Triển khai các thao tác giỏ hàng theo phạm vi đã làm, đặc biệt là kiểm tra số lượng yêu cầu với tồn kho. Việc lưu giỏ hàng vào database và yêu cầu đăng nhập cần được đối chiếu với mã nguồn/migration thực tế trước khi nghiệm thu cuối cùng.

**Chặng 3D – Tài liệu:** Báo cáo tiến độ Tuần 03 đặt tại `progress-report/Tuan-03.md`; nội dung Chương 2 trình bày cơ sở lý thuyết ASP.NET Core MVC, Entity Framework Core, SQL Server, Identity, Bootstrap và bảo mật website. Chỉ tạo/cập nhật các tài liệu này nếu chưa có; **không tạo thêm README theo từng tuần**.

## 5. Môi trường và hướng dẫn chạy

### Yêu cầu

- Windows có cài đặt **.NET SDK 8**.
- SQL Server Express với instance đang sử dụng: **`.\SQLEXPRESS`**.
- Database hiện có: **`MachKitDb`**.
- Cấu hình kết nối cơ sở dữ liệu của project phải trỏ đến instance và database trên.
- Đối với chức năng quản trị, tài khoản và vai trò Identity phải có trong database hiện tại.

> **Không tạo lại project, không xóa/tạo lại database và không chạy lại migration khởi tạo** nếu hệ thống đang hoạt động bình thường. Không đưa mật khẩu, User Secrets hay chuỗi kết nối có thông tin bí mật lên GitHub.

### Chạy bằng PowerShell / Terminal của VS Code

Mở Terminal tại **thư mục gốc repository**, sau đó chạy:

```powershell
cd src/MachKit
dir *.csproj
dotnet --version
dotnet build
dotnet run --launch-profile http
```

Kết quả mong đợi:

- Có file `MachKit.csproj`.
- SDK phù hợp với `global.json` (nhánh .NET 8).
- `dotnet build` thông báo `Build succeeded`.
- Ứng dụng chạy tại địa chỉ được Terminal hiển thị, thường là **http://localhost:5178**.

Nhấn `Ctrl + C` để dừng website trước khi build lại nếu tiến trình còn đang chạy.

### Các đường dẫn chính

| Đường dẫn | Chức năng |
|---|---|
| `/` | Trang chủ, giới thiệu và dự toán bộ thực hành |
| `/LinhKien` | Danh sách linh kiện dành cho khách, tìm kiếm/lọc/sắp xếp/phân trang |
| `/Admin` | Khu vực quản trị (yêu cầu quyền phù hợp) |
| `/Admin/LinhKien` | Quản lý linh kiện, ảnh và trạng thái kinh doanh |

## 6. Cấu trúc mã nguồn và tài liệu

```text
ASPNET-DK25TTC5-VuDucHuy-machkit/
├── README.md                     # README duy nhất của toàn bộ dự án
├── global.json
├── setup/                        # Tài liệu, script hỗ trợ cài đặt
├── progress-report/
│   ├── Tuan-01.md
│   ├── Tuan-02.md
│   └── Tuan-03.md               # Báo cáo Tuần 03 khi được đồng bộ
├── thesis/
│   ├── doc/                     # Nội dung các chương đồ án
│   ├── pdf/
│   ├── abs/
│   └── refs/
└── src/
    └── MachKit/
        ├── Areas/Admin/           # Giao diện và controller quản trị
        ├── Controllers/           # Controller phía khách
        ├── Data/                  # DbContext, seed dữ liệu
        ├── Models/                # Model nghiệp vụ
        ├── ViewModels/            # ViewModel giao diện
        ├── Views/                 # Razor Views
        └── wwwroot/
            ├── css/
            ├── images/
            └── uploads/           # Ảnh linh kiện đã upload
```

Cây thư mục thể hiện cấu trúc dự án và vị trí tài liệu Tuần 03 cần đồng bộ. Một số file mới có thể chỉ đang tồn tại trên máy phát triển nếu chưa được Push lên GitHub.

## 7. Kiểm thử chức năng

Các tình huống cần duy trì khi kiểm tra trên môi trường phát triển:

| Nhóm chức năng | Tình huống kiểm tra | Kết quả mong đợi |
|---|---|---|
| Trang chủ | Mở `/` | Giao diện, banner và form dự toán hoạt động |
| Dự toán | 3 hoặc 4 bộ với tồn kho mẫu ban đầu | Tính đúng số lượng, tiền và phần thiếu theo dữ liệu mẫu |
| Đăng nhập | Admin đăng nhập | Vào được khu vực quản trị |
| Phân quyền | Khách không có quyền truy cập `/Admin` | Bị từ chối hoặc chuyển sang đăng nhập |
| Quản lý linh kiện | Thêm/sửa một sản phẩm và upload ảnh | Dữ liệu và ảnh được lưu đúng |
| Danh sách | Mở `/LinhKien` | Hiện linh kiện đang bán từ database |
| Tìm kiếm | Nhập `LED` hoặc `LK0004` | Trả về linh kiện phù hợp nếu dữ liệu tồn tại |
| Bộ lọc | Lọc theo danh mục, thương hiệu, giá | Chỉ hiện sản phẩm thỏa điều kiện |
| Sắp xếp | Giá tăng/giảm hoặc tên | Đúng thứ tự được chọn |
| Phân trang | Chuyển trang khi có đủ sản phẩm | Giữ lại bộ lọc và từ khóa |
| Chi tiết | Mở một sản phẩm theo slug | Hiện đúng thông tin linh kiện |
| Giỏ hàng | Thêm số lượng vượt tồn | Bị chặn và hiển thị thông báo phù hợp |

**Lưu ý:** Đây là danh sách tiêu chí kiểm thử, không phải kết quả kiểm thử tự động. Cần lưu kết quả kiểm thử thực tế trong báo cáo tiến độ trước khi nghiệm thu.

## 8. Các vấn đề đã gặp và cách xử lý

- **MSB1003:** Terminal chưa ở thư mục chứa `MachKit.csproj`; chuyển đến `src/MachKit`.
- **Không nhận diện lệnh `dotnet`:** Kiểm tra câu lệnh chính xác là `dotnet`, không có ký tự `\` ở đầu; nếu vẫn lỗi, kiểm tra SDK/PATH.
- **Khác phiên bản SDK:** Sử dụng .NET SDK 8 theo `global.json` để tránh lỗi môi trường trước đây.
- **File `MachKit.exe` bị khóa:** Dừng website bằng `Ctrl + C` trước khi build.
- **Đăng nhập Admin:** Đã điều chỉnh seed Identity; cấu hình mật khẩu trong User Secrets, không ghi vào repository.
- **Không lưu được Danh mục:** Tự sinh `Slug` từ tên danh mục và xử lý ModelState.
- **Ảnh LED xanh trên trang chủ hiển thị nhầm ảnh LED đỏ:** Nguyên nhân là logic chọn ảnh theo từ khóa `LED`; hướng xử lý là ưu tiên `HinhAnh` của từng linh kiện từ database, chỉ dùng ảnh mẫu khi chưa có ảnh upload. Cần kiểm tra lại ảnh trên trang chủ sau khi áp dụng thay đổi.