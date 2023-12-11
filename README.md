# QUẢN LÝ MÔN HỌC VÀ LỚP HỌC

## Mô tả đề tài

Một trường đại học ở ĐHQG có tới hàng trăm lớp học và hàng ngàn môn học khác nhau, thế nên cần một ứng dụng có thể giúp cho nhà trường, giảng viên và sinh viên có thể quản lý và xem danh sách các môn học và phòng học. Ứng dụng quản lý môn học và lớp học là một ứng dụng đa năng, giúp tối ưu hóa quá trình giảng dạy và học tập, cũng như tạo ra một cơ sở thông tin đáng tin cậy cho trường học và giáo viên. Nó giúp cải thiện tương tác và giao tiếp giữa tất cả các bên liên quan trong quá trình giáo dục và học tập, mang lại lợi ích lớn cho toàn bộ cộng đồng trường học.

### Mô tả chức năng

1. **Quản lý môn học**
   
   - Mã môn học: là mã số định danh duy nhất cho mỗi môn học, ví dụ: "MATH101".
   - Tên môn học: tên gọi của môn học, ví dụ: "Toán cao cấp".
   - Giảng viên phụ trách môn học.
   - Số tín chỉ: xác định số tín chỉ tương ứng với môn học.
   - Lịch học: Thời gian, ngày và địa điểm diễn ra môn học.
   - Mô tả môn học: cung cấp một mô tả ngắn gọn về nội dung và mục tiêu của môn học.
   - Yêu cầu tiên quyết: danh sách các môn học hoặc điều kiện tiên quyết cần thiết trước khi đăng ký môn học.
   - Số lượng tối đa và tối thiểu của sinh viên trong một lớp: xác định số lượng sinh viên tối đa và tối thiểu cho mỗi lớp học.

2. **Quản lý thông tin sinh viên**

   - Mã sinh viên.
   - Tên sinh viên.
   - Ngày sinh.
   - Địa chỉ.
   - Email.
   - Số điện thoại.
   - Khóa học và chương trình học: xác định khóa học và chương trình mà sinh viên đang theo học.
   - Danh sách môn học đã đăng ký.

3. **Quản lý thông tin lớp học**

   - Mã lớp học: mã số định danh duy nhất cho lớp học.
   - Tên lớp học.
   - Giảng viên phụ trách lớp học.
   - Số lượng sinh viên đã đăng ký.
   - Số lượng tối đa và tối thiểu của sinh viên trong lớp.

4. **Quản lý đăng ký môn học**

   - Đăng ký môn học: Cho phép sinh viên đăng ký môn học với quy định số tín chỉ, môn tiên quyết và số lượng tối đa của môn học.
   - Hủy đăng ký môn học: Cho phép sinh viên huỷ đăng ký môn học đã đăng ký trước đó.

5. **Quản lý lịch trình**

   - Tạo lịch trình: xác định thời gian, ngày và địa điểm cho từng môn học và lớp học.
   - Kiểm tra xung đột lịch trình: đảm bảo không có lịch học xung đột giữa các môn học và lớp học.

### Biểu mẫu và Quy định

- [BM1.1 - Nhập môn học](#bm11-nhập-môn-học)
- [BM1.2 - Thông tin môn học](#bm12-thông-tin-môn-học)
- [BM2.1 - Nhập phòng học](#bm21-nhập-phòng-học)
- [BM2.2 - Thông tin phòng học](#bm22-thông-tin-phòng-học)
- [BM3 - Đăng ký lớp học cho môn học](#bm3-đăng-ký-lớp-học-cho-môn-học)
- [BM4 - Thông tin lớp học](#bm4-thông-tin-lớp-học)

### Quy định

- [QĐ1: Sĩ số mỗi lớp học phải lớn hơn 40 và nhỏ hơn 200.](#qđ1-sĩ-số-mỗi-lớp-học-phải-lớn-hơn-40-và-nhỏ-hơn-200)
- [QĐ2: Sức chứa của một phòng học không nhỏ hơn 40 học sinh.](#qđ2-sức-chứa-của-một-phòng-học-không-nhỏ-hơn-40-học-sinh)
- [QĐ3: Mỗi môn học chỉ đi với một phòng học. Một lớp học có thể có nhiều môn học.
Số tiết học là số nguyên từ 1 - 10.
Mỗi một phòng học trong một tiết không thể có nhiều hơn 1 lớp đang học.
Sĩ số của môn học phải nhỏ hơn hoặc bằng sức chứa của phòng học.
](#qđ3)
