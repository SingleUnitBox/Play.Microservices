﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Play.Items.Infra.Postgres;

#nullable disable

namespace Play.Items.Infra.Postgres.Migrations
{
    [DbContext(typeof(ItemsPostgresDbContext))]
    partial class ItemsPostgresDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("play.items")
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CrafterSkill", b =>
                {
                    b.Property<Guid>("CrafterId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SkillsSkillId")
                        .HasColumnType("uuid");

                    b.HasKey("CrafterId", "SkillsSkillId");

                    b.HasIndex("SkillsSkillId");

                    b.ToTable("CrafterSkill", "play.items");
                });

            modelBuilder.Entity("Play.Items.Domain.Entities.Crafter", b =>
                {
                    b.Property<Guid>("CrafterId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("CrafterId");

                    b.ToTable("Crafters", "play.items");

                    b.HasData(
                        new
                        {
                            CrafterId = new Guid("b69f5ef7-bf93-4de2-a62f-064652d8dd19"),
                            Name = "Din Foo"
                        },
                        new
                        {
                            CrafterId = new Guid("33364e25-6544-48bd-b87d-37760ee27911"),
                            Name = "Arrgond"
                        },
                        new
                        {
                            CrafterId = new Guid("8ce6633f-c318-4017-acef-369b86fd981d"),
                            Name = "Bleatcher"
                        });
                });

            modelBuilder.Entity("Play.Items.Domain.Entities.Element", b =>
                {
                    b.Property<Guid>("ElementId")
                        .HasColumnType("uuid");

                    b.Property<string>("ElementName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ElementId");

                    b.ToTable("Elements", "play.items");

                    b.HasData(
                        new
                        {
                            ElementId = new Guid("437da457-5951-4826-bc84-e4c8d3d45ec5"),
                            ElementName = "Earth"
                        },
                        new
                        {
                            ElementId = new Guid("19c4d812-4e36-453d-a562-ce618c7bae55"),
                            ElementName = "Wind"
                        },
                        new
                        {
                            ElementId = new Guid("6794bc09-2099-4fe0-bf8c-32deb9d719bd"),
                            ElementName = "Water"
                        },
                        new
                        {
                            ElementId = new Guid("149fa849-be35-40af-b8ae-b1079eb29c52"),
                            ElementName = "Fire"
                        });
                });

            modelBuilder.Entity("Play.Items.Domain.Entities.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CrafterId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ElementId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("Version")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CrafterId");

                    b.HasIndex("ElementId");

                    b.ToTable("Items", "play.items");
                });

            modelBuilder.Entity("Play.Items.Domain.Entities.Skill", b =>
                {
                    b.Property<Guid>("SkillId")
                        .HasColumnType("uuid");

                    b.Property<string>("SkillName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("SkillId");

                    b.ToTable("Skills", "play.items");

                    b.HasData(
                        new
                        {
                            SkillId = new Guid("4b392707-748b-4f62-9545-e93feb1827cf"),
                            SkillName = "Forging"
                        },
                        new
                        {
                            SkillId = new Guid("5eb6e794-28a0-47ad-bec8-d4ce7612f7c2"),
                            SkillName = "Mixing"
                        },
                        new
                        {
                            SkillId = new Guid("e9aed6b8-92b6-4ffe-8dcd-25287e553b4d"),
                            SkillName = "Weaving"
                        },
                        new
                        {
                            SkillId = new Guid("fc10a749-c4ae-4f6d-8966-4a51a1ac1af3"),
                            SkillName = "Griding"
                        });
                });

            modelBuilder.Entity("CrafterSkill", b =>
                {
                    b.HasOne("Play.Items.Domain.Entities.Crafter", null)
                        .WithMany()
                        .HasForeignKey("CrafterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Play.Items.Domain.Entities.Skill", null)
                        .WithMany()
                        .HasForeignKey("SkillsSkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Play.Items.Domain.Entities.Item", b =>
                {
                    b.HasOne("Play.Items.Domain.Entities.Crafter", "Crafter")
                        .WithMany("Items")
                        .HasForeignKey("CrafterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Play.Items.Domain.Entities.Element", "Element")
                        .WithMany()
                        .HasForeignKey("ElementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Crafter");

                    b.Navigation("Element");
                });

            modelBuilder.Entity("Play.Items.Domain.Entities.Crafter", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
