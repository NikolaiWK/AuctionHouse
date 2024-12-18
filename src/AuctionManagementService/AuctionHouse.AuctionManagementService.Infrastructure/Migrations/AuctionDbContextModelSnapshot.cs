﻿// <auto-generated />
using System;
using AuctionHouse.AuctionManagementService.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AuctionHouse.AuctionManagementService.Infrastructure.Migrations
{
    [DbContext(typeof(AuctionDbContext))]
    partial class AuctionDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AuctionHouse.AuctionManagementService.Domain.Entities.Auction", b =>
                {
                    b.Property<Guid>("AuctionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("StartingPrice")
                        .HasColumnType("numeric");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("AuctionId");

                    b.ToTable("Auctions");
                });

            modelBuilder.Entity("AuctionHouse.AuctionManagementService.Domain.Entities.BidSummary", b =>
                {
                    b.Property<Guid>("BidSummaryId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuctionId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("CurrentHighestBid")
                        .HasColumnType("numeric");

                    b.Property<int>("TotalBids")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("BidSummaryId");

                    b.HasIndex("AuctionId")
                        .IsUnique();

                    b.ToTable("BidSummaries");
                });

            modelBuilder.Entity("AuctionHouse.AuctionManagementService.Domain.Entities.BidSummary", b =>
                {
                    b.HasOne("AuctionHouse.AuctionManagementService.Domain.Entities.Auction", "Auction")
                        .WithOne("BidSummary")
                        .HasForeignKey("AuctionHouse.AuctionManagementService.Domain.Entities.BidSummary", "AuctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Auction");
                });

            modelBuilder.Entity("AuctionHouse.AuctionManagementService.Domain.Entities.Auction", b =>
                {
                    b.Navigation("BidSummary")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
