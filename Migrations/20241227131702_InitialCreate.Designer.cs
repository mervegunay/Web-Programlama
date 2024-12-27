﻿// <auto-generated />
using System;
using Kuafor1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Kuafor1.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241227131702_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Kuafor1.Models.Calisan", b =>
                {
                    b.Property<int>("CalisanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CalisanId"));

                    b.Property<string>("CalisanAd")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CalisanUygunluk")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CalisanId");

                    b.ToTable("Calisanlar");
                });

            modelBuilder.Entity("Kuafor1.Models.Hizmet", b =>
                {
                    b.Property<int>("HizmetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HizmetId"));

                    b.Property<int?>("CalisanId")
                        .HasColumnType("int");

                    b.Property<string>("HizmetAd")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("HizmetFiyat")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("HizmetSure")
                        .HasColumnType("int");

                    b.HasKey("HizmetId");

                    b.HasIndex("CalisanId");

                    b.ToTable("Hizmetler");
                });

            modelBuilder.Entity("Kuafor1.Models.Randevu", b =>
                {
                    b.Property<int>("RandevuId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RandevuId"));

                    b.Property<int>("CalisanId")
                        .HasColumnType("int");

                    b.Property<int>("HizmetId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RandevuTarih")
                        .HasColumnType("datetime2");

                    b.Property<int>("Sure")
                        .HasColumnType("int");

                    b.Property<string>("Telefon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Ucret")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("RandevuId");

                    b.HasIndex("CalisanId");

                    b.HasIndex("HizmetId");

                    b.ToTable("Randevular");
                });

            modelBuilder.Entity("Kuafor1.Models.Uye", b =>
                {
                    b.Property<int>("UyeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UyeId"));

                    b.Property<string>("KullaniciAdi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UyeId");

                    b.ToTable("Uyeler");
                });

            modelBuilder.Entity("Kuafor1.Models.Hizmet", b =>
                {
                    b.HasOne("Kuafor1.Models.Calisan", null)
                        .WithMany("CalisanUzmanliklar")
                        .HasForeignKey("CalisanId");
                });

            modelBuilder.Entity("Kuafor1.Models.Randevu", b =>
                {
                    b.HasOne("Kuafor1.Models.Calisan", "Calisan")
                        .WithMany()
                        .HasForeignKey("CalisanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kuafor1.Models.Hizmet", "Hizmet")
                        .WithMany()
                        .HasForeignKey("HizmetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Calisan");

                    b.Navigation("Hizmet");
                });

            modelBuilder.Entity("Kuafor1.Models.Calisan", b =>
                {
                    b.Navigation("CalisanUzmanliklar");
                });
#pragma warning restore 612, 618
        }
    }
}
