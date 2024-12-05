namespace ARWNI2S.Engine.Parts
{
    /// <summary>
    /// A part of an NI2S engine.
    /// </summary>
    public interface IEnginePart
    {
        /// <summary>
        /// Gets the <see cref="IEnginePart"/> name.
        /// </summary>
        string Name { get; }
    }
}
