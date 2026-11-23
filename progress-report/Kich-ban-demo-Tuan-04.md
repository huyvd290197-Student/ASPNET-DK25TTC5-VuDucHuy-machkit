# Kịch bản demo — MạchKit đến Tuần 04

Dùng localhost và dữ liệu thử. Đây là kịch bản trình diễn, không thay bảng kiểm thử. Ghi trước tài khoản thử, sản phẩm, tồn, giá, Id đơn; không chiếu mật khẩu, token hoặc thông tin người nhận thật. Không chỉnh kho, xóa database hay chạy migration trong lúc demo.

## 1. Trang chủ và điểm khác biệt

Giới thiệu bài toán mua linh kiện hỗ trợ chuẩn bị theo bộ thực hành. Mở trang chủ, nhập số bộ, chỉ ra định mức, số lượng cần, tổng tiền và linh kiện thiếu. Kết quả “3 bộ đủ, 4 bộ thiếu 1 điện trở” chỉ dùng khi kho đúng dữ liệu mẫu 10 LED/7 điện trở/5 breadboard; nếu kho đã thay đổi do các đơn thử, giải thích theo số tồn hiện tại, không sửa giả dữ liệu.

## 2. Tìm linh kiện và giỏ

Mở /LinhKien, tìm tên/mã, lọc và sắp xếp. Mở chi tiết theo slug, cho thấy ảnh riêng/thông số/datasheet nếu có dữ liệu. Đăng nhập khách A, thêm số lượng hợp lệ; thêm vượt tồn phải bị chặn. Giải thích giỏ theo tài khoản, chưa giữ hàng.

## 3. Đặt đơn và giao

Đi từ giỏ → thông tin giả → xem lại → xác nhận. Chỉ ra Id thật, tiền hàng đã chốt, trạng thái chờ; đối chiếu tồn giảm đúng một lần. Admin mở đơn, lần lượt duyệt/giao/hoàn thành; nhật ký có các mốc, tồn không giảm thêm. Khách A thấy cập nhật trong /DonHangCuaToi.

## 4. Hủy một đơn khác

Tạo đơn mới thực sự qua 4C, ghi tồn trước/sau. Hủy khi chưa giao với lý do hợp lệ: tồn cộng lại đúng số đã đặt, đơn và chi tiết còn nguyên. Không thử hủy đơn đã hoàn thành. Tải lại hoặc dùng tab cũ để cho thấy không hoàn hai lần; không gọi F5 là kiểm thử đủ hai POST đồng thời.

## 5. Quyền truy cập và kết thúc

Khách B mở URL đơn A nhận 404; khách thường không được vào quản lý đơn. Chỉ ra README duy nhất, báo cáo Tuần 04, Chương 3 và bảng kết quả đã điền. Nêu giới hạn: chưa thanh toán/hoàn tiền, chưa tính phí giao, không trả hàng sau giao, chưa triển khai nhiều máy. Không mở màn hình hoặc hứa tính năng Tuần 05.

## Dữ liệu cần điền trước buổi trình diễn

| Nội dung | Giá trị thực tế |
|---|---|
| Phiên bản commit demo | Chưa điền |
| Sản phẩm thử / giá / tồn trước | Chưa điền |
| Id đơn giao thành công | Chưa điền |
| Id đơn hủy trước giao | Chưa điền |
| Bằng chứng SQL / ảnh | Chưa điền |
| Các ca không trình diễn trực tiếp nhưng đã có kết quả | Chưa điền |

Nếu phát hiện lỗi khi chạy, giữ kết quả lỗi để đối chiếu, không cập nhật báo cáo là đạt chỉ vì demo cần trôi chảy.
