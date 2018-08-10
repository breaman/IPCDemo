using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zuul.Web.ViewModels
{
    public class UserAdminViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
    }
}
