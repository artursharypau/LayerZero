using System.Collections.Generic;
using LayerZero.Core.EventBus;
using LayerZero.Core.StateMachine;
using LayerZero.Gameplay.Characters.Common.Animation;
using LayerZero.Gameplay.Characters.Common.EventBus;
using LayerZero.Gameplay.Characters.Common.Movement;
using LayerZero.Gameplay.Combat;
using LayerZero.Gameplay.Combat.Attack;
using LayerZero.Gameplay.Combat.Attack.Executors;
using LayerZero.Gameplay.Combat.Damage;
using LayerZero.Gameplay.Combat.Damage.Resistance;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LayerZero.Gameplay.Characters.Common
{
    internal abstract class CharacterLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<CharacterEventBus>(Lifetime.Scoped)
                .As<ICharacterEventBus>()
                .As<IEventBus>();
            builder.Register<DamageResistances>(Lifetime.Scoped).As<IDamageResistances>();
            builder.Register<StateMachine>(Lifetime.Scoped);
            builder.Register<CharacterAnimator>(Lifetime.Scoped);
            builder.Register<AnimatorStateBinder>(Lifetime.Scoped);

            RegisterAttackExecutors(builder);

            builder.UseComponents(
                transform,
                components =>
                {
                    components.AddInHierarchy<Animator>();
                    components.AddInHierarchy<AnimatorEvents>()
                        .As<IAnimatorEvents>()
                        .As<IAttackEvents>()
                        .As<IAttackParryWindowEvents>();

                    components.AddInHierarchy<CharacterMovement2D>()
                        .AsSelf()
                        .As<IMovement2D>();

                    components.AddInHierarchy<Health>().As<IDamageable>();
                    components.AddInHierarchy<DamageReceiver>().As<IDamageReceiver>();
                    components.AddInHierarchy<CombatSystem>().As<ICombatSystem>();
                });
        }

        private void RegisterAttackExecutors(IContainerBuilder builder)
        {
            IAttackExecutor[] executors = GetComponentsInChildren<IAttackExecutor>(true);

            builder.RegisterInstance(executors).As<IReadOnlyList<IAttackExecutor>>();
            builder.RegisterBuildCallback(container =>
            {
                for (int i = 0; i < executors.Length; i++)
                {
                    container.Inject(executors[i]);
                }
            });
        }
    }
}
