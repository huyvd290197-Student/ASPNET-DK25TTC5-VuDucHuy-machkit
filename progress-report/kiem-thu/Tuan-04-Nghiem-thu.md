# Biên bản nghiệm thu Tuần 04 — Chặng 4E

**Người thực hiện:** Vũ Đức Huy  
**Ngày/giờ thực tế:** Chưa điền (UTC+7)  
**Commit code đang kiểm thử:** Chưa điền; lấy hash thật từ History  
**Thay đổi code chưa commit trong lúc thử:** Chưa ghi nhận  
**Môi trường:** MachKit / .NET 8 / .\SQLEXPRESS / MachKitDb  
**Giới hạn:** mã từ các gói đã cung cấp, không tự suy ra trạng thái máy từ README.

## 1. Cách ghi kết quả

Các cột “Chưa ghi nhận” không phủ nhận thông báo đã xong 4A–4D; chúng chỉ cho biết bản biên bản này chưa có số liệu/ảnh thực tế. Có thể viện dẫn ca Cxx/Dxx đã chạy nếu đúng code và có bằng chứng; chạy lại hai luồng end-to-end sau khi tích hợp 4D. Không bắt làm lại project.

Dùng “Đạt”, “Không đạt”, “Chưa kiểm thử”, hoặc “Không có dữ liệu thử” kèm lý do. Không có tài khoản Nhân viên/không đủ đơn để thử trang 2 không được tự coi là đạt. Ghi mỗi trường hợp nhỏ trong ca gộp; một trường hợp chưa thử thì không ghi cả ca đạt đầy đủ.

Ca Cốt lõi cần có kết quả phù hợp trước khi chốt chức năng liên quan; lỗi tồn, phân quyền, lưu đơn, hoàn trùng hoặc mất dữ liệu phải xử lý. Ca Bổ sung chưa thử được nêu rõ như giới hạn, không gọi “kiểm thử toàn diện 100%”. Không tự gán N/A cho một lỗi thật.

SQL rỗng không chứng minh race condition hay rollback đúng. F5 trang thành công không chứng minh hai POST đồng thời. Lỗi trước COMMIT khác mất kết nối khi COMMIT. Build thành công không phải bộ test tự động.

## 2. Dữ liệu và hai luồng tích hợp bắt buộc

Backup trước các phép thử tạo/hủy hoặc gây lỗi; chỉ dùng localhost/dữ liệu giả. Không dùng tên/mật khẩu tài khoản thật trong ảnh. Không lưu biểu mẫu Admin chỉnh tồn cũ đồng thời với các phép thử cạnh tranh.

| Thông tin | Luồng A: giao thành công | Luồng B: hủy trước giao |
|---|---|---|
| Tài khoản thử (biệt danh A/B, không mật khẩu) | Chưa điền | Chưa điền |
| LinhKienId, mã, giá | Chưa điền | Chưa điền |
| Tồn ngay trước đặt | N = … | M = … (đo lại sau luồng A) |
| Số lượng mua | S = …, S ≤ N | Q = …, Q ≤ M |
| Id đơn tạo thật qua 4C | Chưa điền | Chưa điền |
| Tồn sau đặt (mong đợi) | N − S | M − Q |
| Tồn sau xử lý (mong đợi) | N − S | M |
| Tồn thực tế và ảnh đối chiếu | Chưa điền | Chưa điền |
| Số nhật ký cập nhật mong đợi | 3 mốc duyệt/giao/hoàn thành | 1 log hủy; có thể có log duyệt trước đó |
| Kết quả gửi lại (mong đợi) | Không tạo đơn/trừ tồn thêm | Không hoàn tồn/log hủy thêm |
| Minh chứng thực tế | Chưa điền | Chưa điền |

Đơn B phải là đơn khác, chưa giao. Khi chọn nhiều sản phẩm, ghi N_i/S_i theo từng dòng, không chỉ tổng số chiếc. Nếu dùng cùng sản phẩm cho cả hai luồng, không kỳ vọng kho trở về N sau hủy B vì A đã hoàn thành và vẫn tiêu thụ S.

## 3. Ma trận nghiệm thu

Kết quả và mã minh chứng phải gắn commit code, không gắn hash giả của tài liệu sắp commit.

