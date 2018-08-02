using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zuul.Web.ViewModels
{
    public class VerificationEmailViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string VerifyUrl { get; set; }
    }
}
