# MạchKit | Đề cương đồ án môn ASP.NET

**Tên đăng ký đề xuất:** Xây dựng website bán linh kiện điện tử hỗ trợ lập danh sách mua theo bộ thực hành.  
**Sinh viên:** Vũ Đức Huy · **Lớp:** DK25TTC5 · **MSSV:** 170125378  
**Tên dự án trong mã nguồn:** `MachKit` · **Cơ sở dữ liệu dự kiến:** `MachKit_Huy`  
**Trạng thái:** Đề cương đề xuất, không phải báo cáo kết quả hoặc phiếu phê duyệt của trường.

## 1. Quản lý và nộp đồ án

GitHub dùng để cập nhật công việc hàng tuần; bài cuối kỳ nộp trên hệ thống theo thông báo của lớp. Tên kho: `ASPNET-dk25ttc5-vuduchuy-MachKit`. Tuần đầu mời đúng giảng viên làm Collaborator. Mỗi tuần đẩy ít nhất một commit theo công việc thực tế, cập nhật README và một báo cáo `progress-report/Tuan-0N.md` có minh chứng commit. Việc hoàn thành từng chức năng phải được kiểm tra trên máy trước khi ghi vào báo cáo.

## 2. Bài toán và giới hạn

Một nhóm học cần chuẩn bị cùng một bài thực hành cho nhiều bàn. Người chuẩn bị thường biết số bàn, nhưng không biết tổng số linh kiện theo từng mã hoặc món nào khiến cả nhóm thiếu bộ. Website cho phép mua linh kiện lẻ, đồng thời lập danh sách mua từ số bộ cần thực hành. Ví dụ: 4 bộ dùng 8 điện trở, kho còn 7; hệ thống phải báo thiếu 1 chiếc chứ không chỉ báo chung “hết hàng”. Giá và tồn trong đồ án là dữ liệu minh họa; bộ MK-01 là danh sách hàng để dự toán, không phải hướng dẫn lắp mạch hoàn chỉnh.

**Mục tiêu có thể kiểm chứng:** (1) khách tra được linh kiện lẻ; (2) cấu hình bộ được lưu ở CSDL, không nhân bản sản phẩm; (3) dự toán cho biết số cần, số thiếu, tiền từng dòng, số bộ tối đa; (4) khách nhập một số bộ để thêm toàn bộ thành phần vào giỏ khi đủ; (5) đơn COD ghi nhận đúng giá, không làm tồn âm; (6) quản trị theo dõi đơn và doanh thu đơn hoàn thành.

**Ngoài phạm vi cơ bản:** sàn nhiều cửa hàng, giao vận thật, thanh toán trực tuyến, AI, mã giảm giá, đánh giá, dashboard nâng cao. Không coi những phần cắt khỏi mẫu là yêu cầu đã bị trường miễn; thêm lại khi đề chính thức yêu cầu.

## 3. Tác nhân và ca sử dụng đề xuất

| Mã | Tác nhân | Việc thực hiện | Điều kiện nghiệm thu |
|---|---|---|---|
| MK01 | Khách vãng lai | Tìm linh kiện theo tên/mã và nhóm | Chỉ hiện mặt hàng đang bán. |
| MK02 | Khách vãng lai, khách đã đăng nhập | Chọn bộ và dự toán số lượng | Nhập 3 và 4 bộ mẫu cho kết quả khác nhau; không làm giảm tồn. |
| MK03 | Khách hàng | Thêm món lẻ hoặc thêm cả bộ vào giỏ | Gộp dòng trùng; kiểm tra cả lượng đã có trong giỏ; thiếu một món thì không thêm bất kỳ dòng nào của bộ. |
| MK04 | Khách hàng | Đặt COD và xem đơn của chính mình | Server tính lại tiền/tồn; tạo đơn và chi tiết đồng thời; không xem được đơn của người khác. |
| MK05 | Khách hàng, quản trị viên | Hủy đơn đang chờ xác nhận | Hoàn đúng tồn một lần; hủy lại bị chặn. |
| MK06 | Quản trị viên | Quản lý linh kiện và cấu hình bộ | Một bộ có nhiều dòng; mỗi mã xuất hiện tối đa một lần trong bộ; định mức > 0; có thể ẩn bộ khỏi trang khách. |
| MK07 | Quản trị viên | Xử lý đơn và xem báo cáo | Chờ xác nhận → Đã xác nhận → Đang giao → Hoàn thành. Báo cáo chỉ tính đơn hoàn thành. |

