﻿namespace NI2S.Node.Configuration.Options
{
    public class NI2SNodeHostOptions
    {
        /// <summary>
        /// The command line arguments.
        /// </summary>
        public string[]? Args { get; init; }

        /// <summary>
        /// The environment name.
        /// </summary>
        public string? EnvironmentName { get; init; }

        /// <summary>
        /// The node name.
        /// </summary>
        public string? NodeName { get; init; }

        /// <summary>
        /// The content root path.
        /// </summary>
        public string? ContentRootPath { get; init; }

        /// <summary>
        /// The web root path.
        /// </summary>
        public string? WebRootPath { get; init; }
    }
}
