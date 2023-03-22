using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NI2S.Node
{
    public sealed class Platform
    {
        private static readonly Platform _instance;

        static Platform() { _instance = new Platform(); }

        //NOT CREATABLE
        private Platform()
        {

        }
    }
}
