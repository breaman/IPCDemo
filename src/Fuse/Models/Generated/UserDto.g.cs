using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Mapping;
using IntelliTect.Coalesce.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;

namespace Fuse.Models
{
    public partial class UserDtoGen : GeneratedDto<Fuse.Domain.Models.User>
    {
        public UserDtoGen() { }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool? PhoneNumberConfirmed { get; set; }
        public bool? TwoFactorEnabled { get; set; }
        public int? AccessFailedCount { get; set; }
        public System.DateTimeOffset? LockoutEnd { get; set; }
        public bool? AccountLocked { get; set; }
        public string PasswordHash { get; set; }
        public string ApprovalStatus { get; set; }
        public System.Collections.Generic.IList<Fuse.Models.UserClaimDtoGen> Claims { get; set; }
        public System.Collections.Generic.IList<Fuse.Models.RoleDtoGen> Roles { get; set; }
        public int? Id { get; set; }

        /// <summary>
        /// Map from the domain object to the properties of the current DTO instance.
        /// </summary>
        public override void MapFrom(Fuse.Domain.Models.User obj, IMappingContext context, IncludeTree tree = null)
        {
            if (obj == null) return;
            var includes = context.Includes;

            // Fill the properties of the object.

            this.FirstName = obj.FirstName;
            this.LastName = obj.LastName;
            this.EmailAddress = obj.EmailAddress;
            this.EmailConfirmed = obj.EmailConfirmed;
            this.PhoneNumber = obj.PhoneNumber;
            this.PhoneNumberConfirmed = obj.PhoneNumberConfirmed;
            this.TwoFactorEnabled = obj.TwoFactorEnabled;
            this.AccessFailedCount = obj.AccessFailedCount;
            this.LockoutEnd = obj.LockoutEnd;
            this.AccountLocked = obj.AccountLocked;
            this.PasswordHash = obj.PasswordHash;
            this.ApprovalStatus = obj.ApprovalStatus;
            this.Id = obj.Id;
            var propValClaims = obj.Claims;
            if (propValClaims != null && (tree == null || tree[nameof(this.Claims)] != null))
            {
                this.Claims = propValClaims
                    .AsQueryable().OrderBy("Id ASC").AsEnumerable<Fuse.Domain.Models.UserClaim>()
                    .Select(f => f.MapToDto<Fuse.Domain.Models.UserClaim, UserClaimDtoGen>(context, tree?[nameof(this.Claims)])).ToList();
            }
            else if (propValClaims == null && tree?[nameof(this.Claims)] != null)
            {
                this.Claims = new UserClaimDtoGen[0];
            }

            var propValRoles = obj.Roles;
            if (propValRoles != null && (tree == null || tree[nameof(this.Roles)] != null))
            {
                this.Roles = propValRoles
                    .AsQueryable().OrderBy("Name ASC").AsEnumerable<Fuse.Domain.Models.Role>()
                    .Select(f => f.MapToDto<Fuse.Domain.Models.Role, RoleDtoGen>(context, tree?[nameof(this.Roles)])).ToList();
            }
            else if (propValRoles == null && tree?[nameof(this.Roles)] != null)
            {
                this.Roles = new RoleDtoGen[0];
            }

        }

        /// <summary>
        /// Map from the current DTO instance to the domain object.
        /// </summary>
        public override void MapTo(Fuse.Domain.Models.User entity, IMappingContext context)
        {
            var includes = context.Includes;

            if (OnUpdate(entity, context)) return;

            entity.FirstName = FirstName;
            entity.LastName = LastName;
            entity.EmailAddress = EmailAddress;
            entity.EmailConfirmed = (EmailConfirmed ?? entity.EmailConfirmed);
            entity.PhoneNumber = PhoneNumber;
            entity.PhoneNumberConfirmed = (PhoneNumberConfirmed ?? entity.PhoneNumberConfirmed);
            entity.TwoFactorEnabled = (TwoFactorEnabled ?? entity.TwoFactorEnabled);
            entity.AccessFailedCount = (AccessFailedCount ?? entity.AccessFailedCount);
            entity.LockoutEnd = LockoutEnd;
            entity.PasswordHash = PasswordHash;
            entity.ApprovalStatus = ApprovalStatus;
            entity.Id = (Id ?? entity.Id);
        }
    }
}
