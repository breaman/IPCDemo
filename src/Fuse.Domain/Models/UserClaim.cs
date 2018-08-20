using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fuse.Domain.Models
{
    [Table("AspNetUserClaims")]
    public class UserClaim : EntityBase
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