| Mã | Nhóm / phép thử | Cách làm | Kết quả mong đợi | Mức | Kết quả thực tế / bằng chứng |
|---|---|---|---|---|---|
| E01 | Nền tảng — Build, migrations | Dừng web; build và đối chiếu migrations với SQL; kiểm tra marker gây lỗi đã biết | Build thành công; không còn migration 4A/3C/4C/4D chưa áp dụng; không còn mã thử rollback | Cốt lõi | Chưa ghi nhận |
| E02 | Hồi quy — Trang chủ, dự toán, ảnh | Mở trang chủ; nhập số bộ; kiểm tra ảnh LED riêng | Tính theo tồn thật; 3/4 bộ chỉ so dữ liệu mẫu khi tồn còn đúng mẫu; ảnh upload không bị thay | Cốt lõi | Chưa ghi nhận |
| E03 | Hồi quy — Danh sách và chi tiết | Tìm tên/mã; lọc giá/danh mục/thương hiệu; sắp xếp; mở slug; phân trang khi đủ dữ liệu | Kết quả đúng; giữ bộ lọc; ảnh/thông số/datasheet/liên quan hiển thị theo dữ liệu có | Cốt lõi | Chưa ghi nhận |
| E04 | Giỏ — Giỏ theo tài khoản | Thêm, cộng dồn, sửa, xóa; đăng nhập lại và dùng khách B | Giỏ tồn tại; số dương không vượt tồn lúc thao tác; không lẫn giỏ A/B | Cốt lõi | Chưa ghi nhận |
| E05 | 4B — Bắt buộc đăng nhập / giỏ rỗng | Mở /DatHang chưa đăng nhập; sau đó đăng nhập với giỏ rỗng | Chuyển đăng nhập hoặc quay về giỏ; không tạo đơn | Cốt lõi | Chưa ghi nhận |
| E06 | 4B — Dữ liệu người nhận sai | Tên toàn khoảng trắng, số điện thoại abc123, địa chỉ ngắn; kiểm tra ghi chú quá giới hạn | Báo lỗi ở server; giỏ vẫn hiển thị; không đổi đơn/kho/giỏ | Cốt lõi | Chưa ghi nhận |
| E07 | 4B — Xem lại chưa ghi đơn | Nhập hợp lệ, bấm xem lại và sửa người nhận, chưa xác nhận | Thông tin được giữ khi sửa; sản phẩm/giá được đọc lại; DB chưa tạo đơn hoặc trừ tồn | Cốt lõi | Chưa ghi nhận |
| E08 | 4C — Đơn hợp lệ nhiều dòng | Ghi N_i/G_i, thêm S_i cho ít nhất 2 linh kiện; xác nhận một lần | Một đơn đúng chủ, từng chi tiết đúng; tổng=sum(S_i*G_i); tồn=N_i−S_i; xóa đúng giỏ đã đặt | Cốt lõi | Chưa ghi nhận |
| E09 | 4C — Tải lại trang kết quả | F5 /DatHang/ThanhCong/{id} nhiều lần | Cùng đơn, không trừ tồn/xóa thêm giỏ; không thay thế ca hai POST | Cốt lõi | Chưa ghi nhận |
| E10 | 4C — Gửi hai POST cùng phiếu | Dùng phụ lục A trên đơn thử với cùng PhieuXacNhan | Một MaYeuCau chỉ có một đơn, tồn giảm một lần; nếu bận thì thử lại cùng phiếu | Cốt lõi | Chưa ghi nhận |
| E11 | 4C — Hai phiếu khác cùng giỏ | Mở hai màn hình xem lại cùng tài khoản rồi xác nhận | Không mua cùng giỏ hai lần; yêu cầu sau xem lại giỏ hoặc nhận kết quả phù hợp | Cốt lõi | Chưa ghi nhận |
| E12 | 4C — Giá thay đổi sau xem lại | Đổi giá sản phẩm thử sau khi có phiếu; xác nhận phiếu cũ | Yêu cầu xem lại; không tự đặt giá mới hoặc dùng giá cũ; giỏ giữ nguyên | Cốt lõi | Chưa ghi nhận |
| E13 | 4C — Giỏ thay đổi sau xem lại | Sửa số lượng/thêm/xóa ở tab khác rồi gửi phiếu cũ | Từ chối phiếu cũ; không tạo đơn/trừ tồn do checkout | Cốt lõi | Chưa ghi nhận |
| E14 | 4C — Thiếu tồn / ngừng bán | Giảm tồn dưới lượng mua hoặc ngừng bán sau xem lại; thử riêng từng tình huống | Không ghi đơn hoặc thay đổi tồn/giỏ do checkout; hiển thị thông báo | Cốt lõi | Chưa ghi nhận |
| E15 | 4C — Hai khách cạnh tranh tồn | Hai profile A/B; sản phẩm thử tồn 5, mỗi khách đặt 4 sau khi cùng xem lại | Chỉ một đơn mua 4; tồn 1; người kia không mua thêm 4, giỏ của họ còn | Cốt lõi | Chưa ghi nhận |
| E16 | 4C — Rollback đặt đơn | Theo phụ lục B hoặc viện dẫn kết quả C15 cùng code; ghi DB trước/sau | Lỗi trước commit không để tồn đổi một phần; không thêm đơn/chi tiết, không mất giỏ | Cốt lõi | Chưa ghi nhận |
| E17 | 4C — Phiếu bị sửa / sai tài khoản | Đổi một ký tự phiếu hoặc thử phiếu A trong phiên B bằng form/token của B trên localhost | Bị từ chối; không tạo đơn và không lộ người nhận | Cốt lõi | Chưa ghi nhận |
| E18 | 4C — Phiếu hết hạn chưa đặt | Giữ một phiếu chưa đặt quá hạn 20 phút rồi gửi; không sửa đồng hồ máy | Yêu cầu xem lại; không đặt đơn. Phiếu đã thành công vẫn tra đơn cũ khi gửi lại | Bổ sung | Chưa ghi nhận |
| E19 | Quyền — Khách chỉ xem đơn mình | Khách B mở cả /DatHang/ThanhCong/{idA} và /DonHangCuaToi/ChiTiet/{idA} | 404 ở cả hai; lịch sử B không liệt kê đơn A | Cốt lõi | Chưa ghi nhận |
| E20 | Quyền — Khách thường không xử lý Admin | Thử GET Admin; thử POST xử lý với phiên khách và antiforgery của khách trên localhost | AccessDenied/403 theo cấu hình; không đổi trạng thái/kho/log; không chỉ kiểm tra ẩn menu | Cốt lõi | Chưa ghi nhận |
| E21 | Quyền — Admin / Nhân viên | Dùng Admin và NhanVien đã được cấp role để xem, xử lý đơn thử | Hai role được phép; không tự cấp role từ biểu mẫu; thiếu tài khoản Nhân viên thì ghi chưa thử | Cốt lõi | Chưa ghi nhận |
| E22 | Quyền — Token antiforgery | Thử POST xác nhận và cập nhật trạng thái thiếu/sai token trên localhost | Bị từ chối, không ghi DB; không tắt ValidateAntiForgeryToken để thử | Cốt lõi | Chưa ghi nhận |
| E23 | 4D — Danh sách và phân trang | Tìm mã/tên/điện thoại, lọc trạng thái; chuyển trang khi >10 kết quả phù hợp | Đúng tập; giữ tham số; tối đa 10 đơn/trang; không tạo đơn giả bằng SQL | Cốt lõi | Chưa ghi nhận |
| E24 | 4D — Chuỗi duyệt → giao → hoàn thành | Dùng đơn A, lần lượt DaXacNhan/DangGiao/HoanThanh | Mỗi bước một log; tiền và tồn giữ như sau đặt; khách thấy mốc mới | Cốt lõi | Chưa ghi nhận |
| E25 | 4D — Cấm nhảy/lùi/mở lại | Sửa POST thành chuyển không hợp lệ; thử đơn đã hoàn thành | Bị chặn ở server; trạng thái/kho/log không đổi | Cốt lõi | Chưa ghi nhận |
| E26 | 4D — Hủy trước giao | Tạo đơn khác qua 4C; hủy khi chờ, rồi thử đơn khác khi đã xác nhận | Mỗi đơn được hoàn tồn một lần; trạng thái/log cùng lưu; tiền/chi tiết còn, giỏ không phục hồi | Cốt lõi | Chưa ghi nhận |
| E27 | 4D — Lý do hủy sai | Trống, chỉ khoảng trắng, dưới 5 hoặc quá 500 ký tự | Từ chối; không đổi trạng thái/tồn/log | Cốt lõi | Chưa ghi nhận |
| E28 | 4D — Hủy sản phẩm ngừng bán | Sau đặt, ngừng bán sản phẩm rồi hủy đơn trước giao | Cộng tồn, giữ NgungBan=true; không tự mở bán | Cốt lõi | Chưa ghi nhận |
| E29 | 4D — Gửi hai POST cùng MaThaoTac | Phụ lục C; cùng đơn, cùng dữ liệu hủy và cùng mã | Một log hủy, tồn cộng một lần; không suy từ HTTP 200 | Cốt lõi | Chưa ghi nhận |
| E30 | 4D — Hai tab hủy khác mã | Mở đơn ở hai tab trước hủy; lưu tab 1 rồi tab 2 | Tab cũ bị chặn; không hoàn lần hai | Cốt lõi | Chưa ghi nhận |
| E31 | 4D — Cạnh tranh giao và hủy | Hai profile xử lý cùng đơn đã xác nhận; một giao, một hủy; thử hai thứ tự | Không vừa giao vừa hủy; kết quả theo trạng thái được commit trước; tồn đúng | Cốt lõi | Chưa ghi nhận |
| E32 | 4D — Rollback hủy | Phụ lục D hoặc kết quả D21 phù hợp code; lỗi trước commit sau hoàn tồn | Trạng thái/kho/log trở lại trước thử; xóa code thử và build lại | Cốt lõi | Chưa ghi nhận |
| E33 | 4D — Đơn cũ thiếu MaYeuCau | Chỉ thử nếu thực sự có đơn cũ; không xóa mã của đơn hợp lệ để dựng ca | Chỉ xem, không tự hoàn tồn; không có dữ liệu cũ thì ghi chưa có dữ liệu thử | Bổ sung | Chưa ghi nhận |
| E34 | Lịch sử — Thông tin đã chốt / ghi chú | Đổi giá/tên hiện tại sau đặt; khách xem chi tiết; ghi chú nội bộ thử <b>ghi chú kiểm thử</b> | Tên/mã/giá đơn giữ nguyên; nội bộ không nằm trong HTML khách; chuỗi ghi chú encode | Cốt lõi | Chưa ghi nhận |
| E35 | Lỗi hệ thống — Mất phản hồi quanh commit | Chỉ dùng môi trường thử kiểm soát được; giữ cùng phiếu/mã thao tác để đối chiếu | Không tạo đơn/hoàn tồn trùng; chưa mô phỏng được thì ghi chưa thử, không lấy rollback thay thế | Bổ sung | Chưa ghi nhận |
| E36 | SQL — Đối chiếu dữ liệu thật | Chạy setup/Kiem-tra-Tuan-04.sql sau hai luồng; chọn Id đúng; so với ảnh trước/sau | Đủ schema; có đơn thử; không có dấu hiệu sai nghiêm trọng; thông tin cũ được giải thích | Cốt lõi | Chưa ghi nhận |
| E37 | Tích hợp — Hồi quy sau 4D / dữ liệu bền | Đặt thêm đơn mới, kiểm tra 2 danh sách; dừng/chạy lại, đăng nhập lại, mở giỏ/lịch sử | Checkout, giỏ, Identity, ảnh, lịch sử vẫn đúng; không tạo lại database | Cốt lõi | Chưa ghi nhận |
| E38 | Tài liệu — Chốt code, báo cáo và GitHub | Rà marker; build lại; điền ca thật; đồng nhất README/report; Commit, Push và xem trên nhánh | Một README chính; hash thật; không secrets/mã thử; không triển khai Tuần 05 | Cốt lõi | Chưa ghi nhận |

