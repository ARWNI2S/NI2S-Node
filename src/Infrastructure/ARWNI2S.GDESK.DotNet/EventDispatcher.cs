namespace ARWNI2S.GDESK
{
    public class EventDispatcher
    {
        // Cola de prioridad para los eventos, ordenada por TimeStamp
        private readonly SortedSet<Event> eventQueue;

        // Reloj de simulación global
        public double CurrentTime { get; private set; }

        // Constructor
        public EventDispatcher()
        {
            eventQueue = new SortedSet<Event>(new EventComparer());
            CurrentTime = 0.0;
        }

        // Agregar un evento a la cola
        public void AddEvent(Event newEvent)
        {
            eventQueue.Add(newEvent);
            Console.WriteLine($"Added: {newEvent}");
        }

        // Procesar eventos cuando sea el momento adecuado
        public void ProcessEvents(double currentTime)
        {
            // Actualizamos el reloj de simulación
            CurrentTime = currentTime;

            // Procesamos eventos cuya marca de tiempo ha sido alcanzada
            while (eventQueue.Count > 0 && eventQueue.Min.TimeStamp <= CurrentTime)
            {
                Event evt = eventQueue.Min;
                eventQueue.Remove(evt);

                // Aquí llamamos al método para manejar el evento
                HandleEvent(evt);
            }
        }

        // Manejar un evento
        private static void HandleEvent(Event evt)
        {
            Console.WriteLine($"Processing: {evt}");
            // Aquí podríamos tener lógica de procesamiento específico para cada tipo de evento
            // Se notifica al objeto destino del evento
            ((GameObject)evt.Destination).ReceiveEvent(evt);
        }
    }

    // Comparador de eventos basado en el TimeStamp
    public class EventComparer : IComparer<Event>
    {
        public int Compare(Event x, Event y)
        {
            return x.TimeStamp.CompareTo(y.TimeStamp);
        }
    }
}
