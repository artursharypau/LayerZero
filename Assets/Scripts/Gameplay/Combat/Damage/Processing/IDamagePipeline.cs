namespace LayerZero.Gameplay.Combat.Damage.Processing
{
    internal interface IDamagePipeline
    {
        bool Process(in DamagePayload payload);
    }
}
