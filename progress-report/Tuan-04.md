# Báo cáo tiến độ Tuần 04 — MạchKit

## Thông tin chung

| Nội dung | Thông tin |
|---|---|
| Sinh viên thực hiện | **Vũ Đức Huy** |
| Lớp | DK25TTC5 |
| Mã số sinh viên | 170125378 |
| Đề tài | Xây dựng website bán linh kiện điện tử hỗ trợ mua theo bộ thực hành |
| Tên project | `MachKit` |
| Tuần kế hoạch | 23/11–29/11/2026 |
| Ngày cập nhật báo cáo | 23/11/2026 |
| Trạng thái | **Hoàn thành Tuần 04 — Chặng 4A đến 4E** |
| Kết quả nghiệm thu ghi nhận | **38/38 ca đạt trong ma trận kiểm thử Tuần 04** |

> **Căn cứ ghi nhận:** Báo cáo được cập nhật theo xác nhận của sinh viên rằng đã hoàn thành các chặng 4A–4E và đạt toàn bộ nội dung kiểm thử. Kết quả dưới đây thuộc phạm vi kiểm thử trên môi trường phát triển của dự án, không phải kết luận nghiệm thu của giảng viên hoặc cam kết hệ thống không còn lỗi trong mọi tình huống. Ngày cập nhật báo cáo không thay cho ngày, giờ thực hiện từng phép thử hoặc thời điểm commit.

## 1. Mục tiêu và phần kế thừa

Tuần 04 hoàn thiện quy trình mua hàng từ giỏ hàng đến nhập thông tin người nhận, xác nhận đặt hàng, lưu đơn vào SQL Server, cập nhật tồn kho và quản lý trạng thái đơn. Khách hàng theo dõi được đơn của chính mình; Admin/Nhân viên xử lý đơn theo đúng quyền và luồng nghiệp vụ.

Dự án tiếp tục sử dụng ASP.NET Core MVC trên .NET 8, Entity Framework Core, ASP.NET Core Identity, SQL Server Express tại `.\SQLEXPRESS` và database `MachKitDb`. Giỏ hàng SQL Server của Tuần 03, giao diện trang chủ, chức năng dự toán bộ thực hành, tài khoản và repository hiện tại được giữ nguyên.

Phạm vi Tuần 04 kết thúc ở đặt hàng, xử lý đơn, kiểm thử và tài liệu. Không triển khai thanh toán trực tuyến, trả hàng sau giao hoặc công việc của Tuần 05.

## 2. Công việc hoàn thành theo chặng

| Chặng | Nội dung và kết quả | Trạng thái |
|---|---|---|
| **4A — Dữ liệu đơn hàng** | Bổ sung `DonHang`, `ChiTietDonHang`, quan hệ với tài khoản và linh kiện; cấu hình Entity Framework Core và áp dụng migration vào database hiện có. | **Hoàn thành** |
| **4B — Trang đặt hàng** | Kết nối trực tiếp giỏ hiện tại theo tài khoản; nhập và kiểm tra thông tin người nhận; hiển thị tóm tắt và màn hình xem lại. Chỉ xem lại chưa tạo đơn hoặc trừ tồn. | **Hoàn thành** |
| **4C — Xác nhận và tồn kho** | Kiểm tra lại giỏ, giá, trạng thái bán và tồn; lưu đơn, lưu chi tiết, trừ tồn và xóa đúng dòng giỏ trong cùng transaction; bảo vệ phiếu xác nhận và chống gửi trùng bằng `MaYeuCau`. | **Hoàn thành** |
| **4D — Quản lý và lịch sử đơn** | Admin/Nhân viên tìm kiếm, lọc, phân trang, xem chi tiết và cập nhật trạng thái; hủy trước giao, hoàn tồn một lần và ghi nhật ký; khách chỉ xem đơn của chính mình. | **Hoàn thành** |
| **4E — Nghiệm thu và tài liệu** | Hoàn tất kiểm thử tích hợp, hồi quy và đối chiếu SQL; tổng hợp kết quả; rà soát Chương 3, kịch bản demo, báo cáo Tuần 04 và README chính. | **Hoàn thành** |

### 2.1. Dữ liệu và cấu hình đã bổ sung

