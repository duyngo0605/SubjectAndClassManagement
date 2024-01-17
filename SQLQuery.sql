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
	academic_year NVARCHAR(20),
    phone_number NVARCHAR(20)
);

CREATE TABLE Classes (
    class_id NVARCHAR(10) PRIMARY KEY,
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



CREATE TABLE StudentRegistrations (
    registration_id NVARCHAR(20) PRIMARY KEY,
    student_id NVARCHAR(10),
    class_id NVARCHAR(10),
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


CREATE TABLE StudentResults
(
	student_results_id NVARCHAR(20) PRIMARY KEY,
	student_id NVARCHAR(10),
    class_id NVARCHAR(10),
	subject_id NVARCHAR(10),
    grade FLOAT,
	FOREIGN KEY (student_id) REFERENCES Students(student_id),
    FOREIGN KEY (class_id) REFERENCES Classes(class_id),
	FOREIGN KEY (subject_id) REFERENCES Subjects(subject_id),
)

CREATE TABLE ResultColumn (
    resultcolumn_id int IDENTITY(1,1) PRIMARY KEY,
    student_results_id NVARCHAR(20) FOREIGN KEY REFERENCES StudentResults(student_results_id),
    grade FLOAT,
    weight FLOAT,
    CHECK (grade>=0 and grade <=10),
    CHECK (weight>0 and weight<=1),
)

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

create table Notifications
(
    notify_id INT IDENTITY(1,1) PRIMARY key,
    notify_header nvarchar(50),
    notify_day Date,
    notify_description ntext,
)

insert into Users values('admin', 'admin', 'admin', null, null, 'active')