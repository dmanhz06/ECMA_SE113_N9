# ECMA — Context cho coding agent

## 1. Cách sử dụng và nguồn yêu cầu

Đây là bản định hướng để agent tìm đúng yêu cầu, giữ đúng scope và tránh lỗi triển khai. Không thay thế SRS hoặc mô tả use case.

- Nguồn nghiệp vụ và nghiệm thu: [SRS_ECMA.md](SRS_ECMA.md), mục 1–9.
- Nguồn kiến trúc: SRS mục 10, đã tích hợp ADR 0001–0004.
- Luồng thao tác chi tiết: [ECMA_Use_Case.md](ECMA_Use_Case.md), 24 use case.
- Trước khi triển khai một chức năng, đọc đầy đủ FR/BR liên quan, ma trận quyền ở SRS mục 3.9 và bảng xử lý lỗi tương ứng ở mục 4. Không triển khai chỉ từ bản tóm tắt này.
- Tất cả FR trong SRS hiện tại đều bắt buộc; không còn phân loại Mandatory/Should của scope cũ.
- Khi tài liệu mâu thuẫn và chưa có quyết định rõ, chỉ hỏi vấn đề đang chặn công việc; không tự thêm quy tắc nghiệp vụ. Quyết định mới cần được ghi lại và đối chiếu với SRS.

### Task còn mở

Trước khi lập kế hoạch hoặc sửa code, quét frontmatter của mọi file `tasks/*.md`:

1. Đọc `name`, `description`, `status`, `related` và `depends_on` để lọc.
2. Bỏ qua task có `status: completed` và task có `description` không liên quan đến chức năng đang làm.
3. Đọc toàn bộ nội dung task còn mở có liên quan (`pending`, `blocked` hoặc `in_progress`), rồi đưa dependency/blocker của chúng vào kế hoạch.
4. Không tự triển khai task `blocked` khi dependency chưa tồn tại. Khi hoàn tất đủ điều kiện nghiệm thu, đổi `status` thành `completed`; không xóa file để còn lịch sử quyết định.

Mỗi file task phải có YAML frontmatter tối thiểu:

```yaml
---
name: Tên ngắn của task
description: Mô tả đủ để agent quyết định task liên quan hay không mà chưa cần đọc toàn bộ file.
status: pending # pending | blocked | in_progress | completed
related:
  - FR/BR/UC/NFR hoặc module liên quan
depends_on:
  - Dependency cần có; để [] nếu không có
---
```

Các liên kết trên giả định tài liệu nằm cùng thư mục với file này. Nếu repo dùng đường dẫn khác, tìm đúng tài liệu theo tên trước khi làm việc.

## 2. Sản phẩm và giới hạn scope

ECMA quản lý sự kiện/hội nghị trực tiếp theo luồng:

**Tạo Event → lập chương trình → công bố → đăng ký tham dự → quản lý người tham dự → điểm danh → xem thống kê của Event.**

- Mọi sự kiện miễn phí. Mỗi tài khoản đăng ký đúng một suất cho chính mình tại mỗi Event; xác nhận ngay, không chờ duyệt.
- Chỉ có ba vai trò lưu trong tài khoản: Attendee, Organizer, Admin. Guest là người chưa đăng nhập.
- Conference là Event có `type = Conference`, dùng chung mô hình/luồng; phải có ít nhất một mục chương trình hợp lệ trước khi công bố. Không phân biệt Conference bằng tag.
- Giao diện tiếng Việt, trình duyệt desktop từ 1024 px. Thông báo kết quả thao tác trên trang vẫn bắt buộc.
- Không thêm bán vé/loại vé/đơn hàng/thanh toán giả lập/giữ chỗ/hoàn tiền; Staff/phân công; tag; dashboard/biểu đồ/báo cáo/xuất CSV; upload ảnh/avatar/ảnh bìa; thêm người tham dự thủ công/đăng ký hộ/chuyển suất.
- Không thêm email/thông báo tự động/quên mật khẩu/social login/2FA/QR/waitlist; lịch cá nhân; hoàn tác điểm danh; điểm danh sớm; thực thể diễn giả/phòng/track; chương trình song song/đăng ký từng phiên; bản đồ/mobile/responsive mobile; giao diện audit log.
- Không có unpublish, đổi chủ sở hữu, mở lại Event, hoặc job nền chuyển Completed. Những giới hạn ở SRS mục 9 là quyết định đã chấp nhận, không phải tính năng còn thiếu.

