using System.Collections.Generic;
using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Common.Events;
using LayerZero.Characters.Common.Movement;
using LayerZero.Combat;
using LayerZero.Combat.Attack;
using LayerZero.Combat.Attack.Executors;
using LayerZero.Combat.Damage;
using LayerZero.Combat.Damage.Resistance;
using LayerZero.Core.Events;
using LayerZero.Core.StateMachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LayerZero.Characters.Common
{
    public abstract class CharacterLifetimeScope : LifetimeScope
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
