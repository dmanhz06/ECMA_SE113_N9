# Task 14 — Event Statistics: bàn giao kỹ thuật

## Phạm vi

Task 14 bổ sung màn hình tổng quan thống kê tại `/events/manage/statistics` bên cạnh thống kê theo từng sự kiện đã có trong màn hình người tham dự. Màn hình tổng quan là phần mở rộng so với SRS §1 và §3.6, vốn chỉ yêu cầu thống kê theo từng Event và không yêu cầu dashboard độc lập. Không thêm biểu đồ, tài chính hoặc xuất file.

## Thay đổi

- `src/ECMA.Domain/Interfaces/IEventRepository.cs`: thêm record projection thống kê và port truy vấn.
- `src/ECMA.Infrastructure/Repositories/EventRepository.cs`: thêm truy vấn projection, lọc soft-delete và scope theo chủ sở hữu; aggregate registration trong PostgreSQL trước khi materialize.
- `src/EMCA.Application/DTOs/EventStatisticsDtos.cs`: DTO thống kê tổng quan và theo sự kiện.
- `src/EMCA.Application/Interfaces/IEventStatisticsService.cs`: application port.
- `src/EMCA.Application/Services/EventStatisticsService.cs`: kiểm tra Active/role/actor, xác định trạng thái hiệu lực bằng `IClock.UtcNow`, tính tỷ lệ và tổng hợp.
- `src/ECMA.Infrastructure/DependencyInjection.cs`: đăng ký service.
- `src/ECMA.Web/Models/EventViewModels.cs`: ViewModels mạnh kiểu.
- `src/ECMA.Web/Controllers/EventsController.cs`: endpoint GET `statistics`, truyền actor và `RequestAborted`.
- `src/ECMA.Web/Views/Events/Statistics.cshtml`: giao diện tiếng Việt, thẻ tổng quan, bảng, thanh sử dụng sức chứa và empty state.
- `src/ECMA.Web/Views/Shared/_Layout.cshtml`: điều hướng tới thống kê.
- `tests/ECMA.Domain.Tests/EventStatisticsServiceTests.cs`: test service cho công thức, trạng thái thời gian, role, inactive và scope Organizer.

## Quy tắc tính

- Chỉ sự kiện chưa soft-delete được truy vấn.
- Completed được suy ra nếu stored status là Published và `EndAt <= IClock.UtcNow`; không ghi trạng thái Completed vào database. Draft quá giờ vẫn là Draft.
- Confirmed/Cancelled được đếm theo status của Registration; CheckedIn chỉ tính bản ghi Confirmed có `CheckedInAt != null`.
- RemainingSlots = Capacity − Confirmed; NotCheckedIn = Confirmed − CheckedIn; tỷ lệ tham dự làm tròn 1 chữ số thập phân và bằng 0 khi Confirmed bằng 0.
- AbsentCount chỉ hiển thị cho Completed và bằng NotCheckedIn.
- Organizer chỉ truy vấn dữ liệu có `OwnerId` của chính mình; Admin truy vấn toàn bộ. Attendee/role không khớp/Inactive bị từ chối ở Application service. Cookie authentication cũng kiểm tra trạng thái tài khoản ở mỗi request.
- Tổng đăng ký là tổng các bản ghi Confirmed và Cancelled thuộc các sự kiện hợp lệ, không phải chỉ các đăng ký Confirmed.

## Truy vấn và hiệu năng

Repository sử dụng projection `AsNoTracking`, gom nhóm Registration theo EventId và đếm tại database; không materialize Registration entities, không có truy vấn cho từng Event (tránh N+1), không phụ thuộc filter/phân trang UI. Không thay đổi schema hoặc migration.

## Hướng dẫn chạy phát triển

Yêu cầu .NET 10 SDK, Docker Compose hoặc PostgreSQL, và Bun để build CSS. Tại thư mục gốc:

1. Sao chép `.env.example` thành `.env`, thay `POSTGRES_PASSWORD` và `SEED_ADMIN_PASSWORD` bằng giá trị phát triển riêng.
2. Khởi động database: `docker compose up -d postgres`.
3. Cấu hình `ConnectionStrings:ECMA` hoặc `ECMA_CONNECTION_STRING` (định dạng Npgsql, database `ecma_dev`).
4. Khôi phục local tool nếu cần: `dotnet tool restore`.
5. Áp dụng migrations vào database phát triển được phép sử dụng: `dotnet ef database update --project src/ECMA.Infrastructure/ECMA.Infrastructure.csproj --startup-project src/ECMA.Web/ECMA.Web.csproj --context EcmaDbContext`.
6. Seed Admin: đặt `SEED_ADMIN_USERNAME`, `SEED_ADMIN_FULL_NAME`, `SEED_ADMIN_PASSWORD`, sau đó chạy `dotnet run --project src/ECMA.Web/ECMA.Web.csproj -- --seed`.
7. Build CSS khi thay đổi Razor: `cd src/ECMA.Web`, `bun install --frozen-lockfile`, `bun run build:css`.
8. Chạy ứng dụng: `dotnet run --project src/ECMA.Web/ECMA.Web.csproj --launch-profile http`. Launch profile hiện khai báo `http://localhost:5054`.
9. Chạy kiểm thử: `dotnet test ECMA.sln`.

Không dùng tài khoản/mật khẩu seed phát triển ngoài database cục bộ dùng một lần. Không chạy reset database trên môi trường có dữ liệu cần giữ.

## Trạng thái xác minh khi đóng gói

Môi trường đóng gói hiện tại không có `dotnet`, `bun`, `docker` hoặc `psql`. Vì vậy chưa thể xác minh restore/build/test, Tailwind compilation, PostgreSQL, migration application hoặc HTTP/UI runtime. Các test được thêm vào source nhưng chưa được thực thi; không xem đây là bằng chứng test pass. Cần chạy lại các bước trên môi trường có SDK và PostgreSQL trước khi tuyên bố DONE.


Bảng kết quả kiểm tra môi trường chi tiết: [Task 14 verification record](task14-verification.md).
