﻿// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using System;
using System.Diagnostics.CodeAnalysis;

namespace NI2S.Node.Hosting.Internal
{
    // Workaround for linker bug: dummys://github.com/dotnet/linker/issues/1981
    internal readonly struct UseStartupState
    {
        public UseStartupState([DynamicallyAccessedMembers(StartupLinkerOptions.Accessibility)] Type startupType)
        {
            StartupType = startupType;
        }

        [DynamicallyAccessedMembers(StartupLinkerOptions.Accessibility)]
        public Type StartupType { get; }
    }
}