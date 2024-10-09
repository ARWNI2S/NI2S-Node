namespace ARWNI2S.Node.Data.Mapping.Builders.Gameplay.Localization
{
    /// <summary>
    /// Represents a localized property entity builder
    /// </summary>
    public partial class LocalizedGameplayPropertyBuilder : ServerEntityBuilder<LocalizedGameplayProperty>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(LocalizedGameplayProperty.LocaleKeyGroup)).AsString(400).NotNullable()
                .WithColumn(nameof(LocalizedGameplayProperty.LocaleKey)).AsString(400).NotNullable()
                .WithColumn(nameof(LocalizedGameplayProperty.LocaleValue)).AsString(int.MaxValue).NotNullable()
                .WithColumn(nameof(LocalizedGameplayProperty.LanguageId)).AsInt32().ForeignKey<GameLanguage>();
        }

        #endregion
    }
}