| Thành phần | Nội dung đã triển khai |
|---|---|
| `DonHang` | Thông tin tài khoản, người nhận, địa chỉ, ngày đặt, trạng thái và tổng tiền; bổ sung `MaYeuCau` cho phép `NULL` để tương thích đơn cũ. |
| `ChiTietDonHang` | Lưu mã, tên, số lượng, đơn giá và thành tiền tại thời điểm đặt; liên kết với đơn và linh kiện. Các trường tiền dùng độ chính xác `decimal(18,0)`. |
| `UX_DonHang_MaYeuCau` | Chỉ mục duy nhất có điều kiện `[MaYeuCau] IS NOT NULL`, phục vụ chống tạo nhiều đơn cho cùng yêu cầu. |
| `LichSuTrangThaiDonHang` | Lưu trạng thái cũ/mới, thời gian, người thực hiện, ghi chú nội bộ, mã thao tác và dấu hiệu hoàn tồn. |
| Các ràng buộc nhật ký | `MaThaoTac` duy nhất; chỉ mục chặn nhiều nhật ký hủy của cùng đơn; ràng buộc `DaHoanTon` tương ứng việc hủy. |
| `GioHangItem` và `RowVersion` | Tái sử dụng từ Tuần 03, không tạo giỏ hàng mới hoặc chuyển sang Session. |

Chặng 4E không thêm bảng, cột hoặc migration. Các migration đã có được giữ nguyên lịch sử, không tạo lại database.

### 2.2. Quy tắc nghiệp vụ đã hoàn thiện

Khi xác nhận đặt hàng thành công, đơn có trạng thái `ChoXacNhan`; tồn kho được trừ ngay. Việc lưu đơn, lưu chi tiết, cập nhật tồn và xóa các dòng giỏ tham gia đơn được xử lý trong cùng giao dịch.

| Trạng thái hiện tại | Trạng thái được chuyển tới | Ảnh hưởng tồn kho ở bước quản lý đơn |
|---|---|---|
| `ChoXacNhan` | `DaXacNhan` hoặc `DaHuy` | Duyệt không đổi tồn; hủy hoàn tồn một lần. |
| `DaXacNhan` | `DangGiao` hoặc `DaHuy` | Chuyển giao không đổi tồn; hủy hoàn tồn một lần. |
| `DangGiao` | `HoanThanh` | Không trừ tồn thêm. |
| `HoanThanh` | Không chuyển tiếp | Không mở lại hoặc hủy. |
| `DaHuy` | Không chuyển tiếp | Không hoàn tồn thêm hoặc mở lại. |

Hủy đơn chỉ được thực hiện trước giao, có lý do hợp lệ; hoàn tồn, đổi trạng thái và ghi nhật ký cùng giao dịch. Đơn và chi tiết được giữ để tra cứu; không tự phục hồi giỏ, đổi tổng tiền thành 0 hoặc mở bán lại sản phẩm đã ngừng bán.

Mã, tên và giá trong chi tiết đơn là thông tin đã chốt, không thay đổi theo giá bán hiện tại. Khách chỉ được xem đơn của mình và không thấy ghi chú nội bộ. `MaYeuCau` dùng cho việc tạo đơn, còn `MaThaoTac` dùng cho xử lý trạng thái; hai mã không thay thế nhau. Tổng tiền hiện là tiền hàng, chưa gồm phí giao; trạng thái đơn không xác nhận việc thanh toán.

## 3. Kết quả kiểm thử cuối tuần

Phạm vi, cách thực hiện và tiêu chí đối chiếu theo [ma trận nghiệm thu Tuần 04](kiem-thu/Tuan-04-Nghiem-thu.md). Kết quả được tổng hợp từ xác nhận của sinh viên sau kiểm thử thủ công, kiểm thử tích hợp và đối chiếu dữ liệu SQL; không ghi nhận đây là một bộ kiểm thử tự động.

### 3.1. Tổng hợp

| Chỉ tiêu | Kết quả ghi nhận |
|---|---:|
| Tổng số ca trong ma trận E01–E38 | **38** |
| Ca cốt lõi đạt | **35/35** |
| Ca bổ sung đạt | **3/3** |
| Tổng số ca đạt | **38/38** |
| Ca không đạt | **0** |
| Ca chưa kiểm thử trong ma trận | **0** |

Không còn lỗi nghiêm trọng được báo cáo là chưa xử lý trong phạm vi nghiệm thu Tuần 04. Kết luận này không mở rộng sang những tình huống ngoài ma trận hoặc môi trường vận hành thực tế chưa được đánh giá.

