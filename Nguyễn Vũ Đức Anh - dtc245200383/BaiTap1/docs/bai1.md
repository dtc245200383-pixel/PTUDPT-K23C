# Bài 1 – Tổng quan hệ thống và MEP

Họ tên: Nguyễn Vũ Đức Anh MSSV: dtc245200383 Lớp: K23C

> Nhật ký này ghi kết quả đã quan sát được trong buổi thực hành và các bước đã chạy trong project EduSubmit. Trước khi nộp, bổ sung ảnh chụp màn hình thật tại các vị trí được đánh dấu.

## 1. Cài đặt

- .NET SDK: `10.0.403`.
- Git: `2.55.0.windows.5`.
- `dotnet build` cho solution EduSubmit: thành công.
- Ảnh cần chèn: kết quả `dotnet --version`, `git --version`, và chương trình HelloPtudpt ở Bước 4.

<!-- TODO: chèn ảnh Bước 4 vào docs/ và cập nhật liên kết tại đây. -->

## 2. MepDemo (Bước 7)

- MEP 1 – Request–Response: client gửi yêu cầu và chờ kết quả; ví dụ chấm bài đồng bộ.
- MEP 2 – One-way: bên gửi phát thông điệp rồi tiếp tục, không chờ kết quả nghiệp vụ; ví dụ gửi audit log.
- MEP 3 – Queue-based: job vào hàng đợi và các worker chia nhau xử lý. Trong lần chạy mẫu, mỗi `sub-x` chỉ được một worker xử lý.
- MEP 4 – Publish–Subscribe: một sự kiện được phát cho tất cả bên đăng ký; trong mẫu có 3 dịch vụ nhận sự kiện.
- MEP 5 – Streaming/Push: server gửi nhiều cập nhật trạng thái liên tiếp cho client.
- Khác biệt queue và publish–subscribe: queue phân phối mỗi job cho một worker trong nhóm; publish–subscribe phát cùng một sự kiện cho mọi subscriber phù hợp.

## 3. BasicWebApi (Bước 8)

- Đã có endpoint `GET /`, `GET /hello/{name}`, `POST /greetings`; `language=ja` trả lời `Konnichiwa, {name}!`.
- `builder.Services.AddSingleton(...)` phải chạy trước `builder.Build()` vì dịch vụ cần được đăng ký trước khi ứng dụng tạo service provider.
- `name` lấy từ route `/hello/{name}`; `language` lấy từ query string `?language=en`.
- POST hợp lệ dùng `201 Created` vì đã tạo tài nguyên mới; header `Location` cho biết đường dẫn tài nguyên.
- REST Client mong đợi: GET trả 200; POST tên hợp lệ trả 201; POST tên rỗng trả 400.
- Ảnh cần chèn: REST Client có các kết quả 200, 201 và 400; ảnh debug nếu giảng viên yêu cầu.

<!-- TODO: chèn ảnh BasicWebApi vào docs/ và cập nhật liên kết tại đây. -->

## 4. MiniDistributedSystem (Bước 9)

- Client gọi `GET /api/assignments` và `POST /api/submissions`; đó là các cặp request–response HTTP. Kết quả quan sát được lần lượt là 200 và 201.
- Client không tham chiếu source code API. Hai tiến trình thống nhất qua hợp đồng JSON (tên trường, kiểu dữ liệu, URL và status code).
- URL API truyền qua tham số dòng lệnh để đổi môi trường/cổng mà không sửa code.
- Ảnh cần chèn: hai terminal thể hiện API đang chạy và Client nhận danh sách, mã lượt nộp, trạng thái `received`.

<!-- TODO: chèn ảnh MiniDistributedSystem vào docs/ và cập nhật liên kết tại đây. -->

## 5. Thí nghiệm lỗi (Bước 10)