## 3. Stack và ranh giới kiến trúc

Một ứng dụng triển khai: **ASP.NET Core MVC, .NET 10, Razor Views, EF Core, PostgreSQL, Tailwind CSS v4; Bun để cài dependency/build CSS**.

| Project | Tham chiếu | Trách nhiệm |
|---|---|---|
| `src/ECMA.Domain` | Không có | Bốn thực thể nghiệp vụ, enum, quy tắc miền, domain exceptions; chỉ thư viện chuẩn .NET |
| `src/ECMA.Application` | Domain | Use-case services, DTO/filter, repository interfaces, `Result`/`Error`, một `AccessPolicy` và các abstraction |
| `src/ECMA.Infrastructure` | Application, Domain | EF Core/Npgsql, repositories, migrations, adapter hashing, đồng hồ và sinh mã/stamp |
| `src/ECMA.Web` | Application, Infrastructure | Composition root/DI, MVC Controllers, ViewModels/Razor Views, cookie/antiforgery, ánh xạ lỗi, CSS |

- Controllers gọi Application, không truy vấn DbContext trực tiếp. Domain/Application không phụ thuộc EF Core, MVC, HTTP hoặc Web/Infrastructure.
- Application kiểm tra quyền qua một `AccessPolicy` với dữ liệu người thao tác hiện tại; không chỉ ẩn nút hoặc kiểm tra ở Controller. HTTP status, redirect và thông báo tiếng Việt thuộc Web.
- Repository interfaces phục vụ User, Event, AgendaItem, Registration. Không dựng generic repository, MediatR hoặc framework CQRS. `IUnitOfWork` phục vụ save và transaction boundaries.
- Application dùng `IClock`, `IPasswordService`, `IRegistrationCodeGenerator`, `ISecurityStampGenerator`. Infrastructure cung cấp implementation thật; fake chỉ dùng trong kiểm thử. Application không gọi trực tiếp Identity hoặc tự sinh mã/stamp bằng random/Guid.
- Không tách frontend/backend, thêm SPA/React/Next.js/JWT hoặc API nội bộ để phục vụ Razor. Endpoint JSON chỉ thêm khi có yêu cầu cụ thể.
- Razor dùng MVC Tag Helpers và Tailwind v4 biên dịch cục bộ qua `@tailwindcss/cli` bằng Bun. CSS xuất dưới `wwwroot`, được layout nạp và có trong artifact publish; khóa dependency bằng manifest/lockfile. Không dùng Bootstrap/CDN runtime hoặc pipeline Tailwind v3.

Đây là kiến trúc đích, không phải khẳng định về code hiện có. Kiểm tra repo trước khi mô tả project, dependency, pipeline hoặc chức năng đã triển khai.

## 4. Bản đồ yêu cầu và use case

| SRS | FR | Use case | Phụ thuộc cần đọc |
|---|---|---|---|
| 3.1 Tài khoản/hồ sơ | `FR-AUTH-01..06` | `UC-AUTH-01..05` | USER, phiên/stamp; điều hướng/seed ở 4.1; hashing/idle timeout ở 10.2 |
| 3.2 Event | `FR-EVT-01..07` | `UC-EVT-01..06` | AUTH/USER, SCH khi sửa/công bố, REG khi hủy; trạng thái hiệu lực |
| 3.3 Chương trình | `FR-SCH-01..03` | `UC-SCH-01` | EVT, quyền sở hữu, khóa chương trình và thao tác đồng thời |
| 3.4 Tra cứu | `FR-SRCH-01..03` | `UC-SRCH-01..02` | EVT, REG để tính chỗ, quyền xem riêng theo BR-SRCH-04 |
| 3.5 Đăng ký | `FR-REG-01..04` | `UC-REG-01..03` | AUTH, EVT, sức chứa, nguồn hủy; thứ tự lỗi 4.3/4.5 |
| 3.6 Người tham dự/thống kê | `FR-PART-01..04` | `UC-PART-01..03` | Quyền quản lý EVT, REG, ATT; thống kê toàn Event |
| 3.7 Điểm danh | `FR-ATT-01..03` | `UC-ATT-01` | EVT/REG, quyền quản lý, cửa sổ thời gian; thứ tự lỗi 4.4 |
| 3.8 Người dùng/quyền | `FR-USER-01..03` | `UC-USER-01..03` | AUTH, Event của chủ sở hữu, Admin Active cuối cùng |

