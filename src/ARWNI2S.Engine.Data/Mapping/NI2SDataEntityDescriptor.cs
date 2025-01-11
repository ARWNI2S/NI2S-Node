// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Engine.Data.Mapping
{
    public partial class NI2SDataEntityDescriptor
    {
        public NI2SDataEntityDescriptor()
        {
            Fields = [];
        }

        public string EntityName { get; set; }
        public string SchemaName { get; set; }
        public ICollection<NI2SDataEntityFieldDescriptor> Fields { get; set; }
    }
}