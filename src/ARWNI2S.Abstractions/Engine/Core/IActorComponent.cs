/* Cambio no fusionado mediante combinación del proyecto 'ARWNI2S.Abstractions (net9.0)'
Antes:
using ARWNI2S.Engine.Core.Object;
Después:
using ARWNI2S;
using ARWNI2S.Engine;
using ARWNI2S.Engine.Core;
using ARWNI2S.Engine.Core;
using ARWNI2S.Engine.Core.Actor;
using ARWNI2S.Engine.Core.Object;
*/
namespace ARWNI2S.Engine.Core
{
    public interface IActorComponent : INiisObject
    {
        INiisActor Owner { get; }
        IActorComponent Parent { get; }
        IEnumerable<IActorComponent> Children { get; }
    }
}
