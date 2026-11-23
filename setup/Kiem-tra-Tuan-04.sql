-- MẠCHKIT — NGHIỆM THU TUẦN 04 / 4E. CHỈ ĐỌC DỮ LIỆU.
-- Kết nối .\SQLEXPRESS trong SSMS, dùng đúng MachKitDb đã hoàn tất 4D.
-- Chạy A trước. Nếu thiếu bảng/cột, dừng; CHƯA chạy B/C/D.
-- Không UPDATE/DELETE/INSERT/CREATE/ALTER/DROP; không tự sửa trạng thái hoặc tồn.
-- Không chụp dữ liệu người nhận, tài khoản, token hoặc mật khẩu vào báo cáo.
-- Tránh có giao dịch thử khác/biểu mẫu Admin sửa tồn chạy cùng lúc đối chiếu.
USE [MachKitDb];
GO

-- A1. Ngữ cảnh; @@SERVERNAME có thể là TEN-MAY\SQLEXPRESS, không nhất thiết là .\SQLEXPRESS.
SET NOCOUNT ON;
SELECT @@SERVERNAME AS MayChu, DB_NAME() AS CoSoDuLieu, SYSDATETIMEOFFSET() AS ThoiDiemMayChu;

-- A2. Phải đủ bảng; Đây là kết quả metadata, không phải truy vấn bất thường.
SELECT required.Name AS BangCanCo,
       CASE WHEN OBJECT_ID(N'dbo.' + required.Name, N'U') IS NOT NULL
            THEN N'Có' ELSE N'THIẾU - DỪNG ĐỐI CHIẾU' END AS KetQua
FROM (VALUES
    (N'LinhKien'), (N'GioHangItem'), (N'DonHang'), (N'ChiTietDonHang'),
    (N'LichSuTrangThaiDonHang'), (N'AspNetUsers'), (N'__EFMigrationsHistory')
) AS required(Name);

-- A3. Kiểm tra các cột cần cho 4C/4D; không tự tạo cột thiếu.
SELECT required.Bang, required.Cot,
       CASE WHEN c.column_id IS NULL THEN N'THIẾU - DỪNG ĐỐI CHIẾU' ELSE N'Có' END AS KetQua,
       TYPE_NAME(c.user_type_id) AS KieuSql, c.is_nullable, c.max_length
FROM (VALUES
    (N'DonHang', N'MaYeuCau'), (N'DonHang', N'TrangThai'), (N'DonHang', N'TongTien'),
    (N'GioHangItem', N'RowVersion'), (N'LichSuTrangThaiDonHang', N'MaThaoTac'),
    (N'LichSuTrangThaiDonHang', N'TrangThaiCu'), (N'LichSuTrangThaiDonHang', N'TrangThaiMoi'),
    (N'LichSuTrangThaiDonHang', N'DaHoanTon')
) AS required(Bang, Cot)
LEFT JOIN sys.columns c ON c.object_id = OBJECT_ID(N'dbo.' + required.Bang) AND c.name = required.Cot;

-- A4. Index chống lặp. Mong đợi 3 dòng Có, unique=1, is_disabled=0.
SELECT expected.Bang, expected.TenIndex,
       CASE WHEN i.index_id IS NULL THEN N'THIẾU - DỪNG ĐỐI CHIẾU' ELSE N'Có' END AS KetQua,
       i.is_unique, i.is_disabled, i.filter_definition
FROM (VALUES
    (N'DonHang', N'UX_DonHang_MaYeuCau'),
    (N'LichSuTrangThaiDonHang', N'UX_LichSuDonHang_MaThaoTac'),
    (N'LichSuTrangThaiDonHang', N'UX_LichSuDonHang_HuyMotLan')
) AS expected(Bang, TenIndex)
LEFT JOIN sys.indexes i ON i.object_id = OBJECT_ID(N'dbo.' + expected.Bang) AND i.name = expected.TenIndex;

-- A5. Kiểm tra index UNIQUE của giỏ và chi tiết (tên do EF sinh có thể khác).
SELECT OBJECT_NAME(i.object_id) AS Bang, i.name AS TenIndex, i.is_unique, i.is_disabled,
       ic.key_ordinal, c.name AS CotKhoa
FROM sys.indexes i
JOIN sys.index_columns ic ON ic.object_id=i.object_id AND ic.index_id=i.index_id
JOIN sys.columns c ON c.object_id=ic.object_id AND c.column_id=ic.column_id
WHERE i.object_id IN (OBJECT_ID(N'dbo.GioHangItem'), OBJECT_ID(N'dbo.ChiTietDonHang'))
  AND i.is_unique=1 AND ic.key_ordinal>0
ORDER BY Bang, TenIndex, ic.key_ordinal;

