# ADR 0005: URL path dùng lowercase kebab-case

- Trạng thái: Chấp nhận
- Ngày: 2026-10-18

## Bối cảnh

Conventional MVC route sinh URL theo tên controller/action, ví dụ `/Account/ChangePassword`. Cách này làm URL phụ thuộc tên mã C# và không thống nhất kiểu chữ.

## Quyết định

Mọi URL path công khai dùng chữ thường và kebab-case (`this-case`). Root `/` là ngoại lệ tự nhiên.

Controller phải khai báo attribute route rõ ràng. Không bật conventional route tạo alias PascalCase. Tên route nhiều từ nối bằng dấu gạch ngang, ví dụ:

- `/account/login`
- `/account/change-password`
- `/account/access-denied`
- `/privacy`

Query-string và tên tham số không thuộc quyết định này. Link và form tiếp tục dùng MVC Tag Helpers để sinh URL từ route metadata.

## Hệ quả

- URL ổn định, không phụ thuộc cách đặt tên action C#.
- URL cũ dạng PascalCase trả về 404; không duy trì redirect tương thích vì ứng dụng chưa phát hành URL công khai.
- Route mới phải được khai báo rõ khi thêm action mới.
