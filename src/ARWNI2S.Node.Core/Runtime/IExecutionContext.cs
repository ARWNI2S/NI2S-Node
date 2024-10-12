using ARWNI2S.Node.Core.Network;

namespace ARWNI2S.Node.Core.Runtime
{
    public interface IExecutionContext
    {
        //// AsyncLocal permite mantener el valor asociado con cada contexto asincrónico
        //private static readonly AsyncLocal<Dictionary<string, object>> _asyncLocalContext = new AsyncLocal<Dictionary<string, object>>();

        //public Dictionary<string, object> Context
        //{
        //    get
        //    {
        //        if (_asyncLocalContext.Value == null)
        //            _asyncLocalContext.Value = new Dictionary<string, object>();

        //        return _asyncLocalContext.Value;
        //    }
        //}

        //// Obtener un valor del contexto de la solicitud de manera segura
        //public object Get(string key)
        //{
        //    if (Context.ContainsKey(key))
        //        return Context[key];
        //    return null;
        //}

        //// Establecer un valor en el contexto de la solicitud de manera segura
        //public void Set(string key, object value)
        //{
        //    Context[key] = value;
        //    // Además, propagar el valor al RequestContext de Orleans si es relevante
        //    RequestContext.Set(key, value);
        //}

        //// Limpiar el contexto
        //public void Clear()
        //{
        //    Context.Clear();
        //    RequestContext.Clear();
        //}

        IRuntimeRequest Request { get; }

        INodeConnection Connection { get; }
    }
}