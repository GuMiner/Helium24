using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace H24
{
    public static class HttpContextExtensions
    {
        public static string GetId(this HttpContext context)
        {
            string id = context.User.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;
            return id;
        }
    }
}