## 4. Lỗi và phép thử lại

| Mã lỗi / ca | Thực tế khác mong đợi | Mức ảnh hưởng | File/commit sửa | Kết quả thử lại |
|---|---|---|---|---|
| Chỉ thêm khi có lỗi thật | Chưa ghi nhận | Chưa ghi nhận | Chưa điền | Chưa ghi nhận |

Không nhập sẵn “Không có lỗi” khi chưa chạy. Không sửa tồn, tổng tiền, MaYeuCau hoặc log bằng SQL để che lỗi. Giữ số liệu trước/sau để xác định chỗ sai.

## 5. Danh mục minh chứng dự kiến

Đường dẫn ảnh, khi đã chụp thật: `../minh-chung/Tuan-04/`. Dùng tên dễ đối chiếu như `01-build.png`, `02-don-giao.png`, `03-nhat-ky.png`, `04-huy-ton-truoc.png`, `05-huy-ton-sau.png`, `06-khach-khac-404.png`, `07-sql-doi-chieu.png`. Đây chỉ là tên dự kiến, không phải file đã có. Không tạo ảnh giả từ kết quả mong đợi.

Không chứa token/cookie/phiếu, keys, mật khẩu, địa chỉ hoặc điện thoại người nhận thật. Các mã yêu cầu được dùng đối chiếu cục bộ, không cần đăng toàn bộ ảnh form ẩn lên repo.