-- A6. Constraint/check và FK: disabled=0; rà is_not_trusted nếu dữ liệu từng nạp thủ công.
SELECT OBJECT_NAME(parent_object_id) AS Bang, name, is_disabled, is_not_trusted, definition
FROM sys.check_constraints
WHERE parent_object_id IN (OBJECT_ID(N'dbo.GioHangItem'), OBJECT_ID(N'dbo.LichSuTrangThaiDonHang'));
SELECT OBJECT_NAME(parent_object_id) AS Bang, name, is_disabled, is_not_trusted
FROM sys.foreign_keys
WHERE parent_object_id IN (OBJECT_ID(N'dbo.DonHang'), OBJECT_ID(N'dbo.ChiTietDonHang'),
                          OBJECT_ID(N'dbo.GioHangItem'), OBJECT_ID(N'dbo.LichSuTrangThaiDonHang'));
GO

-- B. CHỈ CHẠY KHI PHẦN A ĐỦ CẤU TRÚC. Migrations đã áp dụng và dữ liệu đối chiếu.
SELECT MigrationId, ProductVersion FROM dbo.__EFMigrationsHistory ORDER BY MigrationId;
SELECT TrangThai, COUNT_BIG(*) AS SoDon FROM dbo.DonHang GROUP BY TrangThai;
SELECT TOP (20) Id, NgayDatUtc, TrangThai, TongTien,
       CASE WHEN MaYeuCau IS NOT NULL AND MaYeuCau <> '00000000-0000-0000-0000-000000000000'
            THEN N'Có mã 4C' ELSE N'Đơn cũ/thiếu mã' END AS KieuDon
FROM dbo.DonHang ORDER BY Id DESC;

-- B1. THAY 0 bằng Id thật nhìn thấy trên trang đơn; không tự đoán Id=1.
DECLARE @DonHangId int = 0;
IF @DonHangId <= 0 OR NOT EXISTS(SELECT 1 FROM dbo.DonHang WHERE Id=@DonHangId)
    SELECT N'Chưa chọn Id đơn hợp lệ: điền @DonHangId rồi chạy lại riêng phần B.' AS HuongDan;
ELSE
BEGIN
    SELECT Id, NgayDatUtc, TrangThai, TongTien FROM dbo.DonHang WHERE Id=@DonHangId;
    SELECT Id, LinhKienId, MaLinhKien, TenLinhKien, SoLuong, DonGia, ThanhTien
    FROM dbo.ChiTietDonHang WHERE DonHangId=@DonHangId ORDER BY Id;
    SELECT d.Id, d.TongTien, COALESCE(SUM(c.ThanhTien),0) AS CongChiTiet, COUNT(c.Id) AS SoDong
    FROM dbo.DonHang d LEFT JOIN dbo.ChiTietDonHang c ON c.DonHangId=d.Id
    WHERE d.Id=@DonHangId GROUP BY d.Id,d.TongTien;
    -- Không chọn GhiChu hoặc NguoiThucHienId để tránh đưa ghi chú/tài khoản vào ảnh public.
    SELECT Id, TrangThaiCu, TrangThaiMoi, DaHoanTon, ThoiDiemUtc
    FROM dbo.LichSuTrangThaiDonHang WHERE DonHangId=@DonHangId ORDER BY Id;
    SELECT lk.Id, lk.MaLinhKien, lk.Gia, lk.SoLuongTon, lk.NgungBan
    FROM dbo.LinhKien lk
    WHERE EXISTS(SELECT 1 FROM dbo.ChiTietDonHang c WHERE c.DonHangId=@DonHangId AND c.LinhKienId=lk.Id)
    ORDER BY lk.Id;
    SELECT g.Id, g.LinhKienId, g.SoLuong, g.CapNhatUtc
    FROM dbo.GioHangItem g JOIN dbo.DonHang d ON d.NguoiDungId=g.NguoiDungId
    WHERE d.Id=@DonHangId ORDER BY g.Id;
END;
-- Ghi tồn TRƯỚC thao tác từ Admin/SQL rồi mới đối chiếu SAU.
-- Tồn hiện tại đơn lẻ không cho biết trước đó từng có giảm/hoàn nhầm hay không.
-- Giỏ có thể có dòng mới thêm sau mua; không yêu cầu giỏ cả database phải rỗng.
GO

