namespace ARWNI2S.Runtime.Simulation.Time.Grains
{
    [Alias("SynchonizedClock")]
    public interface ISynchonizedClockGrain : IGrainWithIntegerKey
    {
        /// <summary>
        /// Obtiene la resolucion del reloj de simulacion.
        /// </summary>
        /// <returns>Resolucion de la simulacion.</returns>
        Task<double> GetResolution();

        /// <summary>
        /// Obtiene el tiempo actual de la simulacion.
        /// </summary>
        /// <returns>Tarea asincrona que contiene el Tiempo Transcurrido.</returns>
        Task<TimeSpan> GetTimeAsync();

        /// <summary>
        /// Inicia el reloj de simulacion global en todos los nodos.
        /// </summary>
        Task StartAsync();

        /// <summary>
        /// Pausa el reloj de simulacion global en todos los nodos.
        /// </summary>
        void Pause();

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
