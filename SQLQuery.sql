﻿DROP DATABASE SubjectAndClassManagement
use  master
CREATE DATABASE SubjectAndClassManagement

use SubjectAndClassManagement

CREATE TABLE Rooms (
    room_id NVARCHAR(10) PRIMARY KEY,
    room_capacity INT CHECK (room_capacity >= 40),
    building_name NVARCHAR(255)
);

CREATE TABLE Subjects (
    subject_id NVARCHAR(10) PRIMARY KEY,
    subject_name NVARCHAR(255) NOT NULL,
    subject_description NTEXT,
    credit INT,
);

CREATE TABLE Teachers (
    teacher_id NVARCHAR(10) PRIMARY KEY,
    teacher_name NVARCHAR(255) NOT NULL,
    email NVARCHAR(255),
    phone_number NVARCHAR(20)
);

CREATE TABLE Students (
    student_id NVARCHAR(10) PRIMARY KEY,
    student_name NVARCHAR(255) NOT NULL,
    email NVARCHAR(255),
	academic_year int,
    phone_number NVARCHAR(20)
);

CREATE TABLE Classes (
    class_id NVARCHAR(30) PRIMARY KEY,
    subject_id NVARCHAR(10) FOREIGN KEY REFERENCES Subjects(subject_id),
    room_id NVARCHAR(10) FOREIGN KEY REFERENCES Rooms(room_id),
    teacher_id NVARCHAR(10) FOREIGN KEY REFERENCES Teachers(teacher_id),
    number_of_members INT,
	max_number_of_members INT ,
    days_of_week NVARCHAR(10),
	first_period INT CHECK (first_period >=1 AND first_period<=10),
    class_period INT CHECK (class_period >= 1 AND class_period <= 10),
    start_date DATE,
    end_date DATE,
    semester INT CHECK (semester >= 1 AND semester <= 2),
    academic_year NVARCHAR(20),
	CHECK (start_date < end_date),
	CHECK (max_number_of_members > 40 AND max_number_of_members < 200 AND number_of_members <= max_number_of_members),
);

CREATE TRIGGER trg_AutoGenerateAcademicYear
ON Classes
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Classes
    SET academic_year = 
        CASE 
            WHEN inserted.semester = 1 THEN 
                CAST(YEAR(inserted.start_date) AS NVARCHAR(4)) + ' - ' + CAST(YEAR(inserted.start_date) + 1 AS NVARCHAR(4))
            WHEN inserted.semester = 2 THEN 
                CAST(YEAR(inserted.start_date) - 1 AS NVARCHAR(4)) + ' - ' + CAST(YEAR(inserted.start_date) AS NVARCHAR(4))
        END
    FROM Classes
    INNER JOIN inserted ON Classes.class_id = inserted.class_id;
END;

-- Tạo hàm kiểm tra
CREATE FUNCTION dbo.CheckOpenSessionCount()
RETURNS INT
AS
BEGIN
    DECLARE @OpenSessionCount INT;
    SELECT @OpenSessionCount = COUNT(*)
    FROM RegistrationSessions
    WHERE status = 'open';

    RETURN @OpenSessionCount;
END;

-- Tạo ràng buộc CHECK sử dụng hàm kiểm tra
ALTER TABLE RegistrationSessions
ADD CONSTRAINT CK_MaxOneOpenSession
CHECK (dbo.CheckOpenSessionCount() <= 1);



CREATE TABLE StudentRegistrations (
    registration_id NVARCHAR(30) PRIMARY KEY,
    student_id NVARCHAR(10),
    class_id NVARCHAR(30),
    registration_date DATE,
    status NVARCHAR(20),
    reason NTEXT,
    FOREIGN KEY (student_id) REFERENCES Students(student_id),
    FOREIGN KEY (class_id) REFERENCES Classes(class_id)
);

CREATE TRIGGER trg_UpdateNumberOfMembers
ON StudentRegistrations
AFTER INSERT, DELETE
AS
BEGIN
    -- Update the number_of_members in Classes table for affected class_id
    UPDATE Classes
    SET number_of_members = ISNULL((SELECT COUNT(*) FROM StudentRegistrations WHERE class_id = Classes.class_id), 0)
    WHERE class_id IN (SELECT class_id FROM inserted UNION SELECT class_id FROM deleted);
END;


