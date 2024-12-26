﻿using ARWNI2S.Extensibility;

namespace ARWNI2S.Engine.Parts
{
    public interface IEnginePartManager
    {
        IList<IEngineModuleProvider> ModuleProviders { get; }

        IList<EnginePart> EngineParts { get; }

        void PopulateModule<TModule>(TModule module);

        void PopulateDefaultParts(string entryAssemblyName);
    }
}