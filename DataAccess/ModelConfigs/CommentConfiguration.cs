using MyEShop.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace MyEShop.DataAccess.ModelConfigs
{
    public class CommentConfiguration : EntityTypeConfiguration<Comment>
    {

        public CommentConfiguration()
        {

            Property(c => c.Text).HasMaxLength(500).IsRequired();

            Property(c => c.UserId).IsRequired();

            Property(c => c.ProductId).IsRequired()
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("IX_Comment_ProductId") { IsUnique = false }));

            HasOptional(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentId)
                .WillCascadeOnDelete(false);

        }
    }
}