## 6. Kết luận để điền sau khi kiểm thử

- Tổng ca đã ghi nhận: … / 38.
- Đạt: …; không đạt: …; chưa kiểm thử/thiếu dữ liệu: … .
- Lỗi nghiêm trọng còn mở: … .
- Ca 4C/4D được viện dẫn kèm commit/bằng chứng: … .
- Mất phản hồi quanh commit: đã thử / chưa thử; mô tả: … .
- Điều kiện nghiệp vụ đã đạt và giới hạn chấp nhận công bố: … .
- Kết luận: chưa chốt / chốt chức năng Tuần 04 với phạm vi và giới hạn ghi trên.
- Người kiểm thử/ngày thực tế: … .

Đừng thay các chỗ trống bằng số liệu dự đoán. Sau khi sửa code để xử lý lỗi, chạy lại ca liên quan và hồi quy hai luồng chính rồi ghi commit đã kiểm thử.

## Phụ lục: cách thử các ca sâu đã có ở 4C–4D

Các đoạn dưới được tập hợp từ hướng dẫn các chặng trước, không cài thêm chức năng ứng dụng. Chỉ dùng localhost, dữ liệu thử và backup. Có bằng chứng phù hợp từ 4C/4D thì viện dẫn thay vì gây lỗi lại.

