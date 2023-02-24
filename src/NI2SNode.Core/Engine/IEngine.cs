using NI2S.Node.Engine.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NI2S.Node.Engine
{
    internal interface IEngine
    {
        IModuleCollection Modules { get; }
    }
}
