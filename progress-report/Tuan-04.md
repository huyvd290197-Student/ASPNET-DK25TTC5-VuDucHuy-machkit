# Báo cáo tiến độ Tuần 04 — MạchKit

**Sinh viên:** Vũ Đức Huy — DK25TTC5 — MSSV 170125378  
**Đề tài:** Xây dựng website bán linh kiện điện tử hỗ trợ mua theo bộ thực hành  
**Tuần kế hoạch:** 23/11–29/11/2026  
**Ngày biên soạn:** 23/11/2026  
**Ngày kiểm thử/hoàn tất thực tế:** Chưa điền  
**Trạng thái:** 4A–4D đã được sinh viên xác nhận hoàn thành; 4E đang nghiệm thu.

> Đây là bản hợp nhất để cập nhật cùng `progress-report/Tuan-04.md`, không tạo báo cáo tuần trùng. Nếu bản hiện có đã chứa ngày, mã commit, lỗi và bằng chứng thật, giữ các thông tin ấy rồi ghép các mục còn thiếu. Không thay bằng chỗ trống của mẫu này. Tài liệu được đối chiếu các gói mã nguồn trong cuộc trò chuyện, chưa kiểm tra bản sửa riêng trên máy hoặc GitHub trực tiếp.

## 1. Mục tiêu và phần kế thừa

Tuần 04 nối giỏ SQL Server của Tuần 03 với trang nhập người nhận, xác nhận đơn, xử lý tồn và quản lý đơn. Giữ project MachKit, .NET 8, SQL Server .\SQLEXPRESS, database MachKitDb, Identity, giao diện trang chủ và repository hiện tại. Không triển khai thanh toán trực tuyến, trả hàng sau giao hoặc Tuần 05.

## 2. Công việc theo chặng

| Chặng | Kết quả chức năng | Căn cứ trạng thái |
|---|---|---|
| 4A | Hai model DonHang/ChiTietDonHang, cấu hình EF, migration bảng đơn | Sinh viên xác nhận đã kiểm tra thành công |
| 4B | Form người nhận, kiểm tra dữ liệu, tóm tắt từ giỏ hiện tại và xem lại | Sinh viên xác nhận hoàn thành |
| 4C | Lưu đơn/chi tiết, trừ tồn, xóa dòng giỏ trong transaction; phiếu bảo vệ và MaYeuCau | Sinh viên xác nhận hoàn thành |
| 4D | Admin tìm/lọc/phân trang, cập nhật trạng thái, hủy hoàn tồn và nhật ký; lịch sử theo chủ đơn | Sinh viên xác nhận hoàn thành |
| 4E | Kiểm thử tích hợp, đối chiếu dữ liệu, minh chứng, Chương 3, README chung | Cần điền kết quả thực tế và kiểm tra Push |

### 2.1. Dữ liệu đã bổ sung

- 4A: DonHang và ChiTietDonHang; liên kết Identity và LinhKien; tiền decimal(18,0).
- 4C: MaYeuCau nullable trên DonHang; chỉ mục duy nhất UX_DonHang_MaYeuCau với filter khác NULL.
- 4D: LichSuTrangThaiDonHang; MaThaoTac duy nhất; chỉ mục chặn nhiều lần hủy cùng đơn; ràng buộc DaHoanTon.
- Giỏ GioHangItem và RowVersion của Tuần 03 được tái sử dụng. 4E không thêm bảng/cột/migration.

### 2.2. Quy tắc đã thống nhất

Tạo đơn thành công trừ tồn ngay và đặt trạng thái ChoXacNhan. Duyệt/giao/hoàn thành không trừ thêm. Chỉ hủy trước giao; hoàn tồn, đổi trạng thái và nhật ký cùng giao dịch, không xử lý lặp. Giá/tên/mã trong chi tiết đơn là dữ liệu đã chốt. Khách chỉ xem đơn mình, không thấy ghi chú nội bộ. MaYeuCau và MaThaoTac không thay thế nhau. Tiền hàng chưa gồm phí giao, trạng thái đơn chưa xác nhận thanh toán.

## 3. Kiểm thử cuối tuần

Tài liệu thực hiện: [Biên bản nghiệm thu](kiem-thu/Tuan-04-Nghiem-thu.md). Mỗi ca có kết quả thực tế, dữ liệu đầu vào, ngày, phiên bản code và bằng chứng. Có thể viện dẫn kiểm thử 4C/4D đã chạy nếu đúng phiên bản, không tự đánh dấu lại tất cả là đạt.

| Nhóm | Kết quả thực tế / liên kết bằng chứng |
|---|---|
| Build, migrations, dữ liệu nền | Chưa ghi nhận trong bản hợp nhất |
| Từ giỏ → đặt → Admin giao → khách xem | Chưa ghi nhận trong bản hợp nhất |
| Đặt đơn khác → hủy trước giao → hoàn tồn đúng một lần | Chưa ghi nhận trong bản hợp nhất |
| Từ chối đầu vào/phiếu cũ/giá đổi/thiếu tồn | Chưa ghi nhận trong bản hợp nhất |
| Chống gửi trùng và cạnh tranh tồn | Chưa ghi nhận trong bản hợp nhất |
| Quyền khách/Admin/Nhân viên, URL khác chủ | Chưa ghi nhận trong bản hợp nhất |
| Rollback và thử lại cùng yêu cầu | Chưa ghi nhận trong bản hợp nhất |
| Hồi quy Tuần 03 và kiểm tra SQL | Chưa ghi nhận trong bản hợp nhất |

**Tổng hợp sau khi thực hiện:** đạt … / không đạt … / chưa kiểm thử … / có giới hạn được ghi nhận … . Không gán số lượng đạt từ số chức năng có code. Không gọi các phép thử thủ công là bộ kiểm thử tự động.

