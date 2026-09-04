using LayerZero.Core.Extensions;
using LayerZero.Core.Timing;
using LayerZero.Gameplay.Characters.Common;
using LayerZero.Gameplay.Characters.Common.Movement;
using LayerZero.Gameplay.Stats.Health;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace LayerZero.Presentation.Ui.HealthBar
{
    internal sealed class EnemyHealthBar : MonoBehaviour, ICharacterView
    {
        [SerializeField] [Min(0f)] private float _visibleDuration = 3f;
        [SerializeField] [Min(0.01f)] private float _fadeOutDuration = 0.25f;

        private Canvas _canvas;
        private CanvasGroup _canvasGroup;
        private Slider _slider;
        private IPositioned _positioned;
        private IHealth _health;

        private Countdown _visibleTimer;

        [Inject]
        public void Construct(IHealth health)
        {
            _health = health;
        }

        private void Awake()
        {
            _canvas = this.GetRequiredComponent<Canvas>();
            _canvasGroup = this.GetRequiredComponent<CanvasGroup>();
            _slider = this.GetRequiredComponentInChildren<Slider>();
            _positioned = this.GetRequiredComponentInParent<IPositioned>();
        }

        private void Start()
        {
            _slider.value = _health.CurrentHealth / _health.MaxHealth;
            Hide();
        }

        private void OnEnable()
        {
            _positioned.FacingDirectionChanged += OnFacingDirectionChanged;
            _health.HealthChanged += OnHealthChanged;
            _health.Died += OnDied;
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
            _health.HealthChanged -= OnHealthChanged;
            _health.Died -= OnDied;
        }

        private void OnFacingDirectionChanged(float _)
        {
            transform.rotation = Quaternion.identity;
        }

        private void OnHealthChanged(float _)
        {
            _slider.value = _health.CurrentHealth / _health.MaxHealth;
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
