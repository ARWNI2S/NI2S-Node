using System.Diagnostics;

namespace NI2S.Node.Hosting
{
    /// <summary>
    /// Default implementation for <see cref="IDummyActivityFeature"/>.
    /// </summary>
    internal sealed class ActivityFeature : IDummyActivityFeature
    {
        internal ActivityFeature(Activity activity)
        {
            Activity = activity;
        }

        /// <inheritdoc />
        public Activity Activity { get; set; }
    }

    internal interface IDummyActivityFeature
    {
        Activity Activity { get; set; }
    }
}