using LayerZero.Core.Diagnostics;
using UnityEngine;

namespace LayerZero.Core.Extensions
{
    public static class ComponentExtensions
    {
        /// <summary>
        /// <see cref="Component.GetComponent{T}" /> that reports a readable error instead of
        /// letting a null reference surface somewhere else a few frames later.
        /// </summary>
        public static T GetRequired<T>(this Component self) where T : class
        {
            T component = self.GetComponent<T>();
            if (component == null)
            {
                GameLog.Error(self, $"'{self.name}' is missing a required component of type '{typeof(T).Name}'.");
            }

            return component;
        }

        public static T GetRequiredInChildren<T>(this Component self) where T : class
        {
            T component = self.GetComponentInChildren<T>(true);
            if (component == null)
            {
                GameLog.Error(self, $"'{self.name}' is missing a required child component of type '{typeof(T).Name}'.");
            }

            return component;
        }
    }
}
