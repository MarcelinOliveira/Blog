using BlogEF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogEF.Data.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            //Tabela
            builder.ToTable("Category");

            //Chave primaria
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .ValueGeneratedOnAdd()//Gera um valor de identity toda vez que um id é adicionado
                    .UseIdentityColumn(); //Referencia com a coluna Identity

            builder.Property(x => x.Name)
                .IsRequired()//NOTNULL
                .HasColumnName("Name")//Nome da propiedade
                .HasColumnType("NVARCHAR")//Tipo de dados
                .HasMaxLength(80);//Maximo de caracteres

            builder.Property(x => x.Slug)
                //NOTNULL
                .HasColumnName("Slug")//Nome da propiedade
                .HasColumnType("NVARCHAR")//Tipo de dados
                .HasMaxLength(80);//Maximo de caracteres

            //Indices (usado para propiedades que são muito acessadas)
            builder.HasIndex(x => x.Slug, "IX_Category_Slug")
                .IsUnique();
            //Garante que essa propiedade é unica;
        }
    }
}