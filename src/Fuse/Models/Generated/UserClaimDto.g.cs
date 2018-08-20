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
    public partial class UserClaimDtoGen : GeneratedDto<Fuse.Domain.Models.UserClaim>
    {
        public UserClaimDtoGen() { }

        public int? UserId { get; set; }
        public Fuse.Models.UserDtoGen User { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public int? Id { get; set; }

        /// <summary>
        /// Map from the domain object to the properties of the current DTO instance.
        /// </summary>
        public override void MapFrom(Fuse.Domain.Models.UserClaim obj, IMappingContext context, IncludeTree tree = null)
        {
            if (obj == null) return;
            var includes = context.Includes;

            // Fill the properties of the object.

            this.UserId = obj.UserId;
            this.ClaimType = obj.ClaimType;
            this.ClaimValue = obj.ClaimValue;
            this.Id = obj.Id;
            if (tree == null || tree[nameof(this.User)] != null)
                this.User = obj.User.MapToDto<Fuse.Domain.Models.User, UserDtoGen>(context, tree?[nameof(this.User)]);

        }

        /// <summary>
        /// Map from the current DTO instance to the domain object.
        /// </summary>
        public override void MapTo(Fuse.Domain.Models.UserClaim entity, IMappingContext context)
        {
            var includes = context.Includes;

            if (OnUpdate(entity, context)) return;

            entity.UserId = (UserId ?? entity.UserId);
            entity.ClaimType = ClaimType;
            entity.ClaimValue = ClaimValue;
            entity.Id = (Id ?? entity.Id);
        }
    }
}
