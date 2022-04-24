using System;

namespace AM_DI.Scripts.Attributes
{
    /// <summary>
    /// Find the component with the same type in child
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FindInChildAttribute : Attribute
    {
        /// <summary>
        /// Search only in game objects enabled
        /// </summary>
        public bool OnlyActive { get; set; }

        /// <summary>
        /// Use to get a specific item in children
        /// </summary>
        public string Path { get; set; }
    }
}
