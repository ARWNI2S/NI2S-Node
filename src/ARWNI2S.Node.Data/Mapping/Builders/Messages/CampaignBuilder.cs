namespace ARWNI2S.Node.Data.Mapping.Builders.Messages
{
    /// <summary>
    /// Represents a campaign entity builder
    /// </summary>
    public partial class CampaignBuilder : ServerEntityBuilder<Campaign>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Campaign.Name)).AsString(int.MaxValue).NotNullable()
                .WithColumn(nameof(Campaign.Subject)).AsString(int.MaxValue).NotNullable()
                .WithColumn(nameof(Campaign.Body)).AsString(int.MaxValue).NotNullable();
        }

        #endregion
    }
}