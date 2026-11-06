# MạchKit – Website bán linh kiện điện tử theo bộ thực hành

## 🎓 Thông tin sinh viên

| Nội dung | Thông tin |
| :--- | :--- |
| **Họ và tên** | **Vũ Đức Huy** |
| **Lớp** | DK25TTC5 |
| **Mã số sinh viên** | 170125378 |
| **Email** | Huyvd290197@tvu-onschool.edu.vn |
| **Số điện thoại** | 0935075821 |
| **Tài khoản GitHub** | huyvd290197-Student |


## Bài toán
Người mua nhập số bộ thực hành cần chuẩn bị. Website tính số lượng của từng linh kiện, dự toán tiền và chỉ ra món nào còn thiếu. Mã khởi đầu chỉ dùng dữ liệu mẫu, chưa có giỏ hàng, đặt hàng hay kết nối cơ sở dữ liệu.

## Chạy bản khởi đầu trên Windows
Máy cần .NET SDK 8. Mở PowerShell ở thư mục chứa README này rồi chạy từng dòng:

```powershell
cd src/MachKit
dotnet build
dotnet run --launch-profile http
```

Mở `http://localhost:5178` (hoặc địa chỉ được Terminal thông báo). Dừng bằng Ctrl+C. Thử nhập 3 bộ (mong đợi: đủ hàng, 63.000 đ), rồi 4 bộ (mong đợi: thiếu 1 điện trở 330 Ω, 84.000 đ). Đây là kết quả mong đợi, không thay cho kết quả tự chạy.

## Tiến độ
- **Tuần 01 (02/11–08/11/2026):** Đã thiết lập GitHub,
  cập nhật README, cấu hình môi trường và chạy được bản khởi đầu.
  [Xem báo cáo tuần 01](progress-report/Tuan-01.md).

## Cấu trúc
`src/MachKit/`: mã nguồn; `setup/`: script và hướng dẫn cài đặt; `progress-report/`: báo cáo tuần; `thesis/doc/`, `thesis/pdf/`, `thesis/abs/`, `thesis/refs/`: báo cáo, bản PDF, tài liệu trình chiếu/demo và nguồn tham khảo.
