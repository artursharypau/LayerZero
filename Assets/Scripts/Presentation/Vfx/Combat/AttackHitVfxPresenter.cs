using System;
using LayerZero.Core.EventBus;
using LayerZero.Core.EventBus.Events;
using UnityEngine;
using VContainer.Unity;

namespace LayerZero.Presentation.Vfx.Combat
{
    internal sealed class AttackHitVfxPresenter : IStartable, IDisposable
    {
        private readonly IGameEventBus _eventBus;
        private readonly IVfxService _vfxService;

        private IDisposable _subscription;

        public AttackHitVfxPresenter(IGameEventBus eventBus, IVfxService vfxService)
        {
            _eventBus = eventBus;
            _vfxService = vfxService;
        }

        public void Start()
        {
            _subscription = _eventBus.SubscribeCallback<AttackHitEvent>(OnAttackHit);
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }

        private void OnAttackHit(AttackHitEvent e)
        {
            _vfxService.Play(VfxKind.AttackHit, e.Position, Quaternion.identity);
        }
    }
}
