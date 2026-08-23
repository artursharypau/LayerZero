using LayerZero.Core.Extensions;
using LayerZero.Core.Timing;
using LayerZero.Gameplay.Characters.Common.Movement;
using LayerZero.Gameplay.Combat.Damage;
using UnityEngine;
using UnityEngine.UI;

namespace LayerZero.Presentation.Ui.HealthBar
{
    internal sealed class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] [Min(0f)] private float _visibleDuration = 3f;
        [SerializeField] [Min(0.01f)] private float _fadeOutDuration = 0.25f;

        private Canvas _canvas;
        private CanvasGroup _canvasGroup;
        private Slider _slider;
        private IPositioned _positioned;
        private IDamageable _damageable;

        private Countdown _visibleTimer;

        private void Awake()
        {
            _canvas = this.GetRequiredComponent<Canvas>();
            _canvasGroup = this.GetRequiredComponent<CanvasGroup>();
            _slider = this.GetRequiredComponentInChildren<Slider>();
            _positioned = this.GetRequiredComponentInParent<IPositioned>();
            _damageable = this.GetRequiredComponentInParent<IDamageable>();
        }

        private void Start()
        {
            _slider.value = _damageable.CurrentHealth;
            Hide();
        }

        private void OnEnable()
        {
            _positioned.FacingDirectionChanged += OnFacingDirectionChanged;
            _damageable.HealthChanged += OnHealthChanged;
            _damageable.Died += OnDied;
        }

        private void Update()
        {
            if (!_visibleTimer.IsExpired)
            {
                return;
            }

            _canvasGroup.alpha -= Time.deltaTime / _fadeOutDuration;

            if (_canvasGroup.alpha <= 0f)
            {
                Hide();
            }
        }

        private void OnDisable()
        {
            _positioned.FacingDirectionChanged -= OnFacingDirectionChanged;
            _damageable.HealthChanged -= OnHealthChanged;
            _damageable.Died -= OnDied;
        }

        private void OnFacingDirectionChanged(float _)
        {
            transform.rotation = Quaternion.identity;
        }

        private void OnHealthChanged(int _)
        {
            _slider.value = (float)_damageable.CurrentHealth / _damageable.MaxHealth;
            Show();
        }

        private void OnDied()
        {
            _visibleTimer.Stop();
        }

        private void Show()
        {
            if (_visibleTimer.IsExpired)
            {
                _canvasGroup.alpha = 1f;
                _canvas.enabled = true;
            }

            _visibleTimer.Start(_visibleDuration);
        }

        private void Hide()
        {
            _canvasGroup.alpha = 0f;
            _canvas.enabled = false;
        }
    }
}
