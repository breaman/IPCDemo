using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zuul.Web.ViewModels
{
    public class EnableTwoFactorAuthViewModel
    {
        public string ReturnUrl { get; set; }
        public IList<SelectListItem> Providers { get; set; }
        public string SelectedProvider { get; set; }
        public string PhoneNumber { get; set; }
        public string AuthenticatorCode { get; set; }
        public string AuthenticatorUri { get; set; }
        public string SharedKey { get; set; }
    }
}
