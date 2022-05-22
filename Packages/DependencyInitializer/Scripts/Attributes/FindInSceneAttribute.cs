using System;

namespace AM_DI.Scripts.Attributes
{
    /// <summary>
    /// Allow search in the current scene
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FindInSceneAttribute : Attribute
    {
        /// <summary>
        /// Only consider active game objects
        /// </summary>
        public bool ActiveOnly { get; set; }
    }
}
