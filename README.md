# MạchKit – Website bán linh kiện điện tử theo bộ thực hành

> **Đề tài:** Xây dựng website bán linh kiện điện tử hỗ trợ mua theo bộ thực hành  
> **Tiến độ cập nhật:** Đang bổ sung Chặng 3B–3D của Tuần 03; bảo toàn Chặng 4A đã hoàn thành  
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

### Tuần 03 (16/11–22/11/2026) – Hoàn thiện trang khách hàng và giỏ hàng

**Đính chính ngày 23/11/2026:** 11 bước đã hoàn thành là **11 bước của Chặng 3A**, không phải toàn bộ Tuần 03. Các chặng 3B–3D được bổ sung sau khi rà soát. Không xem tài liệu này là bằng chứng kiểm thử tự động.

| Chặng | Phạm vi | Trạng thái lúc cập nhật |
|---|---|---|
| 3A | Danh sách từ SQL Server; tìm tên/mã; lọc danh mục, thương hiệu, giá; sắp xếp và phân trang | Đã hoàn thành theo xác nhận của sinh viên |
| 3B | Chi tiết theo slug; ảnh, giá, tồn, mô tả, thông số, datasheet, sản phẩm liên quan | Có mã bổ sung; chờ áp dụng và kiểm thử |
| 3C | Giỏ hàng SQL Server theo tài khoản; thêm/cập nhật/xóa; chặn vượt tồn | Có mã bổ sung; chờ migration và kiểm thử |
| 3D | Chương 2, báo cáo Tuần 03 và README duy nhất | Có bản nháp; chờ rà soát và Push |

Chỉ đổi trạng thái 3B/3C/3D thành **Hoàn thành – đã kiểm thử/đã rà soát** sau khi thực hiện đúng các bước tương ứng. Ghi kết quả thực tế vào [báo cáo Tuần 03](progress-report/Tuan-03.md).

Phần bổ sung dùng `LinhKienChiTietController` cho chi tiết và `GioHangService` cho xử lý giỏ. Bảng `GioHangItem` lưu một dòng cho mỗi cặp tài khoản – linh kiện. Tổng tiền trong giỏ tính từ giá hiện tại trong database; giỏ chưa giữ/trừ tồn và chưa tạo đơn hàng.

[Bản nháp Chương 2 – Nghiên cứu lý thuyết](thesis/doc/Chuong-2-Nghien-cuu-ly-thuyet.md)

### Ghi nhận phần Tuần 04 đã thực hiện

Chặng **4A**: sinh viên đã xác nhận tạo và kiểm thử thành công các bảng `DonHang`, `ChiTietDonHang`. Giữ nguyên model, cấu hình và migration đã áp dụng. Tạm dừng triển khai thêm 4B để hoàn thiện phần còn thiếu của Tuần 03; không ghi toàn bộ Tuần 04 đã hoàn thành.

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
| `/LinhKien/ChiTiet/{slug}` | Chi tiết linh kiện (sau khi áp dụng Chặng 3B) |
| `/GioHang` | Giỏ hàng theo tài khoản (sau khi áp dụng và migration Chặng 3C) |
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
        ├── Services/              # Xử lý nghiệp vụ giỏ hàng
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

## 9. Quy tắc quản lý GitHub

- Duy trì **một repository** và **một file `README.md` tại thư mục gốc**.
- Không sử dụng `README_Tuan_02.md`, `README_Tuan_03.md` hay các README tuần khác; nội dung tiến độ được tổng hợp vào file này.
- Mỗi tuần có thể có **báo cáo riêng** trong `progress-report/Tuan-0N.md` (đây là báo cáo, không phải README).
- Commit theo chức năng hoặc chặng, sau khi chạy thử; Push lên repository hiện có.
- Không tạo commit giả, không đưa mật khẩu, API key hoặc dữ liệu nhạy cảm lên GitHub.

**Đề xuất commit cập nhật tài liệu:**

```text
Tuần 03: Cập nhật báo cáo tiến độ và Chương 2
```

## 10. Trạng thái cập nhật

**Ngày rà soát:** 23/11/2026. Bản này sửa việc nhầm lẫn 11 bước Chặng 3A với toàn bộ Tuần 03. Các kết quả 3B–3D chỉ được xác nhận sau khi chạy thử/rà soát trên máy sinh viên. Không thay thế bằng suy đoán từ nội dung README.

Chỉ giữ file `README.md` ở thư mục gốc. Nếu còn `README_Tuan_02.md`, đối chiếu và sao lưu ngoài repository rồi xóa bản trùng sau khi các nội dung cần thiết đã được hợp nhất. Không xóa README của các thư viện phụ thuộc.
