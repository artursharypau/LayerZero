using System;
using LayerZero.Core.Events;
using UnityEngine;
using VContainer;

namespace LayerZero.Presentation.Vfx
{
    public sealed class AttackHitVfx : MonoBehaviour
    {
        [SerializeField] private GameObject _vfxPrefab;

        private IGameEventBus _eventBus;
        private IVfxService _vfxService;

        private IDisposable _subscription;

        [Inject]
        public void Construct(IGameEventBus gameEventBus, IVfxService vfxService)
        {
            _eventBus = gameEventBus;
            _vfxService = vfxService;
        }

        private void Start()
        {
            _subscription = _eventBus.SubscribeCallback<AttackHitEvent>(OnAttackHit);
        }

        private void OnDestroy()
        {
            _subscription?.Dispose();
        }

        private void OnAttackHit(AttackHitEvent e)
        {
            float angle = Mathf.Atan2(e.Direction.y, e.Direction.x) * Mathf.Rad2Deg;

            _vfxService.Play(_vfxPrefab, e.Point, Quaternion.Euler(0f, 0f, angle));
        }
    }
}
