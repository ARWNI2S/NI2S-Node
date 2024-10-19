﻿namespace ARWNI2S.Runtime.Core.Attributes
{
    /// <summary>
    /// Indicates that the type and any derived types that this attribute is applied to
    /// is not considered a controller by the default controller discovery mechanism.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class NonActorAttribute : Attribute
    {
    }
}