-- C. TRUY VẤN BẤT THƯỜNG: mong đợi mỗi kết quả sau là 0 dòng.
-- Nếu có dòng: giữ lại bằng chứng và tìm nguyên nhân; không sửa SQL để làm đẹp kết quả.
-- C01: tồn âm.
SELECT N'C01-TonAm' AS KiemTra, Id, MaLinhKien, SoLuongTon
FROM dbo.LinhKien WHERE SoLuongTon<0;
-- C02: giỏ sai số lượng/rowversion.
SELECT N'C02-GioSai' AS KiemTra, Id, LinhKienId, SoLuong
FROM dbo.GioHangItem WHERE SoLuong<=0 OR RowVersion IS NULL OR DATALENGTH(RowVersion)<>8;
-- C03: trùng cặp chủ giỏ - linh kiện. Không xuất NguoiDungId vào ảnh.
SELECT N'C03-GioTrung' AS KiemTra, LinhKienId, COUNT_BIG(*) AS SoDong
FROM dbo.GioHangItem GROUP BY NguoiDungId,LinhKienId HAVING COUNT_BIG(*)>1;
-- C04: trùng yêu cầu đặt. Không xuất GUID vì có thể không cần cho báo cáo công khai.
SELECT N'C04-YeuCauTrung' AS KiemTra, MIN(Id) AS DonDau, MAX(Id) AS DonCuoi, COUNT_BIG(*) AS SoDon
FROM dbo.DonHang WHERE MaYeuCau IS NOT NULL GROUP BY MaYeuCau HAVING COUNT_BIG(*)>1;
-- C05: tổng/chi tiết sai, chỉ áp dụng đơn có mã hợp lệ của luồng 4C.
SELECT N'C05-TongDonSai' AS KiemTra, d.Id, d.TongTien,
       COALESCE(SUM(c.ThanhTien),0) AS CongChiTiet, COUNT(c.Id) AS SoDong
FROM dbo.DonHang d LEFT JOIN dbo.ChiTietDonHang c ON c.DonHangId=d.Id
WHERE d.MaYeuCau IS NOT NULL AND d.MaYeuCau<>'00000000-0000-0000-0000-000000000000'
GROUP BY d.Id,d.TongTien
HAVING COUNT(c.Id)=0 OR d.TongTien<0 OR d.TongTien<>COALESCE(SUM(c.ThanhTien),0);
-- C06: thành tiền từng dòng sai. Cast trước phép nhân để tránh làm phép so sánh ở miền tiền nhỏ.
SELECT N'C06-ChiTietSai' AS KiemTra, c.Id,c.DonHangId,c.SoLuong,c.DonGia,c.ThanhTien
FROM dbo.ChiTietDonHang c JOIN dbo.DonHang d ON d.Id=c.DonHangId
WHERE d.MaYeuCau IS NOT NULL AND d.MaYeuCau<>'00000000-0000-0000-0000-000000000000'
  AND (c.SoLuong<=0 OR c.DonGia<0 OR c.ThanhTien<>CAST(c.DonGia AS decimal(38,0))*c.SoLuong);
-- C07: một đơn có nhiều dòng cho cùng linh kiện.
SELECT N'C07-ChiTietTrung' AS KiemTra, DonHangId,LinhKienId,COUNT_BIG(*) AS SoDong
FROM dbo.ChiTietDonHang GROUP BY DonHangId,LinhKienId HAVING COUNT_BIG(*)>1;
-- C08: trạng thái không thuộc phạm vi hiện tại.
SELECT N'C08-TrangThaiLa' AS KiemTra,Id,TrangThai FROM dbo.DonHang
WHERE TrangThai IS NULL OR TrangThai NOT IN (N'ChoXacNhan',N'DaXacNhan',N'DangGiao',N'HoanThanh',N'DaHuy');
-- C09: mã thao tác trùng.
SELECT N'C09-ThaoTacTrung' AS KiemTra,MIN(Id) AS LogDau,MAX(Id) AS LogCuoi,COUNT_BIG(*) AS SoLan
FROM dbo.LichSuTrangThaiDonHang GROUP BY MaThaoTac HAVING COUNT_BIG(*)>1;
-- C10: nhiều nhật ký hủy cùng đơn.
SELECT N'C10-HuyNhieuLan' AS KiemTra,DonHangId,COUNT_BIG(*) AS SoLan
FROM dbo.LichSuTrangThaiDonHang WHERE TrangThaiMoi=N'DaHuy'
GROUP BY DonHangId HAVING COUNT_BIG(*)>1;
-- C11: dấu hiệu hoàn tồn lệch trạng thái hoặc mã thao tác rỗng.
SELECT N'C11-NhatKySai' AS KiemTra,Id,DonHangId,TrangThaiMoi,DaHoanTon
FROM dbo.LichSuTrangThaiDonHang
WHERE (TrangThaiMoi=N'DaHuy' AND DaHoanTon<>1)
   OR (TrangThaiMoi<>N'DaHuy' AND DaHoanTon<>0)
   OR MaThaoTac='00000000-0000-0000-0000-000000000000';
