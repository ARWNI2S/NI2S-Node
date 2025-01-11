// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using AutoMapper;

namespace ARWNI2S.Engine.Environment.Mapper
{
    /// <summary>
    /// AutoMapper configuration
    /// </summary>
    public static class AutoMapperConfiguration
    {
        /// <summary>
        /// Mapper
        /// </summary>
        public static IMapper Mapper { get; private set; }

        /// <summary>
        /// Mapper configuration
        /// </summary>
        public static MapperConfiguration MapperConfiguration { get; private set; }

        /// <summary>
        /// Initialize mapper
        /// </summary>
        /// <param name="config">Mapper configuration</param>
        public static void Init(MapperConfiguration config)
        {
            MapperConfiguration = config;
            Mapper = config.CreateMapper();
        }
    }
}