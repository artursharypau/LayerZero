using UnityEngine;

namespace LayerZero.Characters.Player.Config
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "LayerZero/Characters/Player Config")]
    public sealed class PlayerConfig : ScriptableObject
    {
        [SerializeField] private PlayerMovementConfig _movement = new();
        [SerializeField] private PlayerInputConfig _input = new();
        [SerializeField] private JumpAbilityConfig _jump = new();
        [SerializeField] private DashAbilityConfig _dash = new();
        [SerializeField] private PlayerAttackConfig _attack = new();
        [SerializeField] private PlayerJumpAttackConfig _jumpAttack = new();

        public PlayerMovementConfig Movement => _movement;
        public PlayerInputConfig Input => _input;
        public JumpAbilityConfig Jump => _jump;
        public DashAbilityConfig Dash => _dash;
        public PlayerAttackConfig Attack => _attack;
        public PlayerJumpAttackConfig JumpAttack => _jumpAttack;
    }
}
