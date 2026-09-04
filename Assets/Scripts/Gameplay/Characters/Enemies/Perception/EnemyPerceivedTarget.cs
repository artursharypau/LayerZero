using LayerZero.Gameplay.Combat.Damage;
using UnityEngine;

namespace LayerZero.Gameplay.Characters.Enemies.Perception
{
    internal readonly struct EnemyPerceivedTarget
    {
        public static readonly EnemyPerceivedTarget None = default;

        public Transform Transform { get; }
        public IDamageReceiver DamageReceiver { get; }

        public EnemyPerceivedTarget(Transform transform, IDamageReceiver damageReceiver)
        {
            Transform = transform;
            DamageReceiver = damageReceiver;
        }

        public bool IsValid => Transform && DamageReceiver?.IsDead == false;
    }
}