**Xác thực:** dùng ASP.NET Core Identity ở giai đoạn giỏ hàng. Chỉ có vai trò quản trị được sửa cấu hình bộ và trạng thái đơn; khách vãng lai vẫn được dự toán. Vai trò người chuẩn bị bài thực hành là tình huống sử dụng của khách, không phải quyền hệ thống riêng.

## 4. Quy tắc riêng: một bộ không phải một mặt hàng có kho độc lập

Mỗi dòng thành phần gồm mã linh kiện và số lượng cho **một** bộ (định mức). Khi người dùng yêu cầu `n` bộ:

- Cần mua từng mã = `n × định mức`; thiếu = `max(0, cần mua − tồn)`.
- Số bộ tối đa hiện đủ = giá trị nhỏ nhất của `tồn chia nguyên cho định mức` trong danh sách. Không công bố bộ chưa có thành phần. Chỉ chấp nhận số bộ nguyên từ 1 đến 99 cho mỗi lần dự toán.
- Tiền hàng = tổng `cần mua × đơn giá` cho **toàn bộ số lượng yêu cầu**, kể cả khi thiếu. Dự toán không phải báo giá đã giữ hàng. Ở phạm vi đề xuất, phí vận chuyển cố định 0 đồng cho dữ liệu trình diễn; không kết nối hãng vận chuyển.
- Khi thêm bộ vào giỏ, tính cả số của linh kiện đã có trong giỏ. Khi xác nhận đơn, đọc lại giá và tồn từ server, trừ tồn từng mã có điều kiện trong cùng giao dịch; thiếu một dòng thì hoàn tác toàn bộ. Không nhận đơn giá hay tổng tiền từ trình duyệt làm giá chính thức.

| Dữ liệu mẫu MK-01 | Định mức/bộ | Tồn | Giá/chiếc | 3 bộ | 4 bộ |
|---|---:|---:|---:|---:|---:|
| LED đỏ | 2 | 10 | 2.000 đ | Cần 6, đủ | Cần 8, đủ |
| Điện trở 330 Ω | 2 | 7 | 1.000 đ | Cần 6, đủ | Cần 8, thiếu 1 |
| Breadboard mini | 1 | 5 | 15.000 đ | Cần 3, đủ | Cần 4, đủ |
| **Tổng dự toán** | | | | **63.000 đ** | **84.000 đ** |

Một bộ là 21.000 đ. Số bộ tối đa = `min(10/2, 7/2, 5/1)` sau khi lấy phần nguyên = 3. Các số này là dữ liệu học tập do đề xuất đặt ra, không phải giá thị trường.

## 5. Cơ sở dữ liệu và màn hình

**Hai buổi đầu:** `LinhKien(Id, Ma, Ten, Ton, DonGia)`, `BoThucHanh(Id, Ma, Ten)` và `ThanhPhanBo(BoId, LinhKienId, DinhMuc)`. Khóa chính ghép `(BoId, LinhKienId)` ngăn trùng thành phần; mỗi khóa cũng tham chiếu bảng nguồn tương ứng. Các mã là duy nhất trong bảng của chúng; tồn, giá không âm; định mức dương. Cấu trúc: `BoThucHanh 1—n ThanhPhanBo n—1 LinhKien`. Mức tồn chỉ nằm ở `LinhKien`.

