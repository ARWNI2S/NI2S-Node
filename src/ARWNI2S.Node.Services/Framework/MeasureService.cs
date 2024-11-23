using ARWNI2S.Node.Core;
using ARWNI2S.Node.Core.Framework;
using ARWNI2S.Node.Data;
using ARWNI2S.Node.Data.Entities.Framework;

namespace ARWNI2S.Node.Services.Framework
{
    /// <summary>
    /// Measure dimension service
    /// </summary>
    public partial class MeasureService : IMeasureService
    {
        #region Fields

        private readonly IRepository<MeasureTemperature> _measureTemperatureRepository;
        private readonly IRepository<MeasureDimension> _measureDimensionRepository;
        private readonly IRepository<MeasureWeight> _measureWeightRepository;
        private readonly FrameworkSettings _frameworkSettings;

        #endregion

        #region Ctor

        public MeasureService(IRepository<MeasureTemperature> measureTemperatureRepository,
            IRepository<MeasureDimension> measureDimensionRepository,
            IRepository<MeasureWeight> measureWeightRepository,
            FrameworkSettings frameworkSettings)
        {
            _measureTemperatureRepository = measureTemperatureRepository;
            _measureDimensionRepository = measureDimensionRepository;
            _measureWeightRepository = measureWeightRepository;
            _frameworkSettings = frameworkSettings;
        }

        #endregion

        #region Methods

        #region Temperatures

        /// <summary>
        /// Deletes measure temperature
        /// </summary>
        /// <param name="measureTemperature">Measure temperature</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteMeasureTemperatureAsync(MeasureTemperature measureTemperature)
        {
            await _measureTemperatureRepository.DeleteAsync(measureTemperature);
        }

        /// <summary>
        /// Gets a measure temperature by identifier
        /// </summary>
        /// <param name="measureTemperatureId">Measure temperature identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the measure temperature
        /// </returns>
        public virtual async Task<MeasureTemperature> GetMeasureTemperatureByIdAsync(int measureTemperatureId)
        {
            return await _measureTemperatureRepository.GetByIdAsync(measureTemperatureId, cache => default);
        }

        /// <summary>
        /// Gets a measure temperature by system keyword
        /// </summary>
        /// <param name="systemKeyword">The system keyword</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the measure temperature
        /// </returns>
        public virtual async Task<MeasureTemperature> GetMeasureTemperatureBySystemKeywordAsync(string systemKeyword)
        {
            if (string.IsNullOrEmpty(systemKeyword))
                return null;

            var measureTemperatures = await GetAllMeasureTemperaturesAsync();
            foreach (var measureTemperature in measureTemperatures)
                if (measureTemperature.SystemKeyword.Equals(systemKeyword, StringComparison.InvariantCultureIgnoreCase))
                    return measureTemperature;
            return null;
        }

        /// <summary>
        /// Gets all measure temperatures
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the measure temperatures
        /// </returns>
        public virtual async Task<IList<MeasureTemperature>> GetAllMeasureTemperaturesAsync()
        {
            var measureTemperatures = await _measureTemperatureRepository.GetAllAsync(query =>
            {
                return from mw in query
                       orderby mw.DisplayOrder, mw.Id
                       select mw;
            }, cache => default);

            return measureTemperatures;
        }

        /// <summary>
        /// Inserts a measure temperature
        /// </summary>
        /// <param name="measure">Measure temperature</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertMeasureTemperatureAsync(MeasureTemperature measure)
        {
            await _measureTemperatureRepository.InsertAsync(measure);
        }

        /// <summary>
        /// Updates the measure temperature
        /// </summary>
        /// <param name="measure">Measure temperature</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateMeasureTemperatureAsync(MeasureTemperature measure)
        {
            await _measureTemperatureRepository.UpdateAsync(measure);
        }

