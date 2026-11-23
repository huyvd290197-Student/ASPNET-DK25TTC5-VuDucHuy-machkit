# CHƯƠNG 3. PHÂN TÍCH VÀ THIẾT KẾ HỆ THỐNG MẠCHKIT

**Phạm vi:** bản thiết kế tương ứng mã nguồn đến Chặng 4D Tuần 04.  
**Ngày biên soạn:** 23/11/2026.  
**Tính chất:** bản nháp phục vụ đồ án; cần rà soát định dạng, đánh số và yêu cầu giảng viên trước khi ghép báo cáo chính.

Chương này được tổng hợp từ HANDOFF, bộ mã nguồn Tuần 03 và các gói 4A–4D đã cung cấp trong cuộc trò chuyện. Các yêu cầu, luồng và tên lớp dưới đây phản ánh các nguồn đó, không phải đề xuất thêm chức năng Tuần 05. Chương không thay thế kết quả thực thi trên máy sinh viên. Phần sửa riêng ngoài các gói và bản GitHub mới nhất chưa được đối chiếu trong lần biên soạn này. [S1–S6]

## 3.1. Bài toán và phạm vi giải quyết

MạchKit hỗ trợ tìm mua linh kiện điện tử phục vụ học tập và thực hành. Ngoài danh sách sản phẩm, điểm riêng của đề tài là dự toán linh kiện theo bộ: người dùng nhập số bộ, hệ thống dùng định mức để tính số lượng từng linh kiện, chi phí dự kiến và số lượng thiếu so với tồn. Bộ minh họa “Đèn LED đôi trên breadboard” cần hai LED đỏ, hai điện trở 330 Ω và một breadboard mini cho mỗi bộ. Với dữ liệu mẫu 10 LED, 7 điện trở và 5 breadboard, ba bộ đủ hàng còn bốn bộ thiếu một điện trở. Đây là kiểm thử gắn với bộ dữ liệu mẫu, không phải kết quả cố định sau khi kho bị thay đổi bởi các đơn hàng. [S1]

Đến Tuần 03, hệ thống có trang danh sách, chi tiết theo slug và giỏ SQL Server theo tài khoản. Tuần 04 nối tiếp bằng nhập người nhận, xem lại, xác nhận đơn, cập nhật tồn và xử lý đơn phía quản trị. Giỏ và dự toán chưa giữ tồn. Việc lưu đơn thành công mới tiêu thụ tồn. Bản nguồn hiện tại không chứng minh có chức năng tự chuyển toàn bộ dự toán bộ thực hành thành giỏ; vì vậy chương không mô tả chức năng đó như đã hoàn thành. [S2–S5]

Ngoài phạm vi: thanh toán/hoàn tiền trực tuyến, phí vận chuyển, khách tự hủy đơn, trả hàng sau giao, mở lại/xóa đơn, mã vận đơn, email tự động và cấu hình vận hành nhiều máy. [S6]

## 3.2. Tác nhân và quyền sử dụng

| Tác nhân | Chức năng trong phạm vi | Giới hạn |
|---|---|---|
| Khách chưa đăng nhập | Trang chủ, dự toán, danh sách/chi tiết linh kiện | Phải đăng nhập trước khi dùng giỏ/đặt hàng/lịch sử |
| Người dùng đã đăng nhập | Giỏ riêng, nhập người nhận, xác nhận đơn, xem đơn của mình | Không truy cập đơn của người khác, không xử lý trạng thái Admin |
| Admin | Quản lý dữ liệu hiện có; xem/tìm/lọc đơn, chuyển trạng thái hợp lệ, hủy trước giao và xem nhật ký nội bộ | Không bỏ qua quy tắc tồn/trạng thái, không mở lại đơn kết thúc |
| Nhân viên có role NhanVien | Xem và xử lý đơn theo phân quyền trong Controller Admin | Cùng ràng buộc chuyển trạng thái như Admin |

