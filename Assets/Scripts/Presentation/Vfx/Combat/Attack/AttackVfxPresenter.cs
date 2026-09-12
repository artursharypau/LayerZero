using System;
using LayerZero.Core.Diagnostics;
using LayerZero.Core.EventBus;
using LayerZero.Gameplay.Characters.Common;
using LayerZero.Gameplay.Characters.Common.EventBus;
using LayerZero.Gameplay.Combat.Elements;
using LayerZero.Gameplay.Combat.Events;
using LayerZero.Presentation.Vfx.Combat.Elements;
using LayerZero.Presentation.Vfx.Pooling;
using UnityEngine;
using VContainer;

namespace LayerZero.Presentation.Vfx.Combat.Attack
{
    internal sealed class AttackVfxPresenter : MonoBehaviour, ICharacterView
    {
        private static readonly Quaternion MirroredRotation = Quaternion.Euler(0f, 180f, 0f);

        [SerializeField] private AttackVfxInstance _prefab;
        [SerializeField] private ElementPalette _elementPalette;
        [SerializeField] [Min(0)] private int _prewarmCount = 4;

        private ICharacterEventBus _eventBus;

        private IVfxPool _pool;
        private Transform _root;
        private IDisposable _subscription;

        [Inject]
        public void Construct(ICharacterEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        private void Awake()
        {
            if (!_prefab)
            {
                GameLog.Error(this, $"'{name}' has no attack vfx prefab assigned.");
                return;
            }

            _root = new GameObject($"[Vfx] {name}").transform;
            _pool = new VfxPool(_prefab, _root, _prewarmCount);
        }

        private void OnEnable()
        {
            if (_pool != null)
            {
                _subscription = _eventBus.SubscribeCallback<AttackHitEvent>(OnAttackHit);
            }
        }

        private void OnDisable()
        {
            _subscription?.Dispose();
            _subscription = null;
        }

        private void OnDestroy()
        {
            _pool?.Dispose();

            if (_root)
            {
                Destroy(_root.gameObject);
            }
        }

        private void OnAttackHit(AttackHitEvent e)
        {
            Quaternion rotation = e.Direction < 0f ? MirroredRotation : Quaternion.identity;

            IVfxInstance instance = _pool.Get();

            instance.Finished += OnVfxFinished;
            instance.Play(e.Position, rotation, ResolveTint(e.Element));
        }

        private Color? ResolveTint(ElementKind element)
        {
            if (_elementPalette && _elementPalette.TryGetColor(element, out Color color))
            {
                return color;
            }

            return null;
        }

        private void OnVfxFinished(IVfxInstance instance)
        {
            instance.Finished -= OnVfxFinished;
            instance.Disable();

            _pool.Release(instance);
        }
    }
}
