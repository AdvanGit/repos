﻿// <auto-generated />
using System;
using Hospital.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hospital.EntityFramework.Migrations
{
    [DbContext(typeof(HospitalDbContext))]
    partial class HospitalDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Hospital.Domain.Model.Belay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Info")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Belays");
                });

            modelBuilder.Entity("Hospital.Domain.Model.Change", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte>("ChangeTitle")
                        .HasColumnType("tinyint");

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("TimeSpan")
                        .HasColumnType("time");

                    b.Property<DateTime>("TimeStart")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Changes");
                });

            modelBuilder.Entity("Hospital.Domain.Model.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ManagerId")
                        .HasColumnType("int");

                    b.Property<int?>("TitleId")
                        .HasColumnType("int");

                    b.Property<byte>("Type")
                        .HasColumnType("tinyint");

                    b.Property<string>("_Adress")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ManagerId");

                    b.HasIndex("TitleId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("Hospital.Domain.Model.DepartmentTitle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DepartmentTitles");
                });

            modelBuilder.Entity("Hospital.Domain.Model.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BelayCode")
                        .HasColumnType("int");

                    b.Property<DateTime>("BelayDateOut")
                        .HasColumnType("datetime2");

                    b.Property<int?>("BelayId")
                        .HasColumnType("int");

                    b.Property<DateTime>("BirthDay")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("Gender")
                        .HasColumnType("tinyint");

                    b.Property<bool>("HasChild")
                        .HasColumnType("bit");

                    b.Property<bool>("IsMarried")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MidName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("PhoneNumeber")
                        .HasColumnType("bigint");

                    b.Property<string>("_Adress")
                        .HasColumnName("Adress")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BelayId");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("Hospital.Domain.Model.Staff", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BirthDay")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("Gender")
                        .HasColumnType("tinyint");

                    b.Property<bool>("IsEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MidName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("PhoneNumeber")
                        .HasColumnType("bigint");

                    b.Property<string>("Qualification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_Adress")
                        .HasColumnName("Adress")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Staffs");
                });

            modelBuilder.Entity("Hospital.Domain.Model.Change", b =>
                {
                    b.HasOne("Hospital.Domain.Model.Department", "Department")
                        .WithMany("Changes")
                        .HasForeignKey("DepartmentId");
                });

            modelBuilder.Entity("Hospital.Domain.Model.Department", b =>
                {
                    b.HasOne("Hospital.Domain.Model.Staff", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId");

                    b.HasOne("Hospital.Domain.Model.DepartmentTitle", "Title")
                        .WithMany()
                        .HasForeignKey("TitleId");
                });

            modelBuilder.Entity("Hospital.Domain.Model.Patient", b =>
                {
                    b.HasOne("Hospital.Domain.Model.Belay", "Belay")
                        .WithMany()
                        .HasForeignKey("BelayId");
                });

            modelBuilder.Entity("Hospital.Domain.Model.Staff", b =>
                {
                    b.HasOne("Hospital.Domain.Model.Department", "Department")
                        .WithMany("Staffs")
                        .HasForeignKey("DepartmentId");
                });
#pragma warning restore 612, 618
        }
    }
}