Các role được HANDOFF ghi nhận là Admin, NhanVien, KhachHang. Các Controller khách sử dụng điều kiện đã đăng nhập, không yêu cầu riêng role KhachHang; vì vậy không mô tả chúng như chỉ chấp nhận đúng một role này. Controller quản trị đơn cho phép Admin hoặc NhanVien. Việc kiểm tra quyền đơn cá nhân còn được thực hiện trong truy vấn theo Id và NguoiDungId, không chỉ bằng ẩn menu. [S1, S2, S5, S6]

## 3.3. Yêu cầu chức năng và tiêu chí chất lượng

### 3.3.1. Yêu cầu chức năng

| Mã | Nội dung | Thành phần chính |
|---|---|---|
| FR01 | Tìm tên/mã, lọc danh mục/thương hiệu/giá, sắp xếp và phân trang | LinhKienController và ViewModel 3A |
| FR02 | Chi tiết theo slug, ảnh, giá, tồn, mô tả, thông số, datasheet và liên quan | LinhKienChiTietController |
| FR03 | Giỏ SQL Server theo tài khoản; thêm, sửa, xóa; kiểm tra cộng dồn với tồn | GioHangService |
| FR04 | Nhập và kiểm tra thông tin người nhận; xem lại từ dữ liệu giỏ hiện tại | DatHangController và ThongTinNhanHangInput |
| FR05 | Xác nhận; chốt đơn/chi tiết, trừ tồn, xóa dòng giỏ trong một giao dịch | DatHangService |
| FR06 | Gửi lại cùng yêu cầu không tạo đơn hoặc trừ tồn thêm | MaYeuCau, chỉ mục duy nhất và tra đơn đã lưu |
| FR07 | Tìm/lọc/phân trang và xem chi tiết đơn cho quản trị | DonHangController trong Area Admin |
| FR08 | Chuyển trạng thái hợp lệ, không trừ tồn lần thứ hai | DonHangQuanLyService và DonHang4DHelper |
| FR09 | Hủy trước giao, hoàn tồn đúng một lần, lưu nhật ký | MaThaoTac, transaction và LichSuTrangThaiDonHang |
| FR10 | Khách chỉ xem đơn mình và các mốc trạng thái không chứa ghi chú nội bộ | DonHangCuaToiController |
| FR11 | Duy trì dự toán bộ thực hành và giao diện đã có | Mã trang chủ/giải pháp dự toán hiện hữu |

FR11 kế thừa HANDOFF và phần đã làm, không yêu cầu viết lại trang chủ trong Tuần 04. [S1–S6]

### 3.3.2. Tiêu chí chất lượng trong phạm vi đồ án

Tính nhất quán được ưu tiên: không chấp nhận tạo đơn mà không trừ tồn, hoặc hoàn tồn nhưng chưa đổi trạng thái/ghi nhật ký. Quyền truy cập phải được kiểm tra phía máy chủ. Phiếu và số tiền gửi từ trình duyệt không được mặc nhiên tin cậy. Các lần gửi lặp cần nhận biết để không tiêu thụ hoặc hoàn kho nhiều lần. [S5, S6]

Khả năng sử dụng được kiểm tra bằng biểu mẫu có lỗi rõ ràng, các trang trống, màn hình nhỏ và liên kết quay lại giỏ/lịch sử. Đây là tiêu chí nghiệm thu, không phải kết quả đo sẵn. Nguồn không cung cấp số liệu tải, thời gian đáp ứng hoặc số người dùng đồng thời đạt chuẩn; vì vậy không đặt các con số hiệu năng giả hay khẳng định hệ thống sẵn sàng vận hành thật. [S2, S4–S6]

## 3.4. Tổ chức kiến trúc trong một project MVC

Bản triển khai hiện tại là một ứng dụng ASP.NET Core MVC trong project MachKit, không tách thành các dịch vụ mạng độc lập. Các yêu cầu HTTP đi qua Controller; giao diện dùng ViewModel và Razor View; nghiệp vụ giỏ, checkout và trạng thái đơn được đặt trong các lớp Service; AppDbContext truy cập SQL Server và kế thừa ngữ cảnh Identity. [S1–S6]

