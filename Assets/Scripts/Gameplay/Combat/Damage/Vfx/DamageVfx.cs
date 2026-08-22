using LayerZero.Core.Extensions;
using LayerZero.Core.Timing;
using UnityEngine;

namespace LayerZero.Gameplay.Combat.Damage.Vfx
{
    [RequireComponent(typeof(DamageReceiver))]
    public sealed class DamageVfx : MonoBehaviour
    {
        [SerializeField] [Min(0f)] private float _duration = 0.15f;
        [SerializeField] private Material _material;

        private IDamageReceiver _damageReceiver;
        private SpriteRenderer _renderer;
        private Material _defaultMaterial;

        private Countdown _timer;
        private bool _isFlashing;

        private void Awake()
        {
            _damageReceiver = this.GetRequiredComponent<IDamageReceiver>();
            _renderer = this.GetRequiredComponentInChildren<SpriteRenderer>();
            _defaultMaterial = _renderer.sharedMaterial;
        }

        private void OnEnable()
        {
            _damageReceiver.Damaged += OnDamaged;
        }

        private void OnDisable()
        {
            _damageReceiver.Damaged -= OnDamaged;

            StopFlash();
        }

        private void Update()
        {
            if (_isFlashing && _timer.IsExpired)
            {
                StopFlash();
            }
        }

        private void OnDamaged(DamageInfo damageInfo)
        {
            if (!_material)
            {
                return;
            }

            _renderer.sharedMaterial = _material;
            _isFlashing = true;
            _timer.Start(_duration);
        }

        private void StopFlash()
        {
            if (!_isFlashing)
            {
                return;
            }

            _isFlashing = false;
            if (_renderer)
            {
                _renderer.sharedMaterial = _defaultMaterial;
            }
        }
    }
}