-- C12: trạng thái cuối không khớp nhật ký. Không giao dịch khác đang chạy khi kiểm tra.
;WITH LastLog AS (
    SELECT DonHangId,TrangThaiMoi,ROW_NUMBER() OVER(PARTITION BY DonHangId ORDER BY Id DESC) AS rn
    FROM dbo.LichSuTrangThaiDonHang
)
SELECT N'C12-TrangThaiKhongKhopLog' AS KiemTra,d.Id,d.TrangThai,l.TrangThaiMoi
FROM dbo.DonHang d JOIN LastLog l ON l.DonHangId=d.Id AND l.rn=1
WHERE d.TrangThai<>l.TrangThaiMoi;
-- C13: bước chuyển ngoài quy tắc 4D.
SELECT N'C13-BuocChuyenSai' AS KiemTra,Id,DonHangId,TrangThaiCu,TrangThaiMoi
FROM dbo.LichSuTrangThaiDonHang
WHERE NOT ((TrangThaiCu=N'ChoXacNhan' AND TrangThaiMoi IN (N'DaXacNhan',N'DaHuy'))
        OR (TrangThaiCu=N'DaXacNhan' AND TrangThaiMoi IN (N'DangGiao',N'DaHuy'))
        OR (TrangThaiCu=N'DangGiao' AND TrangThaiMoi=N'HoanThanh'));
-- C14: đứt chuỗi giữa những log đã có; không ép log đầu phải là tạo đơn vì 4D không dựng lịch sử cũ.
;WITH ChainLog AS (
    SELECT Id,DonHangId,TrangThaiCu,
           LAG(TrangThaiMoi) OVER(PARTITION BY DonHangId ORDER BY Id) AS TrangThaiLogTruoc
    FROM dbo.LichSuTrangThaiDonHang
)
SELECT N'C14-ChuoiLogKhongKhop' AS KiemTra,Id,DonHangId,TrangThaiCu,TrangThaiLogTruoc
FROM ChainLog WHERE TrangThaiLogTruoc IS NOT NULL AND TrangThaiCu<>TrangThaiLogTruoc;
-- C15: chi tiết / nhật ký tham chiếu đơn không tồn tại (nếu FK từng bị vô hiệu).
SELECT N'C15-ChiTietMoCoi' AS KiemTra,c.Id,c.DonHangId FROM dbo.ChiTietDonHang c
LEFT JOIN dbo.DonHang d ON d.Id=c.DonHangId WHERE d.Id IS NULL;
SELECT N'C15-NhatKyMoCoi' AS KiemTra,l.Id,l.DonHangId FROM dbo.LichSuTrangThaiDonHang l
LEFT JOIN dbo.DonHang d ON d.Id=l.DonHangId WHERE d.Id IS NULL;
GO

-- D. THÔNG TIN CẦN ĐỐI CHIẾU, KHÔNG MẶC ĐỊNH YÊU CẦU 0 DÒNG.
-- D1. Đơn di sản được phép thiếu mã; chỉ xem. KHÔNG gán mã giả để mở hủy.
SELECT Id,NgayDatUtc,TrangThai FROM dbo.DonHang
WHERE MaYeuCau IS NULL OR MaYeuCau='00000000-0000-0000-0000-000000000000';
-- D2. Đơn không có log hủy nhưng đã hủy. Với ĐƠN THỬ tạo/xử lý qua 4C–4D, đây là bất thường.
-- Nếu là dữ liệu cũ trước khi có nhật ký, ghi rõ nguồn gốc, không dựng log giả.
SELECT d.Id,d.NgayDatUtc,d.TrangThai FROM dbo.DonHang d
WHERE d.TrangThai=N'DaHuy' AND NOT EXISTS(
    SELECT 1 FROM dbo.LichSuTrangThaiDonHang l WHERE l.DonHangId=d.Id AND l.TrangThaiMoi=N'DaHuy');
-- D3. Giỏ vượt tồn / chứa hàng ngừng bán có thể xảy ra khi kho thay đổi sau thêm giỏ.
-- Không tự xóa dòng; kiểm tra UI cảnh báo và server chặn checkout tương ứng.
SELECT g.Id,g.LinhKienId,g.SoLuong,l.SoLuongTon,l.NgungBan
FROM dbo.GioHangItem g JOIN dbo.LinhKien l ON l.Id=g.LinhKienId
WHERE g.SoLuong>l.SoLuongTon OR l.NgungBan=1;
GO

-- KẾT LUẬN: script này không phát minh tồn ban đầu và không tự trả '100% ĐẠT'.
-- Cần biên bản trước/sau, có đơn thử thật, thử quyền, gửi trùng và rollback để kết luận phạm vi đã chạy.
