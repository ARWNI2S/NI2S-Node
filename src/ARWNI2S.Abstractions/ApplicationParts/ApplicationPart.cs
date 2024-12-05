namespace ARWNI2S.ApplicationParts
{
    /// <summary>
    /// A part of an NI2S application.
    /// </summary>
    public abstract class ApplicationPart
    {
        /// <summary>
        /// Gets the <see cref="ApplicationPart"/> name.
        /// </summary>
        public abstract string Name { get; }
    }
}