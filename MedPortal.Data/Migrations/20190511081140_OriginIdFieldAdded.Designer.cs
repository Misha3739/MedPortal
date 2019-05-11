﻿// <auto-generated />
using System;
using MedPortal.Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MedPortal.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20190511081140_OriginIdFieldAdded")]
    partial class OriginIdFieldAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MedPortal.Data.DTO.HBranch", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Alias")
                        .HasMaxLength(100);

                    b.Property<double>("Latitude");

                    b.Property<string>("LineColor")
                        .HasMaxLength(6);

                    b.Property<double>("Longitude");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<long?>("OriginId");

                    b.HasKey("Id");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("MedPortal.Data.DTO.HCity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Alias")
                        .HasMaxLength(100);

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<long?>("OriginId");

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("MedPortal.Data.DTO.HClinic", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<long>("HCityId");

                    b.Property<long>("HDistrictId");

                    b.Property<long>("HStreetId");

                    b.Property<string>("House");

                    b.Property<bool>("IsActive");

                    b.Property<double>("Latitude");

                    b.Property<string>("Logo");

                    b.Property<double>("Longitude");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<bool>("OnlineRecordDoctor");

                    b.Property<long?>("OriginId");

                    b.Property<long?>("ParentId");

                    b.Property<string>("Phone")
                        .HasMaxLength(20);

                    b.Property<string>("RewriteName")
                        .HasMaxLength(200);

                    b.Property<string>("ShortName")
                        .HasMaxLength(200);

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("HCityId");

                    b.HasIndex("HDistrictId");

                    b.HasIndex("HStreetId");

                    b.HasIndex("ParentId");

                    b.ToTable("Clinics");
                });

            modelBuilder.Entity("MedPortal.Data.DTO.HDistrict", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Alias")
                        .HasMaxLength(100);

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<long?>("OriginId");

                    b.HasKey("Id");

                    b.ToTable("Districs");
                });

            modelBuilder.Entity("MedPortal.Data.DTO.HDoctor", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddPhoneNumber");

                    b.Property<string>("Alias")
                        .HasMaxLength(100);

                    b.Property<string>("Category");

                    b.Property<string>("Degree");

                    b.Property<long>("Departure");

                    b.Property<string>("Description");

                    b.Property<long>("ExperienceYear");

                    b.Property<string>("Img");

                    b.Property<string>("ImgFormat");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsExclusivePrice");

                    b.Property<long>("KidsReception");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<long>("OpinionCount");

                    b.Property<long?>("OriginId");

                    b.Property<long>("Price");

                    b.Property<string>("Rank");

                    b.Property<string>("Rating");

                    b.Property<string>("RatingReviewsLabel");

                    b.Property<int>("Sex");

                    b.Property<long?>("SpecialPrice");

                    b.Property<long?>("TelemedId");

                    b.Property<string>("TextAbout");

                    b.Property<string>("TextCourse");

                    b.Property<string>("TextDegree");

                    b.Property<string>("TextEducation");

                    b.Property<string>("TextExperience");

                    b.Property<string>("TextSpec");

                    b.HasKey("Id");

                    b.HasIndex("TelemedId");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("MedPortal.Data.DTO.HSpeciality", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Alias")
                        .HasMaxLength(200);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<long?>("OriginId");

                    b.HasKey("Id");

                    b.ToTable("Specialities");
                });

            modelBuilder.Entity("MedPortal.Data.DTO.HStation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Alias")
                        .HasMaxLength(100);

                    b.Property<long>("BranchId");

                    b.Property<long>("CityId");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<long?>("OriginId");

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

                    b.HasIndex("CityId");

                    b.ToTable("Stations");
                });

            modelBuilder.Entity("MedPortal.Data.DTO.HStreet", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Alias")
                        .HasMaxLength(100);

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<long?>("OriginId");

                    b.HasKey("Id");

                    b.ToTable("Streets");
                });

            modelBuilder.Entity("MedPortal.Data.DTO.HTelemed", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Chat");

                    b.Property<long>("HClinicId");

                    b.Property<long?>("OriginId");

                    b.Property<bool>("Phone");

                    b.HasKey("Id");

                    b.HasIndex("HClinicId");

                    b.ToTable("Telemeds");
                });

            modelBuilder.Entity("MedPortal.Data.DTO.HClinic", b =>
                {
                    b.HasOne("MedPortal.Data.DTO.HCity", "HCity")
                        .WithMany()
                        .HasForeignKey("HCityId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MedPortal.Data.DTO.HDistrict", "HDistrict")
                        .WithMany()
                        .HasForeignKey("HDistrictId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MedPortal.Data.DTO.HStreet", "HStreet")
                        .WithMany()
                        .HasForeignKey("HStreetId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MedPortal.Data.DTO.HClinic", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("MedPortal.Data.DTO.HDoctor", b =>
                {
                    b.HasOne("MedPortal.Data.DTO.HTelemed", "Telemed")
                        .WithMany()
                        .HasForeignKey("TelemedId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("MedPortal.Data.DTO.HStation", b =>
                {
                    b.HasOne("MedPortal.Data.DTO.HBranch", "Branch")
                        .WithMany()
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MedPortal.Data.DTO.HCity", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("MedPortal.Data.DTO.HTelemed", b =>
                {
                    b.HasOne("MedPortal.Data.DTO.HClinic", "HClinic")
                        .WithMany()
                        .HasForeignKey("HClinicId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