### A. Hai POST đặt cùng phiếu; hai khách cạnh tranh tồn
### C03 — kiểm thử gửi trùng đúng CÙNG phiếu (chỉ localhost, dữ liệu thử)

F5 trang thành công chỉ kiểm tra luồng redirect, chưa chứng minh hai POST đồng thời.
Thử mạnh hơn: ở màn hình XemLai có nút Xác nhận đặt hàng, mở F12 -> Console.
Đoạn dưới gửi HAI yêu cầu đặt đơn thử thực sự, không chạy trên website bán hàng thật.
Không sao chép phiếu/địa chỉ trong DevTools lên GitHub.

```javascript
(async () => {
    if (location.hostname !== 'localhost' && location.hostname !== '127.0.0.1') {
        throw new Error('Chỉ dùng trên localhost của dự án thử nghiệm.');
    }
    const form = [...document.forms].find(f => new URL(f.action).pathname.endsWith('/DatHang/XacNhan'));
    if (!form) throw new Error('Hãy mở đúng trang Xem lại có nút Xác nhận đặt hàng.');
    const results = await Promise.all([1, 2].map(async lan => {
        const response = await fetch(form.action, {
            method: 'POST',
            body: new FormData(form),
            credentials: 'same-origin'
        });
        return { lan, status: response.status, url: response.url };
    }));
    console.table(results);
})();
```

SQL chỉ được có một đơn/mã yêu cầu, tồn giảm một lần. Thường hai URL trả về cùng ThanhCong/id.
Nếu một yêu cầu gặp bận và trả trang ThuLai, bấm nút xác nhận cũ để kiểm tra lại cùng phiếu;
không được tạo thêm đơn. Nếu localhost có PathBase, dùng URL thực tế và chọn đúng form.

