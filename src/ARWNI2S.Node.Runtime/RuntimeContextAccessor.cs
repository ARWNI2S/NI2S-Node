﻿using ARWNI2S.Node.Core.Runtime;

namespace ARWNI2S.Node.Runtime
{
    internal class RuntimeContextAccessor : IExecutionContextAccessor
    {
        public IExecutionContext ExecutionContext { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