Phụ thuộc không có nghĩa phải triển khai toàn bộ module trước. Làm các tiền đề cần thiết cho chức năng được giao. Truy vết bằng ID đầy đủ; không dùng mã mơ hồ như `FR-05`.

## 5. Quyền và xác thực cần giữ đúng

- Tài khoản mới là Attendee Active. Mỗi User có đúng một vai trò. Organizer quản lý Event mình sở hữu; Admin quản lý mọi Event nhưng vẫn phải tuân trạng thái/thời gian. Admin tạo Event thì chính Admin đó là chủ sở hữu.
- Mọi tài khoản Active có chức năng cá nhân. Organizer/Admin được đăng ký Event người khác, không được đăng ký Event của chính mình. User/owner/source của thao tác lấy từ máy chủ, không tin giá trị trình duyệt chọn.
- Username là định danh đăng nhập, lưu chữ thường và duy nhất; không dùng email. Hồ sơ chỉ có username, họ tên, vai trò; chỉ cho cập nhật họ tên.
- Dùng cookie authentication; `UseAuthentication` trước `UseAuthorization`. Cookie HttpOnly, SameSite=Lax và Secure trên môi trường HTTPS. Mọi thao tác thay đổi dữ liệu phải chống CSRF.
- `IPasswordService` bọc Identity `PasswordHasher<TUser>`/`VerifyHashedPassword`, trả Failed/Verified/VerifiedNeedsRehash. Rehash sau xác minh thành công không được ghi đè password/stamp mới nếu có đổi password đồng thời.
- Mỗi request có xác thực kiểm tra trạng thái/vai trò/stamp từ dữ liệu có thẩm quyền. Inactive hoặc stamp cũ bị từ chối từ request tiếp theo. Đổi password cập nhật hash và stamp nguyên tử, thu hồi mọi phiên cũ và xóa phiên đang thao tác.
- Idle timeout là **30 phút từ request xác thực hợp lệ gần nhất của từng phiên**, kể cả request sau đó trả 400/403/409; file tĩnh không tính. Tại `now - last_activity_at ≥ 30 phút`, từ chối trước khi gia hạn. Không chỉ dựa vào sliding expiration mặc định; triển khai renew mỗi request hợp lệ theo SRS 10.2. Request đồng thời không được làm lùi mốc hoạt động/hạn phiên.
- Đăng ký tài khoản xong về đăng nhập, không tự đăng nhập. Login dùng returnUrl nội bộ an toàn, đích GET được cho phép và có quyền; sai thì bỏ qua. Mặc định Attendee về tra cứu, Organizer/Admin về danh sách quản lý tương ứng. Guest đi từ đăng ký tham dự phải quay về chi tiết Event và bấm xác nhận, không tự đăng ký sau login.
- Seed Admin vận hành lấy credential từ cấu hình, không ghi password vào source/log; chạy lại không tạo trùng hoặc ghi đè tài khoản. Credential mẫu công khai chỉ dành cho seed kiểm thử.
- Luôn giữ ít nhất một Admin Active, kể cả khi nhiều Admin đổi quyền/trạng thái đồng thời. Không hạ Organizer/Admin xuống Attendee hoặc vô hiệu hóa chủ sở hữu còn Draft chưa xóa hoặc Published chưa kết thúc. Đổi quyền/vô hiệu hóa và tạo Event phải tương đương một thứ tự tuần tự hợp lệ.

## 6. Trạng thái, dữ liệu và bất biến trọng yếu

### Event và chương trình

- CSDL lưu Draft/Published/Cancelled. **Completed suy ra** khi Published và `now ≥ end_at`; dùng nhất quán trong hiển thị, lọc, thống kê và điều kiện nghiệp vụ. Draft quá giờ không tự Completed.
- Draft được sửa mọi trường, sửa chương trình, công bố hoặc xóa mềm. Tạo/sửa/công bố phải bảo đảm bắt đầu ở tương lai; Event dài 5 phút–30 ngày, capacity 1–10.000.
- Published trước giờ bắt đầu: chỉ sửa tiêu đề/mô tả/địa điểm và giữ nguyên/tăng capacity; khóa loại/thời gian/chương trình. So sánh capacity với dữ liệu hiện tại lúc ghi. Đổi địa điểm cần xác nhận **ở UI**, không có cờ server bắt buộc.
- Từ giờ bắt đầu khóa mọi trường; chỉ xem/điểm danh trong thời gian đang diễn ra. Cancelled/Completed chỉ xem. Chỉ hủy Published khi `now < start_at`, lý do 1–200 ký tự.
- Hủy Event lưu người/thời điểm/lý do ngay cả khi không có Registration; hủy mọi Confirmed nguồn EVENT trong cùng thao tác nguyên tử, giữ nguyên các Cancelled cũ.
- Agenda tối đa 50 mục; mỗi mục ít nhất 5 phút, nằm hoàn toàn trong Event. Chồng lấn khi `A.start < B.end AND B.start < A.end`; chạm biên được phép. Khi sửa không so với chính mục đó. Đổi thời gian Draft làm chương trình ra ngoài khoảng mới phải bị từ chối, không tự dời/cắt/xóa.

