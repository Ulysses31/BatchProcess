﻿// <auto-generated />
using System;
using BatchProcess.Api.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BatchProcess.API.Migrations
{
    [DbContext(typeof(BatchProcessDbContext))]
    partial class BatchProcessDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.7");

            modelBuilder.Entity("BatchProcess.Api.Models.Entities.BapDto", b =>
                {
                    b.Property<Guid>("Bap_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("guid")
                        .HasColumnName("Bap_Id")
                        .HasComment("Bap's Id.");

                    b.Property<DateOnly?>("Bap_Cancelled_DateTime")
                        .HasColumnType("datetime")
                        .HasComment("Baps cancelled date time.");

                    b.Property<string>("Bap_Code")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar")
                        .HasColumnName("Bap_Code")
                        .HasComment("Bap's code.");

                    b.Property<DateOnly?>("Bap_Failed_DateTime")
                        .HasColumnType("datetime")
                        .HasComment("Baps failure date time.");

                    b.Property<DateOnly?>("Bap_Finished_DateTime")
                        .HasColumnType("datetime")
                        .HasComment("Baps finish date time.");

                    b.Property<Guid?>("Bap_Session_Id")
                        .HasColumnType("guid")
                        .HasColumnName("Bap_Session_Id")
                        .HasComment("Baps session id.");

                    b.Property<DateOnly?>("Bap_Started_DateTime")
                        .HasColumnType("datetime")
                        .HasComment("Baps started date time.");

                    b.Property<int>("Bap_State")
                        .HasColumnType("int")
                        .HasColumnName("Bap_State")
                        .HasComment("0=Αρχική, 1=Σε εξέλιξη, 2=Διακόπηκε, 3=Απέτυχε, 4=Ολοκληρώθηκε");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasComment("Name of the user who created the record.");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasComment("Date and time the record was created.");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime")
                        .HasComment("Date and time the record was last updated.");

                    b.HasKey("Bap_Id")
                        .HasName("PK_Bap_BapId");

                    b.HasIndex(new[] { "Bap_Id" }, "AK_Bap_BapId")
                        .IsUnique();

                    b.ToTable("Bap", "dbo", t =>
                        {
                            t.HasComment("Batch Process notifications");
                        });
                });

            modelBuilder.Entity("BatchProcess.Api.Models.Entities.BapnDto", b =>
                {
                    b.Property<Guid>("BapN_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("guid")
                        .HasColumnName("BapN_Id")
                        .HasComment("BapN's Id.");

                    b.Property<int>("BapN_AA")
                        .HasColumnType("integer")
                        .HasColumnName("BapN_AA")
                        .HasComment("BapN's BapNAA.");

                    b.Property<Guid>("BapN_BapId")
                        .HasColumnType("guid")
                        .HasColumnName("BapN_BapId")
                        .HasComment("BapN's BapId.");

                    b.Property<string>("BapN_Data")
                        .HasMaxLength(100)
                        .HasColumnType("varchar")
                        .HasColumnName("BapN_Data")
                        .HasComment("Bap's data.");

                    b.Property<DateOnly?>("BapN_DateTime")
                        .HasColumnType("datetime")
                        .HasComment("Date and time bapn was created.");

                    b.Property<int>("BapN_kind")
                        .HasColumnType("integer")
                        .HasColumnName("BapN_Kind")
                        .HasComment("BapN's kind.");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasComment("Name of the user who created the record.");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasComment("Date and time the record was created.");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime")
                        .HasComment("Date and time the record was last updated.");

                    b.HasKey("BapN_Id")
                        .HasName("PK_BapN_BapNId");

                    b.HasIndex("BapN_BapId");

                    b.HasIndex(new[] { "BapN_Id" }, "AK_BapN_BapNId")
                        .IsUnique();

                    b.ToTable("BapN", "dbo", t =>
                        {
                            t.HasComment("Batch Process notifications details");
                        });
                });

            modelBuilder.Entity("BatchProcess.Api.Models.Entities.PostDto", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("PostId")
                        .HasComment("Primary key for post records.");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar")
                        .HasColumnName("Body")
                        .HasComment("Post's body description.");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasComment("Date and time the record was created.");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime")
                        .HasComment("Date and time the record was last updated.");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar")
                        .HasColumnName("Title")
                        .HasComment("Post's title.");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("UserId")
                        .HasComment("Key for post user records.");

                    b.HasKey("PostId")
                        .HasName("PK_Post_PostId");

                    b.HasIndex(new[] { "PostId" }, "AK_Post_PostId")
                        .IsUnique();

                    b.ToTable("Post", "dbo", t =>
                        {
                            t.HasComment("Posts for BatchProcess");
                        });
                });

            modelBuilder.Entity("BatchProcess.Api.Models.Entities.BapnDto", b =>
                {
                    b.HasOne("BatchProcess.Api.Models.Entities.BapDto", "BapN_BapDto")
                        .WithMany("Bap_BapNs")
                        .HasForeignKey("BapN_BapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_BapN_Bap_BapId");

                    b.Navigation("BapN_BapDto");
                });

            modelBuilder.Entity("BatchProcess.Api.Models.Entities.BapDto", b =>
                {
                    b.Navigation("Bap_BapNs");
                });
#pragma warning restore 612, 618
        }
    }
}