### 3.2. Hai luồng tích hợp

| Luồng | Kết quả ghi nhận | Đánh giá |
|---|---|---|
| **Đặt hàng → xác nhận → giao → hoàn thành** | Đơn, chi tiết và tổng tiền đúng; trừ tồn khi tạo đơn; xóa đúng dòng giỏ đã đặt; không trừ thêm khi Admin xử lý; khách xem được trạng thái mới. | **Đạt** |
| **Đặt đơn khác → hủy trước giao → gửi lại yêu cầu** | Hoàn đúng số lượng đã đặt một lần; trạng thái và nhật ký nhất quán; gửi lại không hoàn thêm; vẫn giữ đơn, chi tiết và tiền hàng lịch sử. | **Đạt** |

Tồn sau hủy được đối chiếu với tồn ngay trước khi tạo chính đơn bị hủy, không lấy lại số tồn trước các đơn đã hoàn thành khác. Khi đơn có nhiều linh kiện, việc đối chiếu thực hiện theo từng dòng sản phẩm.

### 3.3. Kết quả theo mã kiểm thử

Các trạng thái “Đạt” dưới đây được ghi nhận theo xác nhận hoàn tất toàn bộ của sinh viên; báo cáo không tự đặt Id đơn, số tồn, thời gian chạy hoặc mã minh chứng thay cho dữ liệu thực tế.

| Mã | Nhóm / phép thử theo ma trận nghiệm thu | Kết quả |
|---|---|---|
| E01 | Nền tảng — Build, migrations | Đạt |
| E02 | Hồi quy — Trang chủ, dự toán, ảnh | Đạt |
| E03 | Hồi quy — Danh sách và chi tiết | Đạt |
| E04 | Giỏ — Giỏ theo tài khoản | Đạt |
| E05 | 4B — Bắt buộc đăng nhập / giỏ rỗng | Đạt |
| E06 | 4B — Dữ liệu người nhận sai | Đạt |
| E07 | 4B — Xem lại chưa ghi đơn | Đạt |
| E08 | 4C — Đơn hợp lệ nhiều dòng | Đạt |
| E09 | 4C — Tải lại trang kết quả | Đạt |
| E10 | 4C — Gửi hai POST cùng phiếu | Đạt |
| E11 | 4C — Hai phiếu khác cùng giỏ | Đạt |
| E12 | 4C — Giá thay đổi sau xem lại | Đạt |
| E13 | 4C — Giỏ thay đổi sau xem lại | Đạt |
| E14 | 4C — Thiếu tồn / ngừng bán | Đạt |
| E15 | 4C — Hai khách cạnh tranh tồn | Đạt |
| E16 | 4C — Rollback đặt đơn | Đạt |
| E17 | 4C — Phiếu bị sửa / sai tài khoản | Đạt |
| E18 | 4C — Phiếu hết hạn chưa đặt | Đạt |
| E19 | Quyền — Khách chỉ xem đơn mình | Đạt |
| E20 | Quyền — Khách thường không xử lý Admin | Đạt |
| E21 | Quyền — Admin / Nhân viên | Đạt |
| E22 | Quyền — Token antiforgery | Đạt |
| E23 | 4D — Danh sách và phân trang | Đạt |
| E24 | 4D — Chuỗi duyệt → giao → hoàn thành | Đạt |
| E25 | 4D — Cấm nhảy/lùi/mở lại | Đạt |
| E26 | 4D — Hủy trước giao | Đạt |
| E27 | 4D — Lý do hủy sai | Đạt |
| E28 | 4D — Hủy sản phẩm ngừng bán | Đạt |
| E29 | 4D — Gửi hai POST cùng MaThaoTac | Đạt |
| E30 | 4D — Hai tab hủy khác mã | Đạt |
| E31 | 4D — Cạnh tranh giao và hủy | Đạt |
| E32 | 4D — Rollback hủy | Đạt |
| E33 | 4D — Đơn cũ thiếu MaYeuCau | Đạt |
| E34 | Lịch sử — Thông tin đã chốt / ghi chú | Đạt |
| E35 | Lỗi hệ thống — Mất phản hồi quanh commit | Đạt |
| E36 | SQL — Đối chiếu dữ liệu thật | Đạt |
| E37 | Tích hợp — Hồi quy sau 4D / dữ liệu bền | Đạt |
| E38 | Tài liệu — Chốt code, báo cáo và GitHub | Đạt |