### Registration và điểm danh

- Bốn thực thể nghiệp vụ: **User, Event, AgendaItem, Registration**. Đọc schema/bất biến ở SRS mục 6; cho phép trường/bảng kỹ thuật cần cho framework, phiên hoặc đồng thời.
- CSDL có unique username chuẩn hóa, mã Registration và `(event_id, user_id)`. Mã `REG-` + 12 chữ hoa ASCII/chữ số, ngẫu nhiên, duy nhất toàn hệ thống; không phải bằng chứng phân quyền.
- Registration chỉ Confirmed/Cancelled; điểm danh bằng cặp `checked_in_at`/`checked_in_by`, không có Used hoặc bảng lịch sử điểm danh.
- Gửi đăng ký lặp trên Confirmed trả bản ghi hiện có sau kiểm tra xác thực/khả năng xem/quyền, kể cả đã đủ chỗ hoặc đóng đăng ký. Đăng ký mới/đăng ký lại cần Published, `now < start_at`, còn chỗ và không phải chủ sở hữu.
- Nguồn hủy do server xác định: SELF khi tự hủy, MANAGER khi hủy qua thao tác quản lý, EVENT khi hủy Event. Admin hủy đăng ký của mình qua màn hình quản lý vẫn là MANAGER.
- Chỉ SELF được đăng ký lại: giữ ID/mã, cập nhật `registered_at`, xóa trường hủy. MANAGER chặn đăng ký lại vĩnh viễn tại Event đó; không có gỡ chặn. Không lưu lịch sử từng lần hủy/đăng ký lại.
- Hủy mới cần Confirmed, chưa điểm danh, Published và `now < start_at`. Tự hủy dùng lý do hệ thống; quản lý hủy cần lý do 1–200 ký tự. Hủy lặp ưu tiên kết quả “Đã hủy” sau kiểm tra bản ghi/quyền, trước trạng thái/thời gian/lý do; không sửa nguồn hoặc trả thêm chỗ.
- Điểm danh chỉ khi Published và `start_at ≤ now < end_at`, đúng Event, Confirmed và chưa điểm danh. Chuẩn hóa mã rồi đổi chữ thường ASCII thành hoa. Request đồng thời chỉ ghi một lần; lần đầu phải được giữ nguyên. Không hoàn tác.
- Inactive/đổi vai trò không tự hủy Registration hoặc mất điểm danh. Confirmed của Inactive vẫn chiếm chỗ và có thể điểm danh nếu đủ điều kiện.
- Chỗ còn lại = capacity − số Confirmed, gồm cả đã điểm danh. Thống kê tính toàn Event, không theo bộ lọc/phân trang; số đã hủy là số bản ghi hiện Cancelled. Công thức tỷ lệ/vắng mặt theo SRS 3.6, không lưu các số suy ra thành nguồn dữ liệu độc lập.

## 7. Dữ liệu đầu vào, quyền xem và thứ tự lỗi

