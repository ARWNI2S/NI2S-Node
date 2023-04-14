// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using System;

namespace NI2S.Node
{
    /// <summary>
    /// This API supports the ASP.NET Core infrastructure and is not intended to be used
    /// directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public interface IStartupConfigureContainerFilter<TContainerBuilder>
    {
        /// <summary>
        /// Extends the provided <paramref name="container"/> and returns a modified <see cref="Action"/> action of the same type.
        /// </summary>
        /// <param name="container">The ConfigureContainer method to extend.</param>
        /// <returns>A modified <see cref="Action"/>.</returns>
        Action<TContainerBuilder> ConfigureContainer(Action<TContainerBuilder> container);
    }
}