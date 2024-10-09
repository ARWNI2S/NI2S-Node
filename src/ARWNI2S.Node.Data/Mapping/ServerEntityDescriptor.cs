namespace ARWNI2S.Node.Data.Mapping
{
    public partial class ServerEntityDescriptor
    {
        public ServerEntityDescriptor()
        {
            Fields = [];
        }

        public string EntityName { get; set; }
        public string SchemaName { get; set; }
        public ICollection<ServerEntityFieldDescriptor> Fields { get; set; }
    }
}