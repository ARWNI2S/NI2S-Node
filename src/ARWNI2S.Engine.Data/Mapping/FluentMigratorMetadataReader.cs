using ARWNI2S.Data.Mapping;
using LinqToDB;
using LinqToDB.Mapping;
using LinqToDB.Metadata;
using System.Collections.Concurrent;
using System.Data;
using System.Reflection;

namespace ARWNI2S.Engine.Data.Mapping
{
    ///// <summary>
    ///// LINQ To DB metadata reader for schema created by FluentMigrator
    ///// </summary>
    //public partial class FluentMigratorMetadataReader : IMetadataReader
    //{
    //    #region Fields

    //    private readonly IMappingEntityAccessor _mappingEntityAccessor;

    //    #endregion

    //    #region Ctor

    //    public FluentMigratorMetadataReader(IMappingEntityAccessor mappingEntityAccessor)
    //    {
    //        _mappingEntityAccessor = mappingEntityAccessor;
    //    }

    //    #endregion

    //    #region Utils

    //    protected T GetAttribute<T>(Type type, MemberInfo memberInfo) where T : Attribute
    //    {
    //        var attribute = Types.GetOrAdd((type, memberInfo), _ =>
    //        {
    //            var entityDescriptor = _mappingEntityAccessor.GetEntityDescriptor(type);

    //            if (typeof(T) == typeof(TableAttribute))
    //                return new TableAttribute(entityDescriptor.EntityName) { Schema = entityDescriptor.SchemaName };

    //            if (typeof(T) != typeof(ColumnAttribute))
    //                return null;

    //            var entityField = entityDescriptor.Fields.SingleOrDefault(cd => cd.Name.Equals(NameCompatibilityManager.GetColumnName(type, memberInfo.Name), StringComparison.OrdinalIgnoreCase));

    //            if (entityField is null)
    //                return null;

    //            var columnSystemType = (memberInfo as PropertyInfo)?.PropertyType ?? typeof(string);

    //            var mappingSchema = _mappingEntityAccessor.GetMappingSchema();

    //            return new ColumnAttribute
    //            {
    //                Name = entityField.Name,
    //                IsPrimaryKey = entityField.IsPrimaryKey,
    //                IsColumn = true,
    //                CanBeNull = entityField.IsNullable ?? false,
    //                Length = entityField.Size ?? 0,
    //                Precision = entityField.Precision ?? 0,
    //                IsIdentity = entityField.IsIdentity,
    //                DataType = mappingSchema.GetDataType(columnSystemType).Type.DataType
    //            };
    //        });

    //        return (T)attribute;
    //    }

    //    protected T[] GetAttributes<T>(Type type, Type attributeType, MemberInfo memberInfo = null)
    //        where T : Attribute
    //    {
    //        if (type.IsSubclassOf(typeof(DataEntity)) && typeof(T) == attributeType && GetAttribute<T>(type, memberInfo) is T attr)
    //        {
    //            return new[] { attr };
    //        }

    //        return Array.Empty<T>();
    //    }

    //    #endregion

    //    #region Methods

    //    /// <summary>
    //    /// Gets attributes of specified type, associated with specified type.
    //    /// </summary>
    //    /// <typeparam name="T">Attribute type.</typeparam>
    //    /// <param name="type">Attributes owner type.</param>
    //    /// <param name="inherit">If <c>true</c> - include inherited attributes.</param>
    //    /// <returns>Attributes of specified type.</returns>
    //    public virtual T[] GetAttributes<T>(Type type, bool inherit = true) where T : Attribute
    //    {
    //        return GetAttributes<T>(type, typeof(TableAttribute));
    //    }

    //    /// <summary>
    //    /// Gets attributes of specified type, associated with specified type member.
    //    /// </summary>
    //    /// <typeparam name="T">Attribute type.</typeparam>
    //    /// <param name="type">Member's owner type.</param>
    //    /// <param name="memberInfo">Attributes owner member.</param>
    //    /// <param name="inherit">If <c>true</c> - include inherited attributes.</param>
    //    /// <returns>Attributes of specified type.</returns>
    //    public virtual T[] GetAttributes<T>(Type type, MemberInfo memberInfo, bool inherit = true) where T : Attribute
    //    {
    //        return GetAttributes<T>(type, typeof(ColumnAttribute), memberInfo);
    //    }

    //    /// <summary>
    //    /// Gets the dynamic columns defined on given type
    //    /// </summary>
    //    /// <param name="type">The type</param>
    //    /// <returns>All dynamic columns defined on given type</returns>
    //    public MemberInfo[] GetDynamicColumns(Type type)
    //    {
    //        return Array.Empty<MemberInfo>();
    //    }


    //    //MappingAttribute[] IMetadataReader.GetAttributes(Type type) => GetAttributes<MappingAttribute>(type);

    //    //MappingAttribute[] IMetadataReader.GetAttributes(Type type, MemberInfo memberInfo) => GetAttributes<MappingAttribute>(type, memberInfo);

    //    //string IMetadataReader.GetObjectID() => $".{nameof(FluentMigratorMetadataReader)}";

