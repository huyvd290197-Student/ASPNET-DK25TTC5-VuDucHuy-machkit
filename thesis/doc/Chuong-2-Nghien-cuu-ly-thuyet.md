# CHƯƠNG 2. NGHIÊN CỨU LÝ THUYẾT

**Đề tài:** Xây dựng website bán linh kiện điện tử hỗ trợ mua theo bộ thực hành.  
**Sinh viên:** Vũ Đức Huy – DK25TTC5 – MSSV 170125378.  
**Tình trạng:** Bản nháp nội dung Chương 2; cần rà soát theo mẫu trình bày của giảng viên. Không phải biên bản xác nhận phần mềm đã chạy thành công.

## 2.1. ASP.NET Core MVC

ASP.NET Core MVC tổ chức ứng dụng theo ba nhóm trách nhiệm: Model biểu diễn dữ liệu và nghiệp vụ; View trình bày giao diện; Controller tiếp nhận yêu cầu, phối hợp xử lý và trả kết quả. Razor cho phép kết hợp HTML với biểu thức C# trong View. ViewModel tập hợp đúng dữ liệu cần cho một màn hình, tránh để View gánh quá nhiều xử lý nghiệp vụ. [1]

Trong MạchKit, trang danh sách dùng `LinhKienController` và `LinhKienDanhSachViewModel`. Thiết kế bổ sung trang chi tiết sử dụng `LinhKienChiTietController` để không thay thế Controller đã hoàn thành ở Chặng 3A. Phần thêm, sửa và xóa giỏ hàng được đưa vào `GioHangService`; Controller nhận yêu cầu và chuyển kết quả cho View.

Cách phân chia này được lựa chọn vì đồ án có nhiều nhóm màn hình: trang khách, quản trị linh kiện, dự toán bộ thực hành và giỏ hàng. Tách trách nhiệm giúp sinh viên kiểm tra từng chức năng, khoanh vùng lỗi và tránh sửa giao diện trang chủ khi bổ sung nghiệp vụ mới.

## 2.2. Entity Framework Core

Entity Framework Core là công cụ ánh xạ đối tượng – quan hệ: các lớp .NET và `DbContext` được dùng để truy vấn, thêm, sửa, xóa dữ liệu. LINQ biểu diễn truy vấn theo kiểu dữ liệu C#. Migration ghi nhận thay đổi mô hình để phát triển cấu trúc database qua các phiên bản. [2]

MạchKit sử dụng `AppDbContext`. `LinhKien`, `DanhMuc`, `ThuongHieu`, `BoThucHanh` và `ThanhPhanBo` là các lớp nghiệp vụ đã có. Bổ sung giỏ hàng dùng `GioHangItem`, không biến giỏ hàng thành đơn hàng. Giá hiển thị trong giỏ được đọc từ `LinhKien.Gia`, thay vì lấy tổng tiền từ biểu mẫu trình duyệt.

Migration phải được đọc lại trước khi áp dụng. Tên migration chỉ mô tả thay đổi; thứ tự thực hiện được xác định bởi dấu thời gian trong tên file. Vì Chặng 4A đã được áp dụng trước, migration bổ sung giỏ hàng Tuần 03 được tạo sau nó và giữ nguyên các bảng đơn hàng. Không sửa lịch sử migration chỉ để làm cho tên tuần có vẻ liên tiếp. [3]

## 2.3. SQL Server

SQL Server được dùng làm hệ quản trị cơ sở dữ liệu quan hệ của đồ án. Khóa chính nhận diện mỗi bản ghi; khóa ngoại ràng buộc quan hệ giữa các bảng và ngăn dữ liệu tham chiếu đến bản ghi không tồn tại. [4]

Môi trường đang sử dụng của MạchKit là instance `.\SQLEXPRESS`, database `MachKitDb`. Bảng giỏ hàng được thiết kế có khóa ngoại tới `AspNetUsers` và `LinhKien`. Ràng buộc duy nhất trên cặp `(NguoiDungId, LinhKienId)` bảo đảm một tài khoản không có hai dòng tách rời cho cùng linh kiện. Khi thêm lại sản phẩm, chương trình cộng số lượng vào dòng hiện có.

SQL Server được giữ lại vì dữ liệu và các bảng nghiệp vụ của đồ án đã vận hành trên hệ này. Thay hệ quản trị giữa chừng không phục vụ mục tiêu hoàn thành chức năng còn thiếu của Tuần 03.

## 2.4. ASP.NET Core Identity

ASP.NET Core Identity cung cấp thành phần quản lý tài khoản và hỗ trợ các luồng đăng ký, đăng nhập, quản lý thông tin người dùng. Trong ứng dụng MVC, giao diện Identity mặc định có thể được cung cấp qua Razor Pages trong Area `Identity`. [5]

