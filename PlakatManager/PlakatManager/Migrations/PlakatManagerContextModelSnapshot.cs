﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PlakatManager.Entities;

#nullable disable

namespace PlakatManager.Migrations
{
    [DbContext(typeof(PlakatManagerContext))]
    partial class PlakatManagerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PlakatManager.Entities.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("city");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("country");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("postalcode");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("street");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("userid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("PlakatManager.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("int")
                        .HasColumnName("authorid");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("createdat")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<int>("ElectionItemId")
                        .HasColumnType("int")
                        .HasColumnName("electionitemid");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("message");

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("updatedat");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ElectionItemId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("PlakatManager.Entities.ElectionItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Area")
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("area");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int")
                        .HasColumnName("authorid");

                    b.Property<decimal>("Cost")
                        .HasPrecision(10, 4)
                        .HasColumnType("decimal(10,4)")
                        .HasColumnName("cost");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)")
                        .HasColumnName("discriminator");

                    b.Property<double>("Latitude")
                        .HasColumnType("float")
                        .HasColumnName("latitude");

                    b.Property<double>("Longitude")
                        .HasColumnType("float")
                        .HasColumnName("longitude");

                    b.Property<int>("Priority")
                        .HasColumnType("int")
                        .HasColumnName("priority");

                    b.Property<string>("Size")
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("size");

                    b.Property<int>("StatusId")
                        .HasColumnType("int")
                        .HasColumnName("statusid");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("StatusId");

                    b.ToTable("ElectionItems");

                    b.HasDiscriminator().HasValue("ElectionItem");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("PlakatManager.Entities.ElectionItemTag", b =>
                {
                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.Property<int>("ElectionItemId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfPublication")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("date_of_publication")
                        .HasDefaultValueSql("getutcdate()");

                    b.HasKey("TagId", "ElectionItemId");

                    b.HasIndex("ElectionItemId");

                    b.ToTable("ElectionItemTag");
                });

            modelBuilder.Entity("PlakatManager.Entities.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("Statuses");
                });

            modelBuilder.Entity("PlakatManager.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("PlakatManager.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("firstname");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("fullname");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("lastname");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PlakatManager.Entities.Billboard", b =>
                {
                    b.HasBaseType("PlakatManager.Entities.ElectionItem");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("end_date");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("start_date");

                    b.HasDiscriminator().HasValue("Billboard");
                });

            modelBuilder.Entity("PlakatManager.Entities.LED", b =>
                {
                    b.HasBaseType("PlakatManager.Entities.ElectionItem");

                    b.Property<int>("RefreshRate")
                        .HasColumnType("int")
                        .HasColumnName("refresh_rate");

                    b.Property<string>("Resolution")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("resolution");

                    b.HasDiscriminator().HasValue("LED");
                });

            modelBuilder.Entity("PlakatManager.Entities.Poster", b =>
                {
                    b.HasBaseType("PlakatManager.Entities.ElectionItem");

                    b.Property<string>("PaperType")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("paper_type");

                    b.HasDiscriminator().HasValue("Poster");
                });

            modelBuilder.Entity("PlakatManager.Entities.Address", b =>
                {
                    b.HasOne("PlakatManager.Entities.User", "User")
                        .WithOne("Address")
                        .HasForeignKey("PlakatManager.Entities.Address", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PlakatManager.Entities.Comment", b =>
                {
                    b.HasOne("PlakatManager.Entities.User", "Author")
                        .WithMany("Comments")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("PlakatManager.Entities.ElectionItem", "ElectionItem")
                        .WithMany("Comments")
                        .HasForeignKey("ElectionItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("ElectionItem");
                });

            modelBuilder.Entity("PlakatManager.Entities.ElectionItem", b =>
                {
                    b.HasOne("PlakatManager.Entities.User", "Author")
                        .WithMany("ElectionItems")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlakatManager.Entities.Status", "Status")
                        .WithMany("ElectionItems")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("PlakatManager.Entities.ElectionItemTag", b =>
                {
                    b.HasOne("PlakatManager.Entities.ElectionItem", "ElectionItem")
                        .WithMany()
                        .HasForeignKey("ElectionItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlakatManager.Entities.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ElectionItem");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("PlakatManager.Entities.ElectionItem", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("PlakatManager.Entities.Status", b =>
                {
                    b.Navigation("ElectionItems");
                });

            modelBuilder.Entity("PlakatManager.Entities.User", b =>
                {
                    b.Navigation("Address");

                    b.Navigation("Comments");

                    b.Navigation("ElectionItems");
                });
#pragma warning restore 612, 618
        }
    }
}
