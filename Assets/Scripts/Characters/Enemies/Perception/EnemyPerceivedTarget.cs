using LayerZero.Combat.Damage;
using UnityEngine;

namespace LayerZero.Characters.Enemies.Perception
{
    public readonly struct EnemyPerceivedTarget
    {
        public static readonly EnemyPerceivedTarget None = default;

        public Transform Transform { get; }
        public IDamageable Health { get; }

        public EnemyPerceivedTarget(Transform transform, IDamageable health)
        {
            Transform = transform;
            Health = health;
        }

        public bool IsValid => Transform && Health?.IsDead == false;
    }
}
