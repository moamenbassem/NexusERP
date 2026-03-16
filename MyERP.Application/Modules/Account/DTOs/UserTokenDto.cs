using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.Account.DTOs
{
    public class UserTokenDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
