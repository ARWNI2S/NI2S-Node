﻿using ARWNI2S.Node.Core.Clustering;
using ARWNI2S.Node.Core.Framework;
using ARWNI2S.Node.Core.Localization;

namespace ARWNI2S.Node.Data.Entities.Framework
{
    /// <summary>
    /// Represents a currency
    /// </summary>
    public partial class Currency : DataEntity, ILocalizedEntity, ISupportNodeMapping
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the currency code
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the rate
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// Gets or sets the display locale
        /// </summary>
        public string DisplayLocale { get; set; }

        /// <summary>
        /// Gets or sets the custom formatting
        /// </summary>
        public string CustomFormatting { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is limited/restricted to certain nodes
        /// </summary>
        public bool LimitedToNodes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance update
        /// </summary>
        public DateTime UpdatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the rounding type identifier
        /// </summary>
        public int RoundingTypeId { get; set; }

        /// <summary>
        /// Gets or sets the rounding type
        /// </summary>
        public RoundingType RoundingType
        {
            get => (RoundingType)RoundingTypeId;
            set => RoundingTypeId = (int)value;
        }
    }
}