### C10 — hai tài khoản cùng mua phần tồn còn lại

Dùng hai cửa sổ/profiles trình duyệt đăng nhập A và B riêng biệt.
Chọn sản phẩm thử tồn 5; mỗi tài khoản có 4 trong giỏ và cùng mở XemLai trước khi gửi.
Bấm xác nhận gần đồng thời. Kết quả: chỉ một đơn mua 4; tồn còn 1; người còn lại bị chặn
hoặc được yêu cầu thử lại, sau thử lại vẫn không mua thêm 4. Đối chiếu SQL.
Không đồng thời chỉnh tồn bằng Admin trong phép thử này.


### B. Rollback khi đặt

### C15 — rollback có kiểm soát, CHỈ máy phát triển và đã sao lưu

Đây là phép thử nâng cao tạo một lỗi có chủ đích, không phải code cần giữ lại.
Không làm trên ứng dụng đang phục vụ người dùng thật.

1. Dừng web, ghi lại số đơn/chi tiết, tồn và giỏ trước thử.
2. Trong Services/DatHangService.cs, tìm DUY NHẤT dòng:
   `var nguoiNhan = phieu.NguoiNhan;`
   Nó nằm sau vòng ExecuteUpdateAsync trong GhiDonMotLanAsync.
3. Tạm chèn ngay trước dòng đó:

```csharp
if (_db.Database.ProviderName is not null)
    throw new InvalidOperationException("TEST_ROLLBACK_4C");
```

4. Build/run, tạo phiếu mới và gửi một đơn hợp lệ. Lúc này dự kiến có lỗi ứng dụng.
   Transaction đã cập nhật tồn nhưng chưa commit sẽ bị dispose/rollback.
5. SQL phải cho thấy không thêm đơn/chi tiết, tồn và giỏ giữ như trước khi thử.
6. DỪNG web; XÓA HAI DÒNG THỬ vừa chèn, lưu, build/run lại và thử đơn hợp lệ.
7. Tìm `TEST_ROLLBACK_4C` trong file C# để chắc đã xóa; không commit code gây lỗi này.

Đây không phải một endpoint hoặc cờ thử lỗi được cài sẵn trong gói phát hành.


### C. Hai yêu cầu hủy cùng mã

KIỂM THỬ GỬI HAI POST CÙNG MÃ THAO TÁC — CHỈ TRÊN WEBSITE LOCAL CỦA BẠN

Đây là phép thử CÓ HỦY một đơn thử thật và hoàn tồn. Không chạy trên dữ liệu cần giữ.
Đã backup; đơn còn Chờ xác nhận/Đã xác nhận, tạo bằng 4C. Ghi lại tồn trước thử.

1. Đăng nhập Admin/NhanVien, mở chi tiết một ĐƠN THỬ.
2. Chọn trạng thái Đã hủy, nhập lý do đủ 5 ký tự. CHƯA nhấn Lưu.
3. F12 → Console. Đọc đoạn sau trước khi chạy. Không gửi token/cookie cho người khác.
4. Chạy đoạn này. Nó chỉ gửi lại form cùng trang, tới đúng endpoint cục bộ.

```javascript
(async () => {
    if (!['localhost', '127.0.0.1'].includes(location.hostname)) {
        throw new Error('Chỉ chạy phép thử trên localhost của bạn.');
    }
    const form = Array.from(document.forms).find(f => {
        const u = new URL(f.action, location.href);
        return u.origin === location.origin && /\/Admin\/DonHang\/CapNhatTrangThai\/?$/i.test(u.pathname);
    });
    if (!form) throw new Error('Không thấy form xử lý đơn đang mở.');
    const first = new FormData(form);
    const second = new FormData(form); // cùng MaThaoTac, không tạo lại mã
    if (first.get('TrangThaiMoi') !== 'DaHuy' || String(first.get('GhiChu') || '').trim().length < 5) {
        throw new Error('Chọn Đã hủy và nhập lý do ít nhất 5 ký tự trước.');
    }
    if (!confirm('Gửi 2 yêu cầu hủy cùng mã cho ĐƠN THỬ này?')) return;
    const results = await Promise.all([
        fetch(form.action, { method: 'POST', body: first, credentials: 'same-origin' }),
        fetch(form.action, { method: 'POST', body: second, credentials: 'same-origin' })
    ]);
    console.table(results.map((r, i) => ({request: i + 1, status: r.status, redirected: r.redirected})));
    alert('Đã gửi. Tải lại trang rồi kiểm tra nhật ký và tồn bằng SQL, không suy ra thành công chỉ từ mã HTTP.');
})();
```


