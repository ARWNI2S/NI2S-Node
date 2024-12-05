using ARWNI2S.Infrastructure.Resources;

namespace ARWNI2S.Engine.Parts
{
    /// <summary>
    /// Provides a <see cref="EnginePartFactory"/> type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class ProvideEnginePartFactoryAttribute : Attribute
    {
        private readonly Type _enginePartFactoryType;
        private readonly string _enginePartFactoryTypeName;

        /// <summary>
        /// Creates a new instance of <see cref="ProvideEnginePartFactoryAttribute"/> with the specified type.
        /// </summary>
        /// <param name="factoryType">The factory type.</param>
        public ProvideEnginePartFactoryAttribute(Type factoryType)
        {
            _enginePartFactoryType = factoryType ?? throw new ArgumentNullException(nameof(factoryType));
        }

        /// <summary>
        /// Creates a new instance of <see cref="ProvideEnginePartFactoryAttribute"/> with the specified type name.
        /// </summary>
        /// <param name="factoryTypeName">The assembly qualified type name.</param>
        public ProvideEnginePartFactoryAttribute(string factoryTypeName)
        {
            if (string.IsNullOrEmpty(factoryTypeName))
            {
                throw new ArgumentException(LocalizedStrings.ArgumentCannotBeNullOrEmpty, nameof(factoryTypeName));
            }

            _enginePartFactoryTypeName = factoryTypeName;
        }

        /// <summary>
        /// Gets the factory type.
        /// </summary>
        /// <returns></returns>
        public Type GetFactoryType()
        {
            return _enginePartFactoryType ??
                Type.GetType(_enginePartFactoryTypeName!, throwOnError: true)!;
        }
    }
}
