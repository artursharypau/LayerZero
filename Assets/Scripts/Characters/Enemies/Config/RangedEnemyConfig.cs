using UnityEngine;

namespace LayerZero.Characters.Enemies.Config
{
    /// <summary>
    /// Config for enemies that fight at range (archers, mages). Extending the base asset keeps
    /// every shared knob in one place and adds only what the archetype actually needs.
    /// </summary>
    [CreateAssetMenu(fileName = "RangedEnemyConfig", menuName = "LayerZero/Characters/Ranged Enemy Config")]
    public sealed class RangedEnemyConfig : EnemyConfig
    {
        [SerializeField] private RangedCombatSettings _ranged = new();

        public RangedCombatSettings Ranged => _ranged;
    }
}