| Nhóm thành phần | Trách nhiệm cụ thể |
|---|---|
| Controllers phía khách | Nhận yêu cầu, kiểm tra đăng nhập/ModelState, gọi Service, trả View hoặc redirect |
| Areas/Admin/Controllers | Kiểm tra role, nhận yêu cầu xử lý đơn, trả màn hình quản trị |
| ViewModels/Input | Chỉ nhận các trường được phép; tách dữ liệu biểu mẫu khỏi entity lưu trữ |
| GioHangService | Quản lý các dòng giỏ theo chủ tài khoản, đọc giá/tồn hiện tại |
| DatHangService | Chuẩn bị phiếu, kiểm tra lại dữ liệu khi xác nhận, transaction tạo đơn |
| DonHangQuanLyService | Danh sách/chi tiết, kiểm tra chuyển trạng thái, transaction hủy và nhật ký |
| AppDbContext + Configurations | Mapping bảng, quan hệ, độ chính xác tiền, unique index và các constraint đã cấu hình |
| Razor Views/CSS | Trình bày kết quả; không quyết định quyền hay tính tiền chốt đơn chỉ ở trình duyệt |
| SQL Server MachKitDb | Lưu sản phẩm, giỏ, đơn, chi tiết, nhật ký và Identity |

Các Service được đăng ký theo phạm vi request theo các hướng dẫn chèn Program.cs. Data Protection được dùng để bảo vệ phiếu xác nhận. Các thành phần này được bổ sung vào ứng dụng hiện tại, không yêu cầu đổi SQL instance, Identity hoặc thiết kế trang chủ. [S2, S4–S6]

## 3.5. Đặc tả các ca sử dụng chính

### UC01 — Xem lại thông tin đặt hàng

**Tác nhân:** người dùng đã đăng nhập. **Tiền điều kiện:** có giỏ theo tài khoản. Người dùng nhập tên, điện thoại, địa chỉ và ghi chú; hệ thống chuẩn hóa/kiểm tra thông tin, đọc lại giỏ và sản phẩm, hiển thị tổng tiền hiện tại. Nếu hợp lệ, tạo phiếu gắn tài khoản, MaYeuCau, hạn xác nhận và các dòng đã xem. Nếu giỏ rỗng, hàng không hợp lệ hoặc thông tin sai, không phát đơn. **Hậu điều kiện:** chưa ghi đơn, chưa trừ tồn, chưa xóa giỏ. [S4, S5]

Kiểm tra đầu vào hiện có: tên 2–120 ký tự, địa chỉ 5–300, điện thoại tối đa 20 ký tự với 9–15 chữ số theo định dạng cơ bản, ghi chú tối đa 1.000. Chuỗi chỉ có khoảng trắng không đáp ứng độ dài tối thiểu sau chuẩn hóa. Kiểm tra điện thoại không xác minh chủ sở hữu số. [S5]

### UC02 — Xác nhận đặt hàng

**Tác nhân:** chủ giỏ đang đăng nhập. **Tiền điều kiện:** phiếu hợp lệ thuộc tài khoản, nội dung giỏ/giá còn phù hợp, tồn đủ tại lúc xác nhận. Controller chỉ nhận phiếu xác nhận thay vì chấp nhận toàn bộ entity đơn và số tiền tự khai. Service kiểm tra phiếu, tra yêu cầu đã lưu, khóa/đọc lại giỏ và kho trong giao dịch, tạo đơn/chi tiết rồi xóa các dòng đã mua. [S5]

**Kết quả thành công:** đơn mới ChoXacNhan với thời gian UTC của server, tiền hàng đã tính, tồn giảm đúng và redirect sang trang kết quả. **Ngoại lệ:** giá/giỏ thay đổi hoặc thiếu tồn thì yêu cầu xem lại; phiếu sai thì từ chối; kết quả commit chưa rõ thì giữ cùng phiếu để kiểm tra/gửi lại. Một phiếu đã thành công được tra lại trước khi xét hết hạn, tránh tạo lại chỉ vì giỏ đã rỗng. [S5]

### UC03 — Xử lý hoặc hủy đơn

