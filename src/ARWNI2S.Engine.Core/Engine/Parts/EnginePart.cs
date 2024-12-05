namespace ARWNI2S.Engine.Parts
{
    /// <summary>
    /// A part of an NI2S engine.
    /// </summary>
    public abstract class EnginePart : IEnginePart
    {
        /// <summary>
        /// Gets the <see cref="EnginePart"/> name.
        /// </summary>
        public abstract string Name { get; }
    }
}
