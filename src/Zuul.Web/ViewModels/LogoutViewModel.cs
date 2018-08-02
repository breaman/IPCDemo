using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zuul.Web.Models;

namespace Zuul.Web.ViewModels
{
    public class LogoutViewModel : LogoutInputModel
    {
        public bool ShowLogoutPrompt { get; set; }
    }
}
