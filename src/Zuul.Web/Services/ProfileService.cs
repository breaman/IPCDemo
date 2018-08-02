using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using System.Linq;
using System.Security.Claims;
using Zuul.Domain.Models;
using IdentityModel;

namespace Zuul.Web.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<User> ClaimsFactory;
        private readonly UserManager<User> UserManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileService{TUser}"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="claimsFactory">The claims factory.</param>
        public ProfileService(UserManager<User> userManager,
            IUserClaimsPrincipalFactory<User> claimsFactory)
        {
            UserManager = userManager;
            ClaimsFactory = claimsFactory;
        }

        /// <summary>
        /// This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo endpoint)
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await UserManager.FindByIdAsync(sub);
            var principal = await ClaimsFactory.CreateAsync(user);
            var claims = principal.Claims.ToList();

            // Modify claims that came from the system (add/override any values that were we explicitly added in the database)

            if (claims.Count(c => c.Type == JwtClaimTypes.GivenName) == 0)
            {
                var nameClaim = claims.SingleOrDefault(c => c.Type == JwtClaimTypes.GivenName);
                claims.Remove(nameClaim);
            }
            claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));

            if (claims.Count(c => c.Type == JwtClaimTypes.FamilyName) == 0)
            {
                var nameClaim = claims.SingleOrDefault(c => c.Type == JwtClaimTypes.FamilyName);
                claims.Remove(nameClaim);
            }
            claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));

            if (claims.Count(c => c.Type == JwtClaimTypes.Name) != 0)
            {
                var nameClaim = claims.SingleOrDefault(c => c.Type == JwtClaimTypes.Name);
                claims.Remove(nameClaim);
            }

            claims.Add(new Claim(JwtClaimTypes.Name, $"{user.FirstName} {user.LastName}"));

            context.AddRequestedClaims(claims);
        }

        /// <summary>
        /// This method gets called whenever identity server needs to determine if the user is valid or active (e.g. if the user's account has been deactivated since they logged in).
        /// (e.g. during token issuance or validation).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await UserManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
