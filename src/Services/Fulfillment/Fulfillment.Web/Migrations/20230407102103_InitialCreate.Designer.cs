﻿// <auto-generated />
using System;
using Fulfillment.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fulfillment.Web.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230407102103_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Fulfillment.Core.Buyer.Buyer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Buyers");
                });

            modelBuilder.Entity("Fulfillment.Core.Order.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BuyerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Fulfillment.Core.Buyer.Buyer", b =>
                {
                    b.OwnsOne("Fulfillment.Core.Buyer.Address", "ShippingAddress", b1 =>
                        {
                            b1.Property<Guid>("BuyerId")
                                .HasColumnType("uuid");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Zip")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("BuyerId");

                            b1.ToTable("Buyers");

                            b1.WithOwner()
                                .HasForeignKey("BuyerId");
                        });

                    b.OwnsOne("Fulfillment.Core.Buyer.PaymentInformation", "PaymentInformation", b1 =>
                        {
                            b1.Property<Guid>("BuyerId")
                                .HasColumnType("uuid");

                            b1.HasKey("BuyerId");

                            b1.ToTable("Buyers");

                            b1.WithOwner()
                                .HasForeignKey("BuyerId");

                            b1.OwnsOne("Fulfillment.Core.Buyer.Address", "Address", b2 =>
                                {
                                    b2.Property<Guid>("PaymentInformationBuyerId")
                                        .HasColumnType("uuid");

                                    b2.Property<string>("City")
                                        .HasColumnType("text");

                                    b2.Property<string>("Country")
                                        .HasColumnType("text");

                                    b2.Property<string>("Street")
                                        .HasColumnType("text");

                                    b2.Property<string>("Zip")
                                        .HasColumnType("text");

                                    b2.HasKey("PaymentInformationBuyerId");

                                    b2.ToTable("Buyers");

                                    b2.WithOwner()
                                        .HasForeignKey("PaymentInformationBuyerId");
                                });

                            b1.Navigation("Address")
                                .IsRequired();
                        });

                    b.OwnsOne("Fulfillment.Core.Buyer.PersonalInformation", "PersonalInformation", b1 =>
                        {
                            b1.Property<Guid>("BuyerId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("BuyerId");

                            b1.ToTable("Buyers");

                            b1.WithOwner()
                                .HasForeignKey("BuyerId");
                        });

                    b.Navigation("PaymentInformation")
                        .IsRequired();

                    b.Navigation("PersonalInformation")
                        .IsRequired();

                    b.Navigation("ShippingAddress")
                        .IsRequired();
                });

            modelBuilder.Entity("Fulfillment.Core.Order.Order", b =>
                {
                    b.OwnsOne("Fulfillment.Core.Order.Price", "TotalPrice", b1 =>
                        {
                            b1.Property<Guid>("OrderId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<decimal>("GrossPrice")
                                .HasColumnType("numeric");

                            b1.Property<decimal>("NetPrice")
                                .HasColumnType("numeric");

                            b1.Property<decimal>("Tax")
                                .HasColumnType("numeric");

                            b1.HasKey("OrderId");

                            b1.ToTable("Orders");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.OwnsMany("Fulfillment.Core.Order.OrderItem", "Products", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<Guid>("OrderId")
                                .HasColumnType("uuid");

                            b1.Property<string>("ProductId")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<int>("Quantity")
                                .HasColumnType("integer");

                            b1.HasKey("Id");

                            b1.HasIndex("OrderId");

                            b1.ToTable("OrderItem");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");

                            b1.OwnsOne("Fulfillment.Core.Order.Price", "Price", b2 =>
                                {
                                    b2.Property<int>("OrderItemId")
                                        .HasColumnType("integer");

                                    b2.Property<string>("Currency")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<decimal>("GrossPrice")
                                        .HasColumnType("numeric");

                                    b2.Property<decimal>("NetPrice")
                                        .HasColumnType("numeric");

                                    b2.Property<decimal>("Tax")
                                        .HasColumnType("numeric");

                                    b2.HasKey("OrderItemId");

                                    b2.ToTable("OrderItem");

                                    b2.WithOwner()
                                        .HasForeignKey("OrderItemId");
                                });

                            b1.OwnsOne("Fulfillment.Core.Order.Price", "TotalPrice", b2 =>
                                {
                                    b2.Property<int>("OrderItemId")
                                        .HasColumnType("integer");

                                    b2.Property<string>("Currency")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<decimal>("GrossPrice")
                                        .HasColumnType("numeric");

                                    b2.Property<decimal>("NetPrice")
                                        .HasColumnType("numeric");

                                    b2.Property<decimal>("Tax")
                                        .HasColumnType("numeric");

                                    b2.HasKey("OrderItemId");

                                    b2.ToTable("OrderItem");

                                    b2.WithOwner()
                                        .HasForeignKey("OrderItemId");
                                });

                            b1.Navigation("Price")
                                .IsRequired();

                            b1.Navigation("TotalPrice")
                                .IsRequired();
                        });

                    b.Navigation("Products");

                    b.Navigation("TotalPrice")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
