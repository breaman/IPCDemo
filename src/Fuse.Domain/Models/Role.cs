using Fuse.Domain.Abstract;
using IntelliTect.Coalesce;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Fuse.Domain.Models
{
    [Table("AspNetRoles")]
    public class Role : EntityBase
    {
        public string Name { get; set; }
        public IList<RoleClaim> Claims { get; set; }
        [NotMapped]
        public IList<User> Users { get; set; }
    }

    [Coalesce]
    public class FetchRolesForApplication : StandardDataSource<Role, ApplicationDbContext>
    {
        public FetchRolesForApplication(CrudContext<ApplicationDbContext> context) : base(context) { }

        [Coalesce]
        public string ApplicationName { get; set; }

        public override IQueryable<Role> GetQuery(IDataSourceParameters parameters)
        {
            var roleIdsWithApplicationClaim = Db.RoleClaims.Where(rc => rc.ClaimType == ApplicationName).Select(rc => rc.RoleId).ToList();
            return Db.Roles.Where(r => roleIdsWithApplicationClaim.Contains(r.Id)).Include(r => r.Claims);
        }
    }
}
