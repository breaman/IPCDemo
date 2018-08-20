using Fuse.Domain.Abstract;
using IntelliTect.Coalesce;
using IntelliTect.Coalesce.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Fuse.Domain.Models
{
    [Table("AspNetUsers")]
    public class User : EntityBase
    {
        [Search]
        public string FirstName { get; set; }
        [Search]
        public string LastName { get; set; }
        [Search]
        [Column("Email")]
        public string EmailAddress { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool AccountLocked => LockoutEnd.HasValue;
        public string PasswordHash { get; set; }
        public IList<UserClaim> Claims { get; set; }
    }

    [Coalesce]
    public class FetchUsersForApplication : StandardDataSource<User, ApplicationDbContext>
    {
        public FetchUsersForApplication(CrudContext<ApplicationDbContext> context) : base(context) { }

        [Coalesce]
        public string ApplicationName { get; set; }

        public override IQueryable<User> GetQuery(IDataSourceParameters parameters)
        {
            var userIdsWithGisClaim = Db.UserClaims.Where(uc => uc.ClaimType == ApplicationName).Select(uc => uc.UserId).ToList();
            return Db.Users.Where(u => userIdsWithGisClaim.Contains(u.Id)).Include(u => u.Claims);
        }
    }
}
