using ARWNI2S.Engine.Simulation.Time;

namespace ARWNI2S.Runtime.Simulation.Time
{
    [Alias("SimulationClock")]
    public interface INI2SClock : IGrainWithIntegerKey, ISimulationClock
    {
        /// <summary>
        ///  Método para la sincronización distribuida por consenso.
        /// </summary>
        /// <param name="proposedTime">the proposed time</param>
        /// <returns></returns>
        Task ProposeTimeAsync(TimeSpan proposedTime);

        // Consulta el estado de sincronización con otros relojes.
        Task<bool> IsSynchronizedAsync();
    }
}
