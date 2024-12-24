namespace ARWNI2S.Engine.Parts
{
    /// <summary>
    /// A part of an MVC application.
    /// </summary>
    public abstract class EnginePart
    {
        /// <summary>
        /// Gets the <see cref="EnginePart"/> name.
        /// </summary>
        public abstract string Name { get; }
    }
}