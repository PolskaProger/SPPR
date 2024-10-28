﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Web_253505_Tarhonski.API.Data;

#nullable disable

namespace Web_253505_Tarhonski.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240921083627_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("Web_253505_Tarhonski.Domain.Entities.Airsoft", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("CategoryId");

                    b.ToTable("Airsofts");
                });

            modelBuilder.Entity("Web_253505_Tarhonski.Domain.Entities.Category", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Web_253505_Tarhonski.Domain.Entities.Airsoft", b =>
                {
                    b.HasOne("Web_253505_Tarhonski.Domain.Entities.Category", "Category")
                        .WithMany("AirsoftObjects")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Web_253505_Tarhonski.Domain.Entities.Category", b =>
                {
                    b.Navigation("AirsoftObjects");
                });
#pragma warning restore 612, 618
        }
    }
}