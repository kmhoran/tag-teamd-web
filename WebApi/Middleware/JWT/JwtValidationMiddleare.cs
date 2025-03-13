using TagTeamdWeb.WebApi.Middleware.Jwt;
using TagTeamdWeb.Common.Interfaces.Jwt;

namespace TagTeamdWeb.WebApi.Middleware.Jwt
{
    public class JwtValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IJwtValidationService _validationService;

        public JwtValidationMiddleware(RequestDelegate next, IJwtValidationService validationService)
        {
            _next = next;
            _validationService = validationService;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == "OPTIONS")
            {
                await _next(context);
                return;
            }
            var username = context.Request.Headers["X-Username"].FirstOrDefault()?.ToString();
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.ToString()?.Replace("Bearer ", "");
            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(token) || !await _validationService.ValidateTokenAsync(username, token))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }
            await _next(context);
        }
    }
}