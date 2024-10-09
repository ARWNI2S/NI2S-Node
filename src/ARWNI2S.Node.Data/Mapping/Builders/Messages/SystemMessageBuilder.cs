using System.Data;

namespace ARWNI2S.Node.Data.Mapping.Builders.Messages
{
    /// <summary>
    /// Represents a system message entity builder
    /// </summary>
    public partial class SystemMessageBuilder : ServerEntityBuilder<SystemMessage>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(SystemMessage.Subject)).AsString(450).NotNullable()
                .WithColumn(nameof(SystemMessage.Text)).AsString(int.MaxValue).NotNullable()
                .WithColumn(nameof(SystemMessage.ToUserId)).AsInt32().ForeignKey<User>().OnDelete(Rule.None);
        }

        #endregion
    }
}
