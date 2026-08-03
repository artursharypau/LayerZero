using LayerZero.Core.Extensions;
using LayerZero.Core.Timing;
using UnityEngine;

namespace LayerZero.Combat.Damage.Vfx
{
    /// <summary>Swaps the sprite material for a short flash whenever the owner takes damage.</summary>
    [RequireComponent(typeof(DamageReceiver))]
    public sealed class DamageFlashVfx : MonoBehaviour
    {
        [SerializeField] [Min(0f)] private float _duration = 0.15f;
        [SerializeField] private Material _material;

        private SpriteRenderer _renderer;
        private IDamageReceiver _damageReceiver;
        private CountdownTimer _timer;
        private Material _defaultMaterial;
        private bool _isFlashing;

        private void Awake()
        {
            _renderer = this.GetRequiredInChildren<SpriteRenderer>();
            _damageReceiver = this.GetRequired<IDamageReceiver>();
            _timer = new CountdownTimer();

            if (_renderer)
            {
                _defaultMaterial = _renderer.sharedMaterial;
            }
        }

        private void OnEnable()
        {
            if (_damageReceiver != null)
            {
                _damageReceiver.Damaged += OnDamaged;
            }
        }

        private void OnDisable()
        {
            if (_damageReceiver != null)
            {
                _damageReceiver.Damaged -= OnDamaged;
            }

            StopFlash();
        }

        private void Update()
        {
            if (!_isFlashing)
            {
                return;
            }

            _timer.Tick(Time.deltaTime);
            if (_timer.IsExpired)
            {
                StopFlash();
            }
        }

        private void OnDamaged(DamageInfo damageInfo)
        {
            if (!_renderer || !_material)
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
