# MạchKit – Website bán linh kiện điện tử theo bộ thực hành

> **Đề tài:** Xây dựng website bán linh kiện điện tử hỗ trợ mua theo bộ thực hành  
> **Tiến độ cập nhật:** Tuần 04 — 4A–4D hoàn thành theo xác nhận của sinh viên; đang chốt nghiệm thu 4E  
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


### Tuần 03 (16/11–22/11/2026) — Trang khách và giỏ hàng

Sinh viên đã xác nhận hoàn thành đầy đủ 3A–3D sau khi bổ sung các chặng còn thiếu. Việc trước đây nhầm 11 bước của 3A với toàn Tuần 03 đã được đính chính; không dùng bản ghi nhận cũ làm kết luận tiến độ hiện tại.

| Chặng | Phạm vi đã triển khai | Trạng thái ghi nhận |
|---|---|---|
| 3A | Danh sách SQL Server; tìm tên/mã; lọc danh mục, thương hiệu, giá; sắp xếp; phân trang giữ bộ lọc | Hoàn thành theo xác nhận của sinh viên |
| 3B | Chi tiết theo slug; ảnh riêng, giá, tồn, mô tả, thông số, datasheet, sản phẩm liên quan | Hoàn thành theo xác nhận của sinh viên |
| 3C | Giỏ `GioHangItem` theo tài khoản; thêm/cập nhật/xóa; kiểm tra số lượng và tồn | Hoàn thành theo xác nhận của sinh viên |
| 3D | Báo cáo Tuần 03, Chương 2, cập nhật README chung | Hoàn thành theo xác nhận của sinh viên |

Giỏ lưu trong SQL Server, không chuyển sang Session. Giá trong giỏ là giá hiện tại. Thêm vào giỏ chưa giữ/trừ tồn và chưa tạo đơn.

Tài liệu: [Báo cáo Tuần 03](progress-report/Tuan-03.md) · [Chương 2](thesis/doc/Chuong-2-Nghien-cuu-ly-thuyet.md).

### Tuần 04 (23/11–29/11/2026) — Đặt hàng và quản lý đơn

Đây là khoảng thời gian kế hoạch, không phải ngày kiểm thử/commit được điền sẵn.

| Chặng | Phạm vi | Trạng thái lúc chuẩn bị 4E |
|---|---|---|
| 4A | `DonHang`, `ChiTietDonHang`, cấu hình EF và migration | Hoàn thành theo xác nhận của sinh viên |
| 4B | Nhập thông tin người nhận, đọc giỏ hiện tại, xem lại đơn | Hoàn thành theo xác nhận của sinh viên |
| 4C | Xác nhận; lưu đơn/chi tiết; trừ tồn; xóa dòng giỏ đã mua trong cùng transaction; chống gửi trùng | Hoàn thành theo xác nhận của sinh viên |
| 4D | Danh sách/chi tiết Admin; cập nhật trạng thái; hủy trước giao và hoàn tồn; lịch sử khách; nhật ký | Hoàn thành theo xác nhận của sinh viên |
| 4E | Kiểm thử tích hợp, đối chiếu SQL, bằng chứng, Chương 3, báo cáo và README | Đang nghiệm thu; kết quả thực tế chưa được điền trong gói tài liệu |

**Không xem bảng này là bằng chứng đã chạy thử tự động.** Khi 4E đạt, cập nhật dòng tiến độ đầu file và hàng 4E thành “Hoàn thành — đã kiểm thử và cập nhật tài liệu”; ghi ngày thực tế và liên kết kết quả vào báo cáo. Không cần tạo README mới.

Tài liệu: [Báo cáo Tuần 04](progress-report/Tuan-04.md) · [Biên bản nghiệm thu](progress-report/kiem-thu/Tuan-04-Nghiem-thu.md) · [Chương 3](thesis/doc/Chuong-3-Phan-tich-va-thiet-ke-he-thong.md) · [Kịch bản demo](progress-report/Kich-ban-demo-Tuan-04.md).

## 5. Quy tắc đơn hàng và tồn kho đang áp dụng

`Giỏ → Nhập người nhận → Xem lại → Xác nhận → Đơn chờ xác nhận → Admin xử lý`.

| Trạng thái hiện tại | Được chuyển tới | Thay đổi tồn khi xử lý ở 4D |
|---|---|---|
| `ChoXacNhan` | `DaXacNhan` hoặc `DaHuy` | Duyệt: không đổi; hủy: hoàn một lần |
| `DaXacNhan` | `DangGiao` hoặc `DaHuy` | Giao: không đổi; hủy: hoàn một lần |
| `DangGiao` | `HoanThanh` | Không đổi |
| `HoanThanh` | Không chuyển tiếp | Không đổi |
| `DaHuy` | Không chuyển tiếp | Không đổi |