    //    #endregion

    //    #region Properties

    //    protected static ConcurrentDictionary<(Type, MemberInfo), Attribute> Types { get; } = new ConcurrentDictionary<(Type, MemberInfo), Attribute>();
    //    protected static ConcurrentDictionary<Type, CreateTableExpression> Expressions { get; } = new ConcurrentDictionary<Type, CreateTableExpression>();

    //    #endregion
    //}

    public partial class FluentMigratorMetadataReader : IMetadataReader
    {
        #region Utilities

        private static DataType DbTypeToDataType(DbType dbType)
        {
            return dbType switch
            {
                DbType.AnsiString => DataType.VarChar,
                DbType.AnsiStringFixedLength => DataType.VarChar,
                DbType.Binary => DataType.Binary,
                DbType.Boolean => DataType.Boolean,
                DbType.Byte => DataType.Byte,
                DbType.Currency => DataType.Money,
                DbType.Date => DataType.Date,
                DbType.DateTime => DataType.DateTime,
                DbType.DateTime2 => DataType.DateTime2,
                DbType.DateTimeOffset => DataType.DateTimeOffset,
                DbType.Decimal => DataType.Decimal,
                DbType.Double => DataType.Double,
                DbType.Guid => DataType.Guid,
                DbType.Int16 => DataType.Int16,
                DbType.Int32 => DataType.Int32,
                DbType.Int64 => DataType.Int64,
                DbType.Object => DataType.Undefined,
                DbType.SByte => DataType.SByte,
                DbType.Single => DataType.Single,
                DbType.String => DataType.NVarChar,
                DbType.StringFixedLength => DataType.NVarChar,
                DbType.Time => DataType.Time,
                DbType.UInt16 => DataType.UInt16,
                DbType.UInt32 => DataType.UInt32,
                DbType.UInt64 => DataType.UInt64,
                DbType.VarNumeric => DataType.VarNumeric,
                DbType.Xml => DataType.Xml,
                _ => DataType.Undefined
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all mapping attributes on specified type.
        /// </summary>
        /// <param name="type">Attributes owner type.</param>
        /// <returns>Array of mapping attributes.</returns>
        public MappingAttribute[] GetAttributes(Type type)
        {
            if (!type.IsSubclassOf(typeof(DataEntity)))
                return [];

            var attribute = Types.GetOrAdd((type, null), _ =>
            {
                var entityDescriptor = NI2SDataMappingSchema.GetEntityDescriptor(type);

                if (entityDescriptor is null)
                    return null;

                return new TableAttribute() { Schema = entityDescriptor.SchemaName, Name = entityDescriptor.EntityName };
            });

            return [attribute];
        }

        /// <summary>
        /// Gets all mapping attributes on specified type member.
        /// </summary>
        /// <param name="type">Member type. Could be used by some metadata providers to identify actual member owner type.</param>
        /// <param name="memberInfo">Type member for which mapping attributes should be returned.</param>
        /// <returns>Array of attributes.</returns>
        public MappingAttribute[] GetAttributes(Type type, MemberInfo memberInfo)
        {
            if (!type.IsSubclassOf(typeof(DataEntity)))
                return [];

            var entityDescriptor = NI2SDataMappingSchema.GetEntityDescriptor(type);

            if (entityDescriptor is null)
                return [];

            var attribute = Types.GetOrAdd((type, memberInfo), _ =>
            {

                var entityField = entityDescriptor.Fields.SingleOrDefault(cd => cd.Name.Equals(NameCompatibilityManager.GetColumnName(type, memberInfo.Name), StringComparison.OrdinalIgnoreCase));

                if (entityField is null)
                    return null;

                if (!(memberInfo as PropertyInfo)?.CanWrite ?? false)
                    return null;

                return new ColumnAttribute
                {
                    Name = entityField.Name,
                    IsPrimaryKey = entityField.IsPrimaryKey,
                    IsColumn = true,
                    CanBeNull = entityField.IsNullable ?? false,
                    Length = entityField.Size ?? 0,
                    Precision = entityField.Precision ?? 0,
                    IsIdentity = entityField.IsIdentity,
                    DataType = DbTypeToDataType(entityField.Type)
                };
            });

            return attribute is null ? [] : [attribute];
        }

        /// <summary>
        /// Gets the dynamic columns defined on given type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>All dynamic columns defined on given type.</returns>
        public MemberInfo[] GetDynamicColumns(Type type)
        {
            return [];
        }

        /// <summary>
        /// Should return a unique ID for cache purposes. If the implemented Metadata reader returns instance-specific
        /// data you'll need to calculate a unique value based on content. Otherwise just use a static const
        /// e.g. $".{nameof(YourMetadataReader)}."
        /// </summary>
        /// <returns>The object ID as string</returns>
        public string GetObjectID()
        {
            return $".{nameof(FluentMigratorMetadataReader)}.";
        }

        #endregion

        #region Properties

        protected static ConcurrentDictionary<(Type, MemberInfo), MappingAttribute> Types { get; } = new ConcurrentDictionary<(Type, MemberInfo), MappingAttribute>();

        #endregion
    }
}
