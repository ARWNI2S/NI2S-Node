using ARWNI2S.Collections;

namespace ARWNI2S.Entities
{
    public interface IActorComponents : ITree<IActorComponent>, IComposition<Guid, IActorComponent>
    {
        /// <summary>
        /// Gets or sets a given component. Setting a null value removes the component.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The requested component, or null if it is not present.</returns>
        IActorComponent this[string name] { get; set; }

        /// <summary>
        /// Retrieves the requested component from the collection.
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="TComponent">The component key.</typeparam>
        /// <returns>The requested component, or null if it is not present.</returns>
        TComponent Get<TComponent>(string name) where TComponent : class, IActorComponent;
    }
}