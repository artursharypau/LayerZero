using System.Collections.Generic;
using LayerZero.Core.StateMachine;
using LayerZero.Gameplay.Characters.Common.Animation;
using LayerZero.Gameplay.Characters.Common.Movement;
using LayerZero.Gameplay.Combat;
using LayerZero.Gameplay.Combat.Attack;
using LayerZero.Gameplay.Combat.Attack.Executors;
using LayerZero.Gameplay.Combat.Damage;
using LayerZero.Gameplay.Combat.Damage.Protections;
using LayerZero.Gameplay.Stats;
using LayerZero.Gameplay.Stats.Health;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LayerZero.Gameplay.Characters.Common
{
    internal abstract class CharacterLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<DamageProtection>(Lifetime.Scoped).As<IDamageProtection>();
            builder.Register<Health>(Lifetime.Scoped)
                .As<IHealth>()
                .As<IDamageable>();
            builder.Register<StateMachine>(Lifetime.Scoped);
            builder.Register<CharacterAnimator>(Lifetime.Scoped);
            builder.Register<AnimatorStateBinder>(Lifetime.Scoped);

            builder.Register<DamageResolver>(Lifetime.Scoped).As<IDamageResolver>();

            IAttackExecutor[] executors = InjectInHierarchy<IAttackExecutor>(builder);
            builder.RegisterInstance(executors).As<IReadOnlyList<IAttackExecutor>>();

            InjectInHierarchy<ICharacterView>(builder);

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
                        .As<IMovement2D>()
                        .As<IPositioned>();

                    components.AddInHierarchy<DamageReceiver>().As<IDamageReceiver>();
                    components.AddInHierarchy<CombatSystem>().As<ICombatSystem>();
                    components.AddInHierarchy<StatsSystem>().As<IStatsSystem>();
                });
        }

        private T[] InjectInHierarchy<T>(IContainerBuilder builder)
        {
            T[] targets = GetComponentsInChildren<T>(true);

            builder.RegisterBuildCallback(container =>
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    container.Inject(targets[i]);
                }
            });

            return targets;
        }
    }
}