**Tác nhân:** Admin/NhanVien. **Tiền điều kiện:** đơn tồn tại, có MaYeuCau hợp lệ của 4C, trạng thái đã xem vẫn khớp dữ liệu hiện tại. Người xử lý chọn trạng thái mới, nhập ghi chú/lý do và gửi MaThaoTac. Service khóa đơn, tra mã thao tác, kiểm tra hướng chuyển rồi thực hiện cập nhật trong giao dịch. [S6]

Duyệt/giao/hoàn thành không đổi tồn. Khi hủy trước giao, số lượng được hoàn từ chi tiết đã lưu, không từ dữ liệu trình duyệt. Lý do hủy cần 5–500 ký tự. Đơn thiếu chi tiết hoặc số lượng/kho không hợp lệ không được tự hoàn. Sau thành công, trạng thái và một dòng nhật ký mới được lưu cùng việc hoàn tồn nếu có. Gửi lại cùng thao tác không thêm log hoặc hoàn tồn lần nữa. [S6]

### UC04 — Theo dõi đơn cá nhân

**Tác nhân:** tài khoản đăng nhập. Hệ thống chỉ truy vấn đơn theo tài khoản đó, cho lọc trạng thái và phân trang. Chi tiết truy vấn đồng thời Id và NguoiDungId; không tìm thấy/không thuộc chủ thì trả 404. Nhật ký khách chỉ nạp thời gian, trạng thái cũ/mới và dấu hiệu hoàn tồn, không lấy GhiChu nội bộ hoặc danh tính nhân viên. Khách không có chức năng tự hủy ở bản này. [S6]

## 3.6. Thiết kế dữ liệu

### 3.6.1. Các quan hệ chính

Các bảng nền DanhMuc, ThuongHieu, LinhKien, ThongSoKyThuat, BoThucHanh, ThanhPhanBo và Identity được kế thừa. Phạm vi dữ liệu bổ sung/được sử dụng trong luồng mua hàng gồm:

| Quan hệ | Ý nghĩa |
|---|---|
| AspNetUsers 1 — nhiều GioHangItem | Mỗi tài khoản có các dòng giỏ riêng |
| LinhKien 1 — nhiều GioHangItem | Một linh kiện có thể nằm trong giỏ nhiều tài khoản |
| AspNetUsers 1 — nhiều DonHang | Một người có nhiều đơn |
| DonHang 1 — nhiều ChiTietDonHang | Đơn có các dòng sản phẩm đã chốt |
| LinhKien 1 — nhiều ChiTietDonHang | Một sản phẩm có thể xuất hiện ở nhiều đơn |
| DonHang 1 — nhiều LichSuTrangThaiDonHang | Lưu các lần chuyển trạng thái thực tế |
| AspNetUsers 1 — nhiều LichSuTrangThaiDonHang | Liên kết người thực hiện thao tác |
| BoThucHanh — ThanhPhanBo — LinhKien | Định mức bộ có khóa ghép BoThucHanhId/LinhKienId |

Mỗi cặp chủ giỏ–linh kiện là duy nhất; mỗi cặp đơn–linh kiện cũng có chỉ mục duy nhất theo cấu hình 4A. Không thêm một entity “Giỏ hàng mới” để phục vụ checkout. [S2, S3, S6]

### 3.6.2. GioHangItem

| Thuộc tính CLR | Kiểu / cấu hình từ nguồn | Vai trò |
|---|---|---|
| Id | int | Khóa dòng giỏ |
| NguoiDungId | string, Required, tối đa 450 | Chủ giỏ Identity |
| LinhKienId | int | Khóa ngoại linh kiện |
| SoLuong | int, Range ≥1; check SQL >0 | Lượng muốn mua |
| CapNhatUtc | DateTime | Mốc cập nhật |
| RowVersion | byte[], Timestamp | Phát hiện thay đổi dòng giỏ khi so lại phiếu |

Giỏ không lưu đơn giá chốt. Khi đọc giỏ, hệ thống dùng giá hiện tại; khi tạo đơn, giá được đưa vào chi tiết đơn. Quan hệ giỏ–user có Cascade, giỏ–linh kiện Restrict theo cấu hình đã cung cấp; đây là thiết kế mapping, không phải yêu cầu thêm chức năng xóa người dùng. [S2]

