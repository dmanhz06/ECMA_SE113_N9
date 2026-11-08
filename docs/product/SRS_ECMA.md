# SOFTWARE REQUIREMENTS SPECIFICATION (SRS)

## Events and Conferences Management Application (ECMA)

| Thông tin | Nội dung |
|---|---|
| Phiên bản | 1.0 |
| Loại ứng dụng | Ứng dụng web quản lý sự kiện và hội nghị trực tiếp |
| Mục đích | Mô tả yêu cầu chức năng, yêu cầu phi chức năng và tiêu chí nghiệm thu của ứng dụng ECMA |
| Ngôn ngữ giao diện | Tiếng Việt |

## 1. Mục tiêu và phạm vi

ECMA hỗ trợ một quy trình hoàn chỉnh: **tạo sự kiện/hội nghị → lập chương trình → công bố → đăng ký tham dự → quản lý người tham dự → điểm danh → xem thống kê**.

Phạm vi của ứng dụng:

- Mọi sự kiện/hội nghị đều **miễn phí**. Không bán vé, không tạo đơn hàng và không thanh toán, kể cả thanh toán giả lập.
- Mỗi tài khoản đăng ký **một suất cho chính mình** tại mỗi sự kiện. Không đăng ký hộ, không chọn số lượng và không chuyển suất tham dự.
- Có ba vai trò: **Attendee, Organizer, Admin**. Organizer trực tiếp quản lý và điểm danh; không có Staff hoặc phân công nhân viên.
- Hội nghị dùng chung mô hình và quy trình với sự kiện; phân biệt bằng trường loại `Event` hoặc `Conference`. Hội nghị phải có chương trình trước khi công bố.
- Giữ thống kê đăng ký và tham dự theo từng sự kiện. Không làm dashboard riêng, biểu đồ, báo cáo tài chính hoặc xuất file.
- Không gửi email/thông báo tự động. Người dùng xem trạng thái trong ứng dụng; thông báo kết quả thao tác ngay trên trang vẫn phải có.

Tất cả FR đều bắt buộc. Phần ngoài phạm vi ở mục 8 không phải backlog bắt buộc.

## 2. Quy ước chung

### 2.1 Người dùng và thuật ngữ

| Thuật ngữ | Ý nghĩa |
|---|---|
| Guest | Người chưa đăng nhập; không phải vai trò lưu trong tài khoản |
| Attendee | Người dùng đăng ký tài khoản, có thể đăng ký tham dự |
| Organizer | Người được Admin cấp quyền tạo và quản lý sự kiện của mình |
| Admin | Người quản lý tài khoản và có quyền quản lý mọi sự kiện |
| Chủ sở hữu | Tài khoản tạo sự kiện; không chuyển quyền sở hữu |
| Event | Thực thể chung cho sự kiện và hội nghị; trường `type` phân biệt hai loại |
| Agenda Item | Một mục chương trình, có tiêu đề và khoảng thời gian |
| Registration | Một bản ghi đăng ký của một tài khoản cho một sự kiện, có mã đăng ký duy nhất |
| Confirmed / Cancelled | Đăng ký còn hiệu lực / đã hủy; điểm danh được lưu riêng |
| FR / BR / NFR | Yêu cầu chức năng / quy tắc nghiệp vụ / yêu cầu phi chức năng |

### 2.2 Dữ liệu, thời gian và danh sách

- Giao diện nhập và hiển thị thời gian theo **Asia/Ho_Chi_Minh (UTC+7)**; lưu các thời điểm trong CSDL theo UTC. Thời gian hiện tại `now` do máy chủ xác định, không lấy từ đồng hồ trình duyệt.
- Với thao tác ghi, máy chủ lấy `now` ngay trước khi kiểm tra/cập nhật dữ liệu, sau khi chờ quyền truy cập dữ liệu nếu có; dùng cùng mốc đó trong toàn bộ thao tác. Không dùng kết quả kiểm tra cũ trên trang thay cho kiểm tra lại trạng thái và sức chứa lúc ghi. Quy tắc áp dụng cả khi gửi yêu cầu trực tiếp hoặc nhiều yêu cầu đồng thời.
- Văn bản người dùng nhập, gồm cả từ khóa tìm kiếm, được **chuẩn hóa Unicode NFC → trim → kiểm tra độ dài/định dạng** rồi lưu hoặc dùng để tìm kiếm. Chuỗi chỉ có khoảng trắng được xem là rỗng. Riêng password và xác nhận password giữ nguyên giá trị, không chuẩn hóa Unicode hoặc trim; returnUrl dùng quy tắc an toàn riêng ở mục 4.1.
- Giới hạn độ dài văn bản tính theo Unicode code point **sau chuẩn hóa NFC**, không theo số byte. Giới hạn password tính theo code point của giá trị nguyên bản. Văn bản là plain text, không hỗ trợ HTML hoặc Markdown. Hai chuỗi tiếng Việt tương đương ở dạng NFC/NFD phải cho cùng kết quả kiểm tra và tìm kiếm.
- Tìm kiếm là tìm chuỗi con, không phân biệt hoa/thường, có phân biệt dấu tiếng Việt; không dùng ký tự đại diện do người dùng nhập. Từ khóa bỏ trống nghĩa là không lọc.
- Các danh sách tra cứu, quản lý Event, Đăng ký của tôi, người tham dự và người dùng có phân trang **20 dòng/trang**. Không gửi `page` thì dùng 1; nếu gửi thì phải là một giá trị khớp `^[0-9]+$`, có giá trị số nguyên từ 1 đến 2.147.483.647. Rỗng, 0, âm, số thực, chữ, quá giới hạn hoặc gửi nhiều giá trị đều trả **400**, thông báo “Tham số trang không hợp lệ”; không tự chuyển về trang 1. Trang hợp lệ vượt số trang hiện có trả danh sách rỗng. Khi đổi bộ lọc, giao diện chủ động về trang 1. Chương trình của một Event hiển thị toàn bộ, không phân trang.
- Các bộ lọc được kết hợp bằng AND. Danh sách rỗng có thông báo; không coi đó là lỗi.

## 3. Yêu cầu chức năng và quy tắc nghiệp vụ

### 3.1 Tài khoản và hồ sơ (AUTH)

| ID | Yêu cầu |
|---|---|
| FR-AUTH-01 | Đăng ký bằng username, họ tên, password và xác nhận password; tạo tài khoản Attendee, Active. |
| FR-AUTH-02 | Đăng nhập bằng username và password; chỉ tài khoản Active được đăng nhập. |
| FR-AUTH-03 | Đăng xuất và hủy phiên hiện tại. |
| FR-AUTH-04 | Xem hồ sơ gồm username, họ tên, vai trò; cập nhật họ tên của chính mình. |
| FR-AUTH-05 | Đổi password bằng password hiện tại, password mới và xác nhận; thành công thì hủy mọi phiên của tài khoản và yêu cầu đăng nhập lại. |
| FR-AUTH-06 | Từ chối truy cập chức năng cần đăng nhập khi phiên hết hạn hoặc tài khoản Inactive. |

**Quy tắc**

- BR-AUTH-01: Username dài 4–20 ký tự, khớp `^[A-Za-z][A-Za-z0-9_]{3,19}$`; lưu dạng chữ thường và duy nhất toàn hệ thống. Đăng nhập không phân biệt hoa/thường ở username. Không được đổi username. Đăng ký với username đã tồn tại (so sánh dạng chữ thường) bị từ chối theo mục 4.2, kể cả khi hai request đồng thời dùng cùng username: request thua ràng buộc duy nhất ở CSDL nhận cùng kết quả như username trùng, không trả lỗi hệ thống.
- BR-AUTH-02: Họ tên bắt buộc, dài 2–100 ký tự. Cho phép Unicode và dấu câu trong tên; không áp dụng regex chỉ cho chữ cái. Hồ sơ và danh sách người tham dự hiển thị họ tên hiện tại.
- BR-AUTH-03: Password dài 8–32 ký tự, có ít nhất một chữ cái ASCII và một chữ số; không có ký tự khoảng trắng. Password phân biệt hoa/thường, xác nhận phải trùng; password mới phải khác password hiện tại.
- BR-AUTH-04: Sai thông tin đăng nhập, username không tồn tại hoặc tài khoản Inactive đều báo chung “Không thể đăng nhập. Kiểm tra thông tin hoặc trạng thái tài khoản”.
- BR-AUTH-05: Phiên hết hạn khi không có yêu cầu đã xác thực trong **30 phút trở lên**. Quyền và trạng thái tài khoản được kiểm tra ở mỗi yêu cầu có xác thực; thay đổi của Admin có hiệu lực từ yêu cầu tiếp theo.
- BR-AUTH-06: User có `security_stamp` (hoặc trường kỹ thuật tương đương của framework). Phiên lưu stamp lúc đăng nhập; máy chủ so với stamp hiện tại ở mỗi yêu cầu có xác thực. Đổi password phải cập nhật hash và tạo stamp mới cùng một thao tác nguyên tử; mọi phiên mang stamp cũ bị từ chối ở yêu cầu tiếp theo, kể cả phiên trên thiết bị khác. Phiên đang thao tác được xóa và chuyển về đăng nhập. Không trả stamp cho người dùng hoặc ghi vào log.

### 3.2 Quản lý sự kiện/hội nghị (EVT)

Organizer thao tác trên sự kiện của mình; Admin thao tác trên mọi sự kiện. Khi Admin tạo sự kiện, Admin đó là chủ sở hữu.

