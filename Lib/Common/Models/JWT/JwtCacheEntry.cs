namespace TagTeamdWeb.Common.Models.Jwt
{
    public class JwtCacheEntry
    {
        public string Token { get; set; }
        public  string Username { get; set; }
        public DateTime Expiration { get; set; }
    }
}
