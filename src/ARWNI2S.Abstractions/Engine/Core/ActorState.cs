namespace ARWNI2S.Engine.Core
{
    public class ActorState
    {
        private readonly Dictionary<string, object> _state = new Dictionary<string, object>();

        public object this[string propertyName]
        {
            get
            {
                return _state[propertyName];
            }
            set
            {
                _state[propertyName] = value;
            }
        }
    }
}
