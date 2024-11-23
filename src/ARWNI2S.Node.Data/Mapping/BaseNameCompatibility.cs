namespace ARWNI2S.Node.Data.Mapping
{
    /// <summary>
    /// Base instance of backward compatibility of table naming
    /// </summary>
    public partial class BaseNameCompatibility : INameCompatibility
    {
        public Dictionary<Type, string> TableNames => [];

        public Dictionary<(Type, string), string> ColumnName => [];
    }
}