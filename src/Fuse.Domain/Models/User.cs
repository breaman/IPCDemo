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
        
        [InternalUse]
        public string PasswordHash { get; set; }
        
        [NotMapped]
        public string ApprovalStatus { get; set; }
        public IList<UserClaim> Claims { get; set; }
        [NotMapped]
        public IList<Role> Roles { get; set; }
    }

    [Coalesce]
    public class FetchUsersForManagement : StandardDataSource<User, ApplicationDbContext>
    {
        public FetchUsersForManagement(CrudContext<ApplicationDbContext> context) : base(context) { }

        public override IQueryable<User> GetQuery(IDataSourceParameters parameters)
        {
            var users = Db.Users;

            return users;
        }
    }

    [Coalesce]
    public class FetchUsersForApplication : StandardDataSource<User, ApplicationDbContext>
    {
        public FetchUsersForApplication(CrudContext<ApplicationDbContext> context) : base(context) { }

        [Coalesce]
        public string ApplicationName { get; set; }

        public override IQueryable<User> GetQuery(IDataSourceParameters parameters)
        {
            var userIdsWithAppClaim = Db.UserClaims.Where(uc => uc.ClaimType == ApplicationName).Select(uc => uc.UserId).ToList();
            var users = Db.Users.Where(u => userIdsWithAppClaim.Contains(u.Id)).Include(u => u.Claims).IncludedSeparately(u => u.Roles);

            return users;
        }

        public override void TransformResults(IReadOnlyList<User> results, IDataSourceParameters parameters)
        {
            var roleIdsWithAppClaim = Db.RoleClaims.Where(rc => rc.ClaimValue == ApplicationName).Select(rc => rc.RoleId).ToList();
            var roles = Db.Roles.Where(r => roleIdsWithAppClaim.Contains(r.Id));

            var usersRoles = Db.UsersRoles.ToList();

            foreach (var user in results)
            {
                // set the approval status
                var applicationClaim = user.Claims.SingleOrDefault(uc => uc.ClaimType == ApplicationName);
                if (applicationClaim != null)
                {
                    user.ApprovalStatus = applicationClaim.ClaimValue;
                }

                // set the application roles associated with the user
                user.Roles = roles.Where(r => usersRoles.Where(ur => ur.UserId == user.Id).Select(ur => ur.RoleId).Contains(r.Id)).ToList();
            }
        }
    }
}
