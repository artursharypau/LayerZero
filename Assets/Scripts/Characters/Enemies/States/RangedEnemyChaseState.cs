using LayerZero.Characters.Enemies.Config;
using UnityEngine;

namespace LayerZero.Characters.Enemies.States
{
    /// <summary>
    /// Kiting version of the chase: holds a preferred distance instead of closing all the way in.
    /// <para>
    /// Registered instead of <see cref="EnemyChaseState" />; every other state keeps asking for
    /// "the chase state" by its base type and transparently gets this one.
    /// </para>
    /// </summary>
    public sealed class RangedEnemyChaseState : EnemyChaseState
    {
        private readonly RangedCombatSettings _settings;

        public RangedEnemyChaseState(EnemyController owner, RangedCombatSettings settings)
            : base(owner)
        {
            _settings = settings;
        }

        protected override float GetMoveDirection()
        {
            float distance = Perception.HorizontalDistanceToTarget;
            float toTarget = Perception.DirectionToTarget;

            if (distance > _settings.PreferredDistance + _settings.DistanceTolerance)
            {
                return toTarget;
            }

            if (distance < _settings.PreferredDistance - _settings.DistanceTolerance)
            {
                // Back off, but keep facing the target - handled by the base state's flip logic.
                return -toTarget * _settings.RetreatSpeedMultiplier;
            }

            return 0f;
        }

        protected override bool CanEngage()
        {
            float distance = Perception.HorizontalDistanceToTarget;
            if (Mathf.Abs(distance - _settings.PreferredDistance) > _settings.DistanceTolerance)
            {
                return false;
            }

            return base.CanEngage();
        }
    }
}
