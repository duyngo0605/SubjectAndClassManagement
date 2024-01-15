using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubjectAndClassManagement.Migrations
{
    public partial class AddFirstPeriodToClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    room_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    room_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    room_capacity = table.Column<int>(type: "int", nullable: false),
                    building_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.room_id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    student_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    student_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    academic_year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.student_id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    subject_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    subject_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    subject_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    credit = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.subject_id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    teacher_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    teacher_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.teacher_id);
                });

            migrationBuilder.CreateTable(
                name: "TuitionPayments",
                columns: table => new
                {
                    payment_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    student_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    total_credits = table.Column<int>(type: "int", nullable: false),
                    tuition_fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    amount_to_pay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    amount_paid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    payment_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    excess_money = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    debt = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TuitionPayments", x => x.payment_id);
                    table.ForeignKey(
                        name: "FK_TuitionPayments_Students_student_id",
                        column: x => x.student_id,
                        principalTable: "Students",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    class_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    subject_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    room_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    teacher_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    number_of_members = table.Column<int>(type: "int", nullable: false),
                    days_of_week = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    class_period = table.Column<int>(type: "int", nullable: false),
                    first_period = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.class_id);
                    table.ForeignKey(
                        name: "FK_Classes_Rooms_room_id",
                        column: x => x.room_id,
                        principalTable: "Rooms",
                        principalColumn: "room_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Classes_Subjects_subject_id",
                        column: x => x.subject_id,
                        principalTable: "Subjects",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Classes_Teachers_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "Teachers",
                        principalColumn: "teacher_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    user_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    student_id = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    teacher_id = table.Column<string>(type: "nvarchar(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.username);
                    table.ForeignKey(
                        name: "FK_Users_Students_student_id",
                        column: x => x.student_id,
                        principalTable: "Students",
                        principalColumn: "student_id");
                    table.ForeignKey(
                        name: "FK_Users_Teachers_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "Teachers",
                        principalColumn: "teacher_id");
                });

            migrationBuilder.CreateTable(
                name: "Fee_Subjects",
                columns: table => new
                {
                    FeeSubjectssubject_id = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    TuitionPaymentspayment_id = table.Column<string>(type: "nvarchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fee_Subjects", x => new { x.FeeSubjectssubject_id, x.TuitionPaymentspayment_id });
                    table.ForeignKey(
                        name: "FK_Fee_Subjects_Subjects_FeeSubjectssubject_id",
                        column: x => x.FeeSubjectssubject_id,
                        principalTable: "Subjects",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fee_Subjects_TuitionPayments_TuitionPaymentspayment_id",
                        column: x => x.TuitionPaymentspayment_id,
                        principalTable: "TuitionPayments",
                        principalColumn: "payment_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentRegistrations",
                columns: table => new
                {
                    registration_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    student_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    class_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    registration_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    reason = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentRegistrations", x => x.registration_id);
                    table.ForeignKey(
                        name: "FK_StudentRegistrations_Classes_class_id",
                        column: x => x.class_id,
                        principalTable: "Classes",
                        principalColumn: "class_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentRegistrations_Students_student_id",
                        column: x => x.student_id,
                        principalTable: "Students",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    profile_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    citizen_id_card = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.profile_id);
                    table.ForeignKey(
                        name: "FK_Profiles_Users_username",
                        column: x => x.username,
                        principalTable: "Users",
                        principalColumn: "username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_room_id",
                table: "Classes",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_subject_id",
                table: "Classes",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_teacher_id",
                table: "Classes",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_Fee_Subjects_TuitionPaymentspayment_id",
                table: "Fee_Subjects",
                column: "TuitionPaymentspayment_id");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_username",
                table: "Profiles",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegistrations_class_id",
                table: "StudentRegistrations",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegistrations_student_id",
                table: "StudentRegistrations",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_TuitionPayments_student_id",
                table: "TuitionPayments",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_student_id",
                table: "Users",
                column: "student_id",
                unique: true,
                filter: "[student_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_teacher_id",
                table: "Users",
                column: "teacher_id",
                unique: true,
                filter: "[teacher_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_username",
                table: "Users",
                column: "username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fee_Subjects");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "StudentRegistrations");

            migrationBuilder.DropTable(
                name: "TuitionPayments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
