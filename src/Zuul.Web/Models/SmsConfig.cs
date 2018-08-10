using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zuul.Web.Models
{
    public class SmsConfig
    {
        public string ServiceName { get; set; }
        public string AccountIdentification { get; set; }
        public string AccountPassword { get; set; }
        public string AccountFrom { get; set; }
    }
}
