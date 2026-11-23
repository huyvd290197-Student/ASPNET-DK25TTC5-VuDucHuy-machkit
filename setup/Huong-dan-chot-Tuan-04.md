# MạchKit — Hướng dẫn Chặng 4E
## Kiểm thử tổng thể, báo cáo và một README duy nhất

Điểm bắt đầu: sinh viên đã xác nhận hoàn tất Tuần 03 và 4A–4D. Gói này chỉ có tài liệu và SQL đọc dữ liệu, không chứa controller/model/service/migration mới. Không làm lại project, database, Identity, trang chủ hoặc chức năng đã chạy; không làm trước Tuần 05.

Nguồn: các gói 3A, 3B–3D, 4A, bản AppDbContext sửa 4A, 4B, 4C và 4D trong cuộc trò chuyện. Không khẳng định đã truy cập source mới nhất trên máy/GitHub. Bảng kết quả chưa được tự điền.

## 1. Chép tài liệu vào gốc repository

Giải nén gói vào thư mục tạm. Gốc đích là `ASPNET-DK25TTC5-VuDucHuy-machkit`, KHÔNG phải `src/MachKit`.

| File trong gói | Cách áp dụng |
|---|---|
| README.md | Đối chiếu nội dung riêng rồi thay cùng README.md ở gốc; không đổi tên sang README_Tuan_04.md |
| progress-report/Tuan-04.md | Nếu đã có: giữ hash, ngày, ảnh, lỗi thật của 4A–4D, bổ sung mục 4E; không ghi đè bằng các chỗ trống |
| progress-report/kiem-thu/Tuan-04-Nghiem-thu.md | Thêm hoặc hợp nhất ca/kết quả sẵn có |
| progress-report/Kich-ban-demo-Tuan-04.md | Thêm kịch bản minh họa |
| thesis/doc/Chuong-3-Phan-tich-va-thiet-ke-he-thong.md | Bản nháp Chương 3; hợp nhất với bản riêng nếu đã viết |
| setup/Kiem-tra-Tuan-04.sql | SQL chỉ đọc để đối chiếu |
| setup/Huong-dan-chot-Tuan-04.md | Hướng dẫn này |

Sao lưu tài liệu sắp thay ra ngoài repository. Không chép cả thư mục MachKit_Tuan04_Chang4E vào src. Không cần sửa .csproj, Program.cs, AppDbContext.cs hay chạy migration mới.

## 2. Kiểm tra môi trường và mã thử lỗi

Dừng web Ctrl+C. Terminal:

```powershell
cd "C:\Users\DUC HUY\Documents\GitHub\ASPNET-DK25TTC5-VuDucHuy-machkit\src\MachKit"
dir *.csproj
dotnet --version
dotnet build
dotnet ef migrations list
```

Mong đợi MachKit.csproj, SDK 8.0.x, build thành công. Đối chiếu migrations đã áp dụng với database. Tên đã hướng dẫn: Tuan04_ThemBangDonHang, Tuan03_BoSungGioHang, Tuan04_4C_ChongGuiTrungDonHang, Tuan04_4D_NhatKyTrangThaiDonHang. Tiền tố timestamp và tên thực tế trên máy là căn cứ; không sửa ngày hoặc tạo lại migration để khớp danh sách.

Nếu muốn kiểm tra thêm và CLI EF đang dùng hỗ trợ EF Core 8:

```powershell
dotnet ef migrations has-pending-model-changes
```

Lệnh trên so model với snapshot, KHÁC việc migration đã áp dụng vào SQL. Không có thay đổi model chưa chứng minh database đồng bộ. Nếu thiếu command, ghi nhận phiên bản công cụ, không nâng framework. Có pending migration hoặc khác model: dừng để đối chiếu, không chạy sửa/xóa hàng loạt.

Tìm code gây lỗi thử nghiệm, chỉ quét .cs trong Services:

```powershell
Get-ChildItem .\Services -Filter *.cs -File |
    Select-String -Pattern 'TEST_ROLLBACK_4C|TEST 4D:|MACHKIT_TEST_FAIL_BEFORE_COMMIT'
```

Nếu có kết quả, đối chiếu đúng đoạn thử đã thêm theo hướng dẫn 4C/4D rồi bỏ đoạn tạm đó. Không xóa mọi `throw` hoặc kiểm tra lỗi của hệ thống. Không có kết quả nghĩa là không thấy các marker đã biết, không phải chứng minh không còn mọi lỗi.

