using UnityEngine;

namespace LayerZero.Gameplay.Collisions
{
    internal static class GameLayers
    {
        public const string GroundName = "Ground";
        public const string PlayerName = "Player";
        public const string EnemyName = "Enemy";

        public static readonly LayerMask Ground = LayerMask.GetMask(GroundName);
        public static readonly LayerMask Player = LayerMask.GetMask(PlayerName);
        public static readonly LayerMask Enemy = LayerMask.GetMask(EnemyName);
    }
}
