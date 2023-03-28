using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NI2S.Node.Dummy
{
    internal class DummyEndpoint
    {
        public MessageDelegate MessageDelegate { get; internal set; }
        public object DisplayName { get; internal set; }
    }
}
