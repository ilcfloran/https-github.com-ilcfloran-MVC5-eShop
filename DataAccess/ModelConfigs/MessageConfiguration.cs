using MyEShop.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace MyEShop.DataAccess.ModelConfigs
{
    public class MessageConfiguration : EntityTypeConfiguration<Message>
    {
        public MessageConfiguration()
        {
            Property(m => m.Title).HasMaxLength(80).IsRequired();

            Property(m => m.Title).HasMaxLength(500).IsRequired();

            HasRequired(m => m.UserRec).WithMany().HasForeignKey(m => m.UserRecId).WillCascadeOnDelete(false);

            //HasRequired(m => m.UserSend).WithMany().HasForeignKey(m => m.UserSendId).WillCascadeOnDelete(false);


        }
    }
}
