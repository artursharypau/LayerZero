using LayerZero.Core.Extensions;
using LayerZero.Core.Timing;
using LayerZero.Gameplay.Characters.Common;
using LayerZero.Gameplay.Stats.Health;
using UnityEngine;
using VContainer;

namespace LayerZero.Presentation.Vfx.Combat
{
    internal sealed class DamageVfx : MonoBehaviour, ICharacterView
    {
        [SerializeField] [Min(0f)] private float _duration = 0.15f;
        [SerializeField] private Material _material;

        private IHealth _health;
        private SpriteRenderer _renderer;
        private Material _defaultMaterial;

        private Countdown _timer;
        private bool _isFlashing;

        [Inject]
        public void Construct(IHealth health)
        {
            _health = health;
        }

        private void Awake()
        {
            _renderer = this.GetRequiredComponentInChildren<SpriteRenderer>();
            _defaultMaterial = _renderer.sharedMaterial;
        }

        private void OnEnable()
        {
            _health.HealthChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            _health.HealthChanged -= OnHealthChanged;

            StopFlash();
        }

        private void Update()
        {
            if (_isFlashing && _timer.IsExpired)
            {
                StopFlash();
            }
        }

        private void OnHealthChanged(float _)
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