**Mở rộng sau mốc 2:** `NhomLinhKien` liên kết 1–n với `LinhKien`; `GioHang` thuộc một tài khoản; `DongGio` duy nhất theo `(GioHangId, LinhKienId)`; `DonHang` lưu người đặt, mã đơn duy nhất, thời điểm, người nhận, trạng thái, tổng tiền; `DongDon` lưu mã linh kiện, số lượng, đơn giá lúc đặt. Giữ dữ liệu đơn khi cấu hình bộ hay giá niêm yết thay đổi. Bảng tài khoản do Identity quản lý; không tạo cột mật khẩu thuần. Trước khi thiết kế báo cáo doanh thu theo ngày hoàn thành, bổ sung `NgayHoanThanh` trên đơn.

**Màn hình riêng:** trang chọn bài thực hành → màn hình dự toán → giỏ linh kiện đã gộp → đặt COD → xác nhận đơn. Phía quản trị: danh sách linh kiện → trình soạn thành phần/định mức bộ → danh sách đơn → báo cáo. Buổi 1–2 chỉ có một bộ mẫu; không có giỏ, thanh toán, tài khoản hoặc trang quản trị trong mã mở đầu.

## 6. Lộ trình và hồ sơ báo cáo

| Mốc | Sản phẩm phải có | Bằng chứng cần lưu |
|---|---|---|
| 1. Máy và MVC | Chạy dự toán đủ/thiếu từ dữ liệu giả lập | Ảnh web yêu cầu 3 và 4 bộ, cùng dữ liệu đầu vào. |
| 2. SQL Server | Ba bảng đúng quan hệ; website đọc từ CSDL | Ảnh danh sách bảng và kết quả truy vấn so với web. |
| 3. Danh mục & quản trị | Đăng nhập/phân quyền; tạo linh kiện và bộ thứ hai qua giao diện; tìm sản phẩm | Ảnh form định mức và thử truy cập quản trị bằng tài khoản khách. |
| 4. Giỏ hàng | Mua lẻ/mua bộ; giỏ gộp đúng; thiếu thì không thêm dở | Ảnh trước/sau khi thêm và ảnh trường hợp bị từ chối. |
| 5. Đơn COD & doanh thu | Đặt, xem, hủy/trả tồn; đổi trạng thái hợp lệ; thống kê | Đối chiếu tồn trước/sau; thử hủy hai lần; khách A không đọc được đơn của B. |
| 6. Hoàn thiện & nộp | Chạy lại từ thư mục sạch, thử tạo DB mới; nén đúng mục hệ thống yêu cầu | Bảng kiểm thử thật, script SQL, dữ liệu, hướng dẫn chạy, báo cáo, minh chứng nộp. |

**Gợi ý viết:** Mở đầu mô tả vấn đề mua cho nhiều bàn. Tổng quan so sánh mua lẻ và mua theo dự án. Lý thuyết chỉ giải thích MVC, dữ liệu quan hệ và giao dịch ở nơi đồ án sử dụng. Chương thiết kế vẽ use case riêng, ERD riêng và sơ đồ trình tự “thêm cả bộ vào giỏ”. Chương kết quả đưa ảnh và số liệu chạy của Huy. Kết luận nêu phần đã làm, chưa làm; không biến tính năng dự kiến thành kết quả. Trình bày, độ dài, thứ tự chương theo quy định môn đang áp dụng.

**Kịch bản demo cuối dự kiến:** Dự toán 4 bộ → chỉ ra thiếu R-330 → giảm còn 3 bộ → thêm vào giỏ → đặt COD → đối chiếu tồn 4 LED, 1 điện trở, 2 breadboard → hủy khi còn chờ xác nhận → tồn trở lại 10/7/5. Ba bước cuối chưa có trong mã mở đầu; đây là tiêu chí để xây tiếp.
