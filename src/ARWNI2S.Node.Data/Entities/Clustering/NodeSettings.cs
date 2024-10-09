using ARWNI2S.Infrastructure.Configuration;

namespace ARWNI2S.Node.Data.Entities.Clustering
{
    public class NodeSettings : ISettings
    {
        public NodeSettings()
        {
            SortingEnumDisabled = [];
            SortingEnumDisplayOrder = [];
        }

        /// <summary>
        /// Gets or sets a default view mode
        /// </summary>
        public string DefaultViewMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the images can be downloaded from remote server
        /// </summary>
        public bool ExportImportAllowDownloadImages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the related entities need to be exported/imported using name
        /// </summary>
        public bool ExportImportRelatedEntitiesByName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether need create dropdown list for export
        /// </summary>
        public bool ExportImportUseDropdownlistsForAssociatedEntities { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore ACL rules (side-wide). It can significantly improve performance when enabled.
        /// </summary>
        public bool IgnoreAcl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore "limit per server" rules (side-wide). It can significantly improve performance when enabled.
        /// </summary>
        public bool IgnoreServerLimitations { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the new content feature is enabled
        /// </summary>
        public bool NewContentEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the search auto complete is enabled
        /// </summary>
        public bool SearchAutoCompleteEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the server search system is enabled
        /// </summary>
        public bool SearchEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the search term minimum length
        /// </summary>
        public int SearchTermMinimumLength { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if mush show entity images in search auto complete
        /// </summary>
        public bool ShowEntityImagesInSearchAutoComplete { get; set; }

        /// <summary>
        /// Gets or sets a list of disabled values of SortingEnum
        /// </summary>
        public List<int> SortingEnumDisabled { get; set; }

        /// <summary>
        /// Gets or sets a display order of SortingEnum values 
        /// </summary>
        public Dictionary<int, int> SortingEnumDisplayOrder { get; set; }
    }
}
