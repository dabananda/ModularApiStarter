using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ModularApiStarter.Shared.Common
{
    public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
    {
        public Guid? UserId
        {
            get
            {
                var id = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                return Guid.TryParse(id, out var guid) ? guid : null;
            }
        }
    }
}
