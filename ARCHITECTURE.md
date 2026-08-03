# LayerZero — архитектура

Код разбит на четыре сборки (asmdef). Зависимости идут строго в одну сторону,
поэтому ядро можно вынести в отдельный пакет и переиспользовать в другом проекте.

```
LayerZero.Core          не знает про игру вообще
   ^          ^
   |          |
LayerZero.Combat    LayerZero.Environment
   ^
   |
LayerZero.Characters  ->  LayerZero.GeneratedInput
```

| Сборка | Папка | Что внутри |
|---|---|---|
| `LayerZero.Core` | `Assets/Scripts/Core` | FSM, таймеры, буферизованный ввод, рейкасты, пул, логирование |
| `LayerZero.Combat` | `Assets/Scripts/Combat` | урон, здоровье, резисты, исполнители атак, снаряды |
| `LayerZero.Characters` | `Assets/Scripts/Characters` | база персонажа, модули, состояния, игрок, враги |
| `LayerZero.Environment` | `Assets/Scripts/Environment` | параллакс и прочее окружение |
| `LayerZero.GeneratedInput` | `Assets/InputSystem` | сгенерированный ассет Input System |

## Ядро: машина состояний по типам

Состояния регистрируются и запрашиваются по `System.Type`, а не по числовому id:

```csharp
States.Register(new EnemyChaseState(this));
ChangeTo<EnemyChaseState>();
```

`StateRegistry` индексирует состояние дважды — под собственным типом и под каждым
типом-предком. Именно это даёт расширяемость: общий код просит «состояние погони»,
а конкретный враг мог зарегистрировать `RangedEnemyChaseState : EnemyChaseState`
и получит своё поведение без единой правки в общих состояниях.

Точное совпадение типа всегда выигрывает у алиаса; неоднозначный алиас падает
с понятной ошибкой, а не выбирает случайное состояние.

Переходы бывают отложенные (по умолчанию — применяются в начале следующего шага,
чтобы шаг оставался консистентным) и немедленные (`StateTransitionMode.Immediate`)
для реакций, которые нельзя откладывать: урон и смерть.

## Персонаж = композиция, а не наследование поведения

`Character` — только точка сборки. Он находит компоненты, крутит список модулей
и машину состояний, и маршрутизирует урон/смерть. Он не содержит игровой логики.

Наследник делает ровно две вещи:

```csharp
protected override void Compose()      // какие модули и состояния у персонажа есть
protected override void OnStarted()    // с какого состояния начинаем
```

Три вида кирпичиков:

* **Модули** (`CharacterModule`) — то, что живёт каждый кадр, но не является состоянием:
  ввод, способности, восприятие. У них есть свой жизненный цикл
  (`OnInitialize/Enable/Tick/FixedTick/Disable/Dispose/DrawGizmos`).
* **Состояния** (`CharacterState<T>`) — что персонаж делает прямо сейчас.
* **Конфиги** (`ScriptableObject`) — все числа. На префабе остаются только ссылки
  и сценовые Transform'ы.

## Бой: доставка урона отделена от решения атаковать

`CombatSystem` — фасад. На персонаже висит по одному `IAttackExecutor` на каждый
`AttackKind`, а состояние лишь «взводит» атаку:

```csharp
Owner.Combat.Arm(attackDefinition);   // в Enter состояния
```

Сам удар происходит на анимационном событии `AttackHit`, которое `CombatSystem`
переадресует нужному исполнителю. Состояние не знает, будет это оверлап хитбокса
(`MeleeAttackExecutor`) или выстрел снарядом (`ProjectileAttackExecutor`).

Проверка «дотянусь ли» тоже принадлежит исполнителю: `Combat.IsInRange(kind, target)`.
Поэтому одно и то же состояние погони работает и для мечника, и для лучника.

## Как добавить нового врага

Общее поведение — восприятие, патруль, погоня, реакция на урон, смерть — уже есть
в `EnemyController`. Архетип задаёт только бой:

```csharp
protected abstract void RegisterCombatBehaviour();
```

**Ближний бой** (скелет, и всё похожее):

1. `public sealed class GhoulController : MeleeEnemyController { }`
2. Ассет `EnemyConfig` (Create → LayerZero → Characters → Enemy Config).
3. Префаб: `Rigidbody2D`, `CharacterMovement2D`, `Health`, `DamageReceiver`,
   `CombatSystem`, `MeleeAttackExecutor`, контроллер; `AnimatorEventRelay` рядом с `Animator`.

**Дальний бой** (лучник):

1. `public sealed class ArcherController : RangedEnemyController { }` — уже есть.
2. Ассет `RangedEnemyConfig` — добавляет секцию дистанции боя и перезарядки.
3. На префабе вместо `MeleeAttackExecutor` ставится `ProjectileAttackExecutor`
   (префаб стрелы, точка вылета, скорость, min/max дистанция).
4. В `AttackDefinition` конфига выставляется `Kind = Ranged`.

Ни одна строчка в `EnemyController`, в общих состояниях и в бою при этом не меняется.

Если нужен свой шаг поведения — наследуемся от общего состояния и регистрируем
наследника вместо базового; остальные состояния продолжают просить базовый тип
и прозрачно получают новый (`RangedEnemyChaseState` — рабочий пример).

Аниматор нового врага должен объявлять те же параметры: `idle`, `patrol`, `chase`,
`attack`, `hurt`, `velocityX`, `moveAnimMultiplier`, `chaseMoveAnimMultiplier`;
события клипов — `TriggerAttackHit` и `TriggerAttackFinished`.

## Правила, которые стоит держать

* Тюнинг — в `ScriptableObject`, на префабе только ссылки и сценовые объекты.
* Состояния не трогают `Animator`, `Rigidbody2D` и `Physics2D` напрямую —
  только через `CharacterAnimator`, `IMovable` и `CombatSystem`.
* Всё, что не зависит от игры, живёт в `LayerZero.Core` и не ссылается ни на что.
* Новый архетип — это подкласс и ассет конфига, а не новое поле в общем контроллере.
