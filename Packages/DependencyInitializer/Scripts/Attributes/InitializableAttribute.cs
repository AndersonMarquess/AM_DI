using System;

namespace AM_DI.Scripts.Attributes
{
    /// <summary>
    /// Sets the field as initializable. Useful when the class isn't a MonoBehaviour.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class InitializableAttribute : Attribute
    {
    }
}
