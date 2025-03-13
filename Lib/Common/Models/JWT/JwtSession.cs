namespace TagTeamdWeb.Common.Models.Jwt
{
    public class JwtSession
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Username { get; set; }
        public DateTime Expiration { get; set; }
    }
}