```powershell
Remove-Item Env:MACHKIT_TEST_FAIL_BEFORE_COMMIT -ErrorAction SilentlyContinue
dotnet build
dotnet run --launch-profile http
```

Lệnh Remove-Item ở đây chỉ bỏ biến môi trường thử lỗi trong Terminal hiện tại, không xóa file. Dùng URL Now listening on, không buộc cổng 5178 nếu thực tế khác.

## 3. Chuẩn bị dữ liệu và bằng chứng

Backup Full MachKitDb trước khi mua/hủy đơn thử hoặc mô phỏng lỗi. Dùng hai tài khoản khách A/B, Admin hiện có và NhanVien đã có role nếu kiểm tra vai trò đó. Không dựng lại Identity hay tự cấp role qua dữ liệu form.

Chọn sản phẩm thử không thuộc bộ mẫu nếu có. Ghi tồn/giá trước mỗi phép thử; không chỉnh tồn thủ công, lưu form Admin cũ hoặc tạo đơn khác trong lúc so sánh. Dùng dữ liệu người nhận giả, chỉ thử tại localhost.

Có thể tạo thư mục ảnh tại gốc repo:

```powershell
New-Item -ItemType Directory -Force ..\..\progress-report\minh-chung\Tuan-04 | Out-Null
```

Tên ảnh do bạn chụp thật. Che email/tài khoản riêng không cần thiết, địa chỉ/số điện thoại thật, cookie, token, secrets, chuỗi kết nối có mật khẩu. Không chụp phiếu xác nhận vào tài liệu public.

## 4. Chạy hai luồng bắt buộc

### A. Đặt và giao thành công

Ghi tồn N, chọn số lượng S không quá N. Khách A: giỏ → thông tin → xem lại → xác nhận. Ghi Id thật; kiểm tra đơn chờ, chi tiết và tổng đúng, tồn N−S, dòng giỏ đã mua bị xóa. Admin duyệt → giao → hoàn thành; tồn không giảm thêm. Khách A thấy trạng thái mới; khách B mở URL đơn A nhận 404. Không xóa đơn để hoàn tồn.

### B. Đặt đơn khác và hủy trước giao

Ghi tồn M ngay trước đặt đơn thứ hai (không dùng lại N cũ). Mua Q ≤ M: tồn M−Q. Admin hủy khi còn chờ hoặc đã xác nhận, lý do hợp lệ: tồn trở về M. Gửi lại cùng thao tác hoặc tab cũ: vẫn M, chỉ một log hủy. Giá và chi tiết không mất; giỏ không tự trở lại.

Mở bảng nghiệm thu để ghi số liệu/ảnh; không đánh dấu hoàn thành chỉ dựa trên thông báo màu xanh.

## 5. Các phép thử còn lại

Đối chiếu ma trận nghiệm thu: quyền truy cập, form sai, thiếu tồn/giá đổi/giỏ đổi, gửi lặp, cạnh tranh tồn, cập nhật sai bước, nhật ký và chức năng Tuần 03. Có thể sử dụng kết quả 4C/4D đã chạy nếu ghi được commit/dữ liệu/bằng chứng và code liên quan không đổi. Kiểm thử lại luồng chính sau khi đã tích hợp 4D.

Hướng dẫn thử gửi lặp và rollback được tập hợp trong phụ lục bảng nghiệm thu. Không chủ động tắt SQL Server đúng lúc commit trên dữ liệu quan trọng. Ca mất phản hồi khó mô phỏng được để “Chưa kiểm thử” kèm giới hạn, không nhận vơ kết quả từ ca rollback.

## 6. Đối chiếu SQL

Kết nối .\SQLEXPRESS, mở setup/Kiem-tra-Tuan-04.sql. Đầu tiên chạy phần A kiểm tra đủ bảng/cột; chỉ chạy phần B/C khi không thiếu. Mỗi phần được ngăn bởi GO để chọn vùng chạy trong SSMS. Không sửa dữ liệu qua SQL để làm kết quả đẹp.

