using ARWNI2S.ComponentModel;
using ARWNI2S.Engine;
using ARWNI2S.Environment;

namespace ARWNI2S.Data.Mapping
{
    /// <summary>
    /// Helper class for maintaining backward compatibility of table naming
    /// </summary>
    public static partial class NameCompatibilityManager
    {
        #region Fields

        private static readonly Dictionary<Type, string> _tableNames = [];
        private static readonly Dictionary<(Type, string), string> _columnName = [];
        private static readonly List<Type> _loadedFor = [];
        private static bool _isInitialized;
        private static readonly ReaderWriterLockSlim _locker = new();

        #endregion

        #region Utils

        private static void Initialize()
        {
            //perform with locked access to resources
            using (new ReaderWriteLockDisposable(_locker))
            {
                if (_isInitialized)
                    return;

                var typeFinder = Singleton<ITypeFinder>.Instance;
                var compatibilities = typeFinder.FindClassesOfType<INameCompatibility>()
                    ?.Select(type => EngineContext.Current.ResolveUnregistered(type) as INameCompatibility).ToList() ?? [];

                compatibilities.AddRange(AdditionalNameCompatibilities.Select(type => EngineContext.Current.ResolveUnregistered(type) as INameCompatibility));

                foreach (var nameCompatibility in compatibilities.Distinct())
                {
                    if (_loadedFor.Contains(nameCompatibility.GetType()))
                        continue;

                    _loadedFor.Add(nameCompatibility.GetType());

                    foreach (var (key, value) in nameCompatibility.TableNames.Where(tableName =>
                        !_tableNames.ContainsKey(tableName.Key)))
                        _tableNames.Add(key, value);

                    foreach (var (key, value) in nameCompatibility.ColumnName.Where(columnName =>
                        !_columnName.ContainsKey(columnName.Key)))
                        _columnName.Add(key, value);
                }

                _isInitialized = true;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets table name for mapping with the type
        /// </summary>
        /// <param name="type">Type to get the table name</param>
        /// <returns>Table name</returns>
        public static string GetTableName(Type type)
        {
            if (!_isInitialized)
                Initialize();

            if (_tableNames.TryGetValue(type, out var tableName))
                return tableName;

            return type.Name;
        }

        /// <summary>
        /// Gets column name for mapping with the entity's property
        /// </summary>
        /// <param name="type">Type of entity</param>
        /// <param name="propertyName">Property name</param>
        /// <returns>Column name</returns>
        public static string GetColumnName(Type type, string propertyName)
        {
            if (!_isInitialized)
                Initialize();

            if (_columnName.TryGetValue((type, propertyName), out var columnName))
                return columnName;

            return propertyName;
        }

        #endregion

        /// <summary>
        /// Additional name compatibility types
        /// </summary>
        public static List<Type> AdditionalNameCompatibilities { get; } = [];
    }
}