Ca gửi hai POST cùng phiếu được phân biệt với tải lại trang kết quả; rollback trước commit được phân biệt với mất phản hồi quanh commit. Các kết quả này được ghi nhận theo đúng phạm vi từng ca, không suy ra ca này đạt chỉ từ kết quả của ca khác.

## 4. Tài liệu và minh chứng

| Hạng mục | Vị trí trong repository | Kết quả thực hiện |
|---|---|---|
| README chính | [README.md](../README.md) | Đã cập nhật tiến độ đến Tuần 04; tiếp tục sử dụng một README chính ở gốc. |
| Báo cáo tiến độ | `progress-report/Tuan-04.md` | Hoàn thiện báo cáo 4A–4E và kết luận nghiệm thu; cập nhật cùng file, không tạo bản tuần trùng. |
| Ma trận nghiệm thu | [Tuan-04-Nghiem-thu.md](kiem-thu/Tuan-04-Nghiem-thu.md) | Phạm vi E01–E38 đã được xác nhận đạt; là tài liệu đối chiếu cách thử và minh chứng chi tiết. |
| Chương 3 | [Chương 3 — Phân tích và thiết kế hệ thống](../thesis/doc/Chuong-3-Phan-tich-va-thiet-ke-he-thong.md) | Hoàn thiện, rà soát bản nháp theo chức năng và dữ liệu đã triển khai; chưa ghi nhận việc giảng viên phê duyệt. |
| SQL đối chiếu | [Kiem-tra-Tuan-04.sql](../setup/Kiem-tra-Tuan-04.sql) | Đã đối chiếu cấu trúc và dữ liệu trong phạm vi nghiệm thu; script chỉ đọc dữ liệu. |
| Kịch bản demo | [Kịch bản demo Tuần 04](Kich-ban-demo-Tuan-04.md) | Đã rà soát các luồng dự toán, mua hàng, quản lý đơn và phân quyền trong phạm vi Tuần 04. |

Thư mục quy ước lưu ảnh thực tế là `progress-report/minh-chung/Tuan-04/`. Báo cáo không gán tên ảnh, số liệu hoặc mã commit chưa được cung cấp. Minh chứng được sử dụng phải che dữ liệu cá nhân, cookie, token, phiếu xác nhận, mật khẩu và khóa bí mật.

## 5. Khó khăn và cách xử lý đã ghi nhận

| Vấn đề | Cách xử lý | Kết quả |
|---|---|---|
| Dấu ngoặc đóng `OnModelCreating` quá sớm khi bổ sung cấu hình đơn hàng ở 4A, làm build lỗi. | Đưa cấu hình `DonHang` và `ChiTietDonHang` vào đúng phương thức; giữ các cấu hình và dữ liệu cũ. | Đã xử lý; Chặng 4A được kiểm tra thành công. |
| Trước đó nhầm 11 bước của Chặng 3A với toàn bộ Tuần 03, dẫn tới thiếu phần chi tiết và giỏ khi chuyển sang đặt hàng. | Bổ sung đầy đủ 3B, 3C, 3D rồi nối trang đặt hàng vào `GioHangService` thực tế. | Đã hoàn tất phần còn thiếu; không tạo giỏ thứ hai. |
| PowerShell không nhận diện lệnh `git`. | Tiếp tục dùng GitHub Desktop để xem Changes, History, Commit và Push; không tạo lại repository. | Quy trình quản lý mã nguồn tiếp tục thực hiện được; không coi đây là lỗi ứng dụng. |

Không ghi các tình huống mô phỏng như thiếu tồn, rollback hoặc mất phản hồi là lỗi đã phát sinh ngoài kiểm thử. Kết quả nghiệm thu ghi nhận không còn lỗi nghiêm trọng chưa xử lý trong các ca đã kiểm tra.

## 6. Quản lý mã nguồn và đồng bộ

Repository tiếp tục sử dụng: `huyvd290197-Student/ASPNET-DK25TTC5-VuDucHuy-machkit`. Mã nguồn và tài liệu được quản lý theo từng chặng, với nội dung mốc đã thống nhất:

