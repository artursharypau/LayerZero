using System.Collections.Generic;
using LayerZero.Core.StateMachine;
using LayerZero.Gameplay.Characters.Common.Animation;
using LayerZero.Gameplay.Characters.Common.EventBus;
using LayerZero.Gameplay.Characters.Common.Movement;
using LayerZero.Gameplay.Combat;
using LayerZero.Gameplay.Combat.Attack;
using LayerZero.Gameplay.Combat.Attack.Executors;
using LayerZero.Gameplay.Combat.Damage.Processing;
using LayerZero.Gameplay.Combat.Damage.Protections;
using LayerZero.Gameplay.Combat.Elements;
using LayerZero.Gameplay.Stats;
using LayerZero.Gameplay.Stats.Config;
using LayerZero.Gameplay.Stats.Health;
using LayerZero.Gameplay.StatusEffects;
using LayerZero.Gameplay.StatusEffects.Effects.Damage;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LayerZero.Gameplay.Characters.Common
{
    internal abstract class CharacterLifetimeScope : LifetimeScope
    {
        protected abstract StatsConfig Stats { get; }

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterEventBus(builder);
            RegisterStateMachine(builder);
            RegisterAnimation(builder);
            RegisterMovement(builder);
            RegisterStats(builder);
            RegisterCombat(builder);
            RegisterStatusEffects(builder);
            RegisterViews(builder);
        }

        private static void RegisterEventBus(IContainerBuilder builder)
        {
            builder.Register<CharacterEventBus>(Lifetime.Scoped).As<ICharacterEventBus>();
        }

        private static void RegisterStateMachine(IContainerBuilder builder)
        {
            builder.Register<StateMachine>(Lifetime.Scoped);
        }

        private void RegisterAnimation(IContainerBuilder builder)
        {
            builder.Register<CharacterAnimator>(Lifetime.Scoped);
            builder.Register<AnimatorStateBinder>(Lifetime.Scoped);

            builder.UseComponents(
                transform,
                components =>
                {
                    components.AddInHierarchy<Animator>();
                    components.AddInHierarchy<AnimatorEvents>()
                        .As<IAnimatorEvents>()
                        .As<IAttackEvents>()
                        .As<IAttackParryWindowEvents>();
                });
        }

        private void RegisterMovement(IContainerBuilder builder)
        {
            builder.UseComponents(
                transform,
                components =>
                {
                    components.AddInHierarchy<CharacterMovement2D>()
                        .AsSelf()
                        .As<IMovement2D>()
                        .As<IPositioned>();
                });
        }

        private void RegisterStats(IContainerBuilder builder)
        {
            builder.Register<Health>(Lifetime.Scoped)
                .As<IHealth>()
                .As<IDamageable>();

            builder.Register<StatsSystem>(Lifetime.Scoped)
                .As<IStatsSystem>()
                .WithParameter(Stats);
        }

        private void RegisterCombat(IContainerBuilder builder)
        {
            builder.Register<ElementalAffinity>(Lifetime.Scoped).As<IElementalAffinity>();

            IAttackExecutor[] executors = InjectInHierarchy<IAttackExecutor>(builder);
            builder.RegisterInstance(executors).As<IReadOnlyList<IAttackExecutor>>();

            builder.UseComponents(transform, components => components.AddInHierarchy<CombatSystem>().As<ICombatSystem>());

            builder.Register<DamageProtection>(Lifetime.Scoped).As<IDamageProtection>();
            builder.Register<DamageResolver>(Lifetime.Scoped).As<IDamageResolver>();
            builder.Register<DamageReceiver>(Lifetime.Scoped).As<IDamageReceiver>();

            builder.UseComponents(transform, components => components.AddInHierarchy<DamagePipeline>().As<IDamagePipeline>());
        }

        private static void RegisterStatusEffects(IContainerBuilder builder)
        {
            builder.Register<DamageOverTimeEffectHandler>(Lifetime.Scoped).As<IStatusEffectHandler>();
            builder.RegisterEntryPoint<StatusEffectsSystem>(Lifetime.Scoped);
        }

        private void RegisterViews(IContainerBuilder builder)
        {
            InjectInHierarchy<ICharacterView>(builder);
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
