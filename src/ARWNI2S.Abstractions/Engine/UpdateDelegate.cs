using ARWNI2S.Infrastructure;

namespace ARWNI2S.Engine
{
    /// <summary>
    /// A function that can process an NI2S update.
    /// </summary>
    /// <param name="context">The <see cref="NiisContext"/> for the update.</param>
    /// <returns>A task that represents the completion of update processing.</returns>
    public delegate Task UpdateDelegate(NiisContext context);
}
