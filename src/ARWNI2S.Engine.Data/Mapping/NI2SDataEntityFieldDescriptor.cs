using System.Data;

namespace ARWNI2S.Data.Mapping
{
    public partial class NI2SDataEntityFieldDescriptor
    {
        public string Name { get; set; }
        public bool IsIdentity { get; set; }
        public bool? IsNullable { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsUnique { get; set; }
        public int? Precision { get; set; }
        public int? Size { get; set; }
        public DbType Type { get; set; }
    }
}