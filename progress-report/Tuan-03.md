# BÁO CÁO TIẾN ĐỘ TUẦN 03 – MẠCHKIT

**Sinh viên:** Vũ Đức Huy – DK25TTC5 – 170125378.  
**Khoảng thời gian kế hoạch:** 16/11–22/11/2026.  
**Ngày rà soát/bổ sung:** 23/11/2026.  
**Trạng thái lúc lập tài liệu:** Chặng 3A đã được sinh viên xác nhận; Chặng 3B–3D đang được bổ sung và chưa ghi nhận kết quả chạy thử.

## 1. Điều chỉnh thông tin tiến độ

“Hoàn thành 11 bước” trước đây là hoàn thành 11 bước hướng dẫn Chặng 3A, không đồng nghĩa đã xong toàn bộ Tuần 03. Không dùng README cũ làm bằng chứng thay cho mã nguồn và kiểm thử.

Chặng 4A (hai bảng đơn hàng) đã được sinh viên xác nhận hoàn thành; giữ nguyên. Tạm dừng việc triển khai thêm Chặng 4B để hoàn tất phần còn thiếu của Tuần 03. Ngày commit phải là ngày thực hiện thật.

## 2. Công việc và trạng thái

- [x] 3A: Danh sách SQL Server, tìm tên/mã, lọc danh mục/thương hiệu/giá, sắp xếp, phân trang; theo xác nhận của sinh viên.
- [ ] 3B: Chi tiết theo slug, ảnh, giá, tồn, mô tả, thông số, datasheet và sản phẩm liên quan.
- [ ] 3C: Giỏ hàng lưu SQL Server theo tài khoản; thêm/cập nhật/xóa và kiểm tra tồn.
- [ ] Kiểm thử đăng nhập, quyền sở hữu giỏ và lưu giỏ sau đăng xuất/khởi động lại.
- [ ] 3D: Rà soát Chương 2; hoàn thiện báo cáo này và README duy nhất.
- [ ] Commit và Push các thay đổi thực tế.

Chỉ đổi `[ ]` thành `[x]` sau khi chức năng tương ứng đã chạy đúng trên máy.

## 3. Thiết kế bổ sung

Trang chi tiết sử dụng Controller riêng và dữ liệu `LinhKien.ThongSos`, `DatasheetUrl` đang có. Không thay thế Controller danh sách hoặc trang chủ.

Giỏ lưu trong bảng `GioHangItem`: một dòng cho mỗi cặp tài khoản – linh kiện. Migration dự kiến `Tuan03_BoSungGioHang` được tạo sau migration 4A theo đúng thứ tự thời gian. Giỏ không tạo đơn, không giữ hoặc trừ tồn kho.

## 4. Bảng kiểm thử thực tế

