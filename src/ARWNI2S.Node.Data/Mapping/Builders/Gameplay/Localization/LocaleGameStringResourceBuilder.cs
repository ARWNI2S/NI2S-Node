namespace ARWNI2S.Node.Data.Mapping.Builders.Gameplay.Localization
{
    /// <summary>
    /// Represents a locale string resource entity builder
    /// </summary>
    public partial class LocaleGameStringResourceBuilder : ServerEntityBuilder<LocaleGameStringResource>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(LocaleGameStringResource.ResourceName)).AsString(200).NotNullable()
                .WithColumn(nameof(LocaleGameStringResource.ResourceValue)).AsString(int.MaxValue).NotNullable()
                .WithColumn(nameof(LocaleGameStringResource.LanguageId)).AsInt32().ForeignKey<GameLanguage>();
        }

        #endregion
    }
}