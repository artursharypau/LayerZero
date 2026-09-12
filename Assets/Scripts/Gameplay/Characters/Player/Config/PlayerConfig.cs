using LayerZero.Gameplay.Combat.Elements;
using LayerZero.Gameplay.Stats.Config;
using UnityEngine;

namespace LayerZero.Gameplay.Characters.Player.Config
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "LayerZero/Characters/Player Config")]
    internal sealed class PlayerConfig : ScriptableObject
    {
        [SerializeField] private StatsConfig _stats;

        [SerializeField] private PlayerMovementConfig _movement = new();
        [SerializeField] private PlayerInputConfig _input = new();
        [SerializeField] private JumpAbilityConfig _jump = new();
        [SerializeField] private DashAbilityConfig _dash = new();
        [SerializeField] private PlayerAttackConfig _attack = new();
        [SerializeField] private PlayerJumpAttackConfig _jumpAttack = new();
        [SerializeField] private PlayerCounterattackConfig _counterattack = new();
        [SerializeField] private ElementKind _initialElement = ElementKind.Fire;

        public StatsConfig Stats => _stats;

        public PlayerMovementConfig Movement => _movement;
        public PlayerInputConfig Input => _input;
        public JumpAbilityConfig Jump => _jump;
        public DashAbilityConfig Dash => _dash;
        public PlayerAttackConfig Attack => _attack;
        public PlayerJumpAttackConfig JumpAttack => _jumpAttack;
        public PlayerCounterattackConfig Counterattack => _counterattack;
        public ElementKind InitialElement => _initialElement;
    }
}
