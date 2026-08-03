using UnityEngine;

namespace LayerZero.Characters.Player.Config
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "LayerZero/Characters/Player Config")]
    public sealed class PlayerConfig : ScriptableObject
    {
        [SerializeField] private PlayerMovementSettings _movement = new();
        [SerializeField] private PlayerInputSettings _input = new();
        [SerializeField] private JumpAbilitySettings _jump = new();
        [SerializeField] private DashAbilitySettings _dash = new();
        [SerializeField] private PlayerAttackSettings _attack = new();
        [SerializeField] private PlayerJumpAttackSettings _jumpAttack = new();

        public PlayerMovementSettings Movement => _movement;
        public PlayerInputSettings Input => _input;
        public JumpAbilitySettings Jump => _jump;
        public DashAbilitySettings Dash => _dash;
        public PlayerAttackSettings Attack => _attack;
        public PlayerJumpAttackSettings JumpAttack => _jumpAttack;
    }
}
