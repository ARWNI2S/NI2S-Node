namespace ARWNI2S.Infrastructure.Network.Protocol
{
    public class ProtocolException : Exception
    {
        public ProtocolException(string message, Exception exception)
            : base(message, exception)
        {

        }

        public ProtocolException(string message)
            : base(message)
        {

        }
    }
}