using System.Diagnostics;

namespace ARWNI2S.Infrastructure.Engine.Features
{
    /// <summary>
    /// Feature to access the <see cref="Activity"/> associated with a request.
    /// </summary>
    public interface IActivityFeature
    {
        /// <summary>
        /// Returns the <see cref="Activity"/> associated with the current request.
        /// </summary>
        Activity Activity { get; set; }
    }
}