| Mốc | Nội dung commit đối chiếu |
|---|---|
| 4A | `Tuần 04: Xây dựng mô hình dữ liệu đơn hàng` |
| 4B | `Tuần 04: Kết nối trang đặt hàng với giỏ hàng SQL Server` |
| 4C | `Tuần 04: Xác nhận đặt hàng và cập nhật tồn kho an toàn` |
| 4D | `Tuần 04: Quản lý đơn hàng và lịch sử mua hàng` |
| 4E | `Tuần 04: Hoàn thiện kiểm thử, báo cáo và README` |

Việc chốt code, tài liệu và đồng bộ GitHub được ghi nhận đạt trong ca E38 theo xác nhận của sinh viên. Tên, mã SHA, ngày giờ và nhánh của từng commit phải đối chiếu với lịch sử thực tế trong GitHub Desktop/GitHub; bảng trên ghi nội dung các mốc, không tự gán hash hoặc sửa lịch sử để khớp kế hoạch tuần.

**Bản cập nhật báo cáo này là một thay đổi tài liệu cần được Commit và Push sau khi thay file.** Việc ghi nhận E38 cho phiên bản đã nghiệm thu không có nghĩa nội dung báo cáo mới tự xuất hiện trên GitHub.

## 7. Giới hạn và nội dung ngoài phạm vi

Kết quả đạt toàn bộ ma trận không bổ sung những chức năng chưa triển khai. Phạm vi hiện tại vẫn có các giới hạn sau:

- Chưa có thanh toán trực tuyến, hoàn tiền, tính phí giao, mã vận đơn hoặc email tự động. Tổng tiền đơn là tiền hàng; trạng thái xử lý đơn không phải trạng thái thanh toán.
- Chưa cho khách tự hủy, trả hàng sau giao, mở lại hoặc xóa đơn. Đơn cũ thiếu `MaYeuCau` chỉ được xem; không cấp mã giả hoặc dựng lại nhật ký trước khi cài 4D.
- Các bảo vệ dữ liệu áp dụng cho luồng Service đã xây dựng; không thay thế kiểm soát quyền SQL và không ngăn người có quyền quản trị database tự sửa dữ liệu. Chưa bổ sung cơ chế chống ghi đè tồn bởi một biểu mẫu chỉnh kho Admin đã mở từ trước.
- Chưa cấu hình triển khai nhiều máy. Kết quả kiểm thử được giới hạn ở môi trường và ma trận Tuần 04, không suy rộng thành bảo đảm cho mọi điều kiện vận hành.

Đây là những giới hạn được công bố của phiên bản, không phải các ca chưa kiểm thử trong ma trận E01–E38 và không được tự chuyển thành tính năng đã hoàn thành.

## 8. Kết luận Tuần 04

Các điều kiện chốt được ghi nhận hoàn thành theo xác nhận của sinh viên:

- [x] Hoàn thành các chặng 4A, 4B, 4C, 4D và 4E.
- [x] Hai luồng tích hợp đặt/giao và đặt/hủy đạt yêu cầu trên bản đã tích hợp 4D.
- [x] Hoàn thành 38/38 ca E01–E38; không còn ca không đạt hoặc chưa kiểm thử trong ma trận.
- [x] Đối chiếu tồn kho, giỏ, đơn, chi tiết, nhật ký và quyền truy cập đạt yêu cầu.
- [x] Bỏ mã thử gây lỗi, build lại thành công và giữ các chức năng Tuần 03 hoạt động.
- [x] Hoàn thiện tài liệu Tuần 04, rà soát bản nháp Chương 3 và duy trì một README chính.
- [x] Chốt và đồng bộ phiên bản nghiệm thu theo quy trình Commit/Push; bản báo cáo cập nhật này được đưa lên bằng commit tài liệu tiếp theo.

**Kết luận: Hoàn thành toàn bộ Tuần 04 của dự án MạchKit trong phạm vi kế hoạch và ma trận nghiệm thu đã xác nhận.** Hệ thống đã nối được giỏ hàng với đặt hàng, xử lý tồn, quản lý trạng thái và lịch sử đơn theo tài khoản; tài liệu đã được tổng hợp đến mốc Tuần 04.

**Dừng tại Tuần 04. Không triển khai thêm chức năng hoặc công việc thuộc Tuần 05 trong báo cáo này.**

---

**Sinh viên thực hiện:** Vũ Đức Huy — DK25TTC5 — MSSV 170125378.
