using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Zuul.Domain.Abstract;

namespace Zuul.Domain.Models
{
    public class Role : IdentityRole<int>, IEntityBase
    {
    }
}