CREATE TABLE RegistrationSessions (
    session_id NVARCHAR(30) PRIMARY KEY,
    start_date DATETIME,
    end_date DATETIME,
	semester INT,
	academic_year NVARCHAR(20),
    status NVARCHAR(20) CHECK (status IN ('open', 'closed')),
);

alter table RegistrationSessions add constraint CHECK_date
CHECK (start_date<=end_date)

CREATE TRIGGER trg_GenerateSessionId
ON RegistrationSessions
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @nextSessionId NVARCHAR(30); 

    -- Tìm session_id cuối cùng trong bảng
    SELECT TOP 1 @nextSessionId = session_id
    FROM RegistrationSessions
    ORDER BY session_id DESC;

    -- Trích xuất số thứ tự từ session_id cuối cùng
    DECLARE @lastIndex INT;
    SET @lastIndex = CAST(SUBSTRING(@nextSessionId, LEN('RegistrationSessions') + 1, LEN(@nextSessionId)) AS INT);

    -- Tạo session_id mới
    DECLARE @newIndex INT;
    SET @newIndex = ISNULL(@lastIndex + 1, 1);
    DECLARE @newSessionId NVARCHAR(30);
    SET @newSessionId = 'RegistrationSessions' + RIGHT('000' + CAST(@newIndex AS NVARCHAR(3)), 3);

    -- Cập nhật session_id trong bảng inserted
    UPDATE RegistrationSessions
    SET session_id = @newSessionId
    WHERE session_id IN (SELECT session_id FROM inserted);
END;


CREATE TABLE StudentResults
(
	student_results_id NVARCHAR(30) PRIMARY KEY,
	registration_id NVARCHAR(30) FOREIGN KEY REFERENCES StudentRegistrations(registration_id),
    grade FLOAT,
)

CREATE TABLE ResultColumns (
    resultcolumn_id int IDENTITY(1,1) PRIMARY KEY,
    student_results_id NVARCHAR(30) FOREIGN KEY REFERENCES StudentResults(student_results_id),
	column_name NVARCHAR(30),
    grade FLOAT,
    CHECK (grade>=0 and grade <=10),
)

drop trigger trg_DeleteStudentResult



-- Trigger to automatically delete StudentResults when a StudentRegistration is deleted
CREATE TRIGGER trg_DeleteStudentResult
ON StudentRegistrations
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM StudentResults
    WHERE registration_id IN (SELECT deleted.registration_id FROM deleted);
END;


CREATE TABLE TuitionPayments (
    payment_id NVARCHAR(10) PRIMARY KEY,
    student_id NVARCHAR(10),
    total_credits INT,
    tuition_fee MONEY,
    amount_to_pay MONEY,
    amount_paid MONEY,
    payment_time DATETIME,
    excess_money MONEY,
    debt MONEY,
    FOREIGN KEY (student_id) REFERENCES Students(student_id)
);



CREATE TABLE Fee_Subjects (
    payment_id NVARCHAR(10),
    subject_id NVARCHAR(10),
    FOREIGN KEY (payment_id) REFERENCES dbo.TuitionPayments(payment_id),
    FOREIGN KEY (subject_id) REFERENCES [dbo].[Subjects](subject_id)
);

CREATE TABLE Users (
    username NVARCHAR(255) NOT NULL PRIMARY KEY,
    password NVARCHAR(255) NOT NULL,
    user_type NVARCHAR(20) CHECK (user_type IN ('admin', 'student', 'teacher')),
    student_id NVARCHAR(10) NULL,
    teacher_id NVARCHAR(10) NULL,
	status NVARCHAR(10),
    FOREIGN KEY (student_id) REFERENCES Students(student_id),
    FOREIGN KEY (teacher_id) REFERENCES Teachers(teacher_id),
	CHECK (status = 'active' or status = 'locked')
);
alter table Users add constraint CHECK_Status Check (status = 'active' or status = 'locked')

CREATE TABLE Profiles
(
    profile_id INT IDENTITY(1,1),
    username NVARCHAR(255) FOREIGN KEY REFERENCES Users(username),
    date_of_birth DATE,
    gender NVARCHAR(10),
    address NVARCHAR(255),
    citizen_id_card NVARCHAR(12),
)

CREATE TRIGGER trg_CreateUserAndProfile_Student
ON Students
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert into Users table for all inserted students
    INSERT INTO Users (username, password, user_type, student_id, status)
    SELECT 
        student_id, 
        CAST(FLOOR(RAND(CHECKSUM(NEWID())) * 1000000) AS NVARCHAR(255)), -- Random password per row
        'student',
        student_id, 
        'active'
    FROM inserted;

    -- Insert into Profiles table for all inserted students
    INSERT INTO Profiles (username)
    SELECT student_id
    FROM inserted;
