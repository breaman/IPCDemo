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
    public partial class RoleClaimDtoGen : GeneratedDto<Fuse.Domain.Models.RoleClaim>
    {
        public RoleClaimDtoGen() { }

        public int? RoleId { get; set; }
        public Fuse.Models.RoleDtoGen Role { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public int? Id { get; set; }

        /// <summary>
        /// Map from the domain object to the properties of the current DTO instance.
        /// </summary>
        public override void MapFrom(Fuse.Domain.Models.RoleClaim obj, IMappingContext context, IncludeTree tree = null)
        {
            if (obj == null) return;
            var includes = context.Includes;

            // Fill the properties of the object.

            this.RoleId = obj.RoleId;
            this.ClaimType = obj.ClaimType;
            this.ClaimValue = obj.ClaimValue;
            this.Id = obj.Id;
            if (tree == null || tree[nameof(this.Role)] != null)
                this.Role = obj.Role.MapToDto<Fuse.Domain.Models.Role, RoleDtoGen>(context, tree?[nameof(this.Role)]);

        }

        /// <summary>
        /// Map from the current DTO instance to the domain object.
        /// </summary>
        public override void MapTo(Fuse.Domain.Models.RoleClaim entity, IMappingContext context)
        {
            var includes = context.Includes;

            if (OnUpdate(entity, context)) return;

            entity.RoleId = (RoleId ?? entity.RoleId);
            entity.ClaimType = ClaimType;
            entity.ClaimValue = ClaimValue;
            entity.Id = (Id ?? entity.Id);
        }
    }
}