MạchKit đã có `ApplicationUser`, kế thừa `IdentityUser`, và các vai trò `Admin`, `NhanVien`, `KhachHang`. Thiết kế giỏ hàng tái sử dụng tài khoản này. Trang giỏ yêu cầu đăng nhập; mã tài khoản lấy từ thông tin xác thực phía máy chủ. Các thao tác cập nhật/xóa đều lọc đồng thời theo ID dòng giỏ và chủ sở hữu. Không tin `NguoiDungId` do trình duyệt tự gửi lên.

Quy tắc nghiệp vụ được chọn là mỗi tài khoản có giỏ riêng, lưu trong database. Đăng xuất hoặc khởi động lại ứng dụng không phải là thao tác xóa giỏ. Việc kết luận tính bền vững này hoạt động đúng cần có kiểm thử với hai tài khoản và một lần khởi động lại website.

## 2.5. Bootstrap và giao diện thích ứng

Bootstrap cung cấp hệ thống lưới dựa trên container, hàng và cột, cùng các lớp thay đổi bố cục theo độ rộng màn hình. Cơ chế này hỗ trợ tổ chức nội dung theo hướng ưu tiên thiết bị di động. [6]

Đồ án tái sử dụng Bootstrap đang có, không thêm một framework giao diện mới. Trang chi tiết chia khu vực ảnh và thông tin; trên màn hình hẹp các cột có thể xếp dọc. Giỏ hàng dùng bảng có vùng cuộn ngang khi cần. CSS bổ sung nằm trong `tuan03-cua-hang.css` với tiền tố `mk-t3`, không sửa các lớp của trang chủ.

## 2.6. Bảo mật web cơ bản

### 2.6.1. CSRF

Khi ứng dụng dùng cookie đăng nhập, trình duyệt có thể gửi cookie cùng yêu cầu phát sinh từ nguồn khác. Mẫu bảo vệ được áp dụng trong đồ án là dùng token chống giả mạo yêu cầu cho các biểu mẫu POST, đồng thời kiểm tra token ở action xử lý. Thao tác GET chỉ đọc dữ liệu, không thêm hoặc xóa giỏ. [7]

Trong thiết kế bổ sung, các action `Them`, `CapNhat`, `Xoa` dùng `[ValidateAntiForgeryToken]`. Các biểu mẫu tương ứng tạo token bằng Form Tag Helper. Đây là một lớp bảo vệ, không thay thế kiểm tra đăng nhập và kiểm tra chủ sở hữu dữ liệu.

### 2.6.2. XSS và liên kết bên ngoài

Razor mã hóa các giá trị được đưa ra giao diện theo cách thông thường. Việc xuất dữ liệu không đáng tin bằng `Html.Raw` có thể làm mất lớp bảo vệ đó. [7] Vì vậy, mô tả sản phẩm được hiển thị dạng văn bản, không tự diễn giải thành HTML.

Datasheet trong bản triển khai chỉ tạo liên kết khi URL có giao thức HTTP hoặc HTTPS; không chấp nhận `javascript:`, `data:` hoặc `file:`. Liên kết mở tab mới dùng `rel="noopener noreferrer"`. Việc chấp nhận giao thức không chứng minh trang đích đáng tin: quản trị viên vẫn phải chọn tài liệu chính xác của linh kiện.

### 2.6.3. SQL injection và dữ liệu nhập

Ghép trực tiếp dữ liệu người dùng vào câu SQL có thể tạo nguy cơ SQL injection. Khi cần SQL trực tiếp, phải dùng cách truyền tham số phù hợp; không coi mọi cách gọi SQL thô đều an toàn. [8]

Phần giỏ hàng được viết bằng truy vấn LINQ. Máy chủ kiểm tra ID, số lượng nguyên dương, trạng thái ngừng bán và tồn kho. Thuộc tính `min`/`max` trên ô nhập chỉ hỗ trợ giao diện; mã phía máy chủ vẫn phải kiểm tra lại. Đơn giá và mã tài khoản không được lấy từ trường ẩn do người dùng có thể sửa.

### 2.6.4. Cập nhật đồng thời

SQL Server `rowversion`, khi cấu hình làm concurrency token, cho phép EF Core phát hiện bản ghi đã thay đổi kể từ lúc được đọc; cập nhật/xóa có xung đột có thể phát sinh `DbUpdateConcurrencyException`. [9] Thiết kế bổ sung dùng cơ chế này trên dòng giỏ và thông báo người dùng xem lại giỏ. Chỉ mục duy nhất chặn hai lần thêm đồng thời tạo dòng trùng. [10]

