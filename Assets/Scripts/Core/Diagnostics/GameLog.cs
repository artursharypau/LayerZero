using UnityEngine;

namespace LayerZero.Core.Diagnostics
{
    /// <summary>
    /// Single entry point for gameplay logging.
    /// Keeps the "Type.Member" tag format consistent and gives us one place to
    /// strip or redirect logs later (build defines, in-game console, analytics).
    /// </summary>
    public static class GameLog
    {
        public static void Info(object context, string message)
        {
            Debug.unityLogger.Log(Tag(context), message);
        }

        public static void Warning(object context, string message)
        {
            Debug.unityLogger.LogWarning(Tag(context), message);
        }

        public static void Error(object context, string message)
        {
            Debug.unityLogger.LogError(Tag(context), message);
        }

        private static string Tag(object context)
        {
            return context switch
            {
                null => "?",
                string text => text,
                _ => context.GetType().Name
            };
        }
    }
}
