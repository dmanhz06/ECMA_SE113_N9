| STT | Tên Actor | Ý nghĩa/Ghi chú |
|---|---|---|
| 1 | Guest | Người chưa đăng nhập; không phải vai trò lưu trong tài khoản. Có thể tra cứu/xem các Event công khai, đăng ký tài khoản và đăng nhập. |
| 2 | Attendee | Người dùng có tài khoản; có thể quản lý hồ sơ cá nhân, đăng ký/hủy đăng ký tham dự và xem các đăng ký của mình. |
| 3 | Organizer | Kế thừa Attendee: dùng được mọi chức năng cá nhân của Attendee đối với Event của người khác. Là người được Admin cấp quyền tạo và quản lý Event của mình; quản lý chương trình, người tham dự, thống kê và điểm danh. |
| 4 | Admin | Kế thừa Attendee: dùng được mọi chức năng cá nhân của Attendee đối với Event của người khác. Là người quản lý tài khoản và có quyền quản lý mọi Event; quản lý chương trình, người tham dự, thống kê, điểm danh và quyền/trạng thái người dùng. |

| STT | Mã Use Case | Tên Use-case | Actor | Ý nghĩa/Ghi chú |
|---|---|---|---|---|
| 1 | UC-AUTH-01 | Đăng ký tài khoản | Guest | Tạo tài khoản Attendee mới. |
| 2 | UC-AUTH-02 | Đăng nhập | Guest | Xác thực người dùng và tạo phiên làm việc hợp lệ. |
| 3 | UC-AUTH-03 | Đăng xuất | Attendee | Hủy phiên hiện tại và trở về tra cứu sự kiện công khai. |
| 4 | UC-AUTH-04 | Xem hồ sơ | Attendee | Xem username, họ tên và vai trò của chính mình. |
| 5 | UC-AUTH-05 | Cập nhật họ tên | Attendee | Cập nhật họ tên của chính mình. |
| 6 | UC-AUTH-06 | Đổi mật khẩu | Attendee | Đổi password và thu hồi mọi phiên hiện có của tài khoản. |
| 7 | UC-AUTH-07 | Xác thực phiên | Attendee | Kiểm tra phiên, trạng thái tài khoản và security stamp ở mỗi request có xác thực; được các use case cần đăng nhập gọi (include). |
| 8 | UC-EVT-01 | Tạo sự kiện/hội nghị | Organizer, Admin | Tạo Event/Conference mới ở trạng thái Draft. |
| 9 | UC-EVT-02 | Xem danh sách quản lý sự kiện | Organizer, Admin | Xem danh sách Event mình có quyền quản lý; tìm theo tiêu đề, lọc theo loại/trạng thái. |
| 10 | UC-EVT-03 | Chỉnh sửa bản nháp sự kiện/hội nghị | Organizer, Admin | Cập nhật mọi trường của Event còn Draft. |
| 11 | UC-EVT-04 | Cập nhật sự kiện đã công bố | Organizer, Admin | Sửa tiêu đề, mô tả, địa điểm và tăng sức chứa của Event Published chưa bắt đầu. |
| 12 | UC-EVT-05 | Công bố sự kiện/hội nghị | Organizer, Admin | Chuyển Draft hợp lệ sang Published và mở đăng ký. |
| 13 | UC-EVT-06 | Hủy sự kiện/hội nghị | Organizer, Admin | Hủy Event Published chưa bắt đầu và hủy các Registration Confirmed liên quan. |
| 14 | UC-EVT-07 | Xóa bản nháp sự kiện/hội nghị | Organizer, Admin | Xóa mềm Event còn Draft. |
| 15 | UC-EVT-08 | Kiểm tra khả năng truy cập Event | Attendee | Kiểm tra ID và khả năng xem Event của Actor; được các use case thao tác trên một Event cụ thể gọi (include). |
| 16 | UC-SCH-01 | Thêm mục chương trình | Organizer, Admin | Thêm Agenda Item vào Event còn Draft. |
| 17 | UC-SCH-02 | Sửa mục chương trình | Organizer, Admin | Sửa Agenda Item của Event còn Draft. |
| 18 | UC-SCH-03 | Xóa mục chương trình | Organizer, Admin | Xóa Agenda Item của Event còn Draft. |
| 19 | UC-SRCH-01 | Tra cứu sự kiện công khai | Guest, Attendee | Xem danh sách Event công khai; tìm kiếm và lọc theo loại/khoảng ngày. |
| 20 | UC-SRCH-02 | Xem chi tiết sự kiện/hội nghị | Guest, Attendee | Xem chi tiết Event theo quyền, gồm chương trình, sức chứa và tình trạng đăng ký. |
| 21 | UC-REG-01 | Đăng ký tham dự | Attendee | Đăng ký một suất cho chính mình tại một Event. |
| 22 | UC-REG-02 | Xem đăng ký của tôi | Attendee | Xem các Registration của chính mình, gồm mã, trạng thái Event/Registration và điểm danh. |
| 23 | UC-REG-03 | Hủy đăng ký của tôi | Attendee | Tự hủy Registration còn hiệu lực trước giờ bắt đầu khi đủ điều kiện. |
| 24 | UC-REG-04 | Kiểm tra điều kiện hủy đăng ký | Attendee | Kiểm tra Event và Registration có đủ điều kiện để hủy; dùng chung cho tự hủy và hủy bởi quản lý (include). |
| 25 | UC-PART-01 | Xem danh sách người tham dự và thống kê | Organizer, Admin | Xem, tìm kiếm, lọc Registration của một Event và các chỉ số đăng ký/điểm danh của toàn bộ Event. |
| 26 | UC-PART-02 | Hủy đăng ký của người tham dự | Organizer, Admin | Hủy Registration còn hiệu lực của người tham dự trước giờ bắt đầu, kèm lý do. |
| 27 | UC-ATT-01 | Điểm danh người tham dự | Organizer, Admin | Nhập mã Registration để ghi nhận điểm danh trong thời gian Event đang diễn ra. |
| 28 | UC-USER-01 | Xem danh sách người dùng | Admin | Xem, tìm kiếm và lọc người dùng theo vai trò/trạng thái. |
| 29 | UC-USER-02 | Cập nhật vai trò hoặc trạng thái tài khoản | Admin | Đổi vai trò giữa Attendee/Organizer/Admin hoặc chuyển trạng thái Active/Inactive, bảo đảm các bất biến BR-USER-01, BR-USER-02. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-AUTH-01 |
| Tên Use Case | Đăng ký tài khoản |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Guest |
| Mô tả | Guest tạo một tài khoản mới; tài khoản được tạo với vai trò Attendee và trạng thái Active. |
| Tiền điều kiện | Người dùng chưa đăng nhập. |
| Hậu điều kiện | Tạo User mới hợp lệ; chuyển về trang đăng nhập và hiển thị “Đăng ký thành công. Vui lòng đăng nhập”; không tự đăng nhập. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Guest mở form đăng ký.<br>2. Guest nhập username, họ tên, password và xác nhận password.<br>3. Hệ thống chuẩn hóa và kiểm tra dữ liệu, gồm kiểm tra username chưa tồn tại.<br>4. Hệ thống tạo tài khoản Attendee, Active; hash password và tạo security stamp.<br>5. Hệ thống chuyển về trang đăng nhập và thông báo “Đăng ký thành công. Vui lòng đăng nhập”. |
| Luồng thay thế | Form đăng ký được mở kèm returnUrl hợp lệ (Guest bấm đăng ký tham dự từ chi tiết Event): hệ thống giữ returnUrl qua luồng đăng ký và chuyển cùng returnUrl về trang đăng nhập (UC-AUTH-02). |
| Ngoại lệ | - Username sai định dạng: 400, hiển thị lỗi trường; không tạo tài khoản.<br>- Username đã tồn tại (không phân biệt hoa/thường), kể cả request đồng thời thua ràng buộc duy nhất: 400, hiển thị lại form với lỗi tại trường username “Tên đăng nhập đã tồn tại”; không tạo tài khoản.<br>- Họ tên rỗng hoặc ngoài 2–100 ký tự sau chuẩn hóa: 400, hiển thị lỗi trường.<br>- Password/xác nhận không đạt BR-AUTH-03 hoặc không trùng nhau: 400, hiển thị lỗi trường. |
| Includes | Không |
| Extends | Không |
| Yêu cầu đặc biệt | - Username 4–20 ký tự theo regex `^[A-Za-z][A-Za-z0-9_]{3,19}$`, lưu dạng chữ thường, duy nhất không phân biệt hoa/thường; họ tên 2–100 ký tự; password 8–32 ký tự, có chữ cái ASCII, chữ số, không có khoảng trắng; password/xác nhận giữ nguyên, không chuẩn hóa hoặc trim.<br>- Password phải được hash; không lưu/ghi log password thô.<br>- Hai request đồng thời dùng cùng username chỉ được tạo tối đa một User; request còn lại thất bại do ràng buộc duy nhất và nhận kết quả như username trùng (400), không trả lỗi hệ thống.<br>- Màn hình đăng ký chỉ dành cho Guest: người đã đăng nhập (phiên hợp lệ) mở hoặc gửi form đăng ký thì không tạo tài khoản, hệ thống redirect 302 về điểm đến mặc định theo vai trò (như UC-AUTH-02 bước 5), bỏ qua returnUrl, không hiển thị lỗi và không đổi phiên; phiên không hợp lệ được xử lý như Guest.<br>- returnUrl sai bị bỏ qua, không phản chiếu vào link/redirect và không làm đăng ký thất bại. |
| Giả định | Không |
| Ghi chú | Nguồn: FR-AUTH-01; BR-AUTH-01..03; mục 4.1, 4.2; NFR-01, NFR-03, NFR-04. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-AUTH-02 |
| Tên Use Case | Đăng nhập |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Guest |
| Mô tả | Guest đăng nhập bằng username và password để tạo phiên xác thực. |
| Tiền điều kiện | Người dùng chưa đăng nhập. |
| Hậu điều kiện | Thành công: tạo phiên hợp lệ và điều hướng đến trang mặc định theo vai trò hoặc returnUrl hợp lệ. Thất bại: không cấp phiên. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Guest mở form đăng nhập.<br>2. Guest nhập username và password.<br>3. Hệ thống kiểm tra định dạng dữ liệu, tìm username không phân biệt hoa/thường, xác minh password hash và kiểm tra tài khoản Active.<br>4. Hệ thống tạo phiên và lưu thông tin cần thiết để kiểm tra vai trò, trạng thái, security stamp và thời gian hoạt động.<br>5. Hệ thống điều hướng đến trang mặc định theo vai trò: Attendee về Tra cứu sự kiện; Organizer về Quản lý sự kiện của mình; Admin về Quản lý toàn bộ sự kiện. |
| Luồng thay thế | - Có returnUrl hợp lệ và Actor truy cập được: hệ thống điều hướng đến returnUrl thay cho trang mặc định; nếu returnUrl là chi tiết Event thì không tự đăng ký tham dự.<br>- returnUrl không hợp lệ hoặc Actor không truy cập được: bỏ qua returnUrl, dùng trang mặc định; đăng nhập vẫn thành công. |
| Ngoại lệ | - Thiếu username/password hoặc đầu vào form không hợp lệ: 400, hiển thị lại form với lỗi trường; không cấp phiên.<br>- Sai password, username không tồn tại hoặc tài khoản Inactive: HTTP 200, hiển thị lại form với “Không thể đăng nhập. Kiểm tra thông tin hoặc trạng thái tài khoản”; không cấp phiên, không redirect. |
| Includes | Không |
| Extends | Không |
| Yêu cầu đặc biệt | - Idle timeout 30 phút kể từ request xác thực hợp lệ gần nhất (kiểm tra bởi UC-AUTH-07).<br>- returnUrl chỉ nhận đường dẫn nội bộ đã kiểm tra ở máy chủ, tới một trang GET hiện có (trừ đăng ký/đăng nhập/đăng xuất); không chấp nhận URL tuyệt đối, URL bắt đầu `//`, dấu `\`, ký tự điều khiển hoặc biến thể mã hóa dẫn ra ngoài; đích được kiểm tra lại bằng quyền hiện tại sau đăng nhập.<br>- Khi password hash cần nâng cấp (VerifiedNeedsRehash), lưu hash nâng cấp sau khi xác minh thành công; không ghi đè password/stamp mới nếu có đổi password đồng thời. |
| Giả định | Không |
| Ghi chú | - Nguồn: FR-AUTH-02; BR-AUTH-04..06; mục 4.1, 4.2; NFR-01..03.<br>- Liên quan: UC-AUTH-01, UC-AUTH-07. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-AUTH-03 |
| Tên Use Case | Đăng xuất |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Attendee |
| Mô tả | Người dùng hủy phiên hiện tại. |
| Tiền điều kiện | Người dùng đã đăng nhập. |
| Hậu điều kiện | Phiên hiện tại bị xóa; chuyển về trang Tra cứu sự kiện công khai. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Người dùng chọn đăng xuất.<br>2. Hệ thống hủy phiên hiện tại.<br>3. Hệ thống chuyển về Tra cứu sự kiện công khai. |
| Luồng thay thế | Không |
| Ngoại lệ | Không |
| Includes | UC-AUTH-07 |
| Extends | Không |
| Yêu cầu đặc biệt | Không dùng returnUrl sau đăng xuất. |
| Giả định | Không |
| Ghi chú | Nguồn: FR-AUTH-03; mục 4.1. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-AUTH-04 |
| Tên Use Case | Xem hồ sơ |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Attendee |
| Mô tả | Người dùng xem username, họ tên và vai trò của chính mình. |
| Tiền điều kiện | Người dùng đã đăng nhập. |
| Hậu điều kiện | Không thay đổi dữ liệu. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Người dùng mở “Tài khoản của tôi”.<br>2. Hệ thống hiển thị username, họ tên và vai trò. |
| Luồng thay thế | Không |
| Ngoại lệ | Không |
| Includes | UC-AUTH-07 |
| Extends | Không |
| Yêu cầu đặc biệt | - Không cho đổi username.<br>- Dữ liệu văn bản xử lý như plain text và encode khi hiển thị. |
| Giả định | Không |
| Ghi chú | - Nguồn: FR-AUTH-04; BR-AUTH-01, BR-AUTH-02; NFR-03.<br>- Liên quan: UC-AUTH-05. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-AUTH-05 |
| Tên Use Case | Cập nhật họ tên |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Attendee |
| Mô tả | Người dùng cập nhật họ tên của chính mình. |
| Tiền điều kiện | Người dùng đã đăng nhập. |
| Hậu điều kiện | Họ tên mới được lưu và dùng khi hiển thị hồ sơ/danh sách người tham dự. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Người dùng mở “Tài khoản của tôi”.<br>2. Người dùng nhập họ tên mới và gửi lưu.<br>3. Hệ thống chuẩn hóa NFC, trim và kiểm tra họ tên.<br>4. Hệ thống lưu họ tên mới và hiển thị kết quả thành công. |
| Luồng thay thế | Không |
| Ngoại lệ | Họ tên rỗng hoặc ngoài 2–100 ký tự sau chuẩn hóa: 400, hiển thị lỗi trường. |
| Includes | UC-AUTH-07 |
| Extends | Không |
| Yêu cầu đặc biệt | - Họ tên cho phép Unicode và dấu câu; không áp dụng regex chỉ cho chữ cái.<br>- Không cho đổi username. Dữ liệu văn bản xử lý như plain text và encode khi hiển thị. |
| Giả định | Không |
| Ghi chú | - Nguồn: FR-AUTH-04; BR-AUTH-02; mục 2.2; NFR-03.<br>- Liên quan: UC-AUTH-04. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-AUTH-06 |
| Tên Use Case | Đổi mật khẩu |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Attendee |
| Mô tả | Người dùng đổi password bằng password hiện tại, password mới và xác nhận password mới. |
| Tiền điều kiện | Người dùng đã đăng nhập. |
| Hậu điều kiện | Hash password và security stamp được cập nhật nguyên tử; mọi phiên cũ bị vô hiệu; phiên hiện tại bị xóa và người dùng được chuyển về đăng nhập. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Người dùng mở chức năng đổi password.<br>2. Người dùng nhập password hiện tại, password mới và xác nhận.<br>3. Hệ thống kiểm tra dữ liệu bắt buộc, định dạng, xác nhận password mới và xác minh password hiện tại.<br>4. Hệ thống cập nhật hash password và tạo security stamp mới trong cùng thao tác nguyên tử.<br>5. Hệ thống hủy phiên hiện tại, chuyển về đăng nhập và báo “Đổi mật khẩu thành công. Vui lòng đăng nhập lại”. |
| Luồng thay thế | Không |
| Ngoại lệ | - Password mới không hợp lệ, trùng password hiện tại hoặc xác nhận không khớp: 400, hiển thị lại form.<br>- Password hiện tại sai: 400, “Mật khẩu hiện tại không đúng”; không đổi hash/stamp và không thu hồi các phiên đang hợp lệ. |
| Includes | UC-AUTH-07 |
| Extends | Không |
| Yêu cầu đặc biệt | - Password mới 8–32 ký tự, có chữ cái ASCII và chữ số, không có khoảng trắng; không normalize/trim password.<br>- Kiểm tra dữ liệu bắt buộc/định dạng/xác nhận trước, rồi mới kiểm tra password hiện tại.<br>- Các phiên mang stamp cũ, kể cả trên thiết bị khác, bị từ chối ở request tiếp theo (UC-AUTH-07). Không trả stamp cho người dùng hoặc ghi vào log. |
| Giả định | Không |
| Ghi chú | Nguồn: FR-AUTH-05; BR-AUTH-03, BR-AUTH-06; mục 4.1, 4.2; NFR-01. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-AUTH-07 |
| Tên Use Case | Xác thực phiên |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Attendee |
| Mô tả | Hệ thống kiểm tra phiên và tài khoản của Actor ở mỗi request đến chức năng cần đăng nhập, rồi gia hạn phiên hợp lệ. |
| Tiền điều kiện | Request nhắm tới một chức năng cần đăng nhập. |
| Hậu điều kiện | Phiên hợp lệ được gia hạn thêm 30 phút tính từ request này và request được tiếp tục xử lý; phiên không hợp lệ bị hủy. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Hệ thống nhận request đến chức năng cần đăng nhập.<br>2. Hệ thống kiểm tra phiên chưa quá 30 phút kể từ request xác thực gần nhất, tài khoản Active, security stamp khớp và đọc vai trò hiện tại từ dữ liệu có thẩm quyền.<br>3. Hệ thống gia hạn phiên và cho request tiếp tục. |
| Luồng thay thế | Không |
| Ngoại lệ | Không có phiên, phiên hết hạn (từ 30 phút trở lên không có request xác thực), tài khoản Inactive hoặc stamp cũ: hủy phiên, redirect 302 về đăng nhập, hiển thị “Vui lòng đăng nhập bằng tài khoản đang hoạt động”, kèm returnUrl an toàn theo mục 4.1. |
| Includes | Không |
| Extends | Không |
| Yêu cầu đặc biệt | - Mọi request có phiên hợp lệ đều gia hạn phiên, kể cả khi thao tác nghiệp vụ sau đó trả 400/403/409; file tĩnh không tính là hoạt động; phiên sai stamp/Inactive không được gia hạn.<br>- Mốc hoạt động tính riêng cho từng phiên; request đồng thời cập nhật theo giá trị mới nhất, không để ghi cũ làm lùi hạn phiên.<br>- Trạng thái, vai trò và stamp được kiểm tra trên dữ liệu có thẩm quyền, không chỉ tin claim trong cookie; thay đổi của Admin có hiệu lực từ request tiếp theo.<br>- Không trả stamp cho người dùng hoặc ghi vào log. |
| Giả định | Không |
| Ghi chú | - Nguồn: FR-AUTH-06; BR-AUTH-05, BR-AUTH-06; BR-USER-04; mục 4.2, 10.2.<br>- Liên quan: được include bởi mọi use case cần đăng nhập. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-EVT-01 |
| Tên Use Case | Tạo sự kiện/hội nghị |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Organizer, Admin |
| Mô tả | Tạo Event hoặc Conference mới với trạng thái ban đầu Draft; người tạo là chủ sở hữu. |
| Tiền điều kiện | Actor đã đăng nhập. |
| Hậu điều kiện | Event được tạo ở trạng thái Draft, chưa xóa; owner là tài khoản tạo. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Actor mở form tạo Event.<br>2. Actor nhập loại, tiêu đề, mô tả, địa điểm, thời gian bắt đầu/kết thúc và sức chứa.<br>3. Hệ thống chuẩn hóa, kiểm tra toàn bộ dữ liệu và kiểm tra lại quyền tạo tại thời điểm ghi.<br>4. Hệ thống tạo Event Draft với Actor là chủ sở hữu.<br>5. Hệ thống hiển thị kết quả thành công. |
| Luồng thay thế | Không |
| Ngoại lệ | - Tài khoản vẫn Active nhưng vai trò hiện tại không còn Organizer/Admin: 403 “Bạn không có quyền thực hiện thao tác này”.<br>- Dữ liệu không hợp lệ: 400, hiển thị lỗi trường.<br>- Thời gian bắt đầu không ở tương lai, bắt đầu ≥ kết thúc, thời lượng ngoài 5 phút–30 ngày hoặc sức chứa ngoài 1–10.000: 400. |
| Includes | UC-AUTH-07 |
| Extends | Không |
| Yêu cầu đặc biệt | - Loại là Event hoặc Conference; cả hai dùng chung một luồng tạo, không có luồng riêng cho Conference.<br>- Quyền tạo được kiểm tra tại server ở thời điểm ghi, không chỉ khi mở form.<br>- Tạo Event đồng thời với đổi vai trò/vô hiệu hóa chủ sở hữu phải tương đương một thứ tự tuần tự hợp lệ: tạo trước thì hạ quyền/vô hiệu hóa bị từ chối (UC-USER-02); hạ quyền trước thì tạo bị 403; vô hiệu hóa trước thì phiên không hợp lệ và xử lý 302. |
| Giả định | Không |
| Ghi chú | Nguồn: FR-EVT-01; BR-EVT-01; dữ liệu bắt buộc mục 3.2; BR-USER-06; NFR-02, NFR-04. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-EVT-02 |
| Tên Use Case | Xem danh sách quản lý sự kiện |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Organizer, Admin |
| Mô tả | Actor xem các Event mình có quyền quản lý, tìm theo tiêu đề và lọc theo loại/trạng thái. |
| Tiền điều kiện | Actor đã đăng nhập. |
| Hậu điều kiện | Không thay đổi dữ liệu. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Actor mở màn hình Quản lý sự kiện.<br>2. Hệ thống xác định phạm vi: Organizer chỉ Event mình sở hữu; Admin mọi Event.<br>3. Actor có thể nhập từ khóa tiêu đề, chọn loại, chọn trạng thái và chọn trang.<br>4. Hệ thống kiểm tra tham số, áp dụng các bộ lọc bằng AND và trả danh sách 20 dòng/trang theo ngày tạo giảm dần, cùng thời điểm thì ID tăng dần. |
| Luồng thay thế | - Không nhập bộ lọc: loại và trạng thái dùng All.<br>- Trang hợp lệ vượt số trang hoặc không có kết quả: hiển thị danh sách rỗng kèm thông báo, không coi là lỗi. |
| Ngoại lệ | - Vai trò hiện tại không phải Organizer/Admin: 403 “Bạn không có quyền thực hiện thao tác này”.<br>- page sai: 400 “Tham số trang không hợp lệ”.<br>- Bộ lọc loại/trạng thái rỗng, ngoài tập giá trị hoặc gửi nhiều giá trị: 400 “Bộ lọc không hợp lệ”.<br>- Từ khóa dài hơn 100 ký tự sau chuẩn hóa: 400. |
| Includes | UC-AUTH-07 |
| Extends | Không |
| Yêu cầu đặc biệt | - Bộ lọc trạng thái gồm All (mặc định) và đúng 4 trạng thái Draft, Published, Cancelled, Completed; không có Deleted. Bộ lọc loại gồm All/Event/Conference. Giá trị phải đúng tên và hoa/thường.<br>- Completed là trạng thái hiệu lực suy ra từ Published và `now ≥ end_at`, không lưu trong CSDL; Published gồm cả chưa bắt đầu và đang diễn ra.<br>- Từ khóa tối đa 100 ký tự; tìm theo tiêu đề. |
| Giả định | Không |
| Ghi chú | - Nguồn: FR-EVT-02, FR-EVT-07; BR-EVT-05, 07, 08; mục 2.2, 4.2.<br>- Liên quan: xem chi tiết một Event thực hiện qua UC-SRCH-02. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-EVT-03 |
| Tên Use Case | Chỉnh sửa bản nháp sự kiện/hội nghị |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Organizer, Admin |
| Mô tả | Cập nhật thông tin của Event còn ở trạng thái Draft. |
| Tiền điều kiện | Actor đã đăng nhập. |
| Hậu điều kiện | Các trường được cập nhật; thời gian Event và chương trình hiện có vẫn hợp lệ. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Actor mở form chỉnh sửa Event Draft.<br>2. Actor sửa các trường và gửi lưu.<br>3. Hệ thống kiểm tra lại quyền quản lý, trạng thái Draft và toàn bộ dữ liệu hiện tại tại thời điểm ghi.<br>4. Hệ thống lưu thay đổi và hiển thị kết quả thành công. |
| Luồng thay thế | Không |
| Ngoại lệ | - Organizer không phải chủ sở hữu Event Draft: 404 “Không tìm thấy dữ liệu” (theo UC-EVT-08); Admin thao tác mọi Draft.<br>- Actor nhìn thấy Event nhưng không có quyền quản lý: 403 “Bạn không có quyền thực hiện thao tác này”.<br>- Dữ liệu trường sai: 400.<br>- Draft đã quá giờ bắt đầu mà thao tác sửa không cập nhật bắt đầu về tương lai: 400.<br>- Đổi thời gian làm Agenda Item nằm ngoài khoảng Event mới: 400, từ chối lưu; hệ thống không tự xóa, cắt ngắn hoặc dời chương trình.<br>- Event không còn Draft (đã Published, Cancelled): 409. Event đã công bố sửa theo UC-EVT-04. |
| Includes | - UC-AUTH-07<br>- UC-EVT-08 |
| Extends | Không |
| Yêu cầu đặc biệt | - Có thể sửa mọi trường: loại, tiêu đề, mô tả, địa điểm, thời gian bắt đầu/kết thúc và sức chứa; thời gian bắt đầu phải ở tương lai, thời lượng 5 phút–30 ngày, sức chứa 1–10.000.<br>- Sửa Draft đồng thời Publish: Publish trước thì yêu cầu sửa trường bị khóa bị từ chối (409); sửa trước thì Publish kiểm tra dữ liệu mới (UC-EVT-05).<br>- Không đổi chủ sở hữu. |
| Giả định | Không |
| Ghi chú | Nguồn: FR-EVT-03; BR-EVT-01, 03, 09; BR-SCH-04; NFR-04. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-EVT-04 |
| Tên Use Case | Cập nhật sự kiện đã công bố |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Organizer, Admin |
| Mô tả | Cập nhật tiêu đề, mô tả, địa điểm và tăng sức chứa của Event Published chưa bắt đầu. |
| Tiền điều kiện | Actor đã đăng nhập. |
| Hậu điều kiện | Các trường được phép được cập nhật; loại, thời gian, chương trình giữ nguyên; sức chứa không giảm. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Actor mở form chỉnh sửa Event Published chưa bắt đầu.<br>2. Actor sửa tiêu đề, mô tả, địa điểm hoặc sức chứa và gửi lưu.<br>3. Hệ thống kiểm tra lại quyền quản lý, trạng thái, `now < start_at` và dữ liệu hiện tại tại thời điểm ghi, gồm sức chứa mới không nhỏ hơn giá trị hiện tại.<br>4. Hệ thống lưu thay đổi và hiển thị kết quả thành công. |
| Luồng thay thế | Địa điểm sau chuẩn hóa khác giá trị hiện tại: UI hiển thị cảnh báo “Địa điểm mới sẽ hiển thị cho người tham dự. Hệ thống không gửi thông báo về thay đổi này”; chọn Hủy thì không gửi yêu cầu lưu; chọn Xác nhận thì gửi yêu cầu lưu và tiếp tục từ bước 3. |
| Ngoại lệ | - Organizer thao tác Event Published của người khác (nhìn thấy nhưng không có quyền): 403 “Bạn không có quyền thực hiện thao tác này”.<br>- Dữ liệu trường sai hoặc sức chứa mới ngoài 1–10.000: 400.<br>- Giảm sức chứa (kể cả khi chưa có đăng ký) hoặc sửa trường bị khóa (loại, thời gian, chương trình): 409.<br>- Event không ở trạng thái Published chưa bắt đầu (Draft, đang diễn ra, Completed, Cancelled): 409, không cho sửa. |
| Includes | - UC-AUTH-07<br>- UC-EVT-08 |
| Extends | Không |
| Yêu cầu đặc biệt | - Sức chứa chỉ được giữ nguyên hoặc tăng, tối đa 10.000, và được so với giá trị hiện tại tại thời điểm ghi, không với giá trị cũ trên form.<br>- Tăng sức chứa đồng thời với đăng ký hoặc với một request tăng sức chứa khác phải tương đương một thứ tự tuần tự hợp lệ; không dùng giá trị cũ để ghi đè kết quả mới.<br>- Xác nhận đổi địa điểm chỉ là xác nhận ở UI, server không yêu cầu cờ xác nhận; request trực tiếp vẫn được lưu nếu thỏa quyền/trạng thái/dữ liệu.<br>- Không unpublish, không đổi chủ sở hữu, không mở lại Cancelled/Completed. |
| Giả định | Không |
| Ghi chú | Nguồn: FR-EVT-03; BR-EVT-03, 09; mục 9; NFR-04. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-EVT-05 |
| Tên Use Case | Công bố sự kiện/hội nghị |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Organizer, Admin |
| Mô tả | Chuyển Event Draft hợp lệ sang Published và mở đăng ký ngay. |
| Tiền điều kiện | Actor đã đăng nhập. |
| Hậu điều kiện | Event ở trạng thái Published; đăng ký được mở theo thời gian/sức chứa; loại, thời gian và chương trình bị khóa. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Actor chọn công bố Event Draft.<br>2. Hệ thống kiểm tra lại quyền quản lý, trạng thái Draft và `now < start_at` tại thời điểm thao tác.<br>3. Hệ thống kiểm tra toàn bộ dữ liệu Event và chương trình theo điều kiện công bố.<br>4. Hệ thống chuyển Draft → Published trong một thao tác nguyên tử.<br>5. Hệ thống hiển thị kết quả thành công. |
| Luồng thay thế | Event thường không có Agenda Item: vẫn được công bố nếu dữ liệu hợp lệ. |
| Ngoại lệ | - Organizer nhắm Event của người khác (nhìn thấy nhưng không có quyền): 403 “Bạn không có quyền thực hiện thao tác này”.<br>- Event không còn Draft (đã Published hoặc Cancelled): 409 “Sự kiện không ở trạng thái bản nháp”.<br>- `now ≥ start_at`: 409 “Thời gian bắt đầu phải ở tương lai. Cập nhật thời gian trước khi công bố”.<br>- Conference chưa có mục chương trình nào: 409 “Hội nghị cần ít nhất một mục chương trình để công bố”.<br>- Dữ liệu Event hoặc chương trình đã lưu vi phạm ràng buộc (độ dài, thời lượng, sức chứa, mục ngoài khoảng Event, chồng lấn, quá 50 mục): 409 “Dữ liệu sự kiện hoặc chương trình không hợp lệ. Vui lòng kiểm tra lại trước khi công bố”. |
| Includes | - UC-AUTH-07<br>- UC-EVT-08 |
| Extends | Không |
| Yêu cầu đặc biệt | - Điều kiện công bố: dữ liệu Event hợp lệ; `now < start_at`; Conference có ít nhất một Agenda Item hợp lệ; chương trình nằm trong khoảng thời gian Event và không chồng lấn.<br>- Thứ tự kiểm tra bắt buộc (mục 4.6): xác thực → ID/khả năng xem Event → quyền quản lý → còn Draft → `now < start_at` → Conference có chương trình → dữ liệu Event/chương trình hợp lệ → công bố. Mọi lỗi sau bước quyền là 409 vì công bố không nhận dữ liệu nhập.<br>- Publish đồng thời với thêm/sửa/xóa Agenda Item, đổi thời gian Draft hoặc Delete phải tương đương một thứ tự tuần tự hợp lệ: Publish trước thì thao tác kia bị từ chối; thao tác kia trước thì Publish kiểm tra dữ liệu mới; Delete trước thì Publish trả 404.<br>- Không được tồn tại Conference Published thiếu chương trình, Event vừa Published vừa deleted, hoặc Agenda Item ngoài khoảng Event/chồng lấn. |
| Giả định | Không |
| Ghi chú | Nguồn: FR-EVT-04; BR-EVT-01, 02, 09; BR-SCH-05; mục 4.6, 7.3; NFR-04. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-EVT-06 |
| Tên Use Case | Hủy sự kiện/hội nghị |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Organizer, Admin |
| Mô tả | Hủy Event Published chưa bắt đầu, ghi lý do và hủy toàn bộ Registration Confirmed. |
| Tiền điều kiện | Actor đã đăng nhập. |
| Hậu điều kiện | Event chuyển Cancelled và lưu cancelled_at/cancelled_by/cancel_reason; mọi Registration Confirmed chuyển Cancelled với nguồn EVENT; các Registration đã Cancelled trước đó giữ nguyên. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Actor chọn hủy Event.<br>2. Actor nhập lý do và xác nhận hủy trên hộp thoại xác nhận.<br>3. Hệ thống kiểm tra lại quyền quản lý, trạng thái Published, `now < start_at` và lý do 1–200 ký tự sau chuẩn hóa.<br>4. Hệ thống hủy Event và chuyển tất cả Registration Confirmed sang Cancelled nguồn EVENT trong cùng một thao tác nguyên tử, dùng cùng người/thời điểm/lý do hủy.<br>5. Hệ thống hiển thị kết quả thành công. |
| Luồng thay thế | Event chưa có Registration: vẫn lưu cancelled_by/at/reason. |
| Ngoại lệ | - Organizer nhắm Event Published của người khác (nhìn thấy nhưng không có quyền): 403 “Bạn không có quyền thực hiện thao tác này”.<br>- Event không ở Published, đã Cancelled hoặc Completed, hoặc `now ≥ start_at`: 409.<br>- Lý do rỗng hoặc ngoài 1–200 ký tự sau chuẩn hóa: 400. |
| Includes | - UC-AUTH-07<br>- UC-EVT-08 |
| Extends | Không |
| Yêu cầu đặc biệt | - Hủy Event và hủy các Registration Confirmed phải cùng thành công hoặc cùng thất bại; Registration đã Cancelled trước đó giữ nguyên nguồn/lý do/người/thời điểm hủy.<br>- Hủy Event đồng thời với đăng ký không được để lại Registration Confirmed trên Event Cancelled.<br>- Không mở lại Event đã hủy. |
| Giả định | Không |
| Ghi chú | Nguồn: FR-EVT-05; BR-EVT-04; BR-REG-04; NFR-04. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-EVT-07 |
| Tên Use Case | Xóa bản nháp sự kiện/hội nghị |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Organizer, Admin |
| Mô tả | Xóa mềm Event còn Draft. |
| Tiền điều kiện | Actor đã đăng nhập. |
| Hậu điều kiện | Event được đánh dấu xóa; không còn xuất hiện/truy cập trong các màn hình nghiệp vụ. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Actor chọn xóa Draft.<br>2. Actor xác nhận xóa trên hộp thoại xác nhận.<br>3. Hệ thống kiểm tra lại quyền quản lý và trạng thái Draft.<br>4. Hệ thống đánh dấu xóa mềm Event và hiển thị kết quả thành công. |
| Luồng thay thế | Không |
| Ngoại lệ | - Organizer nhắm Event của người khác (nhìn thấy nhưng không có quyền): 403 “Bạn không có quyền thực hiện thao tác này”.<br>- Event đã Published hoặc không còn Draft: 409. |
| Includes | - UC-AUTH-07<br>- UC-EVT-08 |
| Extends | Không |
| Yêu cầu đặc biệt | - Không xóa cứng User/Registration; Event đã Published không được xóa.<br>- Delete đồng thời Publish: Publish trước thì Delete bị từ chối (409); Delete trước thì Publish trả 404. |
| Giả định | Không |
| Ghi chú | Nguồn: FR-EVT-06; BR-EVT-06, 09; NFR-04. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-EVT-08 |
| Tên Use Case | Kiểm tra khả năng truy cập Event |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Attendee |
| Mô tả | Hệ thống kiểm tra ID Event và việc Actor được phép nhìn thấy Event, trước khi use case gọi kiểm tra quyền thao tác cụ thể. |
| Tiền điều kiện | Request nhắm tới một Event cụ thể. |
| Hậu điều kiện | Use case gọi được tiếp tục với Event hợp lệ và nhìn thấy được; hoặc request bị từ chối. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Hệ thống kiểm tra ID Event đúng định dạng.<br>2. Hệ thống kiểm tra Event tồn tại, chưa bị xóa và Actor được phép xem theo trạng thái Event.<br>3. Hệ thống cho use case gọi tiếp tục. |
| Luồng thay thế | Không |
| Ngoại lệ | - ID sai định dạng: 400 “Dữ liệu không hợp lệ”.<br>- Event không tồn tại/đã xóa hoặc Actor không được phép xem: 404 “Không tìm thấy dữ liệu”; không phân biệt hai nguyên nhân. |
| Includes | Không |
| Extends | Không |
| Yêu cầu đặc biệt | - Thứ tự xử lý: xác thực (UC-AUTH-07) → định dạng ID → tồn tại/khả năng nhìn thấy → quyền thao tác (do use case gọi kiểm tra) → dữ liệu và quy tắc nghiệp vụ; kiểm tra vai trò không được trả 403 sớm làm lộ Event mà SRS yêu cầu trả 404.<br>- Quy tắc xem: Draft chỉ mọi Admin và Organizer sở hữu; Cancelled/Completed cho Admin bất kỳ, Organizer sở hữu hoặc người từng đăng ký Event đó. Các quyền này yêu cầu tài khoản Active và phiên hợp lệ.<br>- Quyền sở hữu được xác định ở máy chủ, không tin owner_id do trình duyệt gửi. |
| Giả định | Không |
| Ghi chú | - Nguồn: BR-SRCH-04; BR-EVT-07; mục 4.2; NFR-02.<br>- Liên quan: quy tắc xem chi tiết công khai tại UC-SRCH-02. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-SCH-01 |
| Tên Use Case | Thêm mục chương trình |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Organizer, Admin |
| Mô tả | Thêm một Agenda Item vào chương trình của Event còn Draft. |
| Tiền điều kiện | Actor đã đăng nhập. |
| Hậu điều kiện | Agenda Item mới được lưu; chương trình hiển thị theo giờ bắt đầu tăng dần, nhóm theo ngày nếu Event nhiều ngày. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Actor mở chương trình của Event Draft.<br>2. Actor chọn thêm mục và nhập tiêu đề, thời gian bắt đầu/kết thúc, nội dung tùy chọn.<br>3. Hệ thống kiểm tra dữ liệu, giới hạn và tính hợp lệ của khoảng thời gian, đồng thời kiểm tra lại Event vẫn Draft và quyền tại thời điểm ghi.<br>4. Hệ thống lưu mục mới và hiển thị danh sách chương trình cập nhật. |
| Luồng thay thế | Không |
| Ngoại lệ | - Organizer nhắm Event của người khác (nhìn thấy nhưng không có quyền): 403 “Bạn không có quyền thực hiện thao tác này”.<br>- Event đã Published hoặc không còn Draft: 409, chương trình bị khóa.<br>- Tiêu đề/nội dung/thời gian sai định dạng, thời lượng dưới 5 phút hoặc nằm ngoài khoảng thời gian Event: 400.<br>- Vượt giới hạn 50 mục hoặc chồng lấn với Agenda Item khác: 409. |
| Includes | - UC-AUTH-07<br>- UC-EVT-08 |
| Extends | Không |
| Yêu cầu đặc biệt | - Agenda Item: tiêu đề 3–100 ký tự; nội dung tối đa 1000 ký tự; thời điểm chính xác đến phút; dài ít nhất 5 phút; nằm trong khoảng thời gian Event (gồm hai biên); tối đa 50 mục/Event.<br>- Hai mục chồng lấn khi `A.start < B.end AND B.start < A.end`; chạm biên được phép. Không hỗ trợ chương trình song song, nhiều phòng hoặc nhiều track. Tên diễn giả ghi trong nội dung.<br>- Thêm mục đồng thời với Publish, đổi thời gian Draft hoặc thay đổi chương trình khác không được tạo chồng lấn, vượt 50 mục hoặc trạng thái vi phạm (BR-EVT-09). |
| Giả định | Không |
| Ghi chú | - Nguồn: FR-SCH-01..03; BR-SCH-01..05; BR-EVT-09; NFR-04.<br>- Liên quan: xem chương trình thực hiện qua UC-SRCH-02. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-SCH-02 |
| Tên Use Case | Sửa mục chương trình |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Organizer, Admin |
| Mô tả | Sửa một Agenda Item trong chương trình của Event còn Draft. |
| Tiền điều kiện | Actor đã đăng nhập. |
| Hậu điều kiện | Agenda Item được cập nhật; chương trình hiển thị theo giờ bắt đầu tăng dần, nhóm theo ngày nếu Event nhiều ngày. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Actor mở chương trình của Event Draft.<br>2. Actor chọn một mục và nhập thay đổi tiêu đề, thời gian hoặc nội dung.<br>3. Hệ thống kiểm tra dữ liệu, giới hạn và tính hợp lệ của khoảng thời gian (không so chồng lấn với chính mục đang sửa), đồng thời kiểm tra lại Event vẫn Draft và quyền tại thời điểm ghi.<br>4. Hệ thống lưu thay đổi và hiển thị danh sách chương trình cập nhật. |
| Luồng thay thế | Không |
| Ngoại lệ | - Organizer nhắm Event của người khác (nhìn thấy nhưng không có quyền): 403 “Bạn không có quyền thực hiện thao tác này”.<br>- Agenda Item không tồn tại hoặc không thuộc Event được chọn: 404 “Không tìm thấy dữ liệu”.<br>- Event đã Published hoặc không còn Draft: 409, chương trình bị khóa.<br>- Tiêu đề/nội dung/thời gian sai định dạng, thời lượng dưới 5 phút hoặc nằm ngoài khoảng thời gian Event: 400.<br>- Chồng lấn với Agenda Item khác: 409. |
| Includes | - UC-AUTH-07<br>- UC-EVT-08 |
| Extends | Không |
| Yêu cầu đặc biệt | - Quy tắc Agenda Item như UC-SCH-01; chạm biên được phép.<br>- Sửa mục đồng thời với Publish, đổi thời gian Draft hoặc thay đổi chương trình khác không được tạo chồng lấn hoặc trạng thái vi phạm (BR-EVT-09). |
| Giả định | Không |
| Ghi chú | Nguồn: FR-SCH-01..03; BR-SCH-01..05; BR-EVT-09; NFR-04. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-SCH-03 |
| Tên Use Case | Xóa mục chương trình |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Organizer, Admin |
| Mô tả | Xóa một Agenda Item khỏi chương trình của Event còn Draft. |
| Tiền điều kiện | Actor đã đăng nhập. |
| Hậu điều kiện | Agenda Item bị xóa; chương trình hiển thị theo giờ bắt đầu tăng dần. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Actor mở chương trình của Event Draft.<br>2. Actor chọn xóa một mục.<br>3. Hệ thống kiểm tra lại Event vẫn Draft và quyền tại thời điểm ghi.<br>4. Hệ thống xóa mục và hiển thị danh sách chương trình cập nhật. |
| Luồng thay thế | Không |
| Ngoại lệ | - Organizer nhắm Event của người khác (nhìn thấy nhưng không có quyền): 403 “Bạn không có quyền thực hiện thao tác này”.<br>- Agenda Item không tồn tại hoặc không thuộc Event được chọn: 404 “Không tìm thấy dữ liệu”.<br>- Event đã Published hoặc không còn Draft: 409, chương trình bị khóa. |
| Includes | - UC-AUTH-07<br>- UC-EVT-08 |
| Extends | Không |
| Yêu cầu đặc biệt | Xóa mục cuối của Conference đồng thời với Publish: xóa trước thì Publish bị từ chối vì thiếu chương trình; Publish trước thì xóa bị từ chối vì chương trình đã khóa (BR-EVT-09). |
| Giả định | Không |
| Ghi chú | Nguồn: FR-SCH-01; BR-SCH-05; BR-EVT-09; NFR-04. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-SRCH-01 |
| Tên Use Case | Tra cứu sự kiện công khai |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Guest, Attendee |
| Mô tả | Xem danh sách Event công khai, tìm theo tiêu đề/địa điểm và lọc theo loại/khoảng ngày bắt đầu. |
| Tiền điều kiện | Không yêu cầu đăng nhập. |
| Hậu điều kiện | Không thay đổi dữ liệu. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Actor mở trang Tra cứu sự kiện.<br>2. Hệ thống hiển thị các Event Published có `now < end_at`, 20 dòng/trang, sắp xếp `start_at` tăng dần, trùng thì ID tăng dần.<br>3. Actor có thể nhập từ khóa, chọn loại, nhập từ ngày/đến ngày và chọn trang.<br>4. Hệ thống chuẩn hóa và kiểm tra tham số, kết hợp bộ lọc bằng AND, lọc theo ngày bắt đầu tại UTC+7 (gồm hai biên) rồi hiển thị kết quả. |
| Luồng thay thế | - Từ khóa bỏ trống: không lọc.<br>- Chỉ nhập một đầu của khoảng ngày: lọc theo đầu đã nhập.<br>- Trang hợp lệ vượt số trang hoặc không có kết quả: hiển thị danh sách rỗng kèm thông báo, không coi là lỗi. |
| Ngoại lệ | - page sai: 400 “Tham số trang không hợp lệ”.<br>- Có cả hai ngày mà từ ngày lớn hơn đến ngày: 400, lỗi dữ liệu.<br>- Từ khóa dài hơn 100 ký tự hoặc bộ lọc loại không hợp lệ: 400. |
| Includes | Không |
| Extends | Không |
| Yêu cầu đặc biệt | - Tìm chuỗi con, không phân biệt hoa/thường, có phân biệt dấu tiếng Việt; không dùng ký tự đại diện do người dùng nhập; văn bản NFC/NFD tương đương cho cùng kết quả.<br>- Danh sách gồm cả Event đang diễn ra và Event đã đủ chỗ. |
| Giả định | Không |
| Ghi chú | Nguồn: FR-SRCH-01; BR-SRCH-01, 02; mục 2.2. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-SRCH-02 |
| Tên Use Case | Xem chi tiết sự kiện/hội nghị |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Guest, Attendee |
| Mô tả | Xem thông tin chi tiết Event theo quy tắc hiển thị và quyền truy cập. |
| Tiền điều kiện | Không yêu cầu đăng nhập. |
| Hậu điều kiện | Không thay đổi dữ liệu. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Actor chọn/mở chi tiết Event.<br>2. Hệ thống kiểm tra ID và quyền xem theo trạng thái Event.<br>3. Hệ thống hiển thị loại, tiêu đề, mô tả, địa điểm, thời gian, tên chủ sở hữu, sức chứa, chỗ còn lại (sức chứa − số Registration Confirmed) và chương trình theo giờ bắt đầu tăng dần, nhóm theo ngày nếu Event nhiều ngày.<br>4. Hệ thống hiển thị tình trạng đăng ký: “Còn chỗ”, “Đủ chỗ” hoặc “Đã đóng đăng ký”. |
| Luồng thay thế | - Event Draft: mọi Admin xem được; Organizer chỉ xem Draft mình sở hữu.<br>- Event Cancelled/Completed: Admin bất kỳ, Organizer sở hữu hoặc người từng đăng ký Event đó được xem; hiển thị trạng thái và lý do hủy khi có.<br>- `now ≥ start_at`: luôn hiển thị “Đã đóng đăng ký” dù còn chỗ.<br>- Guest bấm đăng ký tham dự: hệ thống chuyển về đăng nhập (UC-AUTH-02) với returnUrl tới chi tiết Event; nếu Guest chuyển sang đăng ký tài khoản (UC-AUTH-01) thì giữ returnUrl hợp lệ; sau đăng nhập không tự đăng ký mà phải bấm xác nhận. |
| Ngoại lệ | - ID sai: 400 “Dữ liệu không hợp lệ”.<br>- Event không tồn tại/đã xóa hoặc Actor không có quyền xem: 404 “Không tìm thấy dữ liệu”, kể cả Guest; không redirect đăng nhập chỉ vì chi tiết bị ẩn. |
| Includes | Không |
| Extends | Không |
| Yêu cầu đặc biệt | - Chỉ hiển thị thao tác gửi đăng ký khi Event Published, `now < start_at` và còn chỗ; server vẫn kiểm tra lại khi đăng ký (UC-REG-01).<br>- Không công khai danh sách người tham dự hoặc mã đăng ký.<br>- Quyền xem Draft/Cancelled/Completed yêu cầu tài khoản Active và phiên hợp lệ. |
| Giả định | Không |
| Ghi chú | - Nguồn: FR-SRCH-02, FR-SRCH-03, FR-SCH-03; BR-SRCH-03, 04; mục 4.1, 4.2.<br>- Liên quan: UC-EVT-02 (mở chi tiết từ danh sách quản lý), UC-REG-01, UC-EVT-08. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-REG-01 |
| Tên Use Case | Đăng ký tham dự |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Attendee |
| Mô tả | Người dùng đăng ký một suất tham dự cho chính mình tại một Event. |
| Tiền điều kiện | Actor đã đăng nhập. |
| Hậu điều kiện | Tồn tại đúng một Registration Confirmed cho cặp User–Event; mã đăng ký được tạo ở lần đầu. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Actor chọn đăng ký Event.<br>2. Hệ thống kiểm tra Event nhìn thấy được, Actor không phải chủ sở hữu và Event đang mở đăng ký (Published, `now < start_at`, còn chỗ) trên dữ liệu hiện tại.<br>3. Hệ thống tạo Registration Confirmed với mã đăng ký mới.<br>4. Hệ thống báo “Đăng ký thành công”. |
| Luồng thay thế | - Registration hiện có đã Confirmed: hệ thống trả bản ghi hiện có và báo “Bạn đã đăng ký sự kiện này”; không kiểm tra tiếp sức chứa/thời gian, không tạo thêm suất hoặc mã.<br>- Registration trước đó Cancelled nguồn SELF (đăng ký lại): nếu Event vẫn mở đăng ký và còn chỗ, hệ thống dùng lại bản ghi/mã, chuyển Confirmed, cập nhật registered_at, xóa các trường hủy và báo “Đăng ký thành công”. |
| Ngoại lệ | - Actor là chủ sở hữu Event: 403 “Bạn không thể đăng ký sự kiện do mình tổ chức”.<br>- Registration Cancelled nguồn MANAGER: 403 “Đăng ký đã bị ban tổ chức hủy. Bạn không thể đăng ký lại sự kiện này”.<br>- Event Draft hoặc Cancelled: 409 “Sự kiện không mở đăng ký”.<br>- `now ≥ start_at`, kể cả Completed: 409 “Đã đóng đăng ký”.<br>- Số Confirmed ≥ sức chứa: 409 “Sự kiện đã đủ chỗ”. |
| Includes | - UC-AUTH-07<br>- UC-EVT-08 |
| Extends | Không |
| Yêu cầu đặc biệt | - Thứ tự kiểm tra bắt buộc (mục 4.3): xác thực → ID/khả năng xem Event → chủ sở hữu → Registration đã Confirmed → nguồn MANAGER → Draft/Cancelled → `now ≥ start_at` → đủ chỗ → xác nhận. Event vừa đủ chỗ vừa đã bắt đầu thì báo “Đã đóng đăng ký”.<br>- Mỗi tài khoản một suất cho chính mình; không đăng ký hộ, không chọn số lượng. Người đăng ký lấy từ phiên, không nhận user_id hoặc nguồn hủy từ trình duyệt.<br>- Mã `REG-` + 12 chữ hoa ASCII/chữ số, ngẫu nhiên, duy nhất toàn hệ thống, không đổi khi đăng ký lại. Chỉ nguồn SELF được đăng ký lại; MANAGER bị chặn, không có chức năng gỡ chặn; EVENT không được đăng ký lại.<br>- Ràng buộc unique `(event_id, user_id)` bảo đảm hai request đồng thời của cùng một user không tạo hai Registration; request còn lại xử lý theo bản ghi hiện có.<br>- Hai người tranh chỗ cuối chỉ một người thành công; đăng ký đồng thời với hủy Event không để lại Confirmed trên Event Cancelled.<br>- Cho phép đăng ký nhiều Event trùng giờ; không kiểm tra xung đột lịch cá nhân. |
| Giả định | Không |
| Ghi chú | Nguồn: FR-REG-01, FR-REG-04; BR-REG-01..04, 08; mục 4.1, 4.3; NFR-04. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-REG-02 |
| Tên Use Case | Xem đăng ký của tôi |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Attendee |
| Mô tả | Xem danh sách các Registration của chính người dùng. |
| Tiền điều kiện | Actor đã đăng nhập. |
| Hậu điều kiện | Không thay đổi dữ liệu. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Actor mở “Đăng ký của tôi”.<br>2. Hệ thống lấy các Registration thuộc tài khoản hiện tại, gồm Confirmed và Cancelled, kể cả Event đã kết thúc.<br>3. Hệ thống hiển thị Event, thời gian/địa điểm, mã đăng ký, trạng thái Registration, trạng thái Event và tình trạng điểm danh; sắp xếp `registered_at` giảm dần, trùng thì ID tăng dần; 20 dòng/trang. |
| Luồng thay thế | Trang hợp lệ vượt số trang hoặc chưa có Registration: hiển thị danh sách rỗng kèm thông báo, không coi là lỗi. |
| Ngoại lệ | page sai: 400 “Tham số trang không hợp lệ”. |
| Includes | UC-AUTH-07 |
| Extends | Không |
| Yêu cầu đặc biệt | - Không có màn hình lịch riêng.<br>- Đổi vai trò/trạng thái tài khoản không làm mất dữ liệu Registration lịch sử. |
| Giả định | Không |
| Ghi chú | Nguồn: FR-REG-02; BR-REG-07; BR-PART-04; mục 2.2. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-REG-03 |
| Tên Use Case | Hủy đăng ký của tôi |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Attendee |
| Mô tả | Người dùng tự hủy Registration của chính mình trước giờ Event bắt đầu. |
| Tiền điều kiện | Actor đã đăng nhập. |
| Hậu điều kiện | Hủy mới thành công: Registration chuyển Cancelled, nguồn SELF; cancelled_by là Actor; lý do hệ thống “Người tham dự tự hủy”; trả lại một chỗ. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Actor chọn hủy Registration của mình.<br>2. Actor xác nhận hủy trên hộp thoại xác nhận.<br>3. Hệ thống kiểm tra Registration thuộc Event được chọn và thuộc Actor.<br>4. Hệ thống kiểm tra điều kiện hủy (UC-REG-04).<br>5. Hệ thống chuyển Registration sang Cancelled nguồn SELF, lưu người/thời điểm/lý do và trả lại một chỗ.<br>6. Hệ thống báo “Hủy đăng ký thành công”. |
| Luồng thay thế | Registration đã Cancelled (sau khi qua kiểm tra quyền): hệ thống trả thành công “Đã hủy” và dừng; không kiểm tra Event/thời gian/điểm danh, không ghi lại dữ liệu, không trả thêm chỗ, kể cả khi Event đã hủy/kết thúc hoặc đã qua giờ bắt đầu. |
| Ngoại lệ | - Registration không tồn tại hoặc không thuộc Event được chọn: 404 “Không tìm thấy dữ liệu”.<br>- Actor không phải chủ Registration: 403 “Bạn không có quyền thực hiện thao tác này”. |
| Includes | - UC-AUTH-07<br>- UC-EVT-08<br>- UC-REG-04 |
| Extends | Không |
| Yêu cầu đặc biệt | - Thứ tự bắt buộc (mục 4.5): xác thực → ID/Event → Registration thuộc Event → chủ Registration → đã Cancelled (idempotent) → điều kiện hủy (UC-REG-04) → hủy. Người không có quyền không được hưởng kết quả idempotent.<br>- Hủy lặp không được trả thêm chỗ hay ghi đè thông tin hủy.<br>- Các kiểm tra và ghi dữ liệu phải nguyên tử khi có request đồng thời. |
| Giả định | Không |
| Ghi chú | Nguồn: FR-REG-03; BR-REG-05, 06; mục 4.5; NFR-04. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-REG-04 |
| Tên Use Case | Kiểm tra điều kiện hủy đăng ký |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Attendee |
| Mô tả | Hệ thống kiểm tra Event và Registration có đủ điều kiện để chuyển sang Cancelled, dùng chung cho UC-REG-03 và UC-PART-02. |
| Tiền điều kiện | Use case gọi đã xác định Registration và quyền của Actor; Registration chưa Cancelled. |
| Hậu điều kiện | Use case gọi được tiếp tục; hoặc yêu cầu hủy bị từ chối và không đổi dữ liệu. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Hệ thống lấy `now` tại thời điểm thao tác.<br>2. Hệ thống kiểm tra Event cho phép hủy (Published).<br>3. Hệ thống kiểm tra Registration chưa điểm danh.<br>4. Hệ thống kiểm tra `now < start_at`.<br>5. Hệ thống cho use case gọi tiếp tục. |
| Luồng thay thế | Không |
| Ngoại lệ | - Event Draft hoặc Cancelled: 409 “Sự kiện không cho phép hủy đăng ký”.<br>- Registration đã điểm danh: 409 “Không thể hủy đăng ký đã điểm danh”.<br>- `now ≥ start_at`, kể cả Completed: 409 “Đã bắt đầu sự kiện. Không thể hủy đăng ký”. |
| Includes | Không |
| Extends | Không |
| Yêu cầu đặc biệt | - Các kiểm tra theo đúng thứ tự trên; tại đúng giờ bắt đầu thì không được hủy.<br>- Kết quả phải được kiểm tra lại trên dữ liệu hiện tại trong thao tác ghi. |
| Giả định | Không |
| Ghi chú | - Nguồn: BR-REG-05, BR-PART-02; mục 4.5 (bước 6–8).<br>- Liên quan: UC-REG-03, UC-PART-02. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-PART-01 |
| Tên Use Case | Xem danh sách người tham dự và thống kê |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Organizer, Admin |
| Mô tả | Xem danh sách Registration của một Event, tìm kiếm, lọc và xem thống kê đăng ký/tham dự của toàn bộ Event trong cùng màn hình. |
| Tiền điều kiện | Actor đã đăng nhập. |
| Hậu điều kiện | Không thay đổi dữ liệu. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Actor mở màn hình Người tham dự và thống kê của Event.<br>2. Hệ thống hiển thị thống kê tính trên toàn bộ Event: đang đăng ký, đã hủy, còn chỗ, đã điểm danh, chưa điểm danh, tỷ lệ tham dự và vắng mặt.<br>3. Hệ thống hiển thị danh sách Registration (mỗi dòng gồm username, họ tên hiện tại, mã, trạng thái, `registered_at` gần nhất, thông tin điểm danh và hủy), sắp xếp `registered_at` tăng dần, trùng thì ID tăng dần, 20 dòng/trang.<br>4. Actor có thể tìm theo username/họ tên/mã đăng ký và lọc theo Confirmed/Cancelled, đã/chưa điểm danh.<br>5. Hệ thống kết hợp bộ lọc bằng AND và hiển thị lại danh sách. |
| Luồng thay thế | - Bộ lọc trạng thái và điểm danh dùng “Tất cả”: không lọc.<br>- Trang hợp lệ vượt số trang hoặc không có kết quả: hiển thị danh sách rỗng kèm thông báo, không coi là lỗi. |
| Ngoại lệ | - Actor nhìn thấy Event nhưng không có quyền quản lý, ví dụ Organizer nhắm Published của người khác: 403 “Bạn không có quyền thực hiện thao tác này”.<br>- page, bộ lọc hoặc từ khóa không hợp lệ (từ khóa quá 100 ký tự): 400. |
| Includes | - UC-AUTH-07<br>- UC-EVT-08 |
| Extends | Không |
| Yêu cầu đặc biệt | - Thống kê tính trên toàn bộ Event, không đổi theo tìm kiếm/phân trang/bộ lọc danh sách. Đã hủy là số Registration hiện đang Cancelled, không phải tổng số lần hủy.<br>- Đang đăng ký = số Confirmed; còn chỗ = sức chứa − đang đăng ký; chưa điểm danh = đang đăng ký − đã điểm danh; tỷ lệ tham dự = đã điểm danh / đang đăng ký × 100%, làm tròn 1 chữ số thập phân, mẫu số 0 thì 0%.<br>- Vắng mặt = số chưa điểm danh, chỉ hiển thị khi Event Completed; trạng thái khác hiển thị “Chưa xác định”.<br>- Không lưu số chỗ còn lại hoặc tỷ lệ tham dự như dữ liệu độc lập; tính từ sức chứa và Registration.<br>- Tài khoản Inactive vẫn giữ đăng ký và kết quả điểm danh. Không thêm người tham dự thủ công, sửa chủ Registration hoặc chuyển suất. |
| Giả định | Không |
| Ghi chú | Nguồn: FR-PART-01, FR-PART-02, FR-PART-04; BR-PART-01, 03, 04; bảng công thức mục 3.6; mục 2.2, 4.2; mô hình dữ liệu mục 6. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-PART-02 |
| Tên Use Case | Hủy đăng ký của người tham dự |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Organizer, Admin |
| Mô tả | Người quản lý hủy Registration còn hiệu lực của người tham dự trước giờ bắt đầu Event. |
| Tiền điều kiện | Actor đã đăng nhập. |
| Hậu điều kiện | Hủy mới thành công: Registration chuyển Cancelled nguồn MANAGER, lưu người thực hiện/thời điểm/lý do và trả lại một chỗ. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Actor chọn Registration cần hủy trong màn hình quản lý.<br>2. Actor nhập lý do và xác nhận trên hộp thoại có cảnh báo rằng hủy từ phía quản lý sẽ chặn người tham dự đăng ký lại Event này và không có chức năng gỡ chặn.<br>3. Hệ thống kiểm tra Registration thuộc Event được chọn và Actor có quyền quản lý Event.<br>4. Hệ thống kiểm tra điều kiện hủy (UC-REG-04) và lý do hủy sau chuẩn hóa dài 1–200 ký tự.<br>5. Hệ thống chuyển Registration sang Cancelled nguồn MANAGER, lưu người/thời điểm/lý do và trả lại một chỗ.<br>6. Hệ thống báo “Hủy đăng ký thành công”. |
| Luồng thay thế | - Registration đã Cancelled (sau khi qua kiểm tra quyền): hệ thống trả thành công “Đã hủy” và dừng; không yêu cầu lại lý do, không đổi nguồn/lý do/người/thời điểm hủy, không trả thêm chỗ.<br>- Admin hủy Registration của chính mình ở Event người khác từ màn hình quản lý: vẫn ghi nguồn MANAGER. |
| Ngoại lệ | - Registration không tồn tại hoặc không thuộc Event được chọn: 404 “Không tìm thấy dữ liệu”.<br>- Actor không có quyền quản lý Event: 403 “Bạn không có quyền thực hiện thao tác này”.<br>- Lý do sau chuẩn hóa rỗng hoặc quá 200 ký tự: 400 “Lý do hủy phải có từ 1 đến 200 ký tự”. |
| Includes | - UC-AUTH-07<br>- UC-EVT-08<br>- UC-REG-04 |
| Extends | Không |
| Yêu cầu đặc biệt | - Nguồn hủy do server xác định từ thao tác được gọi, không cho trình duyệt chọn; gửi lại không được thay nguồn hủy để vượt chặn MANAGER.<br>- Registration đã bị MANAGER hủy không được đăng ký lại; không có chức năng gỡ chặn.<br>- Thứ tự bắt buộc (mục 4.5): xác thực → ID/Event → Registration thuộc Event → quyền quản lý → đã Cancelled (idempotent) → điều kiện hủy (UC-REG-04) → lý do → hủy.<br>- Tài khoản Inactive của người tham dự vẫn có thể bị hủy đăng ký trước giờ bắt đầu. |
| Giả định | Không |
| Ghi chú | Nguồn: FR-PART-03; BR-PART-02, 04; BR-REG-06; mục 4.5. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-ATT-01 |
| Tên Use Case | Điểm danh người tham dự |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Organizer, Admin |
| Mô tả | Ghi nhận điểm danh cho một Registration bằng mã tại Event đang diễn ra. |
| Tiền điều kiện | Actor đã đăng nhập. |
| Hậu điều kiện | Registration Confirmed có checked_in_at và checked_in_by được ghi đúng một lần; danh sách/thống kê phản ánh kết quả. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Actor mở màn hình điểm danh của một Event cụ thể.<br>2. Actor nhập mã đăng ký và gửi yêu cầu điểm danh.<br>3. Hệ thống lấy lại `now`, kiểm tra Event, quyền quản lý và thời gian điểm danh (Event Published, `start_at ≤ now < end_at`).<br>4. Hệ thống chuẩn hóa mã (NFC, trim, chữ thường ASCII thành chữ hoa), kiểm tra định dạng, tra cứu Registration và kiểm tra thuộc đúng Event, Confirmed, chưa điểm danh.<br>5. Hệ thống ghi checked_in_at và checked_in_by đúng một lần.<br>6. Hệ thống báo “Điểm danh thành công” kèm username/họ tên và cập nhật danh sách/thống kê. |
| Luồng thay thế | Không |
| Ngoại lệ | - Actor nhìn thấy Event nhưng không có quyền quản lý: 403 “Bạn không có quyền thực hiện thao tác này”.<br>- Event Draft hoặc Cancelled: 409 “Sự kiện không cho phép điểm danh”.<br>- `now < start_at`: 409 “Chưa đến giờ điểm danh”.<br>- `now ≥ end_at`, kể cả Completed: 409 “Đã hết giờ điểm danh”.<br>- Mã không khớp định dạng `^REG-[A-Z0-9]{12}$` sau chuẩn hóa: 400 “Mã đăng ký không hợp lệ”.<br>- Không tìm thấy mã: 404 “Không tìm thấy mã đăng ký”.<br>- Mã thuộc Event khác: 409 “Mã đăng ký không thuộc sự kiện này”.<br>- Registration Cancelled: 409 “Đăng ký đã bị hủy”.<br>- Registration đã điểm danh: 409 “Đã điểm danh lúc {dd/MM/yyyy HH:mm:ss}” theo UTC+7, giữ nguyên lần đầu. |
| Includes | - UC-AUTH-07<br>- UC-EVT-08 |
| Extends | Không |
| Yêu cầu đặc biệt | - Thứ tự kiểm tra bắt buộc (mục 4.4): xác thực → ID/Event/khả năng xem → quyền quản lý → trạng thái Event → `now < start_at` → `now ≥ end_at` → định dạng mã → tra cứu mã → đúng Event → Cancelled → đã điểm danh. Quyền quản lý và thời gian được kiểm tra trước khi tra cứu mã; mã đã hủy nhưng Event đã hết giờ thì báo “Đã hết giờ điểm danh”.<br>- Chỉ cho điểm danh khi `start_at ≤ now < end_at`: đúng giờ bắt đầu được phép, đúng giờ kết thúc bị từ chối; không điểm danh sớm.<br>- Không hoàn tác điểm danh, không điểm danh theo Agenda Item; Attendee không tự điểm danh.<br>- Hai request cùng mã chỉ một lần được ghi nhận; không tăng số tham dự hai lần.<br>- Tài khoản người tham dự Inactive không làm mất hiệu lực Registration Confirmed và không ngăn điểm danh nếu các điều kiện khác đều thỏa. |
| Giả định | Không |
| Ghi chú | Nguồn: FR-ATT-01..03; BR-ATT-01..04; BR-PART-04; mục 4.4; NFR-04. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-USER-01 |
| Tên Use Case | Xem danh sách người dùng |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Admin |
| Mô tả | Admin xem danh sách người dùng, tìm kiếm và lọc theo vai trò/trạng thái. |
| Tiền điều kiện | Actor đã đăng nhập. |
| Hậu điều kiện | Không thay đổi dữ liệu. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Admin mở màn hình Quản lý người dùng.<br>2. Hệ thống hiển thị username, họ tên, vai trò, trạng thái và ngày tạo, sắp xếp ngày tạo giảm dần, trùng thì ID tăng dần, 20 dòng/trang.<br>3. Admin có thể nhập từ khóa username/họ tên và lọc theo vai trò/trạng thái.<br>4. Hệ thống áp dụng bộ lọc bằng AND và hiển thị lại danh sách. |
| Luồng thay thế | Danh sách rỗng hoặc trang hợp lệ vượt số trang: hiển thị thông báo, không coi là lỗi. |
| Ngoại lệ | - Vai trò hiện tại không phải Admin: 403 “Bạn không có quyền thực hiện thao tác này”.<br>- page, bộ lọc hoặc từ khóa không hợp lệ: 400. |
| Includes | UC-AUTH-07 |
| Extends | Không |
| Yêu cầu đặc biệt | Không trả password hash hoặc thông tin phiên. Từ khóa tối đa 100 ký tự. |
| Giả định | Không |
| Ghi chú | Nguồn: FR-USER-01; BR-USER-05; mục 2.2. |

| Thuộc tính | Nội dung |
|---|---|
| Mã Use Case | UC-USER-02 |
| Tên Use Case | Cập nhật vai trò hoặc trạng thái tài khoản |
| Người tạo | Không |
| Người cập nhật gần nhất | Không |
| Ngày tạo | Không |
| Ngày cập nhật gần nhất | Không |
| Actor | Admin |
| Mô tả | Admin đổi vai trò của một tài khoản giữa Attendee, Organizer, Admin hoặc chuyển tài khoản giữa Active và Inactive. |
| Tiền điều kiện | Actor đã đăng nhập. |
| Hậu điều kiện | Vai trò hoặc trạng thái của tài khoản mục tiêu được cập nhật nếu không vi phạm bất biến; thay đổi có hiệu lực từ request xác thực tiếp theo của tài khoản đó. |
| Độ ưu tiên | Bắt buộc |
| Tần suất sử dụng | Không |
| Luồng chính | 1. Admin chọn tài khoản và chọn vai trò mới hoặc trạng thái mới.<br>2. Admin xác nhận thay đổi trên hộp thoại xác nhận.<br>3. Hệ thống kiểm tra lại quyền Admin, dữ liệu hiện tại và các bất biến về Admin Active cuối cùng và chủ sở hữu Event.<br>4. Hệ thống cập nhật vai trò hoặc trạng thái.<br>5. Hệ thống hiển thị kết quả thành công. |
| Luồng thay thế | Kích hoạt lại tài khoản từ Inactive sang Active: không yêu cầu xác nhận ở bước 2. |
| Ngoại lệ | - Actor không còn vai trò Admin: 403 “Bạn không có quyền thực hiện thao tác này”.<br>- Tài khoản mục tiêu không tồn tại: 404 “Không tìm thấy dữ liệu”.<br>- Giá trị vai trò hoặc trạng thái đầu vào không hợp lệ: 400.<br>- Thay đổi làm mất Admin Active cuối cùng: 409.<br>- Hạ Organizer/Admin xuống Attendee hoặc vô hiệu hóa chủ sở hữu còn Event Draft chưa xóa hoặc Published chưa kết thúc: 409. |
| Includes | UC-AUTH-07 |
| Extends | Không |
| Yêu cầu đặc biệt | - BR-USER-01: luôn còn ít nhất một Admin Active, kể cả khi nhiều Admin thao tác đồng thời; Admin được đổi vai trò/vô hiệu hóa chính mình nếu còn Admin Active khác.<br>- BR-USER-02: không hạ Organizer/Admin xuống Attendee hoặc vô hiệu hóa chủ sở hữu còn Event Draft chưa xóa hoặc Published chưa kết thúc; đổi Organizer ↔ Admin vẫn được phép. Mỗi tài khoản có đúng một vai trò.<br>- BR-USER-06: đổi vai trò/vô hiệu hóa đồng thời với tạo Event mới của cùng chủ sở hữu phải tương đương một thứ tự tuần tự hợp lệ; quyền tạo kiểm tra ở lúc ghi dữ liệu (UC-EVT-01). Kiểm tra quyền trên dữ liệu có thẩm quyền, không chỉ claim cũ.<br>- Không xóa tài khoản; không đổi chủ sở hữu Event hoặc tự động hủy Registration; Inactive giữ đăng ký và kết quả điểm danh, Confirmed của tài khoản Inactive vẫn chiếm chỗ; không có tự mở khóa.<br>- Tài khoản Inactive bị từ chối ở các chức năng có xác thực từ request tiếp theo, kể cả bằng phiên đã mở (UC-AUTH-07). |
| Giả định | Không |
| Ghi chú | Nguồn: FR-USER-02, FR-USER-03; BR-USER-01..04, 06; BR-PART-04; NFR-04. |
