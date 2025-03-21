﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Thunders.TechTest.ApiService.Context;

#nullable disable

namespace Thunders.TechTest.ApiService.Migrations
{
    [DbContext(typeof(ThundersTechTestDbContext))]
    [Migration("20250321133318_CreateDatabase")]
    partial class CreateDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Thunders.TechTest.ApiService.Entities.Pedagio", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataHora")
                        .HasColumnType("datetime2");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Praca")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TipoVeiculo")
                        .HasColumnType("int");

                    b.Property<decimal>("ValorPago")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Pedagios");
                });

            modelBuilder.Entity("Thunders.TechTest.ApiService.Entities.Relatorio", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Dados")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataProcessamento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Parametros")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TipoRelatorio")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Relatorios");
                });
#pragma warning restore 612, 618
        }
    }
}
