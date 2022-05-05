using System;
using System.Collections.Generic;
using BlogVisualStudio.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogEF.Data.Mappings
{
    public class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            //Tabela
            builder.ToTable("Post");

            //Chave primaria
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd() //Gera um valor de identity toda vez que um id é adicionado
                .UseIdentityColumn(); //Referencia com a coluna Identity

            builder.Property(x => x.Title)
                .IsRequired() //NOTNULL
                .HasColumnName("Title") //Nome da propiedade
                .HasColumnType("NVARCHAR") //Tipo de dados
                .HasMaxLength(80); //Maximo de caracteres

            builder.Property(x => x.Slug)
                .IsRequired() //NOTNULL
                .HasColumnName("Slug") //Nome da propiedade
                .HasColumnType("NVARCHAR") //Tipo de dados
                .HasMaxLength(80); //Maximo de caracteres

            builder.Property(x => x.LastUpdateDate)
                .HasColumnName("LastUpdateDate")
                .IsRequired()
                .HasColumnType("SMALLDATETIME")
                .HasMaxLength(60)
                .HasDefaultValue(DateTime.Now.ToUniversalTime());

            //Indices (usado para propiedades que são muito acessadas)
            builder
                .HasIndex(x => x.Slug, "IX_Post_Slug")
                .IsUnique(); //Garante que essa propiedade é unica;


            builder
                .HasOne(x => x.Author)
                .WithMany(x => x.Posts)
                .HasConstraintName("FK_Post_Author")
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.Category)
                .WithMany(x => x.Posts)
                .HasConstraintName("FK_Post_Category")
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(x => x.Tags)
                .WithMany(x => x.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    "PostTag",
                    post => post
                        .HasOne<Tag>()
                        .WithMany()
                        .HasForeignKey("PostId")
                        .HasConstraintName("FK_PostRole_PostId")
                        .OnDelete(DeleteBehavior.Cascade),
                    tag => tag
                        .HasOne<Post>()
                        .WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK_PostTag_TagId")
                        .OnDelete(DeleteBehavior.Cascade));
        }
    }
}