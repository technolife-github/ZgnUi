namespace ZgnWebApi.Core.Utilities.Middlewares
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException(string message) : base(message) { }
    }

}
