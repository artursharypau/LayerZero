using LayerZero.Core.Diagnostics;
using UnityEngine;

namespace LayerZero.Core.Extensions
{
    public static class ComponentExtensions
    {
        public static TComponent GetRequiredComponent<TComponent>(this Component self)
            where TComponent : class
        {
            TComponent component = self.GetComponent<TComponent>();
            if (component == null)
            {
                GameLog.Error(self, $"'{self.name}' is missing a required component of type '{typeof(TComponent).Name}'.");
            }

            return component;
        }

        public static TComponent[] GetRequiredComponents<TComponent>(this Component self)
            where TComponent : class
        {
            TComponent[] components = self.GetComponents<TComponent>();
            if (components == null || components.Length == 0)
            {
                GameLog.Error(self, $"'{self.name}' is missing a required component of type '{typeof(TComponent).Name}'.");
            }

            return components;
        }

        public static bool TryGetRequiredComponent<TComponent>(this Component self, out TComponent component)
            where TComponent : class
        {
            if (self.TryGetComponent(out component))
            {
                return true;
            }

            GameLog.Error(self, $"'{self.name}' is missing a required component of type '{typeof(TComponent).Name}'.");
            return false;
        }

        public static TComponent GetRequiredComponentInChildren<TComponent>(this Component self)
            where TComponent : class
        {
            TComponent component = self.GetComponentInChildren<TComponent>(true);
            if (component == null)
            {
                GameLog.Error(self, $"'{self.name}' is missing a required child component of type '{typeof(TComponent).Name}'.");
            }

            return component;
        }

        public static TComponent[] GetRequiredComponentsInChildren<TComponent>(this Component self)
            where TComponent : class
        {
            TComponent[] components = self.GetComponentsInChildren<TComponent>(true);
            if (components == null || components.Length == 0)
            {
                GameLog.Error(self, $"'{self.name}' is missing a required child component of type '{typeof(TComponent).Name}'.");
            }

            return components;
        }

        public static TComponent GetRequiredComponentInParent<TComponent>(this Component self)
            where TComponent : class
        {
            TComponent component = self.GetComponentInParent<TComponent>(true);
            if (component == null)
            {
                GameLog.Error(self, $"'{self.name}' is missing a required parent component of type '{typeof(TComponent).Name}'.");
            }

            return component;
        }
    }
}
