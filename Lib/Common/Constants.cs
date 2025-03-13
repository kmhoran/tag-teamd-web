namespace TagTeamdWeb.Common
{
    public static class Constants
    {
        public static string JwtValidationUrl
        {
            get
            {
                string? url = Environment.GetEnvironmentVariable("JWT_VALIDATION_URL");
                return url ?? "https://localhost:3000/api/auth/validate";
            }
        }
        public static string JwtSecret
        {
            get
            {
                string? url = Environment.GetEnvironmentVariable("JWT_SECRET");
                return url ?? "supersecret";
            }
        }
        public static string Issuer
        {
            get
            {
                string? url = Environment.GetEnvironmentVariable("JWT_ISSUER");
                return url ?? "https://localhost:3000";
            }
        }
    }
}