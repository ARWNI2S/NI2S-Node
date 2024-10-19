﻿namespace ARWNI2S.Runtime.Core.Attributes
{
    /// <summary>
    /// Indicates that the type and any derived types that this attribute is applied to
    /// are considered a controller by the default controller discovery mechanism, unless
    /// <see cref="NonActorAttribute"/> is applied to any type in the hierarchy.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ActorAttribute : Attribute
    {
    }
}
