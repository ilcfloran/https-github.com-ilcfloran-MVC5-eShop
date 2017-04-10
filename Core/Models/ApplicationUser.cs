using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyEShop.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }

        public ApplicationUser()
        {
            MemberSince = DateTime.Now;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] ProfilePicture { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Mobile { get; set; }
        public string BankName { get; set; }
        public string BankAcount { get; set; }
        public DateTime MemberSince { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }


        public string AddressBlock
        {
            get
            {
                string addressBlock = string.Format("{0}<br/>{1}, {2} {3}", Address, City, State, ZipCode).Trim();
                return addressBlock == "<br/>," ? string.Empty : addressBlock;
            }
        }


        public IEnumerable<SelectListItem> RolesList { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
    }

}