| ID | Yêu cầu |
|---|---|
| FR-EVT-01 | Tạo sự kiện với loại, tiêu đề, mô tả, địa điểm, thời gian bắt đầu/kết thúc và sức chứa; trạng thái ban đầu Draft. |
| FR-EVT-02 | Xem danh sách quản lý và chi tiết; tìm theo tiêu đề, lọc theo loại và trạng thái. Organizer chỉ thấy sự kiện của mình; Admin thấy tất cả. |
| FR-EVT-03 | Chỉnh sửa sự kiện theo trạng thái và các trường được phép trong bảng dưới. |
| FR-EVT-04 | Công bố sự kiện hợp lệ: Draft → Published; mở đăng ký ngay khi công bố. |
| FR-EVT-05 | Hủy sự kiện đã công bố nhưng chưa bắt đầu, kèm lý do; đồng thời hủy tất cả đăng ký Confirmed của sự kiện. |
| FR-EVT-06 | Xóa mềm sự kiện Draft; sự kiện đã xóa không xuất hiện và không truy cập được từ các màn hình nghiệp vụ. |
| FR-EVT-07 | Hiển thị trạng thái Completed khi sự kiện Published đã đến thời điểm kết thúc; áp dụng trạng thái này cho mọi điều kiện nghiệp vụ. |

**Dữ liệu bắt buộc**

| Trường | Ràng buộc |
|---|---|
| Loại | Chính xác một giá trị: `Event` hoặc `Conference`; giao diện hiển thị “Sự kiện” hoặc “Hội nghị” |
| Tiêu đề | 5–100 ký tự |
| Mô tả | 1–3000 ký tự; dùng cho nội dung, thông tin liên hệ và hướng dẫn tham dự |
| Địa điểm | 1–200 ký tự; tên/địa chỉ nơi tổ chức, không tích hợp bản đồ |
| Bắt đầu / kết thúc | Bắt đầu < kết thúc; thời lượng từ **5 phút đến 30 ngày**, gồm hai biên; thời điểm nhập chính xác đến phút |
| Sức chứa | Số nguyên 1–10.000; là số người tối đa có đăng ký Confirmed |

**Trạng thái và thao tác**

| Trạng thái hiệu lực | Điều kiện | Thao tác quản lý được phép |
|---|---|---|
| Draft | Chưa công bố, chưa xóa | Sửa tất cả trường, sửa chương trình, công bố, xóa |
| Published, chưa bắt đầu | Đã công bố và `now < start_at` | Sửa tiêu đề/mô tả/địa điểm, tăng sức chứa; quản lý đăng ký; hủy sự kiện |
| Published, đang diễn ra | `start_at ≤ now < end_at` | Xem dữ liệu và điểm danh |
| Completed | Đã công bố và `now ≥ end_at` | Chỉ xem dữ liệu và thống kê |
| Cancelled | Đã hủy | Chỉ xem dữ liệu và lý do hủy |

- BR-EVT-01: Khi tạo hoặc chỉnh sửa Draft, thời gian bắt đầu phải ở tương lai. Nếu bản nháp đã quá giờ bắt đầu, phải cập nhật thời gian khi sửa và trước khi công bố. Khi công bố, máy chủ kiểm tra lại `now < start_at` và toàn bộ dữ liệu/chương trình; thứ tự kiểm tra và thông báo lỗi theo mục 4.6.
- BR-EVT-02: Conference cần ít nhất **một mục chương trình hợp lệ** để công bố. Event thường có thể không có chương trình. Không có thực thể hay luồng đăng ký riêng cho Conference.
- BR-EVT-03: Từ lúc công bố, khóa loại, thời gian và chương trình. Trước giờ bắt đầu, cho sửa tiêu đề/mô tả/địa điểm và **chỉ tăng sức chứa**, tối đa 10.000; giữ nguyên sức chứa được phép, giảm dù chưa có đăng ký vẫn bị từ chối. So sánh sức chứa mới với giá trị hiện tại lúc ghi, không với giá trị cũ trên form. Với Published, nếu địa điểm sau chuẩn hóa khác giá trị hiện tại, **giao diện** phải hỏi: “Địa điểm mới sẽ hiển thị cho người tham dự. Hệ thống không gửi thông báo về thay đổi này”; chọn Hủy không gửi yêu cầu lưu, chọn Xác nhận mới gửi. Đây là xác nhận ở UI, **không có cờ xác nhận bắt buộc phía server**; request trực tiếp không có cờ vẫn được lưu nếu thỏa quyền/trạng thái/dữ liệu. Từ giờ bắt đầu khóa mọi trường. Không có unpublish, đổi chủ sở hữu hoặc mở lại sự kiện đã hủy/kết thúc. Nếu cần đổi thông tin bị khóa, hủy trước giờ bắt đầu và tạo sự kiện mới.
- BR-EVT-04: Lý do hủy bắt buộc, 1–200 ký tự. Event lưu `cancelled_at`, `cancelled_by` và `cancel_reason` kể cả khi chưa có ai đăng ký. Hủy sự kiện và hủy các đăng ký còn hiệu lực phải cùng thành công hoặc cùng thất bại; đăng ký đã hủy trước đó giữ nguyên nguồn/lý do/người/thời điểm hủy. Không hủy sự kiện khi `now ≥ start_at`.
- BR-EVT-05: Completed được **suy ra từ trạng thái Published và thời gian**, không cần job nền chuyển trạng thái. CSDL chỉ lưu Draft/Published/Cancelled; các danh sách, bộ lọc, thống kê và kiểm tra quyền thao tác đều phải dùng trạng thái hiệu lực trong bảng trên. Draft quá giờ không tự thành Completed.
- BR-EVT-06: Xóa chỉ áp dụng cho Draft; đánh dấu xóa, không xóa tài khoản hoặc lịch sử tham dự. Sự kiện đã công bố không được xóa.
- BR-EVT-07: Danh sách quản lý sắp xếp ngày tạo giảm dần, cùng thời điểm tạo thì ID tăng dần; từ khóa tối đa 100 ký tự. Quyền sở hữu được xác định ở máy chủ, không tin `owner_id` do trình duyệt gửi.
- BR-EVT-08: Bộ lọc trạng thái có “Tất cả” (`All`, mặc định khi không gửi tham số) và đúng **4 trạng thái**: Draft, Published, Cancelled, Completed. Published gồm cả chưa bắt đầu và đang diễn ra; đây là hai giai đoạn hiển thị, không phải hai giá trị trạng thái. Không có bộ lọc Deleted vì dữ liệu đã xóa bị loại khỏi mọi danh sách. Bộ lọc loại nhận All/Event/Conference, mặc định All. Giá trị bộ lọc trạng thái/loại phải đúng tên và hoa/thường vừa nêu; giá trị rỗng/ngoài tập hoặc nhiều giá trị cho một bộ lọc trả 400, thông báo “Bộ lọc không hợp lệ”.
- BR-EVT-09: Công bố và mọi thao tác sửa Draft/chương trình/xóa phải có hiệu quả như được thực hiện tuần tự, với kiểm tra lại quyền, trạng thái và toàn bộ điều kiện trên dữ liệu hiện tại. Nếu Publish hoàn tất trước thì request sửa trường bị khóa/sửa chương trình/Delete bị từ chối; nếu thay đổi Draft hoàn tất trước thì Publish kiểm tra dữ liệu mới. Nếu mục cuối cùng bị xóa trước, Conference không được Publish; nếu Delete hoàn tất trước, Publish trả 404. Không được tồn tại Conference Published thiếu chương trình, chương trình ngoài thời gian Event, hoặc Event vừa Published vừa bị đánh dấu xóa.

### 3.3 Chương trình sự kiện/hội nghị (SCH)

| ID | Yêu cầu |
|---|---|
| FR-SCH-01 | Chủ sở hữu có vai trò Organizer hoặc Admin thêm, sửa, xóa mục chương trình khi Event còn Draft; Admin được thao tác trên mọi Draft. |
| FR-SCH-02 | Kiểm tra khoảng thời gian và từ chối chương trình chồng lấn. |
| FR-SCH-03 | Hiển thị chương trình theo giờ bắt đầu tăng dần, nhóm theo ngày nếu sự kiện nhiều ngày. |

**Quy tắc**

- BR-SCH-01: Mỗi mục có tiêu đề 3–100 ký tự, thời gian bắt đầu/kết thúc bắt buộc, nội dung tùy chọn tối đa 1000 ký tự. Tên diễn giả ghi trong nội dung; không quản lý hồ sơ diễn giả riêng.
- BR-SCH-02: Tối đa 50 mục/sự kiện. Mỗi mục dài ít nhất 5 phút; thời điểm nhập chính xác đến phút; nằm hoàn toàn trong khoảng thời gian Event, được bằng hai biên.
- BR-SCH-03: Hai mục chồng lấn khi `A.start < B.end AND B.start < A.end`. Chạm biên được phép; không hỗ trợ chương trình song song, nhiều phòng hoặc nhiều track.
- BR-SCH-04: Khi đổi thời gian Draft, nếu có mục nằm ngoài khoảng mới thì từ chối lưu; không tự xóa, cắt ngắn hoặc dời chương trình. Organizer cần sửa/xóa các mục đó trước.
- BR-SCH-05: Chương trình bị khóa ngay khi công bố. Khi sửa một mục, kiểm tra chồng lấn với các mục khác, không so với chính nó. Sửa/xóa/thêm mục và Publish hoặc đổi thời gian Draft phải tuân BR-EVT-09; hai thay đổi chương trình đồng thời cũng không được tạo chồng lấn hoặc vượt 50 mục.

### 3.4 Tra cứu công khai (SRCH)

| ID | Yêu cầu |
|---|---|
| FR-SRCH-01 | Guest và mọi vai trò xem danh sách sự kiện, tìm từ khóa trong tiêu đề/địa điểm; lọc theo loại và khoảng ngày bắt đầu. |
| FR-SRCH-02 | Xem chi tiết công khai: loại, tiêu đề, mô tả, địa điểm, thời gian, tên chủ sở hữu, sức chứa, số chỗ còn lại và chương trình. |
| FR-SRCH-03 | Hiển thị tình trạng “Còn chỗ”, “Đủ chỗ” hoặc “Đã đóng đăng ký”; chỉ cho gửi đăng ký khi đủ điều kiện. |

**Quy tắc**

