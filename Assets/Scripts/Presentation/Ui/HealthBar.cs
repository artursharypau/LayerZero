using LayerZero.Core.Extensions;
using LayerZero.Gameplay.Characters.Common.Movement;
using LayerZero.Gameplay.Combat.Damage;
using UnityEngine;
using UnityEngine.UI;

namespace LayerZero.Presentation.Ui
{
    public sealed class HealthBar : MonoBehaviour
    {
        private Slider _slider;
        private IPositioned _positioned;
        private IDamageable _damageable;

        private void Awake()
        {
            _slider = this.GetRequiredComponentInChildren<Slider>();
            _positioned = this.GetRequiredComponentInParent<IPositioned>();
            _damageable = this.GetRequiredComponentInParent<IDamageable>();
        }

        private void Start()
        {
            _slider.value = _damageable.CurrentHealth;
        }

        private void OnEnable()
        {
            _positioned.FacingDirectionChanged += OnFacingDirectionChanged;
            _damageable.HealthChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            _positioned.FacingDirectionChanged -= OnFacingDirectionChanged;
            _damageable.HealthChanged -= OnHealthChanged;
        }

        private void OnFacingDirectionChanged(float _)
        {
            transform.rotation = Quaternion.identity;
        }

        private void OnHealthChanged(int _)
        {
            _slider.value = (float)_damageable.CurrentHealth / _damageable.MaxHealth;
        }
    }
}
