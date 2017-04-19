using MyEShop.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace MyEShop.DataAccess.ModelConfigs
{
    public class ApplicationUserConfiguration : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfiguration()
        {
            //ToTable("AspNetUsers");
            Property(au => au.FirstName).HasMaxLength(50).IsRequired();
            Property(au => au.LastName).HasMaxLength(50).IsRequired();
            Property(au => au.ProfilePicture).IsOptional();
            Property(au => au.Address).HasMaxLength(150).IsOptional();
            Property(au => au.City).HasMaxLength(50).IsRequired();
            Property(au => au.State).HasMaxLength(80).IsRequired();
            Property(au => au.ZipCode).HasMaxLength(20).IsOptional();
            Property(au => au.Mobile).HasMaxLength(15).IsOptional();
            Property(au => au.BankName).HasMaxLength(100).IsOptional();
            Property(au => au.BankAcount).HasMaxLength(20).IsOptional();
            Ignore(au => au.RolesList);
        }
    }
}