- BR-SRCH-01: Danh sách công khai chỉ gồm sự kiện Published với `now < end_at`, kể cả sự kiện đang diễn ra và đã đủ chỗ. Sắp xếp thời gian bắt đầu tăng dần, nếu trùng thì ID tăng dần.
- BR-SRCH-02: Từ khóa tối đa 100 ký tự. Khoảng ngày có thể nhập một hoặc hai đầu; nếu có cả hai thì từ ngày ≤ đến ngày. Lọc theo ngày bắt đầu tại UTC+7, bao gồm cả ngày đầu và ngày cuối.
- BR-SRCH-03: Chỗ còn lại = sức chứa − số đăng ký Confirmed, bao gồm cả người đã điểm danh. Khi `now ≥ start_at`, hiển thị “Đã đóng đăng ký” dù còn chỗ; trước đó hiển thị “Đủ chỗ” nếu chỗ còn lại bằng 0.
- BR-SRCH-04: Với Draft, **mọi Admin được xem, kể cả không phải chủ sở hữu**; Organizer chỉ xem Draft mình sở hữu. Guest/Attendee và Organizer khác không được xem. Với Cancelled/Completed, cho xem nếu người dùng là Admin bất kỳ, Organizer sở hữu Event, hoặc người từng đăng ký Event đó; họ vẫn xem được trạng thái/lý do hủy. Các quyền này yêu cầu tài khoản Active và phiên hợp lệ. Người không có quyền xem chi tiết nhận **404 “Không tìm thấy dữ liệu”**, giống Event không tồn tại/đã xóa, kể cả Guest; không chuyển sang đăng nhập khi họ mở trực tiếp chi tiết bị ẩn. Không công khai danh sách người tham dự hoặc mã đăng ký.

### 3.5 Đăng ký tham dự (REG)

| ID | Yêu cầu |
|---|---|
| FR-REG-01 | Người đã đăng nhập đăng ký một suất cho chính mình; xác nhận ngay, không có bước chờ duyệt hoặc thanh toán. |
| FR-REG-02 | Xem “Đăng ký của tôi”: sự kiện, thời gian/địa điểm, mã đăng ký, trạng thái đăng ký, trạng thái sự kiện và điểm danh. |
| FR-REG-03 | Hủy đăng ký của chính mình trước giờ bắt đầu sự kiện. |
| FR-REG-04 | Chỉ cho đăng ký lại sau khi người tham dự tự hủy, nếu sự kiện vẫn cho phép đăng ký và còn chỗ. |

**Quy tắc**

- BR-REG-01: Đăng ký mới/đăng ký lại thành công khi đồng thời: tài khoản Active, Event Published, `now < start_at`, còn chỗ, người dùng không phải chủ sở hữu và chưa bị phía quản lý hủy đăng ký. Organizer/Admin cũng được đăng ký sự kiện của người khác. Thứ tự kiểm tra và thông báo bắt buộc ở mục 4.3.
- BR-REG-02: Chỉ có **một Registration cho mỗi cặp tài khoản–Event**. Sau khi kiểm tra xác thực, khả năng truy cập Event và quyền đối với đăng ký của chính mình, nếu bản ghi đã Confirmed thì gửi lại yêu cầu trả về bản ghi hiện có, kể cả khi đã đủ chỗ/đóng đăng ký; không tạo thêm suất hoặc mã. BR-REG-01 áp dụng cho xác nhận mới hoặc đăng ký lại. Chỉ Registration Cancelled có `cancel_source = SELF` được đăng ký lại: dùng lại bản ghi/mã, chuyển Confirmed, cập nhật thời điểm đăng ký và xóa các trường hủy. `MANAGER` bị chặn đăng ký lại ở Event này, không có chức năng gỡ chặn; `EVENT` không được đăng ký lại vì Event Cancelled không mở lại. Không suy ra nguồn hủy chỉ từ `cancelled_by` hoặc nội dung lý do.
- BR-REG-03: Mã dạng `REG-` + 12 chữ hoa ASCII/chữ số, ngẫu nhiên, duy nhất toàn hệ thống và không đổi khi đăng ký lại. Mã chỉ phục vụ tra cứu/điểm danh, không thay thế kiểm tra quyền truy cập.
- BR-REG-04: Không vượt sức chứa khi nhiều người đăng ký đồng thời, kể cả đăng ký lại. Với một chỗ cuối và hai người khác nhau, chỉ một người thành công; người còn lại được báo đủ chỗ. Đăng ký đồng thời với hủy Event không được để lại Confirmed trên Event Cancelled.
- BR-REG-05: Người tham dự tự hủy khi Registration Confirmed, chưa điểm danh, Event Published và `now < start_at`. Tại đúng giờ bắt đầu thì không được hủy. Hủy chuyển Cancelled và trả lại một chỗ ngay; máy chủ lưu `cancel_source = SELF`, `cancelled_by` là người tham dự và lý do “Người tham dự tự hủy”.
- BR-REG-06: Sau kiểm tra xác thực, bản ghi và quyền, **kiểm tra Cancelled trước trạng thái Event, thời gian, điểm danh và lý do hủy**. Gửi lại yêu cầu hủy một Registration đã Cancelled trả kết quả “Đã hủy”, kể cả đã tới/quá giờ bắt đầu hoặc Event đã hủy/kết thúc; không thay đổi dữ liệu hoặc trả thêm chỗ. Người dùng không được hủy bản ghi của người khác. Tự hủy và hủy bởi quản lý dùng chung thứ tự ở mục 4.5.
- BR-REG-07: “Đăng ký của tôi” gồm cả Confirmed/Cancelled và sự kiện đã kết thúc; sắp xếp lần đăng ký gần nhất giảm dần, trùng thời điểm thì ID tăng dần. Không có màn hình lịch riêng.
- BR-REG-08: Đăng ký theo cả sự kiện, không theo từng mục chương trình. Cho phép một người đăng ký nhiều sự kiện trùng giờ; hệ thống không kiểm tra xung đột lịch cá nhân.

### 3.6 Người tham dự và thống kê (PART)

| ID | Yêu cầu |
|---|---|
| FR-PART-01 | Organizer xem danh sách đăng ký của Event mình sở hữu; Admin xem của mọi Event. Mỗi dòng là một Registration. |
| FR-PART-02 | Tìm theo username/họ tên/mã đăng ký; lọc theo Confirmed/Cancelled và đã/chưa điểm danh. |
| FR-PART-03 | Organizer/Admin hủy đăng ký còn hiệu lực trước giờ bắt đầu, kèm lý do. |
| FR-PART-04 | Xem thống kê đăng ký và tham dự ngay trong màn hình quản lý người tham dự của Event. |

**Quy tắc**

- BR-PART-01: Các cột gồm username, họ tên hiện tại, mã đăng ký, trạng thái, lần đăng ký gần nhất, thời điểm/người điểm danh, thời điểm/lý do hủy. Sắp xếp lần đăng ký gần nhất tăng dần, trùng thời điểm thì ID tăng dần; từ khóa tối đa 100 ký tự. Bộ lọc trạng thái và điểm danh đều có lựa chọn “Tất cả”.
- BR-PART-02: Điều kiện hủy giống BR-REG-05 nhưng người thao tác phải có quyền quản lý Event; khi hủy mới, lý do bắt buộc 1–200 ký tự và lưu người thực hiện, `cancel_source = MANAGER`. Nguồn hủy do máy chủ xác định từ thao tác, không cho trình duyệt chọn; dùng màn hình quản lý để hủy thì là MANAGER, kể cả Admin hủy đăng ký của chính mình ở Event người khác. Hủy lại dùng thứ tự BR-REG-06/mục 4.5, chỉ báo “Đã hủy”, không đổi nguồn/lý do/người/thời điểm hủy và không trả thêm chỗ; không yêu cầu lại lý do. Không thêm người tham dự thủ công, sửa chủ đăng ký hoặc chuyển SELF thành MANAGER trên bản ghi đã hủy.
- BR-PART-03: Thống kê tính trên **toàn bộ Event**, không đổi theo tìm kiếm/phân trang/bộ lọc danh sách. Số “đã hủy” là số bản ghi hiện đang Cancelled, không phải tổng số lần hủy. Không giữ lịch sử từng lần hủy/đăng ký lại.
- BR-PART-04: Tài khoản Inactive vẫn giữ đăng ký và kết quả điểm danh; Confirmed của tài khoản đó vẫn chiếm chỗ. Người quản lý có thể hủy trước giờ bắt đầu theo BR-PART-02. Việc đổi vai trò không làm mất đăng ký.

| Chỉ số | Công thức |
|---|---|
| Đang đăng ký | Số Registration Confirmed |
| Đã hủy | Số Registration Cancelled |
| Còn chỗ | Sức chứa − đang đăng ký |
| Đã điểm danh | Số Confirmed có `checked_in_at` |
| Chưa điểm danh | Đang đăng ký − đã điểm danh |
| Tỷ lệ tham dự | Đã điểm danh / đang đăng ký × 100%, làm tròn 1 chữ số thập phân; mẫu số 0 thì 0% |
| Vắng mặt | Chỉ hiển thị khi Event Completed; bằng số chưa điểm danh. Trạng thái khác hiển thị “Chưa xác định” |

### 3.7 Điểm danh (ATT)

| ID | Yêu cầu |
|---|---|
| FR-ATT-01 | Organizer của Event hoặc Admin nhập mã đăng ký tại màn hình điểm danh của Event đang chọn. |
| FR-ATT-02 | Kiểm tra điều kiện, hiển thị thành công kèm username/họ tên hoặc từ chối kèm lý do. |
| FR-ATT-03 | Lưu thời điểm và tài khoản thực hiện điểm danh, cập nhật danh sách và thống kê. |

**Quy tắc**