- Tồn đã trừ tại lúc tạo đơn thành công ở 4C. Duyệt/giao/hoàn thành không trừ thêm.
- Hủy trước giao cần lý do 5–500 ký tự. Hoàn tồn, đổi trạng thái và ghi nhật ký nằm trong một giao dịch. Không khôi phục giỏ, không xóa đơn hoặc đặt tổng tiền về 0.
- `MaYeuCau` nhận diện lần đặt; `MaThaoTac` nhận diện lần cập nhật trạng thái. Hai mã có mục đích khác nhau.
- Phiếu xem lại được bảo vệ, mặc định hạn 20 phút; khi xác nhận vẫn đọc lại tài khoản, giỏ, giá, trạng thái bán và tồn tại SQL Server. Phiếu thành công gửi lại tra đơn đã lưu trước khi xét hạn.
- Chi tiết đơn giữ mã/tên/giá tại thời điểm mua. Mã hiển thị `MK-` cộng Id định dạng ít nhất 6 chữ số, không phải cột mã đơn riêng.
- Khách chỉ xem đơn của mình. Chỉ `Admin`/`NhanVien` xử lý đơn. Ghi chú nội bộ và tài khoản nhân viên không được nạp vào nhật ký phía khách.
- `TongTien` hiện là tiền hàng, chưa gồm phí giao. Trạng thái đơn không phải xác nhận thanh toán.

## 6. Môi trường và cách chạy

Giữ project `MachKit`, .NET SDK 8 theo `global.json`, SQL Server `.\SQLEXPRESS` và database `MachKitDb`. Dùng VS Code, SSMS và GitHub Desktop như hiện tại.

Từ gốc repository:

```powershell
cd src/MachKit
dir *.csproj
dotnet --version
dotnet build
dotnet ef migrations list
dotnet run --launch-profile http
```

Dùng URL trong `Now listening on`, thường `http://localhost:5178`. Dừng bằng `Ctrl + C` trước khi build lại. Mật khẩu Admin ở User Secrets, không ghi vào tài liệu/mã nguồn. Không tạo lại database, Identity, project hoặc nâng framework để làm 4E.

4E không thay model và không cần migration mới. Đối chiếu các migration đã có từ 3C/4A/4C/4D với database. Nếu còn pending, kiểm tra đúng kết nối và migration cũ trước; không tạo `Tuan04_4E` rỗng hoặc xóa migrations. Không giả định thứ tự dựa vào tên tuần: 4A từng được làm trước phần bổ sung giỏ Tuần 03.

| Đường dẫn | Chức năng |
|---|---|
| `/` | Trang chủ và dự toán bộ thực hành |
| `/LinhKien` | Danh sách, tìm kiếm, lọc, sắp xếp, phân trang |
| `/LinhKien/ChiTiet/{slug}` | Chi tiết, datasheet, thông số, sản phẩm liên quan |
| `/GioHang` | Giỏ theo tài khoản |
| `/DatHang` | Biểu mẫu người nhận; xem lại rồi xác nhận qua form |
| `/DatHang/ThanhCong/{id}` | Kết quả đơn của chủ tài khoản |
| `/DonHangCuaToi` | Danh sách đơn cá nhân |
| `/DonHangCuaToi/ChiTiet/{id}` | Chi tiết và lịch sử đơn cá nhân |
| `/Admin` | Khu quản trị |
| `/Admin/LinhKien` | Quản lý linh kiện và ảnh |
| `/Admin/DonHang` | Tìm kiếm/lọc/phân trang đơn cho Admin/Nhân viên |
| `/Admin/DonHang/ChiTiet/{id}` | Xem và xử lý đơn |

Không gõ trực tiếp URL của action POST để thử chức năng; dùng biểu mẫu hiện có.

## 7. Cấu trúc chính

```text
ASPNET-DK25TTC5-VuDucHuy-machkit/
├── README.md                       # Một README chính
├── global.json
├── progress-report/
│   ├── Tuan-01.md … Tuan-04.md
│   ├── Kich-ban-demo-Tuan-04.md
│   └── kiem-thu/Tuan-04-Nghiem-thu.md
├── thesis/doc/
│   ├── Chuong-1-Tong-quan.md
│   ├── Chuong-2-Nghien-cuu-ly-thuyet.md
│   └── Chuong-3-Phan-tich-va-thiet-ke-he-thong.md
├── setup/
│   ├── Kiem-tra-Tuan-04.sql
│   └── Huong-dan-chot-Tuan-04.md
└── src/MachKit/
    ├── Areas/Admin/
    ├── Controllers/
    ├── Data/Configurations/
    ├── Models/
    ├── Migrations/
    ├── Services/                   # GioHangService, DatHangService, DonHangQuanLyService
    ├── ViewModels/
    ├── Views/
    └── wwwroot/                   # css, images, uploads
```