### 3.6.3. DonHang

| Thuộc tính CLR | Kiểu / giới hạn | Ý nghĩa |
|---|---|---|
| Id | int | Khóa đơn; dùng tạo mã hiển thị MK-{Id:D6} |
| MaYeuCau | Guid? | Khóa chống gửi trùng bổ sung ở 4C; đơn cũ có thể null |
| NguoiDungId | string, Required, tối đa 450 | Chủ đơn |
| TenNguoiNhan | string, Required, tối đa 120 | Thông tin người nhận đã chốt |
| SoDienThoai | string, Required, tối đa 20 | Điện thoại nhận hàng |
| DiaChiGiaoHang | string, Required, tối đa 300 | Địa chỉ đã chốt |
| GhiChu | string?, tối đa 1.000 | Ghi chú khách lúc đặt |
| NgayDatUtc | DateTime | Thời gian tạo đơn phía server |
| TrangThai | string, Required, tối đa 30 | Một trong 5 trạng thái hiện tại |
| TongTien | decimal; precision(18,0) | Tổng tiền hàng |

Mã MK-… là định dạng giao diện, không được mô tả thành cột MaDonHang không tồn tại. UX_DonHang_MaYeuCau là unique index với filter MaYeuCau khác NULL. Không cấp GUID giả cho đơn cũ để mở xử lý tồn. Quan hệ đơn–user dùng Restrict. [S3, S5, S6]

### 3.6.4. ChiTietDonHang

| Thuộc tính CLR | Kiểu / giới hạn | Ý nghĩa |
|---|---|---|
| Id | int | Khóa dòng chi tiết |
| DonHangId, LinhKienId | int | Liên kết đơn và sản phẩm |
| MaLinhKien | string, Required, tối đa 30 | Mã sản phẩm tại thời điểm đặt |
| TenLinhKien | string, Required, tối đa 200 | Tên tại thời điểm đặt |
| SoLuong | int, Range ≥1 | Lượng đã đặt |
| DonGia | decimal; precision(18,0) | Giá tại lúc đặt |
| ThanhTien | decimal; precision(18,0) | DonGia × SoLuong |

Thông tin bản chụp giúp đơn cũ không đổi giá/tên khi Admin sửa sản phẩm. Bảng vẫn có liên kết LinhKien để đối chiếu tồn/hủy. Mapping chi tiết–đơn là Cascade, chi tiết–linh kiện là Restrict; ứng dụng hiện tại không cung cấp endpoint xóa đơn. Không xem Cascade là cơ chế hoàn tồn. [S3, S5]

### 3.6.5. LichSuTrangThaiDonHang

| Thuộc tính CLR | Kiểu / giới hạn | Ý nghĩa |
|---|---|---|
| Id, DonHangId | int | Khóa log, khóa ngoại đơn |
| MaThaoTac | Guid | Nhận diện yêu cầu đổi trạng thái |
| NguoiThucHienId | string, Required, tối đa 450 | Tài khoản thực hiện |
| TrangThaiCu, TrangThaiMoi | string, Required, tối đa 30 | Bước chuyển thực tế |
| GhiChu | string?, tối đa 500 | Ghi chú nội bộ/lý do hủy |
| ThoiDiemUtc | DateTime | Mốc thao tác |
| DaHoanTon | bool | Có hoàn tồn trong lần cập nhật này |

UX_LichSuDonHang_MaThaoTac duy nhất theo mã; UX_LichSuDonHang_HuyMotLan duy nhất theo DonHangId khi TrangThaiMoi=DaHuy. Check CK_LichSuDonHang_HoanTon buộc dấu hiệu hoàn tồn tương ứng log hủy. Các quan hệ với đơn và người thao tác dùng Restrict. Nhật ký không được tạo lùi cho lịch sử trước khi có 4D; chưa có chức năng sửa/xóa nhật ký. [S6]

