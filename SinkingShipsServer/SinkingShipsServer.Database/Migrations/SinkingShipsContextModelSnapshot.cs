﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SinkingShipsServer.Database;

namespace SinkingShipsServer.Database.Migrations
{
    [DbContext(typeof(SinkingShipsContext))]
    partial class SinkingShipsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("SinkingShipsServer.Database.Models.ClientData", b =>
                {
                    b.Property<int>("PrimaryKey")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Won")
                        .HasColumnType("int");

                    b.HasKey("PrimaryKey");

                    b.ToTable("AllRegisteredPlayers");
                });

            modelBuilder.Entity("SinkingShipsServer.Database.Models.History", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("FirstPlayerPoints")
                        .HasColumnType("int");

                    b.Property<string>("GameID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SecondPlayerPoints")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("History");
                });

            modelBuilder.Entity("SinkingShipsServer.Database.Models.Player", b =>
                {
                    b.Property<int>("PrimaryKey")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("ClientDataPrimaryKey")
                        .HasColumnType("int");

                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PrimaryKey");

                    b.HasIndex("ClientDataPrimaryKey");

                    b.ToTable("GameRequests");
                });

            modelBuilder.Entity("SinkingShipsServer.Database.Models.Player", b =>
                {
                    b.HasOne("SinkingShipsServer.Database.Models.ClientData", null)
                        .WithMany("GameRequests")
                        .HasForeignKey("ClientDataPrimaryKey");
                });

            modelBuilder.Entity("SinkingShipsServer.Database.Models.ClientData", b =>
                {
                    b.Navigation("GameRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
