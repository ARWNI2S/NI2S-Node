﻿using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Engine.Hosting
{
    /// <summary>
    /// Provides information about the NI2S hosting environment an engine is running in.
    /// </summary>
    public interface INiisHostEnvironment : IHostEnvironment
    {
        /// <summary>
        /// Gets or sets the absolute path to the directory that contains the engine content files.
        /// This defaults to the 'root' subfolder.
        /// </summary>
        string NI2SRootPath { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="IFileProvider"/> pointing at <see cref="NI2SRootPath"/>.
        /// This defaults to referencing files from the 'root' subfolder.
        /// </summary>
        IFileProvider NI2SRootFileProvider { get; set; }
    }
}
