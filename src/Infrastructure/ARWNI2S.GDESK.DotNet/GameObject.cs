namespace ARWNI2S.GDESK
{
    public abstract class GameObject(EventDispatcher dispatcher)
    {
        // Método para enviar un evento a otro objeto
        public void SendEvent(GameObject destination, double delay, params object[] eventData)
        {
            // Creamos un nuevo evento con la marca de tiempo correspondiente
            Event newEvent = new(this, destination, dispatcher.CurrentTime + delay, eventData);
            dispatcher.AddEvent(newEvent);
        }

        // Método abstracto que debe implementarse en cada subclase para manejar los eventos recibidos
        public abstract void ReceiveEvent(Event evt);

        // Método opcional para inicializar eventos, útil si los objetos tienen eventos recurrentes
        public virtual void InitializeEvents() { }
    }
}
