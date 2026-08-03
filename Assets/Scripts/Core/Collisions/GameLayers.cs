using UnityEngine;

namespace LayerZero.Core.Collisions
{
    /// <summary>
    /// Project layer registry. One place that knows the layer names, so no gameplay code
    /// ever calls <c>LayerMask.GetMask("...")</c> with a magic string.
    /// </summary>
    public static class GameLayers
    {
        public const string GroundName = "Ground";
        public const string PlayerName = "Player";
        public const string EnemyName = "Enemy";

        public static readonly LayerMask Ground = LayerMask.GetMask(GroundName);
        public static readonly LayerMask Player = LayerMask.GetMask(PlayerName);
        public static readonly LayerMask Enemy = LayerMask.GetMask(EnemyName);

        public static LayerMask Characters => Player | Enemy;
    }
}
