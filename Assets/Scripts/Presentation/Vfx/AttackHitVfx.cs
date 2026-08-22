using System;
using LayerZero.Core.Events;
using UnityEngine;
using VContainer;

namespace LayerZero.Presentation.Presentation.Vfx
{
    public class AttackHitVfx : MonoBehaviour
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

        private void OnEnable()
        {
            _subscription = _eventBus.SubscribeCallback<AttackHitEvent>(OnAttackHit);
        }

        private void OnDisable()
        {
            _subscription.Dispose();
        }

        private void OnAttackHit(AttackHitEvent e)
        {
            _vfxService.Play(_vfxPrefab, e.Target);
        }
    }
}