| Mã | Thao tác | Mong đợi | Kết quả thực tế/ảnh minh chứng |
|---|---|---|---|
| T01 | Build project | 0 lỗi | Chưa kiểm thử |
| T02 | Mở sản phẩm từ danh sách | URL dùng slug; đúng mã/tên/ảnh/giá | Chưa kiểm thử |
| T03 | Mở slug không tồn tại / sản phẩm ngừng bán | HTTP 404 | Chưa kiểm thử |
| T04 | Xem sản phẩm có thông số | Đủ tên, giá trị, đơn vị | Chưa kiểm thử |
| T05 | Xem sản phẩm chưa có mô tả/ảnh/thông số | Thông báo phù hợp, trang không lỗi | Chưa kiểm thử |
| T06 | Mở datasheet HTTP/HTTPS hợp lệ | Tab mới; rel=noopener noreferrer | Chưa kiểm thử |
| T07 | Datasheet có giao thức không cho phép | Không tạo liên kết bấm | Chưa kiểm thử |
| T08 | Xem linh kiện liên quan | Cùng danh mục, không gồm sản phẩm hiện tại/ngừng bán | Chưa kiểm thử |
| T09 | Chưa đăng nhập, mở giỏ | Chuyển tới đăng nhập | Chưa kiểm thử |
| T10 | Chưa đăng nhập, bấm thêm ở chi tiết | Đăng nhập rồi quay về chi tiết; bấm thêm lại | Chưa kiểm thử |
| T11 | Thêm 2, thêm tiếp 1 cùng sản phẩm | Một dòng, số lượng 3 | Chưa kiểm thử |
| T12 | Giỏ có q; thêm n khiến q+n > tồn | Bị chặn, số lượng cũ giữ nguyên | Chưa kiểm thử |
| T13 | Cập nhật số lượng hợp lệ | Đúng số lượng mới và tạm tính | Chưa kiểm thử |
| T14 | Gửi số lượng 0/âm/vượt tồn (bỏ kiểm tra HTML để thử máy chủ) | Máy chủ từ chối, không lưu sai | Chưa kiểm thử |
| T15 | Xóa một dòng rồi xóa dòng cuối | Đúng dòng bị xóa; có màn hình giỏ trống | Chưa kiểm thử |
| T16 | Đăng xuất/đăng nhập cùng tài khoản | Giỏ còn dữ liệu | Chưa kiểm thử |
| T17 | Dừng/chạy lại website | Giỏ còn trong SQL Server | Chưa kiểm thử |
| T18 | Đăng nhập tài khoản B | Không thấy giỏ của A | Chưa kiểm thử |
| T19 | B thử cập nhật/xóa ID dòng giỏ của A | Không thay đổi giỏ của A | Chưa kiểm thử |
| T20 | Admin giảm tồn hoặc ngừng bán sản phẩm trong giỏ | Giỏ cảnh báo, không cho tăng sai; vẫn xóa được | Chưa kiểm thử |
| T21 | Gửi POST không có token chống giả mạo hợp lệ | Yêu cầu bị từ chối | Chưa kiểm thử |
| T22 | Kiểm tra dữ liệu sau thao tác giỏ | Tồn kho không giảm; không sinh đơn hàng | Chưa kiểm thử |
| T23 | Mở trang chủ, Admin và kiểm tra tìm/lọc 3A | Các phần cũ còn hoạt động | Chưa kiểm thử |
| T24 | Kiểm tra git/đồng bộ bằng GitHub Desktop | File và commit thực tế đã Push | Chưa kiểm thử |

## 5. Khó khăn và xử lý

Nhầm 11 bước của Chặng 3A thành toàn bộ Tuần 03: sửa tiến độ, bổ sung 3B–3D, chưa xác nhận hoàn thành trước kiểm thử.

PowerShell không nhận lệnh `git`: tiếp tục commit/push bằng GitHub Desktop đang dùng; đây không phải lỗi mã nguồn ứng dụng.

Từng đặt cấu hình EF ngoài `OnModelCreating`: phần giỏ dùng file cấu hình riêng; chỉ thêm một dòng gọi cấu hình ngay sau `base.OnModelCreating(b)`.

## 6. Commit thực tế

| Phần | Nội dung commit đề xuất | Mã commit thực tế | Ngày thực hiện |
|---|---|---|---|
| 3B | Tuần 03: Hoàn thiện trang chi tiết linh kiện | Chưa ghi | Chưa ghi |
| 3C | Tuần 03: Xây dựng giỏ hàng và kiểm tra tồn kho | Chưa ghi | Chưa ghi |
| 3D | Tuần 03: Cập nhật báo cáo tiến độ và Chương 2 | Chưa ghi | Chưa ghi |

Ghi SHA thật của 3B/3C sau khi commit. Không điền một SHA tự bịa cho chính commit tài liệu chưa được tạo; mã commit tài liệu có thể tra trong History sau khi commit hoặc bổ sung ở lần cập nhật kế tiếp.

## 7. Kết luận và việc tiếp theo

Chỉ khi các ca bắt buộc đạt và tài liệu đã rà soát mới ghi “Hoàn thành Tuần 03”. Sau đó tiếp tục đúng Tuần 04 từ Chặng 4B, tái sử dụng giỏ hàng đã kiểm thử; không triển khai vượt Tuần 04.
