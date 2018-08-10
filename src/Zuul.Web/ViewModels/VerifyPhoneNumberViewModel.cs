using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zuul.Web.ViewModels
{
    public class VerifyPhoneNumberViewModel
    {
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
        public string ReturnUrl { get; set; }
    }
}
