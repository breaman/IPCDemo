using Fuse.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fuse.Domain.Models
{
    public abstract class EntityBase : IEntityBase
    {
        public int Id { get; set; }
    }
}