5. Tải lại trang. Cần thấy Đã hủy, CHỈ MỘT dòng nhật ký hủy.
6. Tồn chỉ cộng đúng số lượng chi tiết một lần. Không tăng thêm khi F5.
7. Dùng KIEM_TRA_4D.sql đối chiếu. HTTP 200 riêng lẻ không chứng minh đúng nghiệp vụ.

Phép thử HAI MÃ khác nhau: với đơn thử khác, mở chi tiết ở hai tab (mỗi GET sinh mã mới),
chọn hủy ở cả hai. Lưu tab thứ nhất rồi lưu tab còn lại. Tab sau phải báo trạng thái đã đổi,
không sinh thêm nhật ký hủy hoặc hoàn thêm tồn.

### D. Rollback khi hủy

## Rollback có kiểm soát — chỉ máy phát triển đã sao lưu

Không có mã thử gây lỗi trong bộ nguồn giao. Chỉ thêm tạm vào Services/DonHangQuanLyService.cs.
Tìm CUỐI CapNhatMotLanAsync, đúng chỗ:

```csharp
await _db.SaveChangesAsync(ct);
await tx.CommitAsync(ct);
```

Tạm chèn ở GIỮA hai dòng, không ở nhánh 'previous is not null':

```csharp
if (Environment.GetEnvironmentVariable("MACHKIT_TEST_FAIL_BEFORE_COMMIT") == "1")
    throw new InvalidOperationException("TEST 4D: dừng trước COMMIT để kiểm tra rollback.");
```

Dừng web, build, trong chính Terminal chạy:

```powershell
$env:MACHKIT_TEST_FAIL_BEFORE_COMMIT = "1"
dotnet run --launch-profile http
```

Ghi tồn trước; hủy một đơn thử (trạng thái cho phép). Dự kiến request lỗi 500 ở môi trường Development.
Đây là lỗi cố ý, không được coi là bản chạy xong. Dùng SSMS kiểm tra: tồn trở về trước hủy,
trạng thái vẫn như cũ, không có nhật ký mới. Do lỗi xảy ra trước COMMIT trong transaction,
tất cả thay đổi hủy phải rollback.

Dừng web, bỏ đoạn code thử, xóa biến môi trường trong cùng Terminal:

```powershell
Remove-Item Env:MACHKIT_TEST_FAIL_BEFORE_COMMIT -ErrorAction SilentlyContinue
dotnet build
dotnet run --launch-profile http
```

Thử hủy lại bình thường và xác nhận thành công. Không commit đoạn gây lỗi/ảnh dữ liệu riêng.
Rollback thử này khác mất kết nối ngay tại COMMIT: không đánh dấu D27 chỉ vì đã làm D21.



### Dọn môi trường sau ca gây lỗi

Xóa đúng đoạn thử đã chèn trong Services, không xóa catch/throw nghiệp vụ. Bỏ biến thử trong Terminal bằng `Remove-Item Env:MACHKIT_TEST_FAIL_BEFORE_COMMIT -ErrorAction SilentlyContinue`, dừng rồi build/chạy lại. Tìm marker trong file .cs để đối chiếu, không tìm cả hướng dẫn vì hướng dẫn có chứa ví dụ marker hợp lệ. Không commit đoạn thử lỗi.

### Nguồn đối chiếu

Gói 4C: HUONG_DAN_CHANG_4C.md, DatHangService.cs. Gói 4D: KIEM_THU_CHANG_4D.md, TEST_HUY_LAP_TRONG_TRINH_DUYET.txt, DonHangQuanLyService.cs. Các mục trên là quy trình/mong đợi, không phải biên bản đã thực hiện.
