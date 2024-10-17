﻿namespace ARWNI2S.Node.Data.Mapping.Builders.Gameplay.Configuration
{
    /// <summary>
    /// Represents a setting entity builder
    /// </summary>
    public partial class GameplaySettingBuilder : ServerEntityBuilder<GameplaySetting>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
               .WithColumn(nameof(Setting.Name)).AsString(200).NotNullable()
               .WithColumn(nameof(Setting.Value)).AsString(6000).NotNullable();
        }

        #endregion
    }
}