## 4. Tài liệu và minh chứng

| Hạng mục | Vị trí | Trạng thái cần chốt |
|---|---|---|
| README chung đến Tuần 04 | ../README.md | Đối chiếu, thay bản gốc, cập nhật trạng thái khi đủ bằng chứng |
| Báo cáo Tuần 04 | Tuan-04.md | Giữ lịch sử thật của 4A–4D và bổ sung 4E |
| Kiểm thử | kiem-thu/Tuan-04-Nghiem-thu.md | Điền kết quả, lỗi, ảnh và commit nguồn |
| Chương 3 | ../thesis/doc/Chuong-3-Phan-tich-va-thiet-ke-he-thong.md | Bản nháp phân tích/thiết kế bám code; cần rà soát quy cách trường |
| SQL đối chiếu | ../setup/Kiem-tra-Tuan-04.sql | Chỉ đọc; kết quả chạy do sinh viên lưu |
| Kịch bản demo | Kich-ban-demo-Tuan-04.md | Thực hiện trên dữ liệu thử, không có người nhận thật |

Ảnh dự kiến lưu trong `progress-report/minh-chung/Tuan-04/` (tạo khi có ảnh thật): build; đơn khách; Admin và nhật ký; SQL tồn trước/sau; không xem được đơn khác chủ. Không có ảnh hoặc hash minh chứng giả được tạo trong bộ tài liệu này.

## 5. Khó khăn và cách xử lý đã ghi nhận

| Vấn đề thực sự xuất hiện trong trao đổi | Cách xử lý |
|---|---|
| AppDbContext đóng OnModelCreating sớm khi làm 4A, gây lỗi build | Chuyển cấu hình đơn hàng vào đúng phương thức, giữ nguyên các cấu hình cũ; sinh viên xác nhận 4A đã chạy |
| Hiểu nhầm 11 bước Chặng 3A là toàn bộ Tuần 03 | Bổ sung 3B/3C/3D rồi mới nối 4B vào GioHangService thực tế |
| Terminal không nhận git | Tiếp tục quản lý bằng GitHub Desktop, không tạo repository mới |

Lỗi chỉ là kịch bản dự kiến (ví dụ deadlock/mất kết nối quanh commit) không được tự ghi là đã xảy ra hoặc đã sửa. Các lỗi khác nếu có: …

## 6. Commit và đồng bộ

Lấy hash thật từ History của GitHub Desktop; ngày là ngày commit thực tế. Không cần sửa ngày về thứ tự tuần; không yêu cầu báo cáo tự chứa hash của chính commit đang tạo.

| Mốc | Nội dung commit dự kiến/đối chiếu | Hash thật | Ngày thật | Đã Push? |
|---|---|---|---|---|
| 4A | Tuần 04: Xây dựng mô hình dữ liệu đơn hàng | Chưa điền | Chưa điền | Chưa ghi nhận |
| 4B | Tuần 04: Kết nối trang đặt hàng với giỏ hàng SQL Server | Chưa điền | Chưa điền | Chưa ghi nhận |
| 4C | Tuần 04: Xác nhận đặt hàng và cập nhật tồn kho an toàn | Chưa điền | Chưa điền | Chưa ghi nhận |
| 4D | Tuần 04: Quản lý đơn hàng và lịch sử mua hàng | Chưa điền | Chưa điền | Chưa ghi nhận |
| 4E | Tuần 04: Hoàn thiện kiểm thử, báo cáo và README | Tra History sau commit | Ngày thực tế | Kiểm tra sau Push |

Nếu chức năng đã được commit bằng tên khác, ghi lại commit thật, không tạo commit trống chỉ để khớp bảng.

## 7. Hạn chế còn ghi nhận

Chưa có thanh toán/hoàn tiền, phí giao, trả hàng sau giao, khách tự hủy, mở lại/xóa đơn, email, mã vận đơn, cấu hình nhiều máy. Các bảo vệ áp dụng cho luồng service hiện tại, không thay thế kiểm soát truy cập SQL hoặc giải quyết ghi đè tồn bởi biểu mẫu Admin cũ. Không tự cập nhật MaYeuCau cho đơn cũ. Không bịa nhật ký trước khi cài 4D. Các ca chưa thử, gồm mất kết nối quanh commit nếu chưa mô phỏng, phải ghi rõ.

## 8. Điều kiện chốt Tuần 04

- [ ] Hai luồng tích hợp đặt/giao và đặt/hủy được chạy trên bản sau 4D.
- [ ] Có kết quả kiểm tra nghiệp vụ quan trọng: quyền sở hữu, tồn, gửi lặp, chuyển trạng thái, rollback; các ca còn thiếu được nêu rõ.
- [ ] Không có lỗi nghiêm trọng chưa xử lý làm sai dữ liệu, âm tồn, hoàn trùng, lộ đơn khác chủ hoặc chặn luồng chính.
- [ ] Không còn mã thử gây lỗi trong C#; build lại thành công.
- [ ] README và báo cáo cùng trạng thái; Chương 3 được rà soát; không thêm README tuần.
- [ ] Commit và Push đúng repository/nhánh; kiểm tra trên GitHub.

**Kết luận hiện tại:** Chưa ghi nhận nghiệm thu 4E trong tài liệu này.  
**Kết luận sau kiểm thử:** …  
**Giới hạn/ca chưa kiểm thử được công bố:** …  
**Phần tiếp theo:** Dừng ở mốc Tuần 04. Chỉ bàn kế hoạch tuần sau khi có yêu cầu mới; không triển khai trước.