Các bảng trên trình bày kiểu CLR và cấu hình trong source đã đọc. Tên/kiểu SQL và FK sinh thực tế cần đối chiếu migration/SSMS; không đoán tên constraint sinh tự động ngoài những tên được đặt rõ trong cấu hình.

## 3.7. Thiết kế xử lý tồn và giao dịch

### 3.7.1. Xác nhận đặt hàng

Phiếu được Data Protection bảo vệ chứa tài khoản, MaYeuCau, hạn 20 phút, người nhận và từng dòng Id giỏ/Id linh kiện/số lượng/giá đã xem/RowVersion. Phiếu không phải bằng chứng hàng đã được giữ. XacNhanAsync đọc và kiểm tra phiếu; GhiDonMotLanAsync tra đơn đã lưu trước, rồi mới đánh giá các điều kiện để tạo đơn mới. [S5]

Giao dịch dùng ReadCommitted với khóa bổ sung UPDLOCK/HOLDLOCK ở giỏ và kho. Hệ thống khóa giỏ theo tài khoản, tra mã yêu cầu lần nữa sau khi có khóa, so số dòng, số lượng và RowVersion. Linh kiện được khóa theo Id tăng dần, kiểm tra tồn, giá, trạng thái bán và giới hạn tiền. Sau đó cập nhật tồn có điều kiện trên SQL, kiểm tra số dòng ảnh hưởng, tạo DonHang/ChiTietDonHang, xóa đúng các dòng giỏ rồi SaveChanges và Commit. Lỗi trước commit phải rollback các thay đổi trong transaction. [S5]

Tối đa 200 loại linh kiện/lần đặt theo hằng số của gói 4C. Tiền dùng decimal(18,0), được kiểm tra không âm, không có phần lẻ và không vượt giới hạn. Đây là giới hạn kỹ thuật hiện có, không phải số liệu hiệu năng đo được. Khóa chỉ giữ trong lúc xác nhận, không kéo dài suốt thời gian người dùng nhập form. [S5]

### 3.7.2. Hủy trước giao

Transaction khóa đơn, kiểm tra MaThaoTac đã ghi, so trạng thái cũ với trạng thái hiện tại và kiểm tra hướng chuyển. Nếu cùng mã thao tác đã được thực hiện với đúng đơn/người/nội dung, trả kết quả cũ mà không hoàn thêm. Nếu yêu cầu mới là hủy, đọc chi tiết, cộng gộp số lượng theo LinhKienId, khóa sản phẩm theo Id tăng dần rồi cộng tồn bằng cập nhật SQL có điều kiện, tránh vượt giới hạn int. [S6]

Cập nhật trạng thái và thêm log nằm trong transaction cùng các cập nhật tồn. Không giảm tiền hàng, không xóa đơn, không tái tạo giỏ và không tự mở bán sản phẩm đang ngừng bán. Nhân viên dùng tab cũ không được âm thầm chuyển tiếp trạng thái từ dữ liệu mới mà họ chưa xem. [S6]

### 3.7.3. Kết quả commit chưa rõ

Hai Service dùng execution strategy theo cấu hình EF của ứng dụng và giữ mã yêu cầu qua lần xử lý lại. Nếu mất phản hồi quanh commit, không thể chỉ từ lỗi kết nối mà kết luận database chưa lưu. Trang ThuLai giữ nguyên phiếu/yêu cầu để tra kết quả đã tồn tại. Nguồn không bảo đảm không bao giờ deadlock hoặc mô tả một kiểm thử lỗi mạng đã chạy; các trường hợp này phải được ghi trạng thái kiểm thử riêng. [S5, S6]

## 3.8. Máy trạng thái đơn hàng

| Trạng thái đang có | Đích hợp lệ | Điều kiện riêng | Thay đổi tồn ở 4D |
|---|---|---|---|
| ChoXacNhan | DaXacNhan, DaHuy | Hủy có lý do hợp lệ | Chỉ hủy mới cộng lại |
| DaXacNhan | DangGiao, DaHuy | Chưa giao | Chỉ hủy mới cộng lại |
| DangGiao | HoanThanh | Không hủy trong phạm vi này | Không đổi |
| HoanThanh | Không có | Không mở lại | Không đổi |
| DaHuy | Không có | Không mở lại | Không đổi |

