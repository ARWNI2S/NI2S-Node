namespace ARWNI2S.GDESK
{
    public class Event
    {
        public object Source { get; private set; }
        public object Destination { get; private set; }
        public double TimeStamp { get; private set; }
        public object[] EventData { get; private set; }

        // Constructor
        public Event(object source, object destination, double timeStamp, params object[] eventData)
        {
            Initialize(source, destination, timeStamp, eventData);
        }

        // Método para inicializar o reiniciar el evento
        public void Initialize(object source, object destination, double timeStamp, params object[] eventData)
        {
            Source = source;
            Destination = destination;
            TimeStamp = timeStamp;
            EventData = eventData;
        }

        // Método para limpiar los datos del evento (opcional)
        public void Reset()
        {
            Source = null;
            Destination = null;
            TimeStamp = 0;
            EventData = null;
        }
    }
}
