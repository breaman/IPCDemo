using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fuse.Domain.Models
{
    [Table("AspNetRoleClaims")]
    public class RoleClaim : EntityBase
    {
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
