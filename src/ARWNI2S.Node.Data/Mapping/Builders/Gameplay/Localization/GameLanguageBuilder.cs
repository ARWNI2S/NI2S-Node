namespace ARWNI2S.Node.Data.Mapping.Builders.Gameplay.Localization
{
    /// <summary>
    /// Represents a language entity builder
    /// </summary>
    public partial class GameLanguageBuilder : ServerEntityBuilder<GameLanguage>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(GameLanguage.Name)).AsString(100).NotNullable()
                .WithColumn(nameof(GameLanguage.Code)).AsString(2).NotNullable()
                .WithColumn(nameof(GameLanguage.ExCode)).AsString(4).NotNullable()
                .WithColumn(nameof(GameLanguage.FlagImageFileName)).AsString(50).Nullable();
        }

        #endregion
    }
}