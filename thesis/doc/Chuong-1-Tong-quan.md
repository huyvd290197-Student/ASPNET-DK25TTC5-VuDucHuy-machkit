# CHƯƠNG 1. TỔNG QUAN

## 1.1. Bối cảnh và lý do chọn đề tài

Linh kiện điện tử là nhóm sản phẩm có số lượng chủng loại lớn, nhiều mặt hàng có hình thức gần giống nhau nhưng khác biệt đáng kể về thông số kỹ thuật, điện áp làm việc, kích thước, chuẩn giao tiếp hoặc công suất. Đối với học sinh, sinh viên và người thực hiện các dự án DIY, quá trình mua linh kiện thường không dừng ở việc chọn một sản phẩm đơn lẻ mà còn phải xác định đúng số lượng của nhiều loại linh kiện để tạo thành một mạch hoặc một bộ thực hành hoàn chỉnh. Nếu tự tính thủ công, người mua dễ thiếu một linh kiện nhỏ làm toàn bộ buổi thực hành bị gián đoạn.

Các website bán linh kiện điện tử hiện nay đã hỗ trợ khá tốt việc xem danh mục, giá, tình trạng hàng, giỏ hàng và một số nội dung hỗ trợ kỹ thuật. Tuy nhiên, trong phạm vi khảo sát của đồ án, chức năng hỗ trợ người dùng nhập số lượng bộ thực hành rồi tự động quy đổi ra danh sách linh kiện cần mua chưa phải là chức năng phổ biến. Vì vậy, đề tài MạchKit được lựa chọn theo hướng xây dựng một website thương mại điện tử bán linh kiện điện tử nhưng bổ sung chức năng đặc trưng: quản lý bộ thực hành và tự động tính số lượng linh kiện cần chuẩn bị theo số bộ người dùng nhập.

Tên hệ thống được sử dụng trong đồ án là **MạchKit**. Hệ thống được phát triển trên ASP.NET Core MVC, sử dụng Entity Framework Core và SQL Server. Ngoài các chức năng bán hàng cơ bản, MạchKit được thiết kế để lưu quan hệ giữa một bộ thực hành và các linh kiện cấu thành. Ví dụ, một bộ “Đèn LED đôi trên breadboard” cần 2 LED đỏ, 2 điện trở 330 Ω và 1 breadboard mini. Khi người dùng nhập 4 bộ, hệ thống tự tính cần 8 LED, 8 điện trở và 4 breadboard, đồng thời đối chiếu tồn kho để chỉ rõ linh kiện còn thiếu.

## 1.2. Khảo sát một số website bán linh kiện điện tử

### 1.2.1. ThegioiIC

ThegioiIC là website kinh doanh nhiều nhóm linh kiện và thiết bị điện tử như IC, mạch và module, linh kiện thụ động, LED, cảm biến, thiết bị đo kiểm và thiết bị tự động hóa. Trang chủ thể hiện rõ giá bán và số lượng hàng còn lại đối với nhiều sản phẩm. Đây là điểm hữu ích đối với khách hàng kỹ thuật vì họ có thể đánh giá nhanh sản phẩm còn đủ số lượng hay không trước khi mua.

Ưu điểm đáng chú ý của website là danh mục sản phẩm rộng, dữ liệu tồn kho được trình bày trực tiếp và sản phẩm được tổ chức theo nhóm kỹ thuật. Tuy nhiên, với người mua theo một bài thực hành cụ thể, khách hàng vẫn phải tự xác định những linh kiện nào cần thiết và tự nhân số lượng theo số bộ cần chuẩn bị.

### 1.2.2. NShop

NShop tập trung vào các nhóm sản phẩm như Arduino, cảm biến, LED, module, robot, phụ kiện DIY và các thiết bị phục vụ học tập hoặc chế tạo. Website có danh mục sản phẩm, giỏ hàng, tài khoản, hướng dẫn mua hàng và chức năng kiểm tra đơn hàng. Cách tổ chức này phù hợp với đối tượng học sinh, sinh viên và người làm dự án cá nhân.

Ưu điểm là giao diện mua sắm tương đối quen thuộc, nhiều nhóm sản phẩm phục vụ thực hành và có quy trình mua hàng trực tuyến. Tuy nhiên, người dùng vẫn chủ yếu lựa chọn từng sản phẩm hoặc combo đã được chuẩn bị sẵn; bài toán “nhập số bộ cần làm và để hệ thống tự tính từng linh kiện cần mua” không phải trọng tâm của mô hình này.

### 1.2.3. HShop

HShop cung cấp mạch lập trình, máy tính nhúng, module, cảm biến, linh kiện điện tử và thiết bị robot phục vụ học tập, nghiên cứu, DIY và chế tạo. Website có các nhóm sản phẩm phổ biến như Arduino, ESP32, Raspberry Pi và nhiều module cảm biến. Một số trang danh mục thể hiện tính năng yêu thích và so sánh sản phẩm.

Ưu điểm của HShop là định hướng rõ đến cộng đồng học tập, nghiên cứu và maker; sản phẩm phù hợp cho các dự án thực hành. Website cũng nhấn mạnh nội dung tư vấn và hỗ trợ kỹ thuật. Điểm mà MạchKit muốn phát triển khác là đưa khái niệm “bộ thực hành” thành một đối tượng dữ liệu riêng. Mỗi bộ có thành phần và định mức cụ thể, từ đó hệ thống có thể tính nhu cầu vật tư một cách tự động.

### 1.2.4. BanLinhKien.com

BanLinhKien.com cung cấp nhiều nhóm linh kiện, kit phát triển và thiết bị điện tử. Trang danh mục kit phát triển cho thấy sản phẩm được chia theo các nền tảng như Arduino, AVR, PIC, STM, FPGA và máy tính nhúng; sản phẩm có giá và trạng thái hàng, đồng thời hỗ trợ thao tác thêm vào giỏ.

Điểm mạnh là danh mục kỹ thuật chi tiết và số lượng sản phẩm lớn. Tuy nhiên, giống các website khảo sát khác, quy trình mua hàng chủ yếu bắt đầu từ từng sản phẩm hoặc kit có sẵn. MạchKit hướng đến việc cho phép người quản trị tự định nghĩa thành phần của một bộ thực hành và cho phép khách thay đổi số lượng bộ cần chuẩn bị trước khi hệ thống tính toán.

## 1.3. Bảng so sánh tổng quát

| Tiêu chí | ThegioiIC | NShop | HShop | BanLinhKien.com | MạchKit dự kiến |
|---|---|---|---|---|---|
| Danh mục linh kiện | Có | Có | Có | Có | Có |
| Hiển thị giá | Có | Có | Có | Có | Có |
| Giỏ hàng / mua trực tuyến | Có | Có | Có | Có | Có |
| Thể hiện tình trạng hàng | Rõ ở nhiều sản phẩm | Có theo từng sản phẩm | Có theo sản phẩm | Có trạng thái hàng | Có, kết hợp tồn kho |
| Thông số kỹ thuật | Có tùy sản phẩm | Có tùy sản phẩm | Có tùy sản phẩm | Có tùy sản phẩm | Có |
| Quản lý bộ thực hành như một cấu trúc linh kiện | Không phải trọng tâm khảo sát | Không phải trọng tâm khảo sát | Không phải trọng tâm khảo sát | Chủ yếu bán kit có sẵn | Có |
| Nhập số bộ rồi tự tính số lượng từng linh kiện | Không phải trọng tâm khảo sát | Không phải trọng tâm khảo sát | Không phải trọng tâm khảo sát | Không phải trọng tâm khảo sát | Có |
| Báo chính xác linh kiện thiếu theo số bộ | Không phải trọng tâm khảo sát | Không phải trọng tâm khảo sát | Không phải trọng tâm khảo sát | Không phải trọng tâm khảo sát | Có |

Bảng trên chỉ phản ánh các chức năng quan sát được trong phạm vi khảo sát phục vụ đồ án, không nhằm đánh giá toàn bộ năng lực thương mại của các website.

## 1.4. Bài toán cần giải quyết

Từ khảo sát trên, đồ án xác định các vấn đề chính cần giải quyết như sau:

Thứ nhất, hệ thống cần quản lý linh kiện theo mã, tên, danh mục, thương hiệu, giá, số lượng tồn, hình ảnh, datasheet và các thông số kỹ thuật. Việc tổ chức dữ liệu này giúp người mua phân biệt các sản phẩm có tên gần giống nhau.

Thứ hai, hệ thống cần quản lý “bộ thực hành” độc lập với linh kiện. Một bộ thực hành gồm nhiều linh kiện và mỗi linh kiện có một định mức trên một bộ. Quan hệ này được lưu trong bảng thành phần bộ, thay vì ghi cố định trong mã nguồn.

Thứ ba, khi khách nhập số bộ cần chuẩn bị, hệ thống phải tính số lượng cần mua của từng linh kiện theo công thức:

**Số lượng cần = Số bộ × Định mức linh kiện trên một bộ.**

Sau đó hệ thống so sánh số lượng cần với tồn kho hiện tại. Nếu tồn nhỏ hơn số lượng cần, hệ thống phải chỉ ra số lượng còn thiếu. Số bộ tối đa có thể ghép được xác định bởi linh kiện giới hạn nhất trong bộ.

Thứ tư, hệ thống cần có khu quản trị được phân quyền để nhân viên hoặc quản trị viên quản lý danh mục, thương hiệu, linh kiện, thông số kỹ thuật, ảnh sản phẩm, bộ thực hành và thành phần bộ. Khách thông thường không được truy cập khu vực quản trị.

Thứ năm, website phải có giao diện trực quan, có khả năng hiển thị trên màn hình máy tính và thiết bị di động. Ảnh sản phẩm cần được kiểm soát định dạng và dung lượng để hạn chế tệp không phù hợp.

## 1.5. Mục tiêu của hệ thống MạchKit

