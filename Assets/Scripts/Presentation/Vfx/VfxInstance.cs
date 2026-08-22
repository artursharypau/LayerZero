using System;
using LayerZero.Core.Extensions;
using UnityEngine;

namespace LayerZero.Presentation.Vfx
{
    public class VfxInstance : MonoBehaviour, IVfxInstance
    {
        [SerializeField] private VfxKind _kind;

        private IVfxAnimatorEvents _vfxAnimatorEvents;

        public event Action<IVfxInstance> Finished;

        public VfxKind Kind => _kind;

        private void Awake()
        {
            _vfxAnimatorEvents = this.GetRequiredComponentInChildren<IVfxAnimatorEvents>();
        }

        private void OnEnable()
        {
            if (_vfxAnimatorEvents != null)
            {
                _vfxAnimatorEvents.VfxFinished += OnFinished;
            }
        }

        private void OnDisable()
        {
            if (_vfxAnimatorEvents != null)
            {
                _vfxAnimatorEvents.VfxFinished -= OnFinished;
            }
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void Play()
        {
            gameObject.SetActive(true);
        }

        private void OnFinished()
        {
            Finished?.Invoke(this);
        }
    }
}
