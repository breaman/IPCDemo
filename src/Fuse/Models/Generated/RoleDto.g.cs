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
    public partial class RoleDtoGen : GeneratedDto<Fuse.Domain.Models.Role>
    {
        public RoleDtoGen() { }

        public string Name { get; set; }
        public System.Collections.Generic.IList<Fuse.Models.RoleClaimDtoGen> Claims { get; set; }
        public System.Collections.Generic.IList<Fuse.Models.UserDtoGen> Users { get; set; }
        public int? Id { get; set; }

        /// <summary>
        /// Map from the domain object to the properties of the current DTO instance.
        /// </summary>
        public override void MapFrom(Fuse.Domain.Models.Role obj, IMappingContext context, IncludeTree tree = null)
        {
            if (obj == null) return;
            var includes = context.Includes;

            // Fill the properties of the object.

            this.Name = obj.Name;
            this.Id = obj.Id;
            var propValClaims = obj.Claims;
            if (propValClaims != null && (tree == null || tree[nameof(this.Claims)] != null))
            {
                this.Claims = propValClaims
                    .AsQueryable().OrderBy("Id ASC").AsEnumerable<Fuse.Domain.Models.RoleClaim>()
                    .Select(f => f.MapToDto<Fuse.Domain.Models.RoleClaim, RoleClaimDtoGen>(context, tree?[nameof(this.Claims)])).ToList();
            }
            else if (propValClaims == null && tree?[nameof(this.Claims)] != null)
            {
                this.Claims = new RoleClaimDtoGen[0];
            }

            var propValUsers = obj.Users;
            if (propValUsers != null && (tree == null || tree[nameof(this.Users)] != null))
            {
                this.Users = propValUsers
                    .AsQueryable().OrderBy("Id ASC").AsEnumerable<Fuse.Domain.Models.User>()
                    .Select(f => f.MapToDto<Fuse.Domain.Models.User, UserDtoGen>(context, tree?[nameof(this.Users)])).ToList();
            }
            else if (propValUsers == null && tree?[nameof(this.Users)] != null)
            {
                this.Users = new UserDtoGen[0];
            }

        }

        /// <summary>
        /// Map from the current DTO instance to the domain object.
        /// </summary>
        public override void MapTo(Fuse.Domain.Models.Role entity, IMappingContext context)
        {
            var includes = context.Includes;

            if (OnUpdate(entity, context)) return;

            entity.Name = Name;
            entity.Id = (Id ?? entity.Id);
        }
    }
}