Đơn bắt đầu ChoXacNhan sau 4C và đã bị trừ tồn tại thời điểm đó. Mọi bước duyệt/giao/hoàn thành ở 4D chỉ xử lý trạng thái và log, không trừ lại. Luồng này là lựa chọn nghiệp vụ cụ thể của MạchKit, không phải quy tắc mặc định do framework áp đặt. [S5, S6]

## 3.9. Thiết kế giao diện và tuyến truy cập

| Trang / tuyến | Trình bày / hành vi |
|---|---|
| /LinhKien | Tìm kiếm/bộ lọc và các thẻ linh kiện đang bán |
| /LinhKien/ChiTiet/{slug} | Chi tiết và nút thêm giỏ theo tình trạng đăng nhập/tồn |
| /GioHang | Bảng dòng giỏ, sửa/xóa, cảnh báo và nút tiếp tục đặt |
| /DatHang | Form người nhận, tóm tắt giỏ; xem lại và xác nhận bằng POST |
| /DatHang/ThanhCong/{id} | Thông tin đơn đã lưu của chủ đơn |
| /DonHangCuaToi | Đơn cá nhân, lọc trạng thái, 10 đơn/trang |
| /DonHangCuaToi/ChiTiet/{id} | Chi tiết và các mốc lịch sử không có ghi chú nội bộ |
| /Admin/DonHang | Tìm mã/tên người nhận/điện thoại; lọc trạng thái; 10 đơn/trang |
| /Admin/DonHang/ChiTiet/{id} | Chi tiết, hướng chuyển hợp lệ và nhật ký quản trị |

Xem lại và xử lý lỗi dùng các action/form hiện có, không yêu cầu người dùng tự gõ URL POST. Các trang mới dùng Layout đang có và CSS riêng theo chặng; không thay giao diện trang chủ. Helper 4D hiển thị thời gian UTC theo UTC+7. Hệ thống không tạo thông báo “đã thanh toán” chỉ vì đơn được tiếp nhận. [S2, S4–S6]

## 3.10. Biện pháp bảo vệ và giới hạn

Yêu cầu đăng nhập/role được thực hiện ở Controller; quyền đọc đơn cá nhân được lọc ở SQL qua NguoiDungId. POST có antiforgery. Input của checkout không nhận giá/tổng tiền/tài khoản tùy ý; input quản trị chỉ nhận Id, mã thao tác, trạng thái cũ/mới và ghi chú, không bind toàn bộ DonHang. Các view dùng cơ chế mã hóa văn bản Razor và không đưa ghi chú nội bộ vào trang khách. [S5, S6]

Thông tin người nhận nằm trong phiếu được bảo vệ, vì vậy không log/chia sẻ phiếu và không đưa Data Protection keys lên repository. Cần giữ kho key của môi trường đang dùng để đọc lại phiếu; cấu hình nhiều máy không nằm trong bản này. Mật khẩu Admin dùng User Secrets theo HANDOFF. [S1, S5]

Những giới hạn phải công bố: người có quyền SQL trực tiếp vẫn có thể phá dữ liệu ngoài service; form Admin sửa tồn cũ chưa được sửa chống ghi đè; các phép kiểm thử race checkout không chứng minh an toàn cho mọi luồng điều chỉnh kho; khóa/unique index không có nghĩa không bao giờ gặp bận/deadlock. Không khẳng định kiểm định bảo mật toàn diện, chống mọi tấn công hoặc phục hồi mọi lỗi mạng khi chưa có bằng chứng. [S5, S6]

## 3.11. Kế hoạch nghiệm thu thiết kế

Chặng 4E đối chiếu hai luồng end-to-end: đặt/giao thành công và đặt đơn khác/hủy trước giao. Ghi tồn ngay trước từng đơn để không nhầm mức tồn sau đơn đã hoàn thành. Thử từ chối đầu vào, thiếu tồn, thay đổi giá/giỏ, phiếu bị sửa, hai POST cùng yêu cầu, hai khách cạnh tranh tồn, hủy lặp và chuyển sai bước. Phân quyền được kiểm tra bằng tài khoản khác và URL/POST, không chỉ quan sát menu. [S5, S6]

