namespace ARWNI2S.GDESK
{
    public class EventPool
    {
        // Lista de eventos disponibles para reutilización
        private readonly Stack<Event> pool;

        // Constructor
        public EventPool()
        {
            pool = new Stack<Event>();
        }

        // Obtener un evento del pool o crear uno nuevo si no hay disponibles
        public Event GetEvent(object source, object destination, double timeStamp, params object[] eventData)
        {
            if (pool.Count > 0)
            {
                // Reutilizamos un evento existente
                Event reusedEvent = pool.Pop();
                reusedEvent.Initialize(source, destination, timeStamp, eventData);
                return reusedEvent;
            }
            else
            {
                // Creamos un nuevo evento si no hay en el pool
                return new Event(source, destination, timeStamp, eventData);
            }
        }

        // Devolver un evento al pool para su reutilización
        public void ReturnEvent(Event evt)
        {
            pool.Push(evt);
        }
    }

}