- Lưu thời điểm UTC, nhập/hiển thị Asia/Ho_Chi_Minh. Dùng `IClock`; lấy một `now` sau khi chờ quyền truy cập dữ liệu, ngay trước kiểm tra/cập nhật và dùng chung trong thao tác. Không tin đồng hồ trình duyệt hoặc kết quả kiểm tra cũ trên form.
- Văn bản: NFC → trim → validate theo Unicode code point; plain text. Password/xác nhận giữ nguyên, không NFC/trim; returnUrl dùng quy tắc riêng. Đọc độ dài/regex đầy đủ trong SRS.
- Tìm chuỗi con, không phân biệt hoa/thường nhưng phân biệt dấu; không cho wildcard người dùng. Bộ lọc kết hợp AND, ngày bắt đầu lọc theo UTC+7 gồm hai biên.
- Danh sách phân trang 20 dòng; thiếu page dùng 1. Page được gửi phải là một giá trị `^[0-9]+$`, trong 1–2.147.483.647; sai trả 400, không tự về trang 1. Trang hợp lệ vượt số trang trả rỗng; đổi bộ lọc ở UI về trang 1. Agenda hiển thị toàn bộ.
- Danh sách công khai chỉ Published với `now < end_at`, gồm đang diễn ra/đủ chỗ. Khi `now ≥ start_at`, tình trạng ưu tiên “Đã đóng đăng ký”. Quy tắc sort/filter từng màn hình theo SRS, không áp dụng một thứ tự chung.
- Draft: mọi Admin xem được, Organizer chỉ Draft mình sở hữu. Cancelled/Completed: Admin, Organizer sở hữu hoặc người từng đăng ký được xem, với phiên hợp lệ/Active. Chi tiết không được xem trả 404, kể cả Guest, không redirect chỉ vì Event bị ẩn. Không công khai người tham dự/mã đăng ký.
- Thao tác ghi cần đăng nhập: **xác thực → định dạng ID → tồn tại/khả năng nhìn thấy → quyền thao tác → dữ liệu/quy tắc nghiệp vụ**. Không để kiểm tra role trả 403 sớm làm lộ Event cần 404.
- Đọc và tuân đúng bảng SRS **4.3 đăng ký, 4.4 điểm danh, 4.5 hủy đăng ký**, dừng ở lỗi đầu tiên; không đảo thứ tự hoặc gộp lý do. Quyền/thời gian điểm danh được kiểm tra trước tra mã.
- HTTP/redirect áp dụng cho MVC: phiên không hợp lệ 302 về login; dữ liệu sai 400; không thấy/bị ẩn 404; thiếu quyền trên bản ghi nhìn thấy 403; xung đột nghiệp vụ 409. Form login hợp lệ nhưng sai credential/Inactive render **200**; password hiện tại sai khi đổi password trả **400**. Nội dung thông báo cố định lấy nguyên từ SRS, không tự diễn đạt lại.

## 8. Thực hiện và kiểm chứng

1. Kiểm tra repo và hướng dẫn agent hiện có; xác định đúng tài liệu, chức năng được giao, FR/BR/UC và tiền đề cần làm.
2. Triển khai qua các lớp đã chốt; giữ scope. Không dựng thêm framework hoặc module chỉ để chuẩn bị cho nhu cầu chưa có.
3. Với thao tác ghi, kiểm tra/cập nhật nguyên tử trên dữ liệu hiện tại; kiểm tra quyền lúc ghi khi có cạnh tranh. Chỉ có transaction hoặc unique constraint chưa đủ chứng minh mọi bất biến đúng.
4. Kiểm thử nhánh thành công, quyền sở hữu, trạng thái/thời gian tại biên, lỗi đồng thời cùng đúng, request trực tiếp và các race liên quan. Không chỉ kiểm thử tuần tự hoặc trạng thái UI.
5. Truy vết kết quả tới FR/BR/NFR; báo rõ thay đổi, kiểm chứng đã chạy và phần còn thiếu. Không khẳng định hoàn thành từ mock hoặc khi chưa có bằng chứng.

Các race bắt buộc xem SRS NFR-04, mục 7.2–7.3: tranh suất cuối/đăng ký trùng; đăng ký với hủy Event/tăng capacity; điểm danh cùng mã; Publish với sửa/thêm/xóa agenda/đổi thời gian/Delete; thay đổi agenda với nhau; tăng capacity với nhau; đổi vai trò/vô hiệu hóa với tạo Event; bảo vệ Admin Active cuối cùng. Kết quả phải tương đương một thứ tự hợp lệ, không cập nhật một phần.

Nghiệm thu gồm toàn bộ FR, BR tương ứng và NFR-01..08. Seed/reset và đồng hồ thay thế chỉ dùng cho kiểm thử; dữ liệu mẫu theo SRS 7.1. Đo NFR-05 đúng bộ dữ liệu/mức tải/môi trường đã nêu; kiểm tra Chrome/Edge desktop theo NFR-06. Kiểm tra project references và pipeline CSS/publish riêng theo mục 10.5; không tính chúng thành FR mới.