- BR-ATT-01: Chỉ điểm danh khi Event Published và `start_at ≤ now < end_at`, Registration Confirmed, thuộc đúng Event và chưa điểm danh. Đúng giờ bắt đầu được phép; đúng giờ kết thúc bị từ chối. Không có điểm danh sớm.
- BR-ATT-02: Mã nhập được chuẩn hóa NFC, trim rồi chuyển chữ thường ASCII thành chữ hoa; định dạng hợp lệ là `^REG-[A-Z0-9]{12}$`. Từ chối mã không tồn tại, thuộc Event khác, đã hủy hoặc đã điểm danh. Thứ tự kiểm tra và thông báo bắt buộc ở mục 4.4; quyền quản lý Event và thời gian được kiểm tra trước khi tra cứu mã.
- BR-ATT-03: Đã điểm danh thì báo “Đã điểm danh lúc {dd/MM/yyyy HH:mm:ss}” theo UTC+7, giữ nguyên lần đầu. Hai yêu cầu cùng điểm danh một bản ghi chỉ ghi nhận một lần; không tăng số tham dự hai lần.
- BR-ATT-04: Không hoàn tác điểm danh và không điểm danh cho từng mục chương trình. Attendee không tự điểm danh. Trạng thái Inactive của người tham dự không tự làm mất hiệu lực Registration (BR-PART-04).

### 3.8 Quản lý người dùng và quyền (USER)

| ID | Yêu cầu |
|---|---|
| FR-USER-01 | Admin xem danh sách username, họ tên, vai trò, trạng thái, ngày tạo; tìm theo username/họ tên, lọc theo vai trò/trạng thái. |
| FR-USER-02 | Admin đổi vai trò giữa Attendee/Organizer/Admin. Mỗi tài khoản có đúng một vai trò. |
| FR-USER-03 | Admin chuyển tài khoản giữa Active/Inactive; không xóa tài khoản. |

**Quy tắc**

- BR-USER-01: Luôn còn ít nhất một Admin Active. Từ chối mọi thay đổi vai trò/trạng thái làm mất Admin Active cuối cùng, kể cả nhiều Admin thao tác đồng thời. Admin được đổi vai trò/vô hiệu hóa chính mình nếu còn Admin Active khác.
- BR-USER-02: Không hạ quyền Organizer/Admin xuống Attendee hoặc vô hiệu hóa chủ sở hữu còn Event Draft chưa xóa hoặc Published chưa kết thúc. Muốn thực hiện thì phải xóa Draft và hủy Event chưa bắt đầu, hoặc chờ các Event đang diễn ra kết thúc. Đổi Organizer ↔ Admin vẫn được phép vì đều giữ quyền quản lý.
- BR-USER-03: Không đổi chủ sở hữu hoặc tự động hủy đăng ký khi đổi vai trò/trạng thái. Bản ghi lịch sử giữ nguyên; người sở hữu đã thành Attendee không còn quyền quản lý Event cũ.
- BR-USER-04: Tài khoản Inactive không dùng được các chức năng có xác thực, kể cả bằng phiên đã mở. Admin khác phải kích hoạt lại; không có quy trình tự mở khóa.
- BR-USER-05: Danh sách người dùng sắp xếp ngày tạo giảm dần, trùng thời điểm thì ID tăng dần; từ khóa tối đa 100 ký tự. Không trả password hash hoặc thông tin phiên.
- BR-USER-06: Đổi vai trò/vô hiệu hóa và tạo Event mới bởi cùng chủ sở hữu phải có hiệu quả như thực hiện tuần tự. Nếu tạo Draft hoàn tất trước thì hạ quyền/vô hiệu hóa bị từ chối theo BR-USER-02; nếu hạ quyền/vô hiệu hóa hoàn tất trước thì tạo Event bị từ chối theo vai trò/trạng thái hiện tại. Kiểm tra quyền tạo ở lúc ghi dữ liệu, không chỉ khi mở form hoặc bắt đầu request. Áp dụng cả Organizer → Attendee và Admin → Attendee.

### 3.9 Ma trận quyền

Mọi quyền của tài khoản dưới đây yêu cầu đăng nhập bằng tài khoản Active. Quy tắc trạng thái/thời gian ở từng module vẫn áp dụng cho Admin.

| Chức năng | Guest | Attendee | Organizer | Admin |
|---|---|---|---|---|
| Xem/tìm kiếm sự kiện công khai | Có | Có | Có | Có |
| Đăng ký tài khoản/đăng nhập | Có | — | — | — |
| Xem/sửa hồ sơ, đổi password | Không | Bản thân | Bản thân | Bản thân |
| Đăng ký, xem/hủy đăng ký cá nhân | Không | Bản thân | Bản thân, trừ Event của mình | Bản thân, trừ Event của mình |
| Tạo Event | Không | Không | Có | Có |
| Quản lý Event và chương trình | Không | Không | Event của mình | Mọi Event |
| Xem/quản lý người tham dự, thống kê | Không | Không | Event của mình | Mọi Event |
| Điểm danh | Không | Không | Event của mình | Mọi Event |
| Quản lý người dùng và quyền | Không | Không | Không | Có |

## 4. Giao diện và xử lý lỗi

Ứng dụng triển khai bằng **ASP.NET Core MVC trên .NET 10, Razor Views dựng trang ở máy chủ, EF Core và PostgreSQL**, chạy trên trình duyệt desktop, độ rộng từ 1024 px. Giao diện dùng Tailwind CSS v4, biên dịch bằng Bun. Các quyết định kiến trúc bắt buộc và cách áp dụng ADR được chốt ở mục 10.

| Màn hình | Nội dung và quyền |
|---|---|
| Tra cứu sự kiện | Danh sách, tìm kiếm, bộ lọc và phân trang; tất cả người dùng |
| Chi tiết sự kiện/hội nghị | Thông tin, chương trình, tình trạng đăng ký; quyền theo BR-SRCH-04 |
| Đăng ký/đăng nhập | Guest; người đã đăng nhập mở màn hình đăng ký bị chuyển về điểm đến mặc định theo mục 4.1 và không tạo thêm tài khoản |
| Tài khoản của tôi | Hồ sơ và đổi password; mọi tài khoản Active |
| Đăng ký của tôi | Danh sách, mã đăng ký và hủy; mọi tài khoản Active |
| Quản lý sự kiện | Danh sách, tạo/sửa, chương trình, công bố/hủy/xóa; Organizer/Admin |
| Người tham dự và thống kê | Danh sách, hủy đăng ký và các số liệu theo Event; Organizer/Admin |
| Điểm danh | Nhập mã cho một Event cụ thể; Organizer/Admin |
| Quản lý người dùng | Danh sách, đổi quyền/trạng thái; Admin |

- Hiển thị trường bắt buộc, lỗi gần trường nhập; khi lưu lỗi giữ lại dữ liệu hợp lệ, không điền lại password. Xác nhận trước khi hủy Event, hủy đăng ký, xóa Draft, đổi quyền hoặc vô hiệu hóa tài khoản.
- Server kiểm tra dữ liệu và quyền cho mọi thao tác; quy tắc trả lỗi ở mục 4.2–4.5 áp dụng cả yêu cầu gửi trực tiếp. Không xây API chỉ để thỏa tài liệu.

### 4.1 Điều hướng và khởi tạo Admin

| Tình huống | Điểm đến/hành vi |
|---|---|
| Đăng ký tài khoản thành công | Về trang đăng nhập, báo “Đăng ký thành công. Vui lòng đăng nhập”; không tự đăng nhập |
| Guest bấm đăng ký tham dự | Về đăng nhập với returnUrl tới chi tiết Event; nếu chuyển qua đăng ký tài khoản thì giữ returnUrl hợp lệ qua luồng này |
| Đăng nhập thành công, có returnUrl hợp lệ và truy cập được | Đến returnUrl; nếu là chi tiết Event thì **không tự đăng ký tham dự**, phải bấm xác nhận |
| Đăng nhập không có returnUrl, hoặc returnUrl sai/không truy cập được | Attendee về Tra cứu sự kiện; Organizer về Quản lý sự kiện của mình; Admin về Quản lý toàn bộ sự kiện |
| Đăng xuất thành công | Về Tra cứu sự kiện công khai; xóa phiên hiện tại, không dùng returnUrl |
| Đổi password thành công | Xóa phiên hiện tại, về đăng nhập, báo “Đổi mật khẩu thành công. Vui lòng đăng nhập lại” |
| Người đã đăng nhập (phiên hợp lệ) mở hoặc gửi form đăng ký tài khoản | Không tạo tài khoản; redirect HTTP 302 về điểm đến mặc định theo vai trò như dòng “Đăng nhập không có returnUrl”; không hiển thị lỗi, bỏ qua returnUrl và không đổi phiên hiện tại. Phiên không hợp lệ (hết hạn, Inactive, stamp cũ) được xử lý như Guest, không bị chuyển hướng |

