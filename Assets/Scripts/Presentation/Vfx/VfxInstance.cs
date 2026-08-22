using System;
using LayerZero.Core.Diagnostics;
using LayerZero.Core.Extensions;
using LayerZero.Core.Timing;
using UnityEngine;

namespace LayerZero.Presentation.Vfx
{
    public sealed class VfxInstance : MonoBehaviour, IVfxInstance
    {
        [SerializeField] private VfxKind _kind;
        [SerializeField] [Min(0f)] private float _fallbackLifetime = 2f;

        private Animator _animator;
        private IVfxAnimatorEvents _vfxAnimatorEvents;

        private Countdown _fallbackTimer;
        private bool _isPlaying;

        public event Action<IVfxInstance> Finished;

        public VfxKind Kind => _kind;

        private void Awake()
        {
            _animator = this.GetRequiredComponentInChildren<Animator>();
            _vfxAnimatorEvents = this.GetRequiredComponentInChildren<IVfxAnimatorEvents>();
        }

        private void OnEnable()
        {
            if (_vfxAnimatorEvents != null)
            {
                _vfxAnimatorEvents.VfxFinished += OnVfxFinished;
            }
        }

        private void OnDisable()
        {
            if (_vfxAnimatorEvents != null)
            {
                _vfxAnimatorEvents.VfxFinished -= OnVfxFinished;
            }
        }

        private void Update()
        {
            if (!_isPlaying || !_fallbackTimer.IsExpired)
            {
                return;
            }

            GameLog.Warning(this, $"'{name}' has not raised a finish event, releasing by timeout.");
            OnVfxFinished();
        }

        public void Play(Vector2 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
            gameObject.SetActive(true);

            _animator.Update(0f);

            _isPlaying = true;
            _fallbackTimer.Start(_fallbackLifetime);
        }

        public void Disable()
        {
            _isPlaying = false;
            gameObject.SetActive(false);
        }

        private void OnVfxFinished()
        {
            if (!_isPlaying)
            {
                return;
            }

            _isPlaying = false;
            Finished?.Invoke(this);
        }
    }
}
