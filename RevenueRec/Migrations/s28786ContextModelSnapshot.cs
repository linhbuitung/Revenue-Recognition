﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RevenueRec.Context;

#nullable disable

namespace RevenueRec.Migrations
{
    [DbContext(typeof(s28786Context))]
    partial class s28786ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RevenueRec.Models.Category", b =>
                {
                    b.Property<int>("IdCategory")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCategory"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IdCategory");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("RevenueRec.Models.Client", b =>
                {
                    b.Property<int>("IdClient")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdClient"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientType")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("IdClient");

                    b.ToTable("Clients");

                    b.HasDiscriminator<string>("ClientType").HasValue("Client");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("RevenueRec.Models.Discount", b =>
                {
                    b.Property<int>("IdDiscount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDiscount"));

                    b.Property<bool>("IsForUpFront")
                        .HasColumnType("bit");

                    b.Property<string>("OfferInfo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Percentage")
                        .HasColumnType("float");

                    b.Property<DateTime>("endDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("startDate")
                        .HasColumnType("datetime2");

                    b.HasKey("IdDiscount");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("RevenueRec.Models.Employee", b =>
                {
                    b.Property<int>("IdEmployee")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdEmployee"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RefreshTokenExp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdEmployee");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("RevenueRec.Models.Payment", b =>
                {
                    b.Property<int>("IdPayment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPayment"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("IdClient")
                        .HasColumnType("int");

                    b.Property<int?>("IdSubscription")
                        .HasColumnType("int");

                    b.Property<int?>("IdUpFrontContract")
                        .HasColumnType("int");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.HasKey("IdPayment");

                    b.HasIndex("IdClient");

                    b.HasIndex("IdSubscription");

                    b.HasIndex("IdUpFrontContract");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("RevenueRec.Models.SoftwareSystem", b =>
                {
                    b.Property<int>("IdSoftwareSystem")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSoftwareSystem"));

                    b.Property<string>("CurrentVersionInfo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdCategory")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("YearlyCost")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("IdSoftwareSystem");

                    b.HasIndex("IdCategory");

                    b.ToTable("SoftwareSystems");
                });

            modelBuilder.Entity("RevenueRec.Models.SoftwareVersion", b =>
                {
                    b.Property<int>("IdSoftwareVersion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSoftwareVersion"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdSoftwareSystem")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdSoftwareVersion");

                    b.HasIndex("IdSoftwareSystem");

                    b.ToTable("SoftwareVersions");
                });

            modelBuilder.Entity("RevenueRec.Models.Subscription", b =>
                {
                    b.Property<int>("IdSubscription")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSubscription"));

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdClient")
                        .HasColumnType("int");

                    b.Property<int>("IdSoftwareSystem")
                        .HasColumnType("int");

                    b.Property<bool>("IsCancelled")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("RenewalPeriod")
                        .HasColumnType("int");

                    b.Property<int>("SoftwareSystemId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("IdSubscription");

                    b.HasIndex("ClientId");

                    b.HasIndex("SoftwareSystemId");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("RevenueRec.Models.UpFrontContract", b =>
                {
                    b.Property<int>("IdUpFrontContract")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUpFrontContract"));

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ContractStartDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdClient")
                        .HasColumnType("int");

                    b.Property<int>("IdSoftwareSystem")
                        .HasColumnType("int");

                    b.Property<int>("IdSoftwareVersion")
                        .HasColumnType("int");

                    b.Property<bool>("IsCancelled")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSigned")
                        .HasColumnType("bit");

                    b.Property<string>("PossibleUpdate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("SoftwareSystemId")
                        .HasColumnType("int");

                    b.Property<int>("SoftwareVersionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SupportYears")
                        .HasColumnType("int");

                    b.HasKey("IdUpFrontContract");

                    b.HasIndex("IdClient");

                    b.HasIndex("IdSoftwareSystem");

                    b.HasIndex("IdSoftwareVersion");

                    b.ToTable("UpFrontContracts");
                });

            modelBuilder.Entity("RevenueRec.Models.CompanyClient", b =>
                {
                    b.HasBaseType("RevenueRec.Models.Client");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("KRS")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasIndex("KRS")
                        .IsUnique()
                        .HasFilter("[KRS] IS NOT NULL");

                    b.HasDiscriminator().HasValue("Company");
                });

            modelBuilder.Entity("RevenueRec.Models.IndividualClient", b =>
                {
                    b.HasBaseType("RevenueRec.Models.Client");

                    b.Property<string>("FirstName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("LastName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PESEL")
                        .HasColumnType("nvarchar(450)");

                    b.HasIndex("PESEL")
                        .IsUnique()
                        .HasFilter("[PESEL] IS NOT NULL");

                    b.HasDiscriminator().HasValue("Individual");
                });

            modelBuilder.Entity("RevenueRec.Models.Payment", b =>
                {
                    b.HasOne("RevenueRec.Models.Client", "Client")
                        .WithMany("Payments")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("RevenueRec.Models.Subscription", "Subscription")
                        .WithMany("Payments")
                        .HasForeignKey("IdSubscription")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("RevenueRec.Models.UpFrontContract", "UpFrontContract")
                        .WithMany("Payments")
                        .HasForeignKey("IdUpFrontContract")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Client");

                    b.Navigation("Subscription");

                    b.Navigation("UpFrontContract");
                });

            modelBuilder.Entity("RevenueRec.Models.SoftwareSystem", b =>
                {
                    b.HasOne("RevenueRec.Models.Category", "Category")
                        .WithMany("SoftwareSystems")
                        .HasForeignKey("IdCategory")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("RevenueRec.Models.SoftwareVersion", b =>
                {
                    b.HasOne("RevenueRec.Models.SoftwareSystem", "SoftwareSystem")
                        .WithMany("SoftwareVersions")
                        .HasForeignKey("IdSoftwareSystem")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("SoftwareSystem");
                });

            modelBuilder.Entity("RevenueRec.Models.Subscription", b =>
                {
                    b.HasOne("RevenueRec.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RevenueRec.Models.SoftwareSystem", "SoftwareSystem")
                        .WithMany()
                        .HasForeignKey("SoftwareSystemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("SoftwareSystem");
                });

            modelBuilder.Entity("RevenueRec.Models.UpFrontContract", b =>
                {
                    b.HasOne("RevenueRec.Models.Client", "Client")
                        .WithMany("UpFrontContracts")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("RevenueRec.Models.SoftwareSystem", "SoftwareSystem")
                        .WithMany("UpFrontContracts")
                        .HasForeignKey("IdSoftwareSystem")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("RevenueRec.Models.SoftwareVersion", "SoftwareVersion")
                        .WithMany("UpFrontContracts")
                        .HasForeignKey("IdSoftwareVersion")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("SoftwareSystem");

                    b.Navigation("SoftwareVersion");
                });

            modelBuilder.Entity("RevenueRec.Models.Category", b =>
                {
                    b.Navigation("SoftwareSystems");
                });

            modelBuilder.Entity("RevenueRec.Models.Client", b =>
                {
                    b.Navigation("Payments");

                    b.Navigation("UpFrontContracts");
                });

            modelBuilder.Entity("RevenueRec.Models.SoftwareSystem", b =>
                {
                    b.Navigation("SoftwareVersions");

                    b.Navigation("UpFrontContracts");
                });

            modelBuilder.Entity("RevenueRec.Models.SoftwareVersion", b =>
                {
                    b.Navigation("UpFrontContracts");
                });

            modelBuilder.Entity("RevenueRec.Models.Subscription", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("RevenueRec.Models.UpFrontContract", b =>
                {
                    b.Navigation("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}