END;



CREATE TRIGGER trg_CreateUserAndProfile_Teacher
ON Teachers
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert into Users table for all inserted teachers
    INSERT INTO Users (username, password, user_type, teacher_id, status)
    SELECT 
        teacher_id, 
        CAST(FLOOR(RAND(CHECKSUM(NEWID())) * 1000000) AS NVARCHAR(255)), -- Random password per row
        'teacher',
        teacher_id, 
        'active'
    FROM inserted;

    -- Insert into Profiles table for all inserted teachers
    INSERT INTO Profiles (username)
    SELECT teacher_id
    FROM inserted;
END;


drop trigger trg_CreateUserAndProfile_Teacher


create table Notifications
(
    notify_id INT IDENTITY(1,1) PRIMARY key,
    notify_header nvarchar(50),
    notify_day Date,
    notify_description ntext,
)

CREATE TABLE ClassWeights (
    classweight_id NVARCHAR(30) PRIMARY KEY,
	class_id NVARCHAR(30) FOREIGN KEY REFERENCES CLASSES(class_id),
    attendance_weight FLOAT CHECK (attendance_weight > 0 AND attendance_weight <= 1),
    midterm_weight FLOAT CHECK (midterm_weight > 0 AND midterm_weight <= 1),
    final_weight FLOAT CHECK (final_weight > 0 AND final_weight <= 1),
    FOREIGN KEY (class_id) REFERENCES Classes(class_id)
);

ALTER TABLE ResultColumns
DROP COLUMN weight; -- Nếu có cột weight trong StudentResults

insert into Users values('admin', 'admin', 'admin', null, null, 'active')

select * from Students
select * from Teachers
select * from Users
select * from Profiles
select * from RegistrationSessions
select * from Classes
select * from StudentRegistrations
select * from StudentResults
select * from ResultColumns  
select * from ClassWeights

delete from StudentResults where registration_id = '21521976_SE100.O11'
delete from StudentResults
drop table ResultColumn


alter table StudentResults alter column grade FLOAT

insert into StudentResults values('R_21521999_SE100.O11', '21521999_SE100.O11', 5.5)
insert into ResultColumns values ('R_21521999_SE100.O11', 'Attendance', 5.0, 0.5)
insert into ResultColumns values ('R_21521999_SE100.O11', 'Final', 6.0, 0.5)
insert into ClassWeights values ('CW_SE100.011', 'SE100.O11', 0.2, 0.3, 0.5)
update ResultColumns set grade = 9.0 where column_name = 'Attendance'

CREATE TRIGGER trg_AutoSetStudentResultGrade
ON ResultColumns
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Update the corresponding grade in StudentResults table
    UPDATE StudentResults
    SET grade = (
        SELECT SUM(rc.grade * rc.weight)
        FROM ResultColumns rc
        WHERE rc.student_results_id = StudentResults.student_results_id
    )
    FROM StudentResults
    INNER JOIN inserted ON StudentResults.student_results_id = inserted.student_results_id;
END;



ALTER TABLE ResultColumns
ADD CONSTRAINT CK_TotalWeight CHECK (dbo.CheckTotalWeight(student_results_id) <= 1);

-- Trigger to automatically create StudentResults when a new StudentRegistration is inserted
CREATE TRIGGER trg_CreateStudentResult
ON StudentRegistrations
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert into StudentResults for each inserted StudentRegistration
    INSERT INTO StudentResults (student_results_id, registration_id, grade)
    SELECT 'R_' + CAST(inserted.registration_id AS NVARCHAR(30)), inserted.registration_id, NULL
    FROM inserted;
END;

drop trigger trg_CreateStudentResult

CREATE TRIGGER trg_CreateClassWeight
ON Classes
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert into ClassWeights for each inserted Class
    INSERT INTO ClassWeights (classweight_id, class_id, attendance_weight, midterm_weight, final_weight)
    SELECT 
        'CW_' + CAST(inserted.class_id AS NVARCHAR(10)), -- Generate classweight_id
        inserted.class_id, 
        0.2 AS attendance_weight, 
        0.3 AS midterm_weight, 
        0.5 AS final_weight
    FROM inserted;
END;

delete from profiles where profile_id = 9
delete  from Teachers 
delete from users where user_type = 'teacher'