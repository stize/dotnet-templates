using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Stize.ApiTemplate.Api.TestHost
{
    public class Identities
    {
        public static IEnumerable<Claim> Basic = new Claim[]
        {
            new Claim(ClaimTypes.Name, "Stize"), 
        };
    }
}