        /// <summary>
        /// Converts temperature
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="sourceMeasureTemperature">Source temperature</param>
        /// <param name="targetMeasureTemperature">Target temperature</param>
        /// <param name="round">A value indicating whether a result should be rounded</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the converted value
        /// </returns>
        public virtual async Task<decimal> ConvertTemperatureAsync(decimal value,
            MeasureTemperature sourceMeasureTemperature, MeasureTemperature targetMeasureTemperature, bool round = true)
        {
            ArgumentNullException.ThrowIfNull(sourceMeasureTemperature);

            ArgumentNullException.ThrowIfNull(targetMeasureTemperature);

            var result = value;
            if (result != decimal.Zero && sourceMeasureTemperature.Id != targetMeasureTemperature.Id)
            {
                result = await ConvertToPrimaryMeasureTemperatureAsync(result, sourceMeasureTemperature);
                result = await ConvertFromPrimaryMeasureTemperatureAsync(result, targetMeasureTemperature);
            }

            if (round)
                result = Math.Round(result, 2);

            return result;
        }

        /// <summary>
        /// Converts from primary temperature
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="targetMeasureTemperature">Target temperature</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the converted value
        /// </returns>
        public virtual async Task<decimal> ConvertFromPrimaryMeasureTemperatureAsync(decimal value,
            MeasureTemperature targetMeasureTemperature)
        {
            ArgumentNullException.ThrowIfNull(targetMeasureTemperature);

            var result = value;
            var baseTemperatureIn = await GetMeasureTemperatureByIdAsync(_frameworkSettings.BaseTemperatureId);
            if (result == decimal.Zero || targetMeasureTemperature.Id == baseTemperatureIn.Id)
                return result;

            var exchangeRatio = targetMeasureTemperature.Ratio;
            if (exchangeRatio == decimal.Zero)
                throw new NiisException($"Exchange ratio not set for temperature [{targetMeasureTemperature.Name}]");
            result *= exchangeRatio;

            return result;
        }

        /// <summary>
        /// Converts to primary measure temperature
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="sourceMeasureTemperature">Source temperature</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the converted value
        /// </returns>
        public virtual async Task<decimal> ConvertToPrimaryMeasureTemperatureAsync(decimal value, MeasureTemperature sourceMeasureTemperature)
        {
            ArgumentNullException.ThrowIfNull(sourceMeasureTemperature);

            var result = value;
            var baseTemperatureIn = await GetMeasureTemperatureByIdAsync(_frameworkSettings.BaseTemperatureId);
            if (result == decimal.Zero || sourceMeasureTemperature.Id == baseTemperatureIn.Id)
                return result;

            var exchangeRatio = sourceMeasureTemperature.Ratio;
            if (exchangeRatio == decimal.Zero)
                throw new NiisException($"Exchange ratio not set for temperature [{sourceMeasureTemperature.Name}]");
            result /= exchangeRatio;

            return result;
        }

        #endregion

        #region Dimensions

        /// <summary>
        /// Deletes measure dimension
        /// </summary>
        /// <param name="measureDimension">Measure dimension</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteMeasureDimensionAsync(MeasureDimension measureDimension)
        {
            await _measureDimensionRepository.DeleteAsync(measureDimension);
        }

        /// <summary>
        /// Gets a measure dimension by identifier
        /// </summary>
        /// <param name="measureDimensionId">Measure dimension identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the measure dimension
        /// </returns>
        public virtual async Task<MeasureDimension> GetMeasureDimensionByIdAsync(int measureDimensionId)
        {
            return await _measureDimensionRepository.GetByIdAsync(measureDimensionId, cache => default);
        }

        /// <summary>
        /// Gets a measure dimension by system keyword
        /// </summary>
        /// <param name="systemKeyword">The system keyword</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the measure dimension
        /// </returns>
        public virtual async Task<MeasureDimension> GetMeasureDimensionBySystemKeywordAsync(string systemKeyword)
        {
            if (string.IsNullOrEmpty(systemKeyword))
                return null;

            var measureDimensions = await GetAllMeasureDimensionsAsync();
            foreach (var measureDimension in measureDimensions)
                if (measureDimension.SystemKeyword.Equals(systemKeyword, StringComparison.InvariantCultureIgnoreCase))
                    return measureDimension;
            return null;
        }

        /// <summary>
        /// Gets all measure dimensions
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the measure dimensions
        /// </returns>
        public virtual async Task<IList<MeasureDimension>> GetAllMeasureDimensionsAsync()
        {
            var measureDimensions = await _measureDimensionRepository.GetAllAsync(query =>
            {
                return from md in query
                       orderby md.DisplayOrder, md.Id
                       select md;
            }, cache => default);

            return measureDimensions;
        }

