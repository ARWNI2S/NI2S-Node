using System.Diagnostics;

namespace NI2S.Node.Engine.Modules
{
    /// <summary>
    /// Feature to access the <see cref="Activity"/> associated with a request.
    /// </summary>
    public interface IDummyActivityFeature
    {
        /// <summary>
        /// Returns the <see cref="Activity"/> associated with the current request.
        /// </summary>
        Activity Activity { get; set; }
    }
}
