                              
QUẢN LÝ MÔN HỌC VÀ LỚP HỌC


Một trường đại học ở ĐHQG có tới hàng trăm lớp học và hàng ngàn môn học khác nhau, thế nên cần một ứng dụng có thể giúp cho nhà trường, giảng viên và sinh viên có thể quản lý và xem danh sách các môn học và phòng học.
Ứng dụng quản lý môn học và lớp học là một ứng dụng đa năng, giúp tối ưu hóa quá trình giảng dạy và học tập, cũng như tạo ra một cơ sở thông tin đáng tin cậy cho trường học và giáo viên. Nó giúp cải thiện tương tác và giao tiếp giữa tất cả các bên liên quan trong quá trình giáo dục và học tập, mang lại lợi ích lớn cho toàn bộ cộng đồng trường học.
Mô tả đề tài : 
Quản lý môn học và lớp học tại một trường đại học đòi hỏi tổ chức và quản lý thông tin một cách có hệ thống và hiệu quả. Đầu tiên, cần xác định và tổ chức danh sách các môn học, bao gồm mã môn học, tên môn học, số tín chỉ, giáo viên phụ trách, và nội dung chương trình. Công việc này giúp quản lý môn học một cách cụ thể và dễ dàng hơn.
Tiếp theo, phải lên kế hoạch chi tiết cho từng môn học, xác định lịch trình giảng dạy, địa điểm, và giáo viên phù hợp. Điều này đảm bảo rằng việc giảng dạy diễn ra đúng thời gian và đáp ứng được nhu cầu học tập của sinh viên. 
Quản lý giáo viên và gán giáo viên cho từng môn học cũng đóng vai trò quan trọng, đảm bảo sự phù hợp giữa chuyên môn của giáo viên và nội dung môn học. 
Ở mặt khác, quản lý lớp học yêu cầu theo dõi thông tin về từng lớp, gồm mã lớp, tên lớp, môn học tương ứng, giáo viên chủ nhiệm và số lượng sinh viên. Điều này giúp xác định đầy đủ thông tin về lớp học và tạo cơ sở để quản lý sinh viên một cách hiệu quả. 
Theo dõi chuyên cần và kết quả học tập của sinh viên trong lớp là một phần quan trọng, giúp đánh giá hiệu suất học tập và đề xuất các biện pháp hỗ trợ học tập phù hợp. 
Tóm lại, bài toán quản lý môn học và lớp học tại trường đại học đặt ra yêu cầu về sự quản lý thông tin, lập kế hoạch, và theo dõi hiệu suất học tập, tạo nền tảng cho một môi trường học tập chất lượng và hiệu quả.
Các đặc điểm quan trọng của một phần mềm quản lý môn học và lớp học:
Môn học : Dùng để lưu trữ thông tin về môn học , mỗi môn học bao gồm mã môn học(subject_id) , tên môn học(subject_name), mô tả môn học(subject_description), số tín chí(credit), mã môn học tiên quyết, môn học trước.
Phòng học: Lưu trữ thông tin về phòng học trong trường. Mỗi phòng học bao gồm Mã phòng (room_id), Tên phòng (room_name), Sức chứa (room_capacity), Tên tòa nhà (building_name).
Giáo viên : mã giáo viên(teacher_id) , tên giáo viên (teacher_name), email, số điện thoại.
Sinh viên : mã sinh viên(student_id) , tẻn sinh viên (student_name), email, số điện thoại (phone_number), danh sách lớp học đăng ký (class_id).
Lớp học:  Dùng để lưu trữ thông tin về lớp học, mỗi lớp học bao gồm: mã lớp học (class_id), môn học (subject_name), phòng học (room_id), sĩ số (number_of_members), thứ (days_of_week), tiết học (class_period), ngày bắt đầu (start_date), ngày kết thúc (end_date), năm học (year).
Dưới đây là phần mô tả về các chức năng chính được thực hiện thường xuyên của hệ thống quản lý bán hàng siêu thị:
Nhập/Xuất:
Nhập môn học:
Người dùng kiểm tra danh sách môn học và nhập thêm một môn học mới. Thông tin của một môn học bao gồm: Mã lớp học (class_id), Tên môn học (subject_name), Sĩ số (number_of_students), Ghi chú (note).
Nhập phòng học:
Người dùng kiểm tra danh sách phòng học và nhập phòng học mới. Thông tin của một phòng học bao gồm: Mã phòng (room_id), Tên phòng (room_name), Sức chứa (room_capacity), Tòa nhà (building_name).
Nhập lớp học:
Người dùng kiểm tra danh sách môn học và phòng học, sau đó nhập thông tin của lớp học. Thông tin của lớp học bao gồm: Mã lớp (class_id), Mã môn học(subject_id), Mã phòng học (room_id), Số tiết học (class_period), Thứ (week_day), Ngày bắt đầu (start_date), Ngày kết thúc (end_date), Tên giảng viên phụ trách (teacher_name)