| # | Dự đoán | Thực tế | Đúng/Sai | Giải thích |
|---|---|---|---|---|
| a | Thêm bài tập ở API thì client hiện thêm mà không sửa client. | Client in Bài tập 1, 2 và 3. | Đúng | Client lấy danh sách từ API và lặp qua các phần tử trả về. |
| b | Tắt API thì client không thể lấy danh sách. | Client ném `HttpRequestException` tại `GetFromJsonAsync` (dòng 14). | Đúng | Không có tiến trình lắng nghe cổng 5000 để nhận request. |
| c | StudentCode rỗng bị API từ chối; client báo lỗi khi kiểm tra status. | API trả 400; `EnsureSuccessStatusCode()` ném `HttpRequestException`. | Đúng | Server phải xác thực input vì không thể tin mọi client đều gửi dữ liệu hợp lệ. |
| d | API ở cổng 5050 nhưng client gọi 5000 sẽ lỗi kết nối. | Client gặp lỗi kết nối tại GET `/api/assignments`. | Đúng | Địa chỉ/cổng đích không có API đang lắng nghe; chạy client với tham số `http://localhost:5050` để kết nối mà không đổi code. |

## 6. EduSubmit tự làm (Bước 11–12)

- API có `GET /`, `GET /api/assignments`, `POST /api/submissions` và `GET /api/submissions/{id:guid}`.
- POST hợp lệ trả 201 và `Location`; dữ liệu thiếu hoặc `assignmentId <= 0` trả 400.
- Lượt nộp được lưu trong bộ nhớ; tra cứu ID tồn tại trả 200, ID không tồn tại trả 404.
- Client lấy URL từ `args[0]` (mặc định `http://localhost:5000`), lấy danh sách, gửi lượt nộp, kiểm tra status rồi tra cứu lại lượt nộp.
- Hai project độc lập, không tham chiếu lẫn nhau; mỗi bên tự khai báo record trao đổi JSON.
- Luồng thông điệp:
  - Client --(HTTP GET `/api/assignments`)--> API: request–response, 200.
  - Client --(HTTP POST `/api/submissions`)--> API: request–response, 201 Created.
  - Client --(HTTP GET `/api/submissions/{id}`)--> API: request–response, 200 hoặc 404.
- `requests.http` có health check, danh sách, POST hợp lệ, POST lỗi, GET ID hợp lệ và GET ID không tồn tại.
- Ảnh cần chèn: hai terminal của EduSubmit; các response 200, 201, 400, 404 trong REST Client.
- Phần cần tự xác nhận: chạy đủ các request trong VS Code REST Client và chụp ảnh đầu ra; bổ sung ảnh thật vào `docs/`.

<!-- TODO: chèn ảnh EduSubmit và REST Client vào docs/ và cập nhật liên kết tại đây. -->

## 7. Khó khăn gặp phải và cách giải quyết

- Khi tạo project, một số lệnh thiếu đối số (`-o` hoặc project path) nên CLI báo thiếu tham số; chạy lại với đường dẫn đầy đủ thì tạo thành công.
- Khi thêm bài tập thứ 3, thiếu dấu phẩy sau phần tử thứ 2 gây CS1003; thêm dấu phẩy và chạy lại.
- Khi thử StudentCode rỗng, sửa nhầm tên tham số; khôi phục `StudentCode` và `RepositoryUrl` đúng tên để tiếp tục thí nghiệm.
- Khi đổi cổng API, client vẫn trỏ cổng cũ nên không kết nối; truyền URL mới qua tham số dòng lệnh.
- Khi push, cần tạo repository GitHub trống, thay `USERNAME` bằng tên tài khoản thật, rồi cấu hình remote.

## 8. Tự kiểm tra mã trạng thái

- 200 OK: yêu cầu thành công và có phản hồi.
- 201 Created: tạo tài nguyên mới thành công; thường trả thêm `Location`.
- 400 Bad Request: dữ liệu yêu cầu không hợp lệ.
- 404 Not Found: không tìm thấy tài nguyên theo ID.
- Client kiểm tra status trước khi xử lý body để không coi phản hồi lỗi là dữ liệu thành công.