Điều này không phải cơ chế giữ hàng. Nếu tồn kho thay đổi sau khi khách thêm giỏ, trang giỏ phải cảnh báo. Giai đoạn đặt hàng sau này cần xác nhận lại tồn kho trong quy trình riêng; không tuyên bố Tuần 03 đã giải quyết việc bán hàng đồng thời hay thanh toán.

## 2.7. Quy trình thương mại điện tử trong phạm vi đề tài

Luồng nghiệp vụ được đề xuất cho MạchKit gồm: xem/tìm/lọc linh kiện → mở chi tiết → đăng nhập → thêm và chỉnh giỏ → đặt hàng → theo dõi xử lý. Đây là thiết kế cho đồ án, không phải quy trình bắt buộc áp dụng cho mọi website bán hàng.

Phạm vi hoàn thiện Tuần 03 dừng ở giỏ hàng: lưu sản phẩm theo tài khoản, tính tạm tiền theo giá hiện tại, chặn thêm/cập nhật số lượng vượt tồn tại thời điểm kiểm tra. Không tạo đơn và không giảm tồn khi khách chỉ thêm vào giỏ. Chặng 4A đã tạo cấu trúc đơn hàng được bảo toàn, nhưng không phải bằng chứng rằng quy trình đặt hàng đã hoàn tất.

Điểm riêng của đề tài là dự toán theo số bộ thực hành. Ví dụ bộ LED đôi dùng hai LED đỏ, hai điện trở 330 Ω và một breadboard mini cho mỗi bộ. Kết quả đủ/thiếu phải dựa trên tồn kho thực tế, không cố định theo dữ liệu mẫu ban đầu. Phần dự toán này được giữ nguyên khi bổ sung trang chi tiết và giỏ.

## 2.8. Lý do lựa chọn công nghệ

Bộ công nghệ C#/.NET 8, ASP.NET Core MVC, EF Core, SQL Server, Identity và Bootstrap đã được sử dụng trong các chặng trước. Tiếp tục tái sử dụng giúp tránh di chuyển dữ liệu, thay cơ chế tài khoản hoặc thiết kế lại giao diện trong thời gian hoàn thành đồ án. Phân chia MVC và Service phù hợp với cách triển khai, kiểm thử và commit từng chặng của MạchKit.

Giới hạn cần ghi nhận là môi trường hiện tại vẫn là môi trường phát triển. Việc có kiểm tra đầu vào, token CSRF và quan hệ khóa ngoại không thay cho đánh giá an toàn toàn diện, kiểm thử hiệu năng hay triển khai sản xuất. Kết quả thực nghiệm sẽ được ghi riêng sau khi chạy các ca kiểm thử trên `MachKitDb`.

## Tài liệu tham khảo

Các tài liệu công nghệ dưới đây được đối chiếu ngày 23/11/2026; chỉ áp dụng các API phù hợp .NET/EF Core 8, không nâng phiên bản dự án theo ví dụ của bản mới hơn.

[1] Microsoft Learn, *Overview of ASP.NET Core MVC*: https://learn.microsoft.com/vi-vn/aspnet/core/mvc/overview?view=aspnetcore-8.0

[2] Microsoft Learn, *Entity Framework Core*: https://learn.microsoft.com/en-us/ef/core/

[3] Microsoft Learn, *Managing Migrations*: https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/managing

[4] Microsoft Learn, *Primary and foreign key constraints*: https://learn.microsoft.com/en-us/sql/relational-databases/tables/primary-and-foreign-key-constraints?view=sql-server-ver16

[5] Microsoft Learn, *Introduction to Identity on ASP.NET Core*: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-8.0

[6] Bootstrap, *Grid system*: https://getbootstrap.com/docs/5.3/layout/grid/

[7] Microsoft Learn, *Prevent Cross-Site Request Forgery (XSRF/CSRF) attacks in ASP.NET Core*: https://learn.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-8.0

[8] Microsoft Learn, *SQL Queries*: https://learn.microsoft.com/en-us/ef/core/querying/sql-queries

[9] Microsoft Learn, *Handling Concurrency Conflicts*: https://learn.microsoft.com/en-us/ef/core/saving/concurrency

[10] Microsoft Learn, *Indexes*: https://learn.microsoft.com/en-us/ef/core/modeling/indexes

Nguồn yêu cầu và bối cảnh đề tài: `HANDOFF_GPT_MachKit_Tuan03.md`; mã `LinhKien`, `ThongSoKyThuat`, `ApplicationUser` trong repository MạchKit đã được đối chiếu; xác nhận của sinh viên rằng mới hoàn thành 11 bước Chặng 3A và đã kiểm thử Chặng 4A.
