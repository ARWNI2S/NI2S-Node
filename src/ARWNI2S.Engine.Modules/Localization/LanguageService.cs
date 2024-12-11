﻿using ARWNI2S.Caching;
using ARWNI2S.Clustering.Services;
using ARWNI2S.Core.Configuration;
using ARWNI2S.Core.Data.Localization;
using ARWNI2S.Data;
using ARWNI2S.Data.Extensions;
using System.Globalization;

namespace ARWNI2S.Core.Localization
{
    /// <summary>
    /// Language service
    /// </summary>
    public partial class LanguageService : ILanguageService
    {
        #region Fields

        private readonly IRepository<Language> _languageRepository;
        private readonly ISettingService _settingService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly INodeMappingService _nodeMappingService;
        private readonly LocalizationSettings _localizationSettings;

        #endregion

        #region Ctor

        public LanguageService(IRepository<Language> languageRepository,
            ISettingService settingService,
            IStaticCacheManager staticCacheManager,
            INodeMappingService nodeMappingService,
            LocalizationSettings localizationSettings)
        {
            _languageRepository = languageRepository;
            _settingService = settingService;
            _staticCacheManager = staticCacheManager;
            _nodeMappingService = nodeMappingService;
            _localizationSettings = localizationSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a language
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteLanguageAsync(Language language)
        {
            ArgumentNullException.ThrowIfNull(language);

            //update default admin area language (if required)
            if (_localizationSettings.DefaultAdminLanguageId == language.Id)
                foreach (var activeLanguage in await GetAllLanguagesAsync())
                {
                    if (activeLanguage.Id == language.Id)
                        continue;

                    _localizationSettings.DefaultAdminLanguageId = activeLanguage.Id;
                    await _settingService.SaveSettingAsync(_localizationSettings);
                    break;
                }

            await _languageRepository.DeleteAsync(language);
        }

        /// <summary>
        /// Gets all languages
        /// </summary>
        /// <param name="nodeId">Load records allowed only in a specified node; pass 0 to load all records</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the languages
        /// </returns>
        public virtual async Task<IList<Language>> GetAllLanguagesAsync(bool showHidden = false, int nodeId = 0)
        {
            //cacheable copy
            var key = _staticCacheManager.PrepareKeyForDefaultCache(LocalizationServicesDefaults.LanguagesAllCacheKey, nodeId, showHidden);

            var languages = await _staticCacheManager.GetAsync(key, async () =>
            {
                var allLanguages = await _languageRepository.GetAllAsync(query =>
                {
                    if (!showHidden)
                        query = query.Where(l => l.Published);
                    query = query.OrderBy(l => l.DisplayOrder).ThenBy(l => l.Id);

                    return query;
                });

                //node mapping
                if (nodeId > 0)
                    allLanguages = await allLanguages
                        .WhereAwait(async l => await _nodeMappingService.AuthorizeAsync(l, nodeId))
                        .ToListAsync();

                return allLanguages;
            });

            return languages;
        }

        /// <summary>
        /// Gets all languages
        /// </summary>
        /// <param name="nodeId">Load records allowed only in a specified node; pass 0 to load all records</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>
        /// The languages
        /// </returns>
        public virtual IList<Language> GetAllLanguages(bool showHidden = false, int nodeId = 0)
        {
            //cacheable copy
            var key = _staticCacheManager.PrepareKeyForDefaultCache(LocalizationServicesDefaults.LanguagesAllCacheKey, nodeId, showHidden);

            var languages = _staticCacheManager.Get(key, () =>
            {
                var allLanguages = _languageRepository.GetAll(query =>
                {
                    if (!showHidden)
                        query = query.Where(l => l.Published);
                    query = query.OrderBy(l => l.DisplayOrder).ThenBy(l => l.Id);

                    return query;
                });

                //node mapping
                if (nodeId > 0)
                    allLanguages = allLanguages
                        .Where(l => _nodeMappingService.Authorize(l, nodeId))
                        .ToList();

                return allLanguages;
            });

            return languages;
        }

        /// <summary>
        /// Gets a language
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the language
        /// </returns>
        public virtual async Task<Language> GetLanguageByIdAsync(int languageId)
        {
            return await _languageRepository.GetByIdAsync(languageId, cache => default);
        }

        public async Task<Language> GetLanguageByCultureAsync(CultureInfo cultureInfo)
        {
            var language = await _languageRepository.Table.FirstOrDefaultAsync(l => l.LanguageCulture == cultureInfo.TwoLetterISOLanguageName && l.Published);

            return language;
        }

        /// <summary>
        /// Inserts a language
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertLanguageAsync(Language language)
        {
            await _languageRepository.InsertAsync(language);
        }

        /// <summary>
        /// Updates a language
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateLanguageAsync(Language language)
        {
            //update language
            await _languageRepository.UpdateAsync(language);
        }

        /// <summary>
        /// Get 2 letter ISO language code
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>ISO language code</returns>
        public virtual string GetTwoLetterIsoLanguageName(Language language)
        {
            ArgumentNullException.ThrowIfNull(language);

            if (string.IsNullOrEmpty(language.LanguageCulture))
                return "en";

            var culture = new CultureInfo(language.LanguageCulture);
            var code = culture.TwoLetterISOLanguageName;

            return string.IsNullOrEmpty(code) ? "en" : code;
        }

        #endregion
    }
}