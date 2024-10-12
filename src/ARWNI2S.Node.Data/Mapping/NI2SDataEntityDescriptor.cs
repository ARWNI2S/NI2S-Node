namespace ARWNI2S.Node.Data.Mapping
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