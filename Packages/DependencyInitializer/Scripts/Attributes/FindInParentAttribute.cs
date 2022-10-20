using System;

namespace AM_DI.Scripts.Attributes
{
    /// <summary>
    /// Find the component in parent transform
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FindInParentAttribute : Attribute
    {
        /// <summary>
        /// When ignore self is set to false, the search will look for the target component in this game object before move to parent
        /// </summary>
        public bool IgnoreSelf { get; set; } = true;
    }
}