        /// <summary>
        /// Inserts a measure dimension
        /// </summary>
        /// <param name="measure">Measure dimension</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertMeasureDimensionAsync(MeasureDimension measure)
        {
            await _measureDimensionRepository.InsertAsync(measure);
        }

        /// <summary>
        /// Updates the measure dimension
        /// </summary>
        /// <param name="measure">Measure dimension</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateMeasureDimensionAsync(MeasureDimension measure)
        {
            await _measureDimensionRepository.UpdateAsync(measure);
        }

        /// <summary>
        /// Converts dimension
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="sourceMeasureDimension">Source dimension</param>
        /// <param name="targetMeasureDimension">Target dimension</param>
        /// <param name="round">A value indicating whether a result should be rounded</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the converted value
        /// </returns>
        public virtual async Task<decimal> ConvertDimensionAsync(decimal value,
            MeasureDimension sourceMeasureDimension, MeasureDimension targetMeasureDimension, bool round = true)
        {
            ArgumentNullException.ThrowIfNull(sourceMeasureDimension);

            ArgumentNullException.ThrowIfNull(targetMeasureDimension);

            var result = value;
            if (result != decimal.Zero && sourceMeasureDimension.Id != targetMeasureDimension.Id)
            {
                result = await ConvertToPrimaryMeasureDimensionAsync(result, sourceMeasureDimension);
                result = await ConvertFromPrimaryMeasureDimensionAsync(result, targetMeasureDimension);
            }

            if (round)
                result = Math.Round(result, 2);

            return result;
        }

        /// <summary>
        /// Converts from primary dimension
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="targetMeasureDimension">Target dimension</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the converted value
        /// </returns>
        public virtual async Task<decimal> ConvertFromPrimaryMeasureDimensionAsync(decimal value,
            MeasureDimension targetMeasureDimension)
        {
            ArgumentNullException.ThrowIfNull(targetMeasureDimension);

            var result = value;
            var baseDimensionIn = await GetMeasureDimensionByIdAsync(_frameworkSettings.BaseDimensionId);
            if (result == decimal.Zero || targetMeasureDimension.Id == baseDimensionIn.Id)
                return result;

            var exchangeRatio = targetMeasureDimension.Ratio;
            if (exchangeRatio == decimal.Zero)
                throw new NiisException($"Exchange ratio not set for dimension [{targetMeasureDimension.Name}]");
            result *= exchangeRatio;

            return result;
        }

        /// <summary>
        /// Converts to primary measure dimension
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="sourceMeasureDimension">Source dimension</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the converted value
        /// </returns>
        public virtual async Task<decimal> ConvertToPrimaryMeasureDimensionAsync(decimal value,
            MeasureDimension sourceMeasureDimension)
        {
            ArgumentNullException.ThrowIfNull(sourceMeasureDimension);

            var result = value;
            var baseDimensionIn = await GetMeasureDimensionByIdAsync(_frameworkSettings.BaseDimensionId);
            if (result == decimal.Zero || sourceMeasureDimension.Id == baseDimensionIn.Id)
                return result;

            var exchangeRatio = sourceMeasureDimension.Ratio;
            if (exchangeRatio == decimal.Zero)
                throw new NiisException($"Exchange ratio not set for dimension [{sourceMeasureDimension.Name}]");
            result /= exchangeRatio;

            return result;
        }


        #endregion

        #region Weights

        /// <summary>
        /// Deletes measure weight
        /// </summary>
        /// <param name="measureWeight">Measure weight</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteMeasureWeightAsync(MeasureWeight measureWeight)
        {
            await _measureWeightRepository.DeleteAsync(measureWeight);
        }

        /// <summary>
        /// Gets a measure weight by identifier
        /// </summary>
        /// <param name="measureWeightId">Measure weight identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the measure weight
        /// </returns>
        public virtual async Task<MeasureWeight> GetMeasureWeightByIdAsync(int measureWeightId)
        {
            return await _measureWeightRepository.GetByIdAsync(measureWeightId, cache => default);
        }

