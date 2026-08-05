using LayerZero.Core.Diagnostics;
using UnityEngine;

namespace LayerZero.Core.Extensions
{
    public static class ComponentExtensions
    {
        public static TComponent GetRequired<TComponent>(this Component self)
            where TComponent : class
        {
            TComponent component = self.GetComponent<TComponent>();
            if (component == null)
            {
                GameLog.Error(self, $"'{self.name}' is missing a required component of type '{typeof(TComponent).Name}'.");
            }

            return component;
        }

        public static TComponent GetRequiredInChildren<TComponent>(this Component self)
            where TComponent : class
        {
            TComponent component = self.GetComponentInChildren<TComponent>(true);
            if (component == null)
            {
                GameLog.Error(self, $"'{self.name}' is missing a required child component of type '{typeof(TComponent).Name}'.");
            }

            return component;
        }
    }
}
