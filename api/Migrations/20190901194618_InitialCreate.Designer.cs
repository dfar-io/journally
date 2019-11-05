﻿// <auto-generated />
using System;
using HD.Journally.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace api.Migrations
{
  [DbContext(typeof(Context))]
  [Migration("20190901194618_InitialCreate")]
  partial class InitialCreate
  {
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
      modelBuilder
          .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
          .HasAnnotation("Relational:MaxIdentifierLength", 128)
          .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

      modelBuilder.Entity("HD.Journally.Models.Entry", b =>
          {
            b.Property<int>("EntryId")
                      .ValueGeneratedOnAdd()
                      .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            b.Property<string>("Content")
                      .IsRequired();

            b.Property<DateTime>("Date");

            b.HasKey("EntryId");

            b.ToTable("Entries");
          });
#pragma warning restore 612, 618
    }
  }
}
