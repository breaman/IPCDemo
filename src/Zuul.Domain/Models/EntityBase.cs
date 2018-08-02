using System;
using System.Collections.Generic;
using System.Text;
using Zuul.Domain.Abstract;

namespace Zuul.Domain.Models
{
    public abstract class EntityBase : IEntityBase
    {
        public int Id { get; set; }
    }
}
