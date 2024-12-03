namespace ARWNI2S.Data.Mapping
{
    public partial class DataEntityDescriptor
    {
        public DataEntityDescriptor()
        {
            Fields = [];
        }

        public string EntityName { get; set; }
        public string SchemaName { get; set; }
        public ICollection<DataEntityFieldDescriptor> Fields { get; set; }
    }
}