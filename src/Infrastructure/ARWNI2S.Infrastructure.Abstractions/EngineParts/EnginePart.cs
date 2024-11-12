namespace ARWNI2S.Infrastructure.EngineParts
{
    /// <summary>
    /// A part of an NI2S engine.
    /// </summary>
    public abstract class EnginePart
    {
        /// <summary>
        /// Gets the <see cref="EnginePart"/> name.
        /// </summary>
        public abstract string Name { get; }
    }
}
