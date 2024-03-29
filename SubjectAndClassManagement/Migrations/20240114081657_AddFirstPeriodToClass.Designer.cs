﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace SubjectAndClassManagement.Migrations
{
    [DbContext(typeof(SchoolContext))]
    [Migration("20240114081657_AddFirstPeriodToClass")]
    partial class AddFirstPeriodToClass
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.23")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SubjectAndClassManagement.Models.Class", b =>
                {
                    b.Property<string>("class_id")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("class_period")
                        .HasColumnType("int");

                    b.Property<string>("days_of_week")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("end_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("first_period")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<int>("number_of_members")
                        .HasColumnType("int");

                    b.Property<string>("room_id")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("start_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("subject_id")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("teacher_id")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("class_id");

                    b.HasIndex("room_id");

                    b.HasIndex("subject_id");

                    b.HasIndex("teacher_id");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("SubjectAndClassManagement.Models.Profile", b =>
                {
                    b.Property<int>("profile_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("profile_id"), 1L, 1);

                    b.Property<string>("address")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("citizen_id_card")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<DateTime?>("date_of_birth")
                        .HasColumnType("datetime2");

                    b.Property<string>("gender")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("profile_id");

                    b.HasIndex("username")
                        .IsUnique();

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("SubjectAndClassManagement.Models.Room", b =>
                {
                    b.Property<string>("room_id")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("building_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("room_capacity")
                        .HasColumnType("int");

                    b.Property<string>("room_name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("room_id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("SubjectAndClassManagement.Models.Student", b =>
                {
                    b.Property<string>("student_id")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("academic_year")
                        .HasColumnType("int");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phone_number")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("student_name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("student_id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("SubjectAndClassManagement.Models.StudentRegistration", b =>
                {
                    b.Property<string>("registration_id")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("class_id")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("registration_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("student_id")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("registration_id");

                    b.HasIndex("class_id");

                    b.HasIndex("student_id");

                    b.ToTable("StudentRegistrations");
                });

            modelBuilder.Entity("SubjectAndClassManagement.Models.Subject", b =>
                {
                    b.Property<string>("subject_id")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("credit")
                        .HasColumnType("int");

                    b.Property<string>("subject_description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("subject_name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("subject_id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("SubjectAndClassManagement.Models.Teacher", b =>
                {
                    b.Property<string>("teacher_id")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phone_number")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("teacher_name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("teacher_id");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("SubjectAndClassManagement.Models.TuitionPayment", b =>
                {
                    b.Property<string>("payment_id")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<decimal>("amount_paid")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("amount_to_pay")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("debt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("excess_money")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("payment_time")
                        .HasColumnType("datetime2");

                    b.Property<string>("student_id")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("total_credits")
                        .HasColumnType("int");

                    b.Property<decimal>("tuition_fee")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("payment_id");

                    b.HasIndex("student_id");

                    b.ToTable("TuitionPayments");
                });

            modelBuilder.Entity("SubjectAndClassManagement.Models.User", b =>
                {
                    b.Property<string>("username")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("student_id")
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("teacher_id")
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("user_type")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("username");

                    b.HasIndex("student_id")
                        .IsUnique()
                        .HasFilter("[student_id] IS NOT NULL");

                    b.HasIndex("teacher_id")
                        .IsUnique()
                        .HasFilter("[teacher_id] IS NOT NULL");

                    b.HasIndex("username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SubjectTuitionPayment", b =>
                {
                    b.Property<string>("FeeSubjectssubject_id")
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("TuitionPaymentspayment_id")
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("FeeSubjectssubject_id", "TuitionPaymentspayment_id");

                    b.HasIndex("TuitionPaymentspayment_id");

                    b.ToTable("Fee_Subjects", (string)null);
                });

            modelBuilder.Entity("SubjectAndClassManagement.Models.Class", b =>
                {
                    b.HasOne("SubjectAndClassManagement.Models.Room", "Room")
                        .WithMany("Classes")
                        .HasForeignKey("room_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SubjectAndClassManagement.Models.Subject", "Subject")
                        .WithMany("Classes")
                        .HasForeignKey("subject_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SubjectAndClassManagement.Models.Teacher", "Teacher")
                        .WithMany("Classes")
                        .HasForeignKey("teacher_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("SubjectAndClassManagement.Models.Profile", b =>
                {
                    b.HasOne("SubjectAndClassManagement.Models.User", "User")
                        .WithOne("Profile")
                        .HasForeignKey("SubjectAndClassManagement.Models.Profile", "username")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SubjectAndClassManagement.Models.StudentRegistration", b =>
                {
                    b.HasOne("SubjectAndClassManagement.Models.Class", "Class")
                        .WithMany("StudentRegistrations")
                        .HasForeignKey("class_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SubjectAndClassManagement.Models.Student", "Student")
                        .WithMany("Registrations")
                        .HasForeignKey("student_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("SubjectAndClassManagement.Models.TuitionPayment", b =>
                {
                    b.HasOne("SubjectAndClassManagement.Models.Student", "Student")
                        .WithMany("Payments")
                        .HasForeignKey("student_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("SubjectAndClassManagement.Models.User", b =>
                {
                    b.HasOne("SubjectAndClassManagement.Models.Student", "Student")
                        .WithOne("User")
                        .HasForeignKey("SubjectAndClassManagement.Models.User", "student_id");

                    b.HasOne("SubjectAndClassManagement.Models.Teacher", "Teacher")
                        .WithOne("User")
                        .HasForeignKey("SubjectAndClassManagement.Models.User", "teacher_id");

                    b.Navigation("Student");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("SubjectTuitionPayment", b =>
                {
                    b.HasOne("SubjectAndClassManagement.Models.Subject", null)
                        .WithMany()
                        .HasForeignKey("FeeSubjectssubject_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SubjectAndClassManagement.Models.TuitionPayment", null)
                        .WithMany()
                        .HasForeignKey("TuitionPaymentspayment_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SubjectAndClassManagement.Models.Class", b =>
                {
                    b.Navigation("StudentRegistrations");
                });

            modelBuilder.Entity("SubjectAndClassManagement.Models.Room", b =>
                {
                    b.Navigation("Classes");
                });

            modelBuilder.Entity("SubjectAndClassManagement.Models.Student", b =>
                {
                    b.Navigation("Payments");

                    b.Navigation("Registrations");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SubjectAndClassManagement.Models.Subject", b =>
                {
                    b.Navigation("Classes");
                });

            modelBuilder.Entity("SubjectAndClassManagement.Models.Teacher", b =>
                {
                    b.Navigation("Classes");

                    b.Navigation("User")
                        .IsRequired();
                });

            modelBuilder.Entity("SubjectAndClassManagement.Models.User", b =>
                {
                    b.Navigation("Profile")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
