using System.Security.Claims;
using InternshipLogbook.API.Models;

namespace InternshipLogbook.API.Services
{
    public class JwtChecker
    {
        private readonly RequestDelegate _next;

        public JwtChecker(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, InternshipLogbookDbContext dbContext)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var tokenStamp = context.User.FindFirst("SecurityStamp")?.Value;

                if (!string.IsNullOrEmpty(userIdClaim) && !string.IsNullOrEmpty(tokenStamp))
                {

                    var user = await dbContext.Users.FindAsync(int.Parse(userIdClaim));

                    if (user == null || user.SecurityStamp != tokenStamp)
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Token has been revoked (changed/expired).");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}