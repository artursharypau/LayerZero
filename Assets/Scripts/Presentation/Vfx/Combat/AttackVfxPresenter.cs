using System;
using LayerZero.Core.EventBus;
using LayerZero.Gameplay.Combat.Events;
using UnityEngine;
using VContainer.Unity;

namespace LayerZero.Presentation.Vfx.Combat
{
    internal sealed class AttackVfxPresenter : IStartable, IDisposable
    {
        private static readonly Quaternion MirroredRotation = Quaternion.Euler(0f, 180f, 0f);

        private readonly IGameEventBus _eventBus;
        private readonly IVfxService _vfxService;

        private IDisposable _subscription;

        public AttackVfxPresenter(IGameEventBus eventBus, IVfxService vfxService)
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
            VfxKind kind = e.IsCritical ? VfxKind.CriticalAttack : VfxKind.Attack;
            Quaternion rotation = e.Direction < 0f ? MirroredRotation : Quaternion.identity;

            _vfxService.Play(kind, e.Position, rotation);
        }
    }
}
