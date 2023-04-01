using System.Diagnostics;

namespace NI2S.Node.Hosting
{
    /// <summary>
    /// Default implementation for <see cref="IHttpActivityFeature"/>.
    /// </summary>
    internal sealed class ActivityFeature : IHttpActivityFeature
    {
        internal ActivityFeature(Activity activity)
        {
            Activity = activity;
        }

        /// <inheritdoc />
        public Activity Activity { get; set; }
    }

    internal interface IHttpActivityFeature
    {
        Activity Activity { get; set; }
    }
}