        /// <summary>
        /// Gets a measure weight by system keyword
        /// </summary>
        /// <param name="systemKeyword">The system keyword</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the measure weight
        /// </returns>
        public virtual async Task<MeasureWeight> GetMeasureWeightBySystemKeywordAsync(string systemKeyword)
        {
            if (string.IsNullOrEmpty(systemKeyword))
                return null;

            var measureWeights = await GetAllMeasureWeightsAsync();
            foreach (var measureWeight in measureWeights)
                if (measureWeight.SystemKeyword.Equals(systemKeyword, StringComparison.InvariantCultureIgnoreCase))
                    return measureWeight;
            return null;
        }

        /// <summary>
        /// Gets all measure weights
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the measure weights
        /// </returns>
        public virtual async Task<IList<MeasureWeight>> GetAllMeasureWeightsAsync()
        {
            var measureWeights = await _measureWeightRepository.GetAllAsync(query =>
            {
                return from mw in query
                       orderby mw.DisplayOrder, mw.Id
                       select mw;
            }, cache => default);

            return measureWeights;
        }

        /// <summary>
        /// Inserts a measure weight
        /// </summary>
        /// <param name="measure">Measure weight</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertMeasureWeightAsync(MeasureWeight measure)
        {
            await _measureWeightRepository.InsertAsync(measure);
        }

        /// <summary>
        /// Updates the measure weight
        /// </summary>
        /// <param name="measure">Measure weight</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateMeasureWeightAsync(MeasureWeight measure)
        {
            await _measureWeightRepository.UpdateAsync(measure);
        }

        /// <summary>
        /// Converts weight
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="sourceMeasureWeight">Source weight</param>
        /// <param name="targetMeasureWeight">Target weight</param>
        /// <param name="round">A value indicating whether a result should be rounded</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the converted value
        /// </returns>
        public virtual async Task<decimal> ConvertWeightAsync(decimal value,
            MeasureWeight sourceMeasureWeight, MeasureWeight targetMeasureWeight, bool round = true)
        {
            ArgumentNullException.ThrowIfNull(sourceMeasureWeight);

            ArgumentNullException.ThrowIfNull(targetMeasureWeight);

            var result = value;
            if (result != decimal.Zero && sourceMeasureWeight.Id != targetMeasureWeight.Id)
            {
                result = await ConvertToPrimaryMeasureWeightAsync(result, sourceMeasureWeight);
                result = await ConvertFromPrimaryMeasureWeightAsync(result, targetMeasureWeight);
            }

            if (round)
                result = Math.Round(result, 2);

            return result;
        }

        /// <summary>
        /// Converts from primary weight
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="targetMeasureWeight">Target weight</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the converted value
        /// </returns>
        public virtual async Task<decimal> ConvertFromPrimaryMeasureWeightAsync(decimal value,
            MeasureWeight targetMeasureWeight)
        {
            ArgumentNullException.ThrowIfNull(targetMeasureWeight);

            var result = value;
            var baseWeightIn = await GetMeasureWeightByIdAsync(_frameworkSettings.BaseWeightId);
            if (result == decimal.Zero || targetMeasureWeight.Id == baseWeightIn.Id)
                return result;

            var exchangeRatio = targetMeasureWeight.Ratio;
            if (exchangeRatio == decimal.Zero)
                throw new NiisException($"Exchange ratio not set for weight [{targetMeasureWeight.Name}]");
            result *= exchangeRatio;

            return result;
        }

        /// <summary>
        /// Converts to primary measure weight
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="sourceMeasureWeight">Source weight</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the converted value
        /// </returns>
        public virtual async Task<decimal> ConvertToPrimaryMeasureWeightAsync(decimal value, MeasureWeight sourceMeasureWeight)
        {
            ArgumentNullException.ThrowIfNull(sourceMeasureWeight);

            var result = value;
            var baseWeightIn = await GetMeasureWeightByIdAsync(_frameworkSettings.BaseWeightId);
            if (result == decimal.Zero || sourceMeasureWeight.Id == baseWeightIn.Id)
                return result;

            var exchangeRatio = sourceMeasureWeight.Ratio;
            if (exchangeRatio == decimal.Zero)
                throw new NiisException($"Exchange ratio not set for weight [{sourceMeasureWeight.Name}]");
            result /= exchangeRatio;

            return result;
        }

        #endregion

        #endregion
    }
}