Hủy môn học
Sau khi giai đoạn đăng ký kết thúc, các sinh viên có thể hủy các môn học, lớp học không còn năm trong kế hoạch của minh. Việc hủy môn học bao gồm: Mã lớp học(class_id), mã môn học (subject_id), lý do hủy (reason), trạng thái (status). Có 3 trạng thái hủy môn học bao gồm: Chưa duyệt, Duyệt và Không được duyệt; trong trường hợp không được duyệt sẽ bao gồm lý do không được duyệt. Sau khi sinh viên gửi biểu mẫu đi thì người phụ trách (hệ thống) sẽ xem xét việc duyệt hay không duyệt đơn hủy môn học của các sinh viên.
Thanh Toán học phí
Sau khi hoàn thành giai đoạn đăng kí cũng như hủy môn học, các sinh viên có nghĩa vụ hoàn thành việc thanh toán học phí. Việc thanh toán học phí bao gồm: Số tín chỉ đăng kí (total_credits), danh sách các môn đăng kí (subject_id), học phí (tuition_fee), số tiền phải đóng (amoumt_to_pay), số tiền đã đóng (amount_paid), thời gian đóng (payment_time). Nếu sinh viên đóng thừa tiền ở học kì trước thì sẽ có thêm mục số tiền còn thừa (excess_money). Ngược lại, nếu sinh viên còn nợ tiền thì sẽ có mục còn nợ (debt).					
							 	
Quản lý:						 								
●  Quản lý môn học:
 								
Trường cần quản lý thông tin môn học như tên môn học, ví dụ: "Toán cao cấp".
 Mã môn học: Là mã số định danh duy nhất cho mỗi môn học, ví dụ: "MATH101". 
Giảng viên phụ trách môn học. Số tín chỉ: xác định số tín chỉ tương ứng với môn học. 
Lịch học: Thời gian, ngày và địa điểm diễn ra môn học.
 Mô tả môn học: cung cấp một mô tả ngắn gọn về nội dung và mục tiêu của môn học.
 Yêu cầu tiên quyết: danh sách các môn học hoặc điều kiện tiên quyết cần thiết trước khi đăng ký môn học. 
Số lượng tối đa và tối thiểu của sinh viên trong một lớp: xác định số lượng sinh viên tối đa và tối thiểu cho mỗi lớp học.
 													
●  Quản lý thông tin sinh viên:

Trường cần quản lý thông tin cá nhân của sinh viên như tên sinh viên , mã sinh viên , ngày sinh , địa chỉ, email, số điện thoại. 
Khóa học và chương trình học: xác định khóa học và chương trình mà sinh viên đang theo học. 
Danh sách môn học đã đăng ký: Liệt kê các môn học mà sinh viên đã đăng ký .			
●  Quản lý thông tin lớp học:
Trường cần quản lý thông tin lớp học như : 
Mã lớp: mã số định danh duy nhất cho lớp học. 
Tên lớp: tên gọi của lớp học, ví dụ: "Lớp A". 
Giảng viên: giảng viên phụ trách lớp học. 
Số lượng sinh viên đã đăng ký: số lượng sinh viên đã đăng ký vào lớp học. 
Số lượng tối đa và tối thiểu của sinh viên: xác định số lượng sinh viên tối đa và tối thiểu cho lớp học.
 									
●  Quản lý đăng ký môn học:
Đăng ký môn học: Cho phép sinh viên đăng ký môn học với quy định số tín chỉ, môn tiên quyết và số lượng tối đa của môn học. 
Huỷ đăng ký môn học: Cho phép sinh viên huỷ đăng ký môn học đã đăng ký trước đó. 				
● Quản lý lịch trình :
Tạo lịch trình: xác định thời gian, ngày và địa điểm cho từng môn học và lớp học. 
Kiểm tra xung đột lịch trình: đảm bảo không có lịch học xung đột giữa các môn học và lớp học.					
Tìm kiếm:								
Tìm kiếm theo tên môn học :					
Người dùng có thể tìm kiếm môn học bằng cách nhập tên môn học hoặc một phần của tên môn học vào ô tìm kiếm.				
Hệ thống sẽ tìm kiếm trong cơ sở dữ liệu và hiển thị các kết quả phù hợp với từ khóa tìm kiếm.					
Tìm kiếm theo tên lớp hoặc mã lớp :					
Người dùng có thể tìm kiếm lớp bằng cách nhập mã lớp hoặc tên lớp để xem thông tin lớp và học sinh trong lớp. 		
Tìm kiếm theo sinh viên :						
Cho phép tìm kiếm thông tin về sinh viên dựa trên tên, mã số sinh viên .			
Kết quả tìm kiếm sẽ hiển thị danh sách sinh viên phù hợp.										
Những thông tin báo cáo, thống kê cần phải có từ hệ thống này là:			
							 				
-  Báo cáo số lượng sinh viên đăng ký môn học .							 								
-  Báo cáo lịch sử đăng ký.							 	
 									
- Thống kê số lượng sinh viên theo môn học và lớp học.
