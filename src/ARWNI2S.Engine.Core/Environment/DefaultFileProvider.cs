namespace ARWNI2S.Engine.Environment
{
    public sealed class FileProvider
    {
        #region Properties

        /// <summary>
        /// Gets or sets the default file provider
        /// </summary>
        public static INiisFileProvider Default
        {
            get
            {
                if (_defaultFileProvider == null)
                    throw new InvalidOperationException("Default file provider not set.");
                return _defaultFileProvider;
            }
            set { _defaultFileProvider = value; }
        }

        private static INiisFileProvider _defaultFileProvider = null;
        #endregion

        private FileProvider() { } //Not creatable
    }
}
