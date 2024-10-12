namespace ARWNI2S.Node.Core
{
    public interface INodeHelper
    {
        /// <summary>
        /// Get IP address from the NI2S context
        /// </summary>
        /// <returns>String of IP address</returns>
        string GetCurrentIpAddress();
    }
}
