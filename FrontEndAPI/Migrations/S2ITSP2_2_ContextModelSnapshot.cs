﻿// <auto-generated />
using FrontEndAPI.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Storage.Internal;
using System;

namespace FrontEndAPI.Migrations
{
    [DbContext(typeof(S2ITSP2_2_Context))]
    partial class S2ITSP2_2_ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026");

            modelBuilder.Entity("FrontEndAPI.Models.Entities.Activity", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("description")
                        .HasColumnType("text");

                    b.Property<DateTime>("EndTime")
                        .HasColumnName("endtime");

                    b.Property<long>("EventId")
                        .HasColumnName("eventid");

                    b.Property<bool>("IsActive")
                        .HasColumnName("isactive");

                    b.Property<DateTime>("LastUpdated")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnName("lastupdated")
                        .HasColumnType("timestamp");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasMaxLength(500);

                    b.Property<decimal>("Price")
                        .HasColumnName("price");

                    b.Property<int>("RemainingCapacity")
                        .HasColumnName("remainingcapacity");

                    b.Property<long?>("SpeakerId")
                        .HasColumnName("speakerid");

                    b.Property<DateTime>("StartTime")
                        .HasColumnName("starttime");

                    b.Property<string>("UUID")
                        .IsRequired()
                        .HasColumnName("uuid")
                        .HasMaxLength(36);

                    b.Property<long>("Version")
                        .HasColumnName("version");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("SpeakerId");

                    b.HasIndex("UUID", "IsActive")
                        .IsUnique();

                    b.HasIndex("Name", "UUID", "IsActive")
                        .IsUnique();

                    b.ToTable("api_activities");
                });

            modelBuilder.Entity("FrontEndAPI.Models.Entities.Event", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("description")
                        .HasColumnType("text");

                    b.Property<DateTime>("EndTime")
                        .HasColumnName("endtime");

                    b.Property<string>("ImageURL")
                        .HasColumnName("imageurl")
                        .HasMaxLength(500);

                    b.Property<bool>("IsActive")
                        .HasColumnName("isactive");

                    b.Property<DateTime>("LastUpdated")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnName("lastupdated")
                        .HasColumnType("timestamp");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasMaxLength(500);

                    b.Property<DateTime>("StartTime")
                        .HasColumnName("starttime");

                    b.Property<string>("UUID")
                        .IsRequired()
                        .HasColumnName("uuid")
                        .HasMaxLength(36);

                    b.Property<long>("Version")
                        .HasColumnName("version");

                    b.HasKey("Id");

                    b.HasIndex("Name", "IsActive")
                        .IsUnique();

                    b.HasIndex("UUID", "IsActive")
                        .IsUnique();

                    b.ToTable("api_events");
                });

            modelBuilder.Entity("FrontEndAPI.Models.Entities.Reservation", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<long>("ActivityId")
                        .HasColumnName("activityid");

                    b.Property<bool>("HasAttended")
                        .HasColumnName("hasattended");

                    b.Property<bool>("IsActive")
                        .HasColumnName("isactive");

                    b.Property<DateTime>("LastUpdated")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnName("lastupdated")
                        .HasColumnType("timestamp");

                    b.Property<bool>("PayedFee")
                        .HasColumnName("payedfee");

                    b.Property<string>("UUID")
                        .IsRequired()
                        .HasColumnName("uuid")
                        .HasMaxLength(36);

                    b.Property<long>("Version")
                        .HasColumnName("version");

                    b.Property<long>("VisitorId")
                        .HasColumnName("visitorid");

                    b.Property<bool>("WithInvoice")
                        .HasColumnName("withinvoice");

                    b.HasKey("Id");

                    b.HasIndex("VisitorId");

                    b.HasIndex("UUID", "IsActive")
                        .IsUnique();

                    b.HasIndex("ActivityId", "VisitorId", "IsActive")
                        .IsUnique();

                    b.ToTable("api_reservations");
                });

            modelBuilder.Entity("FrontEndAPI.Models.Entities.User", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Bus")
                        .HasColumnName("bus")
                        .HasMaxLength(24);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnName("city")
                        .HasMaxLength(500);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("email")
                        .HasMaxLength(500);

                    b.Property<bool>("EmailVerified")
                        .HasColumnName("emailverified");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnName("firstname")
                        .HasMaxLength(500);

                    b.Property<bool>("IsActive")
                        .HasColumnName("isactive");

                    b.Property<DateTime>("LastUpdated")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnName("lastupdated")
                        .HasColumnType("timestamp");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnName("lastname")
                        .HasMaxLength(500);

                    b.Property<int>("Number")
                        .HasColumnName("number")
                        .HasMaxLength(500);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("password")
                        .HasMaxLength(500);

                    b.Property<string>("RolesString")
                        .IsRequired()
                        .HasColumnName("roles")
                        .HasMaxLength(30);

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnName("salt")
                        .HasMaxLength(100);

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnName("street")
                        .HasMaxLength(500);

                    b.Property<string>("UUID")
                        .IsRequired()
                        .HasColumnName("uuid")
                        .HasMaxLength(36);

                    b.Property<long>("Version")
                        .HasColumnName("version");

                    b.Property<int>("ZipCode")
                        .HasColumnName("zipcode");

                    b.HasKey("Id");

                    b.HasIndex("Email", "IsActive")
                        .IsUnique();

                    b.HasIndex("UUID", "IsActive")
                        .IsUnique();

                    b.ToTable("api_users");
                });

            modelBuilder.Entity("FrontEndAPI.Models.Entities.Activity", b =>
                {
                    b.HasOne("FrontEndAPI.Models.Entities.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FrontEndAPI.Models.Entities.User", "Speaker")
                        .WithMany()
                        .HasForeignKey("SpeakerId");
                });

            modelBuilder.Entity("FrontEndAPI.Models.Entities.Reservation", b =>
                {
                    b.HasOne("FrontEndAPI.Models.Entities.Activity", "Activity")
                        .WithMany()
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FrontEndAPI.Models.Entities.User", "Visitor")
                        .WithMany()
                        .HasForeignKey("VisitorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
