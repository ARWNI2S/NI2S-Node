using System.Collections;

namespace ARWNI2S.Engine.Core.Actor
{
    internal class ActorComponents : IEnumerable<IActorComponent>
    {
        private List<ComponentNode> _list;
        public IEnumerator<IActorComponent> GetEnumerator() => new Enumerator(_list);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();









        private class ComponentNode
        {
            public IActorComponent Object { get; }

            public ComponentRoot Root { get; }
            public ComponentNode Parent { get; }
            public IList<ComponentNode> Children { get; } = [];

            public ComponentNode(IActorComponent component)
            {
                Object = component;
            }
        }

        private class ComponentRoot : ComponentNode
        {
            public ComponentRoot(IActorComponent component)
                : base(component) { }
        }

        private class Enumerator : IEnumerator<IActorComponent>, IEnumerator
        {
            private bool disposedValue;

            public Enumerator(List<ComponentNode> children)
            {
            }

            public IActorComponent Current => throw new NotImplementedException();

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                throw new NotImplementedException();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // TODO: eliminar el estado administrado (objetos administrados)
                    }

                    // TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
                    // TODO: establecer los campos grandes como NULL
                    disposedValue = true;
                }
            }

            // // TODO: reemplazar el finalizador solo si "Dispose(bool disposing)" tiene código para liberar los recursos no administrados
            // ~Enumerator()
            // {
            //     // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
            //     Dispose(disposing: false);
            // }

            public void Dispose()
            {
                // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }
    }
}