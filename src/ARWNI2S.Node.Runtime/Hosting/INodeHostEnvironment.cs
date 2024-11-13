﻿using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Node.Hosting
{
    /// <summary>
    /// Provides information about the node hosting environment an application is running in.
    /// </summary>
    public interface INodeHostEnvironment : IHostEnvironment
    {
        /// <summary>
        /// Gets or sets the absolute path to the directory that contains the node-servable application content files.
        /// This defaults to the 'wwwroot' subfolder.
        /// </summary>
        string NodeRootPath { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="IFileProvider"/> pointing at <see cref="NodeRootPath"/>.
        /// This defaults to referencing files from the 'wwwroot' subfolder.
        /// </summary>
        IFileProvider NodeRootFileProvider { get; set; }
    }
}
