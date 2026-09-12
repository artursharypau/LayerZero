using LayerZero.Gameplay.Stats.Health;
using UnityEngine;

namespace LayerZero.Gameplay.Characters.Enemies.Perception
{
    internal readonly struct EnemyPerceivedTarget
    {
        public static readonly EnemyPerceivedTarget None = default;

        public Transform Transform { get; }
        public IHealth Health { get; }

        public EnemyPerceivedTarget(Transform transform, IHealth health)
        {
            Transform = transform;
            Health = health;
        }

        public bool IsValid => Transform && Health?.IsDead == false;
    }
}