Các ca rollback có kiểm soát cần backup, chỉ chạy môi trường phát triển và phải bỏ đoạn gây lỗi trước commit. Mất phản hồi tại commit là ca khác; nếu chưa mô phỏng thì ghi chưa thử. Script SQL đối chiếu chỉ cho biết trạng thái dữ liệu lúc chạy, không thay thế thử cạnh tranh, quyền truy cập hoặc ảnh tồn trước/sau. Kết quả được ghi trong [biên bản Tuần 04](../../progress-report/kiem-thu/Tuan-04-Nghiem-thu.md), không tự suy từ phần thiết kế này.

## 3.12. Kết luận chương

Thiết kế đến hết phần lập trình Tuần 04 tái sử dụng danh sách, chi tiết, giỏ và Identity để hình thành luồng đặt và xử lý đơn. Điểm cần nhất quán là trừ tồn tại checkout, không trừ lại khi duyệt, chỉ hoàn một lần khi hủy trước giao và giữ thông tin đơn đã chốt. Nhật ký, mã yêu cầu và quyền theo chủ đơn giúp đối chiếu kết quả và kiểm soát các lần thao tác lặp trong phạm vi service hiện có.

Nghiệm thu vẫn cần kết quả trên source/database thực tế. Bản chương này hoàn tất phần mô tả thiết kế theo nguồn đã đọc, không thay lời xác nhận kiểm thử và không mở rộng phạm vi Tuần 05.

## Nguồn nội bộ và vị trí đối chiếu

- **[S1]** HANDOFF_GPT_MachKit_Tuan03.md và Handoff_GPT_Pro_MachKit.md: đề tài, bộ mẫu, môi trường, phần nền và phạm vi Tuần 03.
- **[S2]** MachKit_Tuan03_Chang3A.zip; MachKit_Tuan03_3B_3C_3D.zip: model GioHangItem, cấu hình giỏ, GioHangService, Controller và trang chi tiết. Vị trí hiện tại: `src/MachKit/Models/GioHangItem.cs`, `Data/Configurations/GioHangItemConfiguration.cs`, `Services/GioHangService.cs`, `Controllers/LinhKienChiTietController.cs`.
- **[S3]** MachKit_Tuan04_Chang4A.zip và MachKit_Tuan04_SuaLoi/AppDbContext.cs: model DonHang/ChiTietDonHang, quan hệ và precision tiền. Phải đối chiếu với cấu hình được giữ trên máy, không thay bằng bản 4A cũ.
- **[S4]** MachKit_Tuan04_Chang4B.zip: biểu mẫu Index, _TomTatGio, CSS và hướng dẫn liên kết giỏ. Controller/ViewModel/XemLai đã được cập nhật bởi 4C.
- **[S5]** MachKit_Tuan04_Chang4C.zip: `Services/DatHangService.cs`, `Controllers/DatHangController.cs`, `ViewModels/DatHangViewModel.cs`, `ViewModels/XacNhanDatHangInput.cs`, `Data/Configurations/DonHangYeuCauConfiguration.cs`, hướng dẫn bổ sung MaYeuCau và kiểm thử 4C.
- **[S6]** MachKit_Tuan04_Chang4D.zip: `Services/DonHangQuanLyService.cs`, `Helpers/DonHang4DHelper.cs`, `Models/LichSuTrangThaiDonHang.cs`, cấu hình nhật ký, Controller/ViewModel quản trị và khách, hướng dẫn/kiểm thử 4D.

Các đường dẫn sau `src/MachKit/` được ghi rút gọn trong danh mục. Chương không dựa vào tên file trống hoặc một trang web thay thế project riêng; các gói nguồn nêu trên đã được mở để đối chiếu nội dung. Giữ chương này trong một file cùng tên khi chỉnh sửa theo góp ý của giảng viên.
