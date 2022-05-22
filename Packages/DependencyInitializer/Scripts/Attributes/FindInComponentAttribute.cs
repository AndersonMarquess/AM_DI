using System;

namespace AM_DI.Scripts.Attributes
{
    /// <summary>
    /// Get a component in current game object
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FindInComponentAttribute : Attribute
    {

    }
}