Bảng nghiệp vụ chính: `DanhMuc`, `ThuongHieu`, `LinhKien`, `ThongSoKyThuat`, `BoThucHanh`, `ThanhPhanBo`, `GioHangItem`, `DonHang`, `ChiTietDonHang`, `LichSuTrangThaiDonHang`. Identity và `__EFMigrationsHistory` được giữ nguyên.

## 8. Nghiệm thu và giới hạn

Theo [hướng dẫn chốt Tuần 04](setup/Huong-dan-chot-Tuan-04.md), thực hiện hai luồng end-to-end: đơn giao thành công và đơn hủy trước giao. Đối chiếu tồn trước/sau, kiểm tra không gửi trùng, quyền khách/Admin, luồng cạnh tranh tồn và các chức năng Tuần 03. Các kết quả sâu ở 4C/4D được tái sử dụng nếu có bằng chứng và vẫn đúng phiên bản đang nghiệm thu; không cần gây lỗi lại tùy tiện.

[SQL chỉ đọc](setup/Kiem-tra-Tuan-04.sql) kiểm tra schema, dữ liệu và dấu hiệu bất thường. Không có dòng bất thường không chứng minh đã kiểm thử cạnh tranh, rollback, phân quyền hay mất kết nối quanh commit. Bảng rỗng không phải bằng chứng chức năng thành công.

Phạm vi còn ngoài bản đến hết Tuần 04: thanh toán/hoàn tiền trực tuyến, phí vận chuyển, tự hủy phía khách, trả hàng sau giao, mở lại hoặc xóa đơn, mã vận đơn, email tự động, triển khai nhiều máy. Chưa kết nối dự toán bộ thực hành với một luồng “mua cả bộ” mới; không tự ghi đã có chức năng đó.

Đơn cũ thiếu `MaYeuCau` chỉ xem; không tự gán GUID để mở hủy. Nhật ký chỉ có từ khi dùng 4D, không tạo lịch sử quá khứ giả. Cơ chế hiện tại không ngăn người có quyền SQL trực tiếp sửa dữ liệu, và chưa sửa nguy cơ ghi đè tồn từ biểu mẫu Admin cũ. Chưa tuyên bố sẵn sàng vận hành thật hoặc kiểm định bảo mật toàn diện.

## 9. Các vấn đề đã gặp

| Vấn đề | Cách xử lý đã hướng dẫn |
|---|---|
| `MSB1003` | Đứng tại `src/MachKit`, nơi có `MachKit.csproj` |
| Lệnh `\dotnet` | Bỏ ký tự `\` trước lệnh; kiểm tra SDK/PATH khi cần |
| Không tìm thấy `git` trong Terminal | Tiếp tục dùng GitHub Desktop; không tạo lại repository |
| `MachKit.exe` bị khóa | Dừng tiến trình web trước khi build |
| `Slug` bắt buộc nhưng form không gửi | Tự sinh slug và điều chỉnh ModelState như bản CRUD hiện có |
| Hai LED dùng cùng ảnh trang chủ | Ưu tiên `HinhAnh` từng linh kiện, chỉ dùng ảnh mặc định khi thiếu |
| Lỗi Bước 6 của 4A | Đưa cấu hình đơn vào trong `OnModelCreating`, sửa dấu `}` đóng quá sớm |
| Nhầm 11 bước 3A với cả Tuần 03 | Đã bổ sung 3B–3D rồi mới tiếp tục 4B |
| Tab xác nhận/trạng thái đã cũ | Đọc thông báo, xem lại dữ liệu; không bỏ validation/transaction để chạy được |

## 10. GitHub và chốt mốc

Chỉ một repository và một `README.md` chính tại gốc. Báo cáo tuần trong `progress-report/` không phải README. Nếu còn `README_Tuan_02.md` hoặc bản tuần tương tự do dự án tạo, chỉ xóa sau khi đã đối chiếu/hợp nhất; không xóa README của thư viện.

Commit từng mốc đã kiểm thử, không sửa ngày, không điền hash giả. Commit 4E dự kiến:

```text
Tuần 04: Hoàn thiện kiểm thử, báo cáo và README
```
**Ngày biên soạn bản tài liệu này:** 23/11/2026. **Trạng thái cuối Tuần 04:** chờ ghi kết quả 4E thực tế vào báo cáo và bảng nghiệm thu.
