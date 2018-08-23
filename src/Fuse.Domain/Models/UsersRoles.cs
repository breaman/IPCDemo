using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fuse.Domain.Models
{
    [Table("aspnetuserroles")]
    public class UsersRoles
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public int RoleId { get; set; }
    }
}
