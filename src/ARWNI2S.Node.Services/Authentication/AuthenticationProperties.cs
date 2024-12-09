namespace ARWNI2S.Node.Services.Authentication
{
    public class AuthenticationProperties
    {
        public bool IsPersistent { get; internal set; }
        public DateTime IssuedUtc { get; internal set; }
        public string Token { get; internal set; }
    }
}
