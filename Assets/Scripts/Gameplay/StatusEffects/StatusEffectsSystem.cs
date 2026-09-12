using System;
using System.Collections.Generic;
using LayerZero.Core.Diagnostics;
using UnityEngine;
using VContainer.Unity;

namespace LayerZero.Gameplay.StatusEffects
{
    internal sealed class StatusEffectsSystem : IStatusEffectsSystem, ITickable, IDisposable
    {
        private readonly IReadOnlyDictionary<Type, IStatusEffectHandler> _handlers;

        private int _nextId = 1;

        public StatusEffectsSystem(IEnumerable<IStatusEffectHandler> handlers)
        {
            Dictionary<Type, IStatusEffectHandler> map = new();

            foreach (IStatusEffectHandler handler in handlers)
            {
                if (!map.TryAdd(handler.EffectType, handler))
                {
                    GameLog.Error(this, $"Duplicate status effect handler for '{handler.EffectType.Name}'.");
                }
            }

            _handlers = map;
        }

        public StatusEffectHandle Apply<TEffect>(in TEffect statusEffect)
            where TEffect : struct, IStatusEffect
        {
            Type type = typeof(TEffect);
            if (!_handlers.TryGetValue(type, out IStatusEffectHandler handler))
            {
                return default;
            }

            int id = _nextId++;
            if (!((IStatusEffectHandler<TEffect>)handler).TryApply(id, in statusEffect))
            {
                return default;
            }

            return new StatusEffectHandle(type, id);
        }

        public void Remove(StatusEffectHandle handle)
        {
            if (handle.IsValid && _handlers.TryGetValue(handle.EffectType, out IStatusEffectHandler handler))
            {
                handler.Remove(handle.Id);
            }
        }

        public void Tick()
        {
            foreach (IStatusEffectHandler handler in _handlers.Values)
            {
                if (handler.IsActive)
                {
                    handler.Tick(Time.deltaTime);
                }
            }
        }

        public void Dispose()
        {
            foreach (IStatusEffectHandler handler in _handlers.Values)
            {
                if (handler.IsActive)
                {
                    handler.Clear();
                }
            }
        }
    }
}