Mục tiêu tổng quát của đồ án là xây dựng website bán linh kiện điện tử trên nền tảng ASP.NET Core MVC, hỗ trợ quy trình quản lý sản phẩm, tồn kho và mua hàng trực tuyến, đồng thời bổ sung chức năng dự toán linh kiện theo bộ thực hành.

Các mục tiêu cụ thể trong giai đoạn hiện tại gồm:

1. Xây dựng cơ sở dữ liệu SQL Server quản lý danh mục, thương hiệu, linh kiện, thông số kỹ thuật, bộ thực hành và thành phần bộ.
2. Xây dựng giao diện trang chủ giới thiệu bộ thực hành và công cụ dự toán linh kiện.
3. Cho phép người dùng nhập số bộ từ 1 đến 99 và nhận kết quả số lượng cần, số lượng tồn, số lượng thiếu và tổng tiền dự toán.
4. Xây dựng khu quản trị có phân quyền Admin và Nhân viên.
5. Cho phép thêm, sửa, ẩn/hiện linh kiện, tải ảnh sản phẩm và quản lý thông số kỹ thuật.
6. Quản lý thành phần của bộ thực hành để thay đổi định mức mà không cần sửa mã chương trình.
7. Làm nền tảng cho các chức năng ở các tuần tiếp theo như tìm kiếm, lọc, giỏ hàng, đặt hàng, xử lý đơn và báo cáo.

## 1.6. Phạm vi thực hiện

Đồ án triển khai dưới dạng website chạy trên trình duyệt và sử dụng dữ liệu mẫu phục vụ học tập. Giai đoạn đầu tập trung vào bán lẻ linh kiện và hỗ trợ bộ thực hành. Thanh toán trực tuyến hoàn chỉnh, tích hợp đơn vị vận chuyển thật và ứng dụng di động độc lập không thuộc phạm vi bắt buộc.

Về người dùng, hệ thống định hướng ba nhóm chính: khách hàng, nhân viên và quản trị viên. Khách hàng xem và mua sản phẩm; nhân viên quản lý dữ liệu bán hàng theo quyền được cấp; quản trị viên có quyền cao hơn đối với cấu hình và tài khoản.

Về công nghệ, hệ thống sử dụng ASP.NET Core MVC cho ứng dụng web, Entity Framework Core để truy cập dữ liệu, SQL Server để lưu trữ, ASP.NET Core Identity cho xác thực và phân quyền, Razor View và Bootstrap cho giao diện.

## 1.7. Điểm khác biệt của đề tài

Điểm khác biệt chính của MạchKit không nằm ở việc chỉ xây dựng thêm một website bán linh kiện, mà ở mô hình dữ liệu và luồng sử dụng dành cho bộ thực hành. Trong website bán hàng thông thường, khách chọn từng sản phẩm rồi tự quyết định số lượng. Trong MạchKit, người dùng có thể bắt đầu từ một bộ thực hành, nhập số bộ muốn chuẩn bị và để hệ thống tự quy đổi thành nhu cầu linh kiện.

Ví dụ, kho có 10 LED đỏ, 7 điện trở 330 Ω và 5 breadboard mini. Một bộ cần lần lượt 2, 2 và 1 linh kiện. Với 3 bộ, hệ thống cần 6 LED, 6 điện trở và 3 breadboard nên đủ hàng. Với 4 bộ, hệ thống cần 8 điện trở nhưng kho chỉ có 7, vì vậy thông báo thiếu 1 điện trở. Cách tiếp cận này thể hiện rõ mối liên hệ giữa dữ liệu tồn kho và nhu cầu thực hành, đồng thời tạo ra nội dung kiểm thử riêng cho đồ án.

## 1.8. Kết luận chương

Chương 1 đã trình bày bối cảnh của bài toán, khảo sát một số website bán linh kiện điện tử và xác định yêu cầu riêng của MạchKit. Kết quả khảo sát cho thấy các website hiện có đã hỗ trợ tốt việc phân loại, giới thiệu và bán sản phẩm. Trên cơ sở đó, MạchKit kế thừa các chức năng cần thiết của một website bán hàng nhưng tập trung thêm vào quản lý bộ thực hành và tự động tính nhu cầu linh kiện. Các yêu cầu này là cơ sở để lựa chọn công nghệ và thiết kế hệ thống ở các chương tiếp theo.

## Tài liệu tham khảo dự kiến cho Chương 1

[1] ThegioiIC, “Linh kiện điện tử, điện công nghiệp, tự động hóa, cơ khí,” truy cập ngày 10/11/2026, https://www.thegioiic.com/.

[2] NShop, “Linh kiện Điện tử, Arduino, Cảm biến, Đèn LED, Robot,” truy cập ngày 10/11/2026, https://nshopvn.com/.

[3] HShop, “Hshop.vn - Điện tử & Robot,” truy cập ngày 10/11/2026, https://hshop.vn/.

[4] BanLinhKien.com, “Vi Điều Khiển - KIT Phát Triển,” truy cập ngày 10/11/2026, https://banlinhkien.com/vi-dieu-khien-kit-phat-trien-pc535009.html.