Phần A là metadata/migrations. Phần B thống kê và dữ liệu đơn chọn theo Id thật; thay @DonHangId=0. Phần C là các truy vấn bất thường, mong đợi 0 dòng nếu dữ liệu đúng. Phần D liệt kê các trường hợp cần đối chiếu (đơn cũ thiếu mã, đơn hủy thiếu log): không tự coi tất cả là lỗi mới. Không có đơn thử thì chưa đủ bằng chứng dù truy vấn bất thường rỗng.

## 7. Hoàn thiện tài liệu

Điền kết quả thật và hạn chế vào bảng nghiệm thu. Tuan-04.md giữ thông tin 4A–4D và thêm ngày/code/ảnh 4E. Chương 3 là bản nháp dựa trên code, cần rà soát quy cách giảng viên; không thay Chương 1/2 hoặc lập báo cáo hiệu năng chưa đo. Không có sơ đồ/ảnh kiểm thử giả trong gói.

Khi tiêu chí chốt đạt, sửa dòng tiến độ đầu README, hàng 4E, trạng thái cuối README và kết luận báo cáo thành cùng một mốc. Nếu chưa đủ, giữ “Đang nghiệm thu” và nêu các ca chưa thử. Không coi việc nhận gói tài liệu là đã chạy kiểm thử.

## 8. Một README chính

Từ src/MachKit, kiểm tra các README do dự án đặt tại gốc:

```powershell
Get-ChildItem ..\.. -File -Filter 'README*.md' | Select-Object Name
```

Sau khi đã hợp nhất và sao lưu, chỉ xóa các README_Tuan_0N.md trùng của dự án nếu còn. Không quét/xóa README trong wwwroot/lib, node_modules hay thư viện. Giữ đúng tên README.md. Tài liệu khác trong progress-report/setup không phải README.

## 9. Commit và Push

Dùng GitHub Desktop, đúng repo và nhánh đang dùng. Xem Changes, các thay đổi dự kiến là 7 tài liệu của gói và ảnh thật nếu thêm. Nếu .cs hoặc migration xuất hiện, đối chiếu nguồn: có thể là phần 4D chưa commit hoặc sửa lỗi thực tế, không mặc định chọn/xóa tất cả.

Trước commit bảo đảm không còn đoạn thử rollback; build lại sau mọi thay đổi code. Không commit .bak, bản sao project, khóa, credentials, dữ liệu người nhận thật hoặc ảnh có token.

Summary sau nghiệm thu đạt:

```text
Tuần 04: Hoàn thiện kiểm thử, báo cáo và README
```

Description:

```text
Ghi nhận kiểm thử tích hợp đặt hàng, giao hàng, hủy và hoàn tồn.
Đối chiếu SQL, quyền truy cập và các ca chống xử lý lặp.
Hoàn thiện báo cáo Tuần 04 và bản nháp Chương 3.
Cập nhật README chính; giữ các giới hạn và ca chưa kiểm thử được công bố.
Không triển khai ngoài phạm vi Tuần 04.
```

Nếu còn đang chuẩn bị tài liệu, dùng commit “Tuần 04: Chuẩn bị tài liệu nghiệm thu 4E”, không ghi đã hoàn tất. Sau commit bấm Push origin, mở đúng nhánh trên GitHub kiểm tra các file. Lấy hash thật từ History. Không cần nhét hash của chính commit 4E vào nội dung trước khi tạo nó; đối chiếu qua History là đủ. Không sửa lịch sử chỉ để điền hash.

## 10. Điều kiện dừng

Hai luồng end-to-end đúng; các ca nghiêm trọng có kết quả/bằng chứng; không còn lỗi tồn, phân quyền hoặc dữ liệu chưa xử lý; tài liệu nhất quán, giới hạn công khai; commit đã Push. Các phép thử phụ chưa thực hiện phải được nêu rõ, không tuyên bố “100% mọi tình huống”. Hoàn tất mốc này thì dừng Tuần 04.

## Tài liệu công cụ đối chiếu

- Microsoft: Managing Migrations, mục Listing migrations và Checking for pending model changes — https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/managing
- GitHub: Pushing changes from GitHub Desktop — https://docs.github.com/en/desktop/making-changes-in-a-branch/pushing-changes-to-github-from-github-desktop

Nguồn công cụ chỉ hỗ trợ cách dùng lệnh/Push, không chứng minh kết quả project. Quy tắc nghiệp vụ lấy từ gói mã nguồn MạchKit đã cung cấp.
