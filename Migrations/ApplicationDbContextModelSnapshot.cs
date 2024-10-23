﻿// <auto-generated />
using System;
using Contract_Monthly_Claim_System.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Contract_Monthly_Claim_System.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Contract_Monthly_Claim_System.Models.AcademicManager", b =>
                {
                    b.Property<int>("ManagerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ManagerID"));

                    b.Property<string>("ManagerEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ManagerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ManagerPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ManagerPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ManagerSurname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ManagerID");

                    b.ToTable("AcademicManagers");
                });

            modelBuilder.Entity("Contract_Monthly_Claim_System.Models.ApprovalProcess", b =>
                {
                    b.Property<int>("ApprovalID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ApprovalID"));

                    b.Property<DateTime>("ApprovalDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ApprovalStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClaimID")
                        .HasColumnType("int");

                    b.Property<int>("CoordinatorID")
                        .HasColumnType("int");

                    b.Property<string>("Feedback")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ManagerID")
                        .HasColumnType("int");

                    b.HasKey("ApprovalID");

                    b.HasIndex("ClaimID");

                    b.HasIndex("CoordinatorID");

                    b.HasIndex("ManagerID");

                    b.ToTable("ApprovalProcesses");
                });

            modelBuilder.Entity("Contract_Monthly_Claim_System.Models.Claims", b =>
                {
                    b.Property<int>("ClaimID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClaimID"));

                    b.Property<string>("AdditionalNotes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("HourlyRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("HoursWorked")
                        .HasColumnType("int");

                    b.Property<int>("LecturerID")
                        .HasColumnType("int");

                    b.Property<string>("RejectionFeedback")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SubmissionDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TotalClaimAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ClaimID");

                    b.HasIndex("LecturerID");

                    b.ToTable("Claims");
                });

            modelBuilder.Entity("Contract_Monthly_Claim_System.Models.ClaimsModules", b =>
                {
                    b.Property<int>("ClaimID")
                        .HasColumnType("int");

                    b.Property<string>("ModuleCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ClaimsModulesID")
                        .HasColumnType("int");

                    b.HasKey("ClaimID", "ModuleCode");

                    b.HasIndex("ModuleCode");

                    b.ToTable("ClaimsModules");
                });

            modelBuilder.Entity("Contract_Monthly_Claim_System.Models.Lecturer", b =>
                {
                    b.Property<int>("LecturerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LecturerID"));

                    b.Property<string>("LecturerEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LecturerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LecturerPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LecturerPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LecturerSurname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LecturerID");

                    b.ToTable("Lecturers");
                });

            modelBuilder.Entity("Contract_Monthly_Claim_System.Models.Module", b =>
                {
                    b.Property<string>("ModuleCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModuleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ModuleCode");

                    b.ToTable("Modules");
                });

            modelBuilder.Entity("Contract_Monthly_Claim_System.Models.ProgrammeCoordinator", b =>
                {
                    b.Property<int>("CoordinatorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CoordinatorID"));

                    b.Property<string>("CoordinatorEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CoordinatorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CoordinatorPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CoordinatorPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CoordinatorSurname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CoordinatorID");

                    b.ToTable("ProgrammeCoordinators");
                });

            modelBuilder.Entity("Contract_Monthly_Claim_System.Models.SupportingDocuments", b =>
                {
                    b.Property<int>("DocumentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocumentID"));

                    b.Property<int>("ClaimID")
                        .HasColumnType("int");

                    b.Property<string>("DocumentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SubmissionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("DocumentID");

                    b.HasIndex("ClaimID");

                    b.ToTable("SupportingDocuments");
                });

            modelBuilder.Entity("Contract_Monthly_Claim_System.Models.Users", b =>
                {
                    b.Property<int>("userID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("userID"));

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("userID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Contract_Monthly_Claim_System.Models.ApprovalProcess", b =>
                {
                    b.HasOne("Contract_Monthly_Claim_System.Models.Claims", "Claims")
                        .WithMany("ApprovalProcesses")
                        .HasForeignKey("ClaimID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Contract_Monthly_Claim_System.Models.ProgrammeCoordinator", "Coordinator")
                        .WithMany("ApprovalProcesses")
                        .HasForeignKey("CoordinatorID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Contract_Monthly_Claim_System.Models.AcademicManager", "Manager")
                        .WithMany("ApprovalProcesses")
                        .HasForeignKey("ManagerID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Claims");

                    b.Navigation("Coordinator");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("Contract_Monthly_Claim_System.Models.Claims", b =>
                {
                    b.HasOne("Contract_Monthly_Claim_System.Models.Lecturer", "Lecturer")
                        .WithMany("Claims")
                        .HasForeignKey("LecturerID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Lecturer");
                });

            modelBuilder.Entity("Contract_Monthly_Claim_System.Models.ClaimsModules", b =>
                {
                    b.HasOne("Contract_Monthly_Claim_System.Models.Claims", "Claims")
                        .WithMany("ClaimsModules")
                        .HasForeignKey("ClaimID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Contract_Monthly_Claim_System.Models.Module", "Module")
                        .WithMany("ClaimsModules")
                        .HasForeignKey("ModuleCode")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Claims");

                    b.Navigation("Module");
                });

            modelBuilder.Entity("Contract_Monthly_Claim_System.Models.SupportingDocuments", b =>
                {
                    b.HasOne("Contract_Monthly_Claim_System.Models.Claims", "Claims")
                        .WithMany("SupportingDocuments")
                        .HasForeignKey("ClaimID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Claims");
                });

            modelBuilder.Entity("Contract_Monthly_Claim_System.Models.AcademicManager", b =>
                {
                    b.Navigation("ApprovalProcesses");
                });

            modelBuilder.Entity("Contract_Monthly_Claim_System.Models.Claims", b =>
                {
                    b.Navigation("ApprovalProcesses");

                    b.Navigation("ClaimsModules");

                    b.Navigation("SupportingDocuments");
                });

            modelBuilder.Entity("Contract_Monthly_Claim_System.Models.Lecturer", b =>
                {
                    b.Navigation("Claims");
                });

            modelBuilder.Entity("Contract_Monthly_Claim_System.Models.Module", b =>
                {
                    b.Navigation("ClaimsModules");
                });

            modelBuilder.Entity("Contract_Monthly_Claim_System.Models.ProgrammeCoordinator", b =>
                {
                    b.Navigation("ApprovalProcesses");
                });
#pragma warning restore 612, 618
        }
    }
}
