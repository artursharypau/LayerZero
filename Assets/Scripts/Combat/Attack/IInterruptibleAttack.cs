using LayerZero.Combat.Damage;

namespace LayerZero.Combat.Attack
{
    public interface IInterruptibleAttack
    {
        bool TryInterrupt(DamageInfo damageInfo);
    }
}
