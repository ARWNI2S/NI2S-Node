using ARWNI2S.Configuration;

namespace ARWNI2S.Clustering.Configuration
{
    public class ClusteringSettings : ISettings
    {
        //public ClusteringSettings()
        //{
        //    SortingEnumDisabled = [];
        //    SortingEnumDisplayOrder = [];
        //}

        ///// <summary>
        ///// Gets or sets a default view mode
        ///// </summary>
        //public string DefaultViewMode { get; set; }

        ///// <summary>
        ///// Gets or sets a value indicating whether the images can be downloaded from remote node
        ///// </summary>
        //public bool ExportImportAllowDownloadImages { get; set; }

        ///// <summary>
        ///// Gets or sets a value indicating whether the related entities need to be exported/imported using name
        ///// </summary>
        //public bool ExportImportRelatedEntitiesByName { get; set; }

        ///// <summary>
        ///// Gets or sets a value indicating whether need create dropdown list for export
        ///// </summary>
        //public bool ExportImportUseDropdownlistsForAssociatedEntities { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore ACL rules (side-wide). It can significantly improve performance when enabled.
        /// </summary>
        public bool IgnoreAcl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore "limit per node" rules (side-wide). It can significantly improve performance when enabled.
        /// </summary>
        public bool IgnoreNodeLimitations { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the health monitor frequency in seconds
        /// </summary>
        public int HealthMonitorFrequencySeconds { get; set; } = 60;

        /// <summary>
        /// Gets or sets a value indicating the health monitor ping timeout
        /// </summary>
        public int HealthMonitorTimeoutMs { get; set; } = 3000;

        ///// <summary>
        ///// Gets or sets a value indicating whether the new content feature is enabled
        ///// </summary>
        //public bool NewContentEnabled { get; set; }

        ///// <summary>
        ///// Gets or sets a value indicating whether the search auto complete is enabled
        ///// </summary>
        //public bool SearchAutoCompleteEnabled { get; set; }

        ///// <summary>
        ///// Gets or sets a value indicating whether the node search system is enabled
        ///// </summary>
        //public bool SearchEnabled { get; set; }

        ///// <summary>
        ///// Gets or sets a value indicating the search term minimum length
        ///// </summary>
        //public int SearchTermMinimumLength { get; set; }

        ///// <summary>
        ///// Gets or sets a value indicating if mush show entity images in search auto complete
        ///// </summary>
        //public bool ShowEntityImagesInSearchAutoComplete { get; set; }

        ///// <summary>
        ///// Gets or sets a list of disabled values of SortingEnum
        ///// </summary>
        //public List<int> SortingEnumDisabled { get; set; }

        ///// <summary>
        ///// Gets or sets a display order of SortingEnum values 
        ///// </summary>
        //public Dictionary<int, int> SortingEnumDisplayOrder { get; set; }
    }
}
