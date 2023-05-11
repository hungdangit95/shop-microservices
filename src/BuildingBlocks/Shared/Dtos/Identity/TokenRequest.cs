using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Identity
{
    public class TokenRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