- `returnUrl` chỉ nhận đường dẫn **nội bộ đã kiểm tra ở máy chủ**, tới một trang GET hiện có thuộc danh sách màn hình cho phép, trừ đăng ký/đăng nhập/đăng xuất. Cho phép query của trang nếu hợp lệ; không chấp nhận URL tuyệt đối, URL bắt đầu `//`, dấu `\`, ký tự điều khiển hoặc biến thể mã hóa/canonical hóa dẫn ra ngoài ứng dụng. Đích được kiểm tra lại sau đăng nhập bằng quyền hiện tại; không tồn tại hoặc thiếu quyền thì dùng điểm đến mặc định. returnUrl sai bị bỏ qua, không phản chiếu vào link/redirect và không làm đăng nhập thất bại.
- Admin đầu tiên của **môi trường vận hành** được tạo bằng seed khởi tạo CSDL mới: một tài khoản Admin Active, username/họ tên/password lấy từ cấu hình khởi tạo, password được hash và security_stamp được tạo mới. Không có trang công khai tạo Admin. Password seed của Admin vận hành không được đặt trong mã nguồn; không ghi password vào log ở bất kỳ môi trường nào. Seed kiểm thử được phép có tài khoản/mật khẩu mẫu công khai, biết trước, gồm Admin thứ hai theo mục 7.1, nhưng chỉ chạy ở môi trường kiểm thử và không dùng các mật khẩu đó cho tài khoản vận hành. Chạy lại seed không tạo trùng hoặc ghi đè password/quyền/trạng thái của tài khoản đã có.

### 4.2 Quy tắc lỗi chung

Các bảng ở mục 4.3–4.6 được kiểm tra **từ trên xuống**, dừng ở kết quả đầu tiên; không gộp nhiều lý do. Thông báo trong bảng là nội dung cố định; phần trong `{...}` là giá trị được định dạng. Chỉ ghi dữ liệu khi mọi điều kiện đã qua. Các mã HTTP dưới đây áp dụng cho MVC, không giả định có JSON API.

| Trường hợp | Kết quả |
|---|---|
| Thao tác cần xác thực nhưng không có phiên hợp lệ, tài khoản Inactive, phiên hết hạn hoặc stamp cũ | Hủy phiên không hợp lệ, trả redirect HTTP 302 về đăng nhập; hiển thị “Vui lòng đăng nhập bằng tài khoản đang hoạt động”; dùng returnUrl an toàn theo mục 4.1 |
| ID mục tiêu không đúng định dạng | 400, “Dữ liệu không hợp lệ”; kiểm tra sau xác thực với thao tác cần xác thực |
| Bản ghi không tồn tại/đã xóa; hoặc Event không được phép xem theo BR-SRCH-04 | 404, “Không tìm thấy dữ liệu”; không phân biệt hai nguyên nhân |
| Bản ghi nhìn thấy được nhưng thiếu quyền thực hiện thao tác | 403, “Bạn không có quyền thực hiện thao tác này”, trừ thông báo cụ thể ở bảng đăng ký |
| Dữ liệu đầu vào sai | 400; lỗi trường cụ thể; page sai dùng thông báo ở mục 2.2 |
| Đăng ký tài khoản với username đã tồn tại (không phân biệt hoa/thường), kể cả request đồng thời thua ràng buộc duy nhất | 400; hiển thị lại form, lỗi ở trường username “Tên đăng nhập đã tồn tại”; không tạo tài khoản. Dùng 400 vì đây là dữ liệu nhập cần sửa trên form, không phải xung đột trạng thái, sức chứa hoặc chương trình |
| Xung đột trạng thái, sức chứa hoặc chương trình | 409; thông báo nghiệp vụ cụ thể |
| Form đăng nhập thiếu username/password hoặc đầu vào không hợp lệ | 400, hiển thị lại form với lỗi trường; không cấp phiên |
| Form đăng nhập hợp lệ nhưng sai thông tin, username không tồn tại hoặc Inactive | **200**, hiển thị lại form với thông báo BR-AUTH-04; không cấp phiên, không redirect. HTTP 200 ở đây là render form, không phải đăng nhập thành công |
| Đổi password, phiên hợp lệ nhưng password hiện tại sai | **400**, hiển thị lại form, “Mật khẩu hiện tại không đúng”; không đổi hash/stamp hoặc thu hồi các phiên đang hợp lệ |

GET chi tiết Event tuân BR-SRCH-04: người không được xem nhận 404 dù chưa đăng nhập. Các thao tác ghi cần đăng nhập tuân thứ tự **xác thực → định dạng ID → tồn tại/khả năng nhìn thấy bản ghi → quyền thao tác → dữ liệu và quy tắc nghiệp vụ**; riêng hủy lại ưu tiên idempotent như mục 4.5. Không tra cứu mã người tham dự trước khi kiểm tra quyền. Các màn hình quản lý theo vai trò, không nhắm một Event cụ thể, trả 403 nếu sai vai trò. Với đổi password, kiểm tra phiên trước, dữ liệu các trường bắt buộc/định dạng/xác nhận trước, rồi kiểm tra password hiện tại; lỗi trả 400, chỉ thay hash/stamp khi mọi kiểm tra thành công. Đăng ký/đăng nhập là form công khai, không yêu cầu có phiên từ trước.

### 4.3 Thứ tự xử lý đăng ký tham dự

Đăng ký không nhận user_id hoặc nguồn hủy do trình duyệt lựa chọn; người đăng ký lấy từ phiên. Áp dụng BR-REG-01/02 theo thứ tự dưới đây.

| Thứ tự | Điều kiện | Kết quả |
|---|---|---|
| 1 | Không đủ điều kiện xác thực | Theo mục 4.2 |
| 2 | ID sai; Event không tồn tại/đã xóa/không được phép xem | 400 hoặc 404 theo mục 4.2 |
| 3 | Người đăng ký là chủ sở hữu | 403, “Bạn không thể đăng ký sự kiện do mình tổ chức” |
| 4 | Registration của người này đã Confirmed | Thành công, trả bản ghi hiện có, “Bạn đã đăng ký sự kiện này”; không kiểm tra tiếp sức chứa/thời gian |
| 5 | Registration Cancelled có nguồn MANAGER | 403, “Đăng ký đã bị ban tổ chức hủy. Bạn không thể đăng ký lại sự kiện này” |
| 6 | Event Draft hoặc Cancelled | 409, “Sự kiện không mở đăng ký” |
| 7 | `now ≥ start_at`, kể cả Completed | 409, “Đã đóng đăng ký” |
| 8 | Số Confirmed ≥ sức chứa | 409, “Sự kiện đã đủ chỗ” |
| 9 | Các điều kiện trên đều không áp dụng | Xác nhận mới hoặc đăng ký lại nguồn SELF, “Đăng ký thành công” |

Ví dụ Event vừa đủ chỗ vừa đã bắt đầu thì báo “Đã đóng đăng ký”; người bị quản lý hủy thì báo bị chặn trước khi xét đủ chỗ/đóng đăng ký. Người dùng có Confirmed gửi lại yêu cầu sau giờ bắt đầu vẫn nhận bản ghi hiện có, không chiếm thêm chỗ. Nguồn EVENT chỉ xuất hiện trên Event Cancelled và bị từ chối ở bước 6; không có thao tác mở lại.

Với Draft, chỉ Admin không phải chủ sở hữu có thể qua bước 2/3 để nhận lỗi ở bước 6: chủ sở hữu bị chặn ở bước 3, các người khác nhận 404 ở bước 2 theo BR-SRCH-04.

### 4.4 Thứ tự xử lý điểm danh

| Thứ tự | Điều kiện | Kết quả |
|---|---|---|
| 1 | Không đủ điều kiện xác thực | Theo mục 4.2 |
| 2 | ID Event sai; Event không tồn tại/đã xóa/không được phép xem | 400 hoặc 404 theo mục 4.2 |
| 3 | Không có quyền quản lý Event | 403, “Bạn không có quyền thực hiện thao tác này” |
| 4 | Event Draft hoặc Cancelled | 409, “Sự kiện không cho phép điểm danh” |
| 5 | `now < start_at` | 409, “Chưa đến giờ điểm danh” |
| 6 | `now ≥ end_at`, kể cả Completed | 409, “Đã hết giờ điểm danh” |
| 7 | Mã không khớp định dạng sau chuẩn hóa | 400, “Mã đăng ký không hợp lệ” |
| 8 | Không tìm thấy mã | 404, “Không tìm thấy mã đăng ký” |
| 9 | Mã thuộc Event khác | 409, “Mã đăng ký không thuộc sự kiện này” |
| 10 | Registration Cancelled | 409, “Đăng ký đã bị hủy” |
| 11 | Registration đã điểm danh | 409, “Đã điểm danh lúc {dd/MM/yyyy HH:mm:ss}” theo UTC+7 |
| 12 | Đủ điều kiện | Ghi nhận đúng một lần, “Điểm danh thành công”, kèm username/họ tên |

Ví dụ mã đã hủy nhưng Event đã hết giờ thì báo “Đã hết giờ điểm danh”, vì chưa tra cứu mã tại bước đó. Kết quả cuối cùng phải được kiểm tra lại trên dữ liệu hiện tại trong thao tác ghi; request cùng mã chạy đồng thời không được cùng thành công.

### 4.5 Thứ tự xử lý hủy đăng ký

Áp dụng cho tự hủy và hủy bởi quản lý. Request nhắm Event và Registration cụ thể; máy chủ xác định nguồn SELF/MANAGER từ thao tác được gọi, không từ cờ do trình duyệt lựa chọn. Các kiểm tra và ghi dữ liệu vẫn phải nguyên tử khi có request đồng thời.

| Thứ tự | Điều kiện | Kết quả |
|---|---|---|
| 1 | Không đủ điều kiện xác thực | Redirect đăng nhập theo mục 4.2 |
| 2 | ID sai; Event không tồn tại/đã xóa/không được phép xem | 400 hoặc 404 theo mục 4.2 |
| 3 | Registration không tồn tại hoặc không thuộc Event được chọn | 404, “Không tìm thấy dữ liệu” |
| 4 | Tự hủy nhưng không phải chủ Registration; hoặc hủy quản lý nhưng không có quyền quản lý Event | 403, “Bạn không có quyền thực hiện thao tác này” |
| 5 | Registration đã Cancelled | Thành công, “Đã hủy”; không kiểm tra tiếp thời gian/trạng thái/lý do, không ghi lại dữ liệu |
| 6 | Event Draft hoặc Cancelled | 409, “Sự kiện không cho phép hủy đăng ký” |
| 7 | Registration đã điểm danh | 409, “Không thể hủy đăng ký đã điểm danh” |
| 8 | `now ≥ start_at`, kể cả Completed | 409, “Đã bắt đầu sự kiện. Không thể hủy đăng ký” |
| 9 | Hủy quản lý nhưng lý do sau chuẩn hóa rỗng hoặc quá 200 ký tự | 400, “Lý do hủy phải có từ 1 đến 200 ký tự” |
| 10 | Đủ điều kiện | Chuyển Cancelled, lưu nguồn/người/thời điểm/lý do, trả lại một chỗ, “Hủy đăng ký thành công” |

Tự hủy mới dùng lý do hệ thống ở BR-REG-05. Bản ghi đã Cancelled vẫn trả “Đã hủy” sau giờ bắt đầu hoặc khi quản lý không gửi lý do; người không có quyền không được hưởng kết quả idempotent. Gửi lại không được thay nguồn hủy để vượt chặn MANAGER.

### 4.6 Thứ tự xử lý công bố sự kiện

Công bố chỉ nhận ID Event, không nhận dữ liệu nhập; vì vậy mọi lỗi sau khi qua kiểm tra quyền là xung đột với dữ liệu hiện tại và trả **409**, không trả 400. `now` lấy theo mục 2.2. Các điều kiện được kiểm tra từ trên xuống, dừng ở kết quả đầu tiên.

| Thứ tự | Điều kiện | Kết quả |
|---|---|---|
| 1 | Không đủ điều kiện xác thực | Theo mục 4.2 |
| 2 | ID sai; Event không tồn tại/đã xóa/không được phép xem | 400 hoặc 404 theo mục 4.2 |
| 3 | Không có quyền quản lý Event | 403, “Bạn không có quyền thực hiện thao tác này” |
| 4 | Event không còn Draft (đã Published hoặc Cancelled) | 409, “Sự kiện không ở trạng thái bản nháp” |
| 5 | `now ≥ start_at` | 409, “Thời gian bắt đầu phải ở tương lai. Cập nhật thời gian trước khi công bố” |
| 6 | Event là Conference và chưa có mục chương trình nào | 409, “Hội nghị cần ít nhất một mục chương trình để công bố” |
| 7 | Dữ liệu Event hoặc chương trình đã lưu vi phạm ràng buộc mục 3.2/3.3 (độ dài, thời lượng, sức chứa, mục ngoài khoảng Event, chồng lấn, quá 50 mục) | 409, “Dữ liệu sự kiện hoặc chương trình không hợp lệ. Vui lòng kiểm tra lại trước khi công bố” |
| 8 | Các điều kiện trên đều không áp dụng | Chuyển Draft → Published nguyên tử, “Công bố thành công” |

Bước 7 là kiểm tra phòng vệ: các thao tác ghi Draft/chương trình đã kiểm tra ràng buộc nên bước này không phát sinh qua giao diện bình thường, nhưng máy chủ vẫn kiểm tra lại trên dữ liệu hiện tại. Ví dụ Draft đã quá giờ bắt đầu và Conference chưa có chương trình thì báo lỗi thời gian (bước 5) trước. Publish đồng thời với Delete hoàn tất trước nhận 404 ở bước 2; Publish đồng thời với Publish khác hoàn tất trước nhận 409 ở bước 4; Publish đồng thời với xóa mục chương trình cuối hoàn tất trước nhận 409 ở bước 6 (BR-EVT-09).

## 5. Yêu cầu phi chức năng

| ID | Yêu cầu và tiêu chí kiểm chứng |
|---|---|
| NFR-01 | Hash password bằng `Microsoft.AspNetCore.Identity.PasswordHasher<TUser>`, kiểm tra bằng `VerifyHashedPassword` và lưu hash nâng cấp khi nhận `SuccessRehashNeeded` theo mục 10.2; không tự viết thuật toán, lưu thô, trả hash ra giao diện hoặc ghi password vào log. |
| NFR-02 | Kiểm tra xác thực, vai trò và quyền sở hữu ở máy chủ. Thử thay ID của người khác hoặc gọi trực tiếp thao tác phải bị từ chối. |
| NFR-03 | Dùng truy vấn có tham số; encode văn bản khi hiển thị. Payload SQL/HTML/script được xử lý như dữ liệu, không thực thi. Với xác thực cookie, mọi thao tác thay đổi dữ liệu phải có kiểm tra chống CSRF. Kiểm tra returnUrl theo mục 4.1 ở máy chủ; URL ngoài và biến thể mã hóa không được gây open redirect. |
| NFR-04 | Các bất biến về sức chứa, đăng ký trùng, chặn đăng ký lại nguồn MANAGER, điểm danh một lần, hủy Event, Admin cuối cùng, Conference Published có chương trình, chương trình nằm trong Event/không chồng lấn, Published không bị xóa và quyền của chủ sở hữu vẫn đúng khi thao tác đồng thời. Các cặp bắt buộc kiểm thử: Publish với thêm/sửa/xóa chương trình; Publish với đổi thời gian Draft; Publish với Delete; đổi vai trò/vô hiệu hóa với tạo Event mới; tăng sức chứa với đăng ký và với tăng sức chứa khác. Kết quả phải tương đương một thứ tự thực hiện hợp lệ, không để request dùng kiểm tra cũ vượt qua khóa trạng thái/quyền. Thao tác nhiều bước không được để lại dữ liệu cập nhật một phần. |
| NFR-05 | Với 1000 Event và 10.000 Registration, thời gian nhận phản hồi trang tra cứu không quá 3 giây ở ít nhất 95% trong 100 yêu cầu sau khởi động, với 10 phiên đồng thời. Đo cùng môi trường triển khai kiểm thử, ghi cấu hình máy chủ/CSDL và điều kiện mạng trong kết quả. |
| NFR-06 | Kiểm thử các luồng chính trên Chrome và Edge phiên bản được ghi trong biên bản kiểm thử, độ rộng 1024 px trở lên; không có lỗi khiến người dùng không hoàn thành luồng. |
| NFR-07 | Có seed và hướng dẫn đặt lại CSDL kiểm thử; có cách thay đồng hồ máy chủ trong môi trường kiểm thử để kiểm tra các mốc thời gian. Không mở chức năng thay giờ cho người dùng trên môi trường vận hành. |
| NFR-08 | Lỗi bất ngờ có thông báo dễ hiểu, không lộ stack trace hoặc chuỗi kết nối; ghi chi tiết lỗi ở máy chủ. Không yêu cầu màn hình audit log. |

## 6. Mô hình dữ liệu tối thiểu

Đây là mô hình nghiệp vụ, không bắt buộc tên class hoặc tên cột cụ thể. **Cho phép thêm trường/bảng kỹ thuật cần thiết** của framework cho xác thực, phiên hoặc kiểm soát đồng thời; không xem đó là mở rộng nghiệp vụ. `security_stamp` hoặc cơ chế tương đương là bắt buộc để thực hiện FR-AUTH-05. Không lưu riêng thông tin thanh toán, vé, tag, phân công nhân viên hoặc lịch sử điểm danh.

| Thực thể | Thuộc tính nghiệp vụ |
|---|---|
| User | id, username, password_hash, security_stamp, full_name, role, status, created_at |
| Event | id, owner_id, type, title, description, location, start_at, end_at, capacity, status, is_deleted, cancel_reason, cancelled_at, cancelled_by, created_at |
| AgendaItem | id, event_id, title, start_at, end_at, description |
| Registration | id, event_id, user_id, code, status, registered_at, cancel_source, cancelled_at, cancel_reason, cancelled_by, checked_in_at, checked_in_by |

**Quan hệ và bất biến**

- User có nhiều Event và Registration; Event có nhiều AgendaItem và Registration. `owner_id`, `user_id`, `event_id` và các người thực hiện thao tác là khóa ngoại tới bản ghi tương ứng.
- Unique: `User.username`, `Registration.code`, cặp `Registration(event_id, user_id)`. Mã đăng ký và username chuẩn hóa phải có ràng buộc duy nhất tại CSDL.
- Event Cancelled có thời điểm/lý do/người hủy; Event Draft/Published có các trường này bằng null. `Event.cancelled_by` tham chiếu User, độc lập với việc Event có Registration hay không.
- Một Registration Confirmed có các trường hủy bằng null. Registration Cancelled có nguồn/thời điểm/lý do/người hủy, không có điểm danh. `cancel_source` chỉ có SELF/MANAGER/EVENT trên Cancelled; do máy chủ ghi theo thao tác. Khi Event hủy, các Confirmed chuyển Cancelled với nguồn EVENT và `cancelled_by`, thời điểm, lý do lấy từ Event; các Cancelled cũ giữ nguyên.
- `checked_in_at` và `checked_in_by` cùng null hoặc cùng có giá trị; chỉ có giá trị trên Confirmed. Không cần trạng thái Used hoặc bảng lịch sử điểm danh.
- `registered_at` là thời điểm xác nhận gần nhất; chỉ nguồn SELF được đăng ký lại, giữ ID/mã và làm mới thời điểm này. Không lưu lịch sử nhiều lần tự hủy/đăng ký lại. Nguồn MANAGER không được đăng ký lại nên thông tin hủy của quản lý được giữ nguyên.
- Không lưu số chỗ còn lại hoặc tỷ lệ tham dự thành nguồn dữ liệu độc lập; tính từ sức chứa và Registration. Người đã điểm danh vẫn chiếm một suất.
- Chỉ Event Draft được đánh dấu xóa. Không xóa cứng User/Registration; giữ dữ liệu tham dự của Event Cancelled/Completed.
- Các bước kiểm tra và cập nhật liên quan phải thực hiện nguyên tử; không quy định bắt buộc một thư viện hoặc kỹ thuật khóa cụ thể.

## 7. Nghiệm thu và dữ liệu mẫu

### 7.1 Dữ liệu mẫu tối thiểu

- Hai Admin Active để kiểm thử đổi quyền và quy tắc Admin cuối cùng; hai Organizer để kiểm tra cách ly quyền sở hữu; ba Attendee Active và một tài khoản Inactive. Các tài khoản kiểm thử có mật khẩu mẫu biết trước được ghi trong hướng dẫn seed, theo ngoại lệ môi trường kiểm thử ở mục 4.1.
- Event Draft thường; Conference Draft thiếu chương trình; Published sắp tới còn chỗ; Published sắp tới đủ chỗ; Published đang diễn ra; Cancelled; Published đã quá giờ kết thúc để kiểm tra trạng thái hiệu lực Completed; một Draft đã xóa.
- Chương trình hợp lệ, hai mục chạm biên; dữ liệu đầu vào để thử chồng lấn và ngoài khoảng Event. Không seed bản ghi vi phạm bất biến.
- Registration Confirmed chưa điểm danh, Confirmed đã điểm danh và Cancelled với đủ nguồn SELF/MANAGER/EVENT, gắn vào các Event phù hợp. Có Event chưa có đăng ký để kiểm tra thống kê bằng 0; có Event Cancelled không có Registration để kiểm tra `Event.cancelled_by`.

### 7.2 Truy vết kiểm thử

| Nhóm yêu cầu | Trọng tâm kiểm chứng |
|---|---|
| AUTH | Độ dài/định dạng; username trùng khác hoa/thường; password; HTTP 200 với form đăng nhập sai thông tin và 400 khi password hiện tại sai; hết hạn phiên 30 phút tính lại sau mỗi request xác thực; Inactive; đổi password làm stamp cũ vô hiệu ở tất cả phiên; seed chạy lại; điều hướng và returnUrl; người đã đăng nhập mở/gửi form đăng ký bị chuyển hướng; username trùng trả 400, kể cả request đồng thời |
| EVT | Draft → Published/Cancelled; Completed suy ra; 4 giá trị trạng thái; thời lượng đúng 5 phút/30 ngày; trường bị khóa; sửa địa điểm và chỉ tăng sức chứa; Publish đồng thời đổi thời gian Draft/Delete; thứ tự và thông báo lỗi công bố theo mục 4.6 |
| SCH | 5 phút; nằm trong Event; chồng lấn/chạm biên; giới hạn 50 mục; quyền sở hữu; Publish đồng thời thêm/sửa/xóa mục cuối; đổi thời gian Draft đồng thời sửa chương trình |
| SRCH | Loại và khoảng ngày UTC+7; tổ hợp bộ lọc; đủ chỗ/đóng đăng ký; Admin xem Draft người khác, Organizer chỉ xem Draft của mình; 404 khi chi tiết bị ẩn; page sai; NFC/NFD cho cùng kết quả |
| REG | Đăng ký trùng; chỉ đăng ký lại nguồn SELF, chặn MANAGER; đúng giờ bắt đầu; hủy lại sau giờ bắt đầu ưu tiên “Đã hủy” sau kiểm tra quyền; hai người tranh suất cuối; đăng ký đồng thời hủy Event/tăng sức chứa; nhiều điều kiện lỗi cùng đúng theo mục 4.3/4.5 |
| PART | Dữ liệu theo quyền; lý do hủy; thống kê toàn Event; tài khoản Inactive; tỷ lệ mẫu số 0; vắng mặt chỉ sau kết thúc |
| ATT | Đúng Event/mã/trạng thái; trước/đúng giờ bắt đầu và kết thúc; điểm danh đồng thời một lần; nhiều điều kiện lỗi cùng đúng theo mục 4.4 |
| USER | Ma trận quyền; hiệu lực thay đổi từ request tiếp theo; chủ sở hữu còn Event; đổi vai trò/vô hiệu hóa đồng thời tạo Event; Admin cuối cùng khi thao tác đồng thời |
| NFR | CSRF, SQL injection, XSS, open redirect (URL tuyệt đối, //, dấu backslash, ký tự điều khiển và biến thể mã hóa); ghi dữ liệu nguyên tử; mọi cặp xung đột trong NFR-04; thời gian phản hồi và tái lập kiểm thử |

Ứng dụng được nghiệm thu khi hoàn thành toàn bộ FR, đáp ứng NFR và không vi phạm các BR tương ứng. Backlog và test case truy vết tới ID của tài liệu này.

### 7.3 Kết quả bắt buộc cho các race

| Hai thao tác đồng thời | Kết quả hợp lệ |
|---|---|
| Publish Conference và xóa mục chương trình cuối | Publish trước: xóa bị từ chối vì chương trình đã khóa; xóa trước: Publish bị từ chối vì thiếu chương trình |
| Publish và sửa chương trình/đổi thời gian Draft | Publish trước: thao tác sửa bị từ chối; sửa trước: Publish kiểm tra toàn bộ dữ liệu mới và chỉ thành công nếu hợp lệ |
| Publish và Delete Draft | Publish trước: Delete bị từ chối; Delete trước: Publish trả 404. Không được vừa Published vừa đã xóa |
| Organizer/Admin → Attendee và tạo Event | Tạo trước: hạ quyền bị từ chối do còn Draft; hạ quyền trước: tạo bị từ chối vì thiếu quyền. Không có Event mới do Attendee tạo |
| Vô hiệu hóa chủ sở hữu và tạo Event | Tạo trước: vô hiệu hóa bị từ chối do còn Draft; vô hiệu hóa trước: tạo bị từ chối vì phiên/tài khoản không hợp lệ |

Trong kiểm thử đồng thời, không yêu cầu thao tác cụ thể phải thắng; phải kiểm chứng trạng thái cuối và phản hồi phù hợp với một trong các thứ tự hợp lệ. Lặp thử hoặc điều khiển các điểm chờ trong môi trường kiểm thử để đi qua cả hai thứ tự.

## 8. Ngoài phạm vi

Các nội dung sau không thuộc ứng dụng và không phải backlog bắt buộc:

- Bán vé, loại vé, đơn hàng, thanh toán (kể cả giả lập), giữ chỗ, hoàn tiền, phí/thuế/hóa đơn.
- Vai trò Staff và phân công nhân viên điểm danh.
- Tag/danh mục sự kiện; việc phân loại chỉ dùng trường loại `Event`/`Conference`.
- Dashboard, báo cáo (kể cả tài chính), biểu đồ, xuất CSV.
- Hồ sơ nhiều trường, ảnh đại diện, ảnh bìa và mọi chức năng upload ảnh.
- Thêm người tham dự thủ công, nhiều suất mỗi tài khoản, chuyển đăng ký.
- Unpublish; sửa thời gian, loại hoặc chương trình, và giảm sức chứa sau khi công bố.
- Hoàn tác điểm danh; lịch cá nhân riêng; job nền chuyển trạng thái.
- Email/hộp thư thông báo, quên mật khẩu qua email, đăng nhập mạng xã hội, 2FA, QR và quét QR, waitlist, đánh giá, chuyển đăng ký, phí/thuế/hóa đơn, hội nghị trực tuyến, quản lý riêng diễn giả/phòng/track, đăng ký theo phiên, bản đồ, ứng dụng mobile, yêu cầu responsive mobile, báo cáo tài chính, dashboard toàn hệ thống và giao diện audit log.


## 9. Giới hạn và rủi ro đã chấp nhận

| Quyết định giữ | Hệ quả cần biết khi sử dụng/kiểm thử |
|---|---|
| Không hoàn tác điểm danh | Nhập nhầm một mã hợp lệ có thể làm sai số liệu và không sửa lại được trong giao diện. Đây là giới hạn đã chọn để giữ scope; không coi hoàn tác là yêu cầu còn thiếu |
| Không đăng ký tại chỗ, không điểm danh sớm | Người chưa đăng ký khi Event bắt đầu không được cấp suất mới; khách đã đăng ký chỉ điểm danh từ giờ bắt đầu. Không có thao tác bỏ qua mốc thời gian cho Admin |
| Không vô hiệu hóa chủ sở hữu còn Event Draft/Published chưa kết thúc | Nếu Organizer lạm dụng khi Event đang diễn ra, Admin không thể hủy Event hoặc vô hiệu hóa ngay; phải chờ Event kết thúc và xử lý các Draft khác trước. Ứng dụng chưa có quyền đình chỉ khẩn cấp |
| Cho sửa địa điểm trước giờ bắt đầu nhưng không gửi thông báo | Người đã đăng ký phải tự xem thông tin hiện tại; có thể đến địa điểm cũ. Cảnh báo/xác nhận bắt buộc ở BR-EVT-03 không thay thế thông báo cho người tham dự |
| Hủy nguồn MANAGER chặn đăng ký lại, không có gỡ chặn | Hủy nhầm từ màn hình quản lý không thể khôi phục suất trong Event đó; phải cân nhắc ở hộp thoại xác nhận. Hủy SELF mới cho đăng ký lại |

Thời lượng Event tối thiểu 5 phút (mục 3.2) bảo đảm Conference luôn có thể chứa ít nhất một mục chương trình hợp lệ.

## 10. Phụ lục kỹ thuật — quyết định kiến trúc tích hợp từ ADR

Mục này là **phụ lục thiết kế/triển khai** tích hợp các quyết định kiến trúc từ bốn ADR (0001–0004); không phải danh sách use case hoặc tiêu chí nghiệm thu nghiệp vụ bổ sung. Kiểm tra project reference/build CSS thuộc kiểm tra kỹ thuật riêng (mục 10.5), không tính vào số FR. Nghiệp vụ, thứ tự lỗi và điều hướng theo các mục 1–9.

### 10.1 Đối chiếu và phần được áp dụng

| ADR nguồn | Quyết định áp dụng |
|---|---|
| 0001 — `0001-server-rendered-mvc.md` | Một ứng dụng ASP.NET Core MVC, .NET 10, Razor server rendering; EF Core/PostgreSQL; không tách frontend/backend; không thêm API nội bộ chỉ để phục vụ Razor |
| 0002 — `0002-cookie-authentication-password-hashing.md` | Cookie authentication, Identity `PasswordHasher`; kiểm tra trạng thái/vai trò mỗi request; bảo vệ cookie và antiforgery. Điều hướng và Admin đầu tiên theo mục 4.1; thu hồi phiên theo BR-AUTH-06 |
| 0003 — `0003-razor-tailwind-styling.md` | Razor `.cshtml`, MVC Tag Helpers, Tailwind v4 biên dịch cục bộ bằng Bun; tích hợp build CSS vào publish |
| 0004 — `0004-clean-architecture.md` | Bốn project theo bố cục `src/ECMA.*`; Domain độc lập, Application chứa use-case services và AccessPolicy; Infrastructure triển khai persistence; Web là composition root. Giao dịch/đồng thời và đồng hồ điều khiển được theo NFR-04 và NFR-07 |

### 10.2 MVC, xác thực và lưu mật khẩu

- Có **một ứng dụng triển khai** là `ECMA.Web`; Controllers tiếp nhận HTTP, gọi use-case services và trả Razor Views. Không có SPA, ứng dụng API riêng hoặc API nội bộ thay cho việc gọi Application. Endpoint JSON chỉ thêm khi có yêu cầu cụ thể.
- Dùng **ASP.NET Core cookie authentication**. Cấu hình xác thực chạy trước phân quyền (`UseAuthentication` trước `UseAuthorization`). Cookie có `HttpOnly = true`, `SameSite = Lax`, `Secure = true` trong môi trường triển khai HTTPS. Antiforgery và kiểm tra returnUrl vẫn bắt buộc theo NFR-03.
- Application dùng `IPasswordService` tự định nghĩa; Infrastructure triển khai adapter dùng `PasswordHasher<TUser>` và `VerifyHashedPassword`. Adapter trả kết quả trung lập Failed/Verified/VerifiedNeedsRehash, không trả enum/kiểu Identity cho Application. Chỉ lưu hash trong User ở PostgreSQL. Khi nhận VerifiedNeedsRehash (ánh xạ từ `SuccessRehashNeeded`), tạo/lưu hash nâng cấp sau xác minh thành công; không ghi đè password/stamp mới nếu có đổi password đồng thời. Web đăng ký adapter qua dependency injection. Không bắt buộc dùng toàn bộ Identity account/store/UI; không thêm JWT hoặc bearer authentication.
- Idle timeout phải đúng **30 phút kể từ request được xác thực gần nhất của từng phiên** theo BR-AUTH-05. **Mỗi request đi qua xác thực ứng dụng và có phiên hợp lệ phải cập nhật hoạt động/gia hạn**, kể cả thao tác nghiệp vụ sau đó trả 400/403/409; file tĩnh không tính là hoạt động phiên. Tại `now - last_activity_at ≥ 30 phút`, từ chối trước khi gia hạn. Đăng nhập thành công khởi tạo mốc hoạt động; phiên sai stamp/Inactive không được gia hạn.
- Với cookie middleware, dùng `OnValidatePrincipal` kiểm tra phiên/trạng thái/vai trò/stamp và mốc hoạt động; sau khi chấp nhận, yêu cầu renew cho **mọi request hợp lệ** bằng `ShouldRenew = true` và bảo đảm thời hạn phiên mới là mốc hoạt động + 30 phút. Không dựa vào riêng cơ chế `SlidingExpiration` mặc định. Đồng hồ middleware và `IClock` phải dùng cùng nguồn thời gian khi kiểm thử.
- Cho phép metadata kỹ thuật theo từng phiên (`session_id`, `last_activity_at`, hoặc session/ticket store tương đương), theo mục 6. Nếu nhiều request của một phiên chạy đồng thời, cập nhật mốc hoạt động theo giá trị mới nhất, không để ghi/response cũ làm lùi hạn phiên; không dùng một mốc hoạt động chung cho cả tài khoản. Test tối thiểu: đăng nhập ở T0, request hợp lệ ở T0+10 phút, vẫn hợp lệ ở T0+39 phút; khi không có request sau T0+10 phút thì hết hạn đúng T0+40 phút. Hai nhánh dùng phiên kiểm thử độc lập để request ở phút 39 không làm đổi expected result nhánh hết hạn.
- Kiểm tra trạng thái/vai trò/stamp với dữ liệu có thẩm quyền ở mỗi request có xác thực; không chỉ tin claim vai trò còn lưu trong cookie. Đổi password thu hồi phiên theo BR-AUTH-06; điều hướng và Admin đầu tiên theo mục 4.1.
- Cookie authentication, AccessPolicy và Controllers phải phối hợp đúng thứ tự ở mục 4.2–4.6. Không để một kiểm tra vai trò trả 403 sớm làm lộ Event mà SRS yêu cầu trả 404.

Tham khảo API cho phần renew: [Microsoft Learn — CookieValidatePrincipalContext (.NET 10)](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.cookies.cookievalidateprincipalcontext?view=aspnetcore-10.0). `ShouldRenew` yêu cầu middleware renew cookie; quy tắc hết hạn đúng mốc và kiểm tra trạng thái/stamp vẫn do thiết kế ở trên đảm bảo.

### 10.3 Razor và Tailwind

- Trang sản phẩm dùng Razor Views `.cshtml`, MVC Tag Helpers và **Tailwind CSS v4 biên dịch cục bộ**. Bun phục vụ cài dependency và build asset, không phải runtime của một frontend riêng.
- Có script build/watch CSS dùng `@tailwindcss/cli` qua Bun; quét các nguồn Razor, xuất CSS dưới `wwwroot` và nạp file đã biên dịch qua Razor layout. Dùng class utility đầy đủ ở nguồn hoặc đăng ký rõ nguồn/class không được nhận diện tự động; không dùng cấu hình Tailwind v3 cho pipeline v4.
- Build/deployment/publish phải biên dịch và đưa CSS vào artifact. Ghi rõ đường dẫn input/output, lệnh Bun thực tế và bước build trong hướng dẫn dự án; phiên bản package cụ thể được khóa trong manifest/lockfile khi triển khai.
- Không dùng CDN runtime ở môi trường vận hành. Trang sản phẩm dùng Tailwind, không nạp Bootstrap. Không tự bổ sung React/Next.js hoặc thư viện thiết kế khác.
- Giữ label, thông báo validation, khả năng dùng bàn phím và kiểm tra ở máy chủ. Styling không thay đổi quyền, thứ tự lỗi hoặc chức năng được quy định trong SRS.

### 10.4 Clean Architecture và hướng phụ thuộc

Cấu trúc đích:

| Project / thư mục | Project tham chiếu | Trách nhiệm |
|---|---|---|
| `src/ECMA.Domain` | Không có | User, Event, AgendaItem, Registration; enum, quy tắc miền và domain exceptions; chỉ dùng thư viện chuẩn .NET, không phụ thuộc framework web/persistence |
| `src/ECMA.Application` | Domain | Plain use-case service classes; DTO/query filters; repository interfaces; `IUnitOfWork`, `IClock`, `IPasswordService`, `IRegistrationCodeGenerator`, `ISecurityStampGenerator`; `Result`/`Error` và một `AccessPolicy` cho quyền thực hiện use case |
| `src/ECMA.Infrastructure` | Application, Domain | EF Core, Npgsql/PostgreSQL, repository implementations, migrations; adapter Identity password hashing, đồng hồ và triển khai sinh mã/stamp cho môi trường vận hành |
| `src/ECMA.Web` | Application, Infrastructure | Composition root và đăng ký dependency injection; MVC Controllers, Razor Views, ViewModels, cookie/antiforgery, Tailwind asset và ánh xạ lỗi ra HTTP/giao diện |

- Domain/Application không tham chiếu Web/Infrastructure, EF Core, MVC hoặc kiểu HTTP. Infrastructure được Web tham chiếu để lắp ghép dependency; Controllers gọi use-case services, không truy vấn DbContext trực tiếp.
- Authorization trong Application đi qua **một AccessPolicy** và dữ liệu người thao tác hiện tại. Web xác thực và ánh xạ lỗi; không chỉ ẩn nút hoặc chỉ kiểm tra quyền trong Controller. Việc kiểm tra lại quyền lúc ghi vẫn bắt buộc theo BR-USER-06/NFR-04.
- Domain/Application trả mã lỗi hoặc `Result`/`Error` có cấu trúc; HTTP status và câu thông báo tiếng Việt được ánh xạ ở Web theo mục 4.2–4.5. Không đưa display text/redirect/HTML vào Domain hoặc Application.
- Repository interfaces là các ranh giới cụ thể phục vụ bốn thực thể User, Event, AgendaItem và Registration; không dựng generic repository, MediatR hoặc framework CQRS.
- `IUnitOfWork` chỉ phục vụ save và transaction boundaries. EF Core/PostgreSQL phải thực thi các kiểm tra/cập nhật nguyên tử và xử lý đồng thời của NFR-04; có interface này không thay thế kiểm chứng các bất biến.
- Các use case cần thời gian lấy `now` qua `IClock` trong Application theo thời điểm ở mục 2.2. Kiểm thử có thể thay clock; mã hạ tầng cung cấp đồng hồ thật, không mở quyền đổi giờ cho người dùng.
- Các nguồn băm/ngẫu nhiên được thay thế qua các interface dưới đây. Application không tự tạo mã/stamp bằng API random/Guid và không trực tiếp gọi Identity; fake/mock chỉ dùng trong kiểm thử tự động, không được bật ở môi trường vận hành.
- Việc chia project là ranh giới mã nguồn của **một ứng dụng MVC**, không tạo microservices, nhiều ứng dụng triển khai hoặc thêm nghiệp vụ ngoài scope.

| Abstraction trong Application | Hợp đồng và cách thay thế khi kiểm thử tự động |
|---|---|
| `IPasswordService` | Hash password và verify hash; verify trả Failed/Verified/VerifiedNeedsRehash bằng kiểu tự định nghĩa. Fake quyết định kết quả và hash đầu ra để test nhánh đăng nhập/đổi password/rehash mà không phụ thuộc Identity |
| `IRegistrationCodeGenerator` | Sinh mã đúng BR-REG-03 khi tạo Registration mới; không gọi khi trả bản ghi Confirmed hiện có hoặc đăng ký lại dùng mã cũ. Fake trả chuỗi/chuỗi kế tiếp cố định để kiểm tra lưu mã và xử lý trùng mã; ràng buộc unique tại CSDL vẫn bắt buộc |
| `ISecurityStampGenerator` | Sinh stamp mới khi tạo tài khoản hoặc đổi password thành công; giá trị mới phải khác stamp hiện tại. Fake trả giá trị cố định/chuỗi định trước để kiểm tra lưu stamp và thu hồi phiên; không gọi khi password hiện tại sai |

### 10.5 Kiểm tra kỹ thuật riêng

- Kiểm tra project/package references theo mục 10.4; đây không phải tiêu chí nghiệm thu nghiệp vụ.
- Kiểm tra pipeline build/publish có CSS Tailwind đã biên dịch, Razor layout nạp được CSS và artifact không phụ thuộc CDN runtime/tiến trình Bun khi chạy. Đây là kiểm tra build/triển khai, không phải FR hay NFR nghiệm thu nghiệp vụ.
- Kiểm thử tự động kiểm tra hành vi Domain/Application bằng các dependency thay thế ở mục 10.4. Cookie, redirect và trạng thái HTTP được kiểm tra qua giao diện web; không dùng thành phần giả có kết quả cố định để kết luận thuật toán hash thật hoặc mức an toàn của random.
