﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VdarApi.Models;

namespace VdarApi.Migrations
{
    [DbContext(typeof(VdarDbContext))]
    [Migration("20190226172301_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VdarApi.Models.TokensPair", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnName("access_token");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnName("client_id");

                    b.Property<DateTime>("CreatedDateUTC")
                        .HasColumnName("created_date_utc");

                    b.Property<string>("FingerPrint")
                        .IsRequired()
                        .HasColumnName("finger_print");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnName("refresh_token");

                    b.Property<string>("UpdateHashSum")
                        .IsRequired()
                        .HasColumnName("update_hash_sum");

                    b.HasKey("Id");

                    b.ToTable("tokens");
                });
#pragma warning restore 612, 618
        }
    }
}
