using UnityEngine;

namespace LayerZero.Core.Diagnostics
{
    public static class GameLog
    {
        public static void Info(object context, string message)
        {
            Debug.unityLogger.Log(Tag(context), message, context as Object);
        }

        public static void Warning(object context, string message)
        {
            Debug.unityLogger.LogWarning(Tag(context), message, context as Object);
        }

        public static void Error(object context, string message)
        {
            Debug.unityLogger.LogError(Tag(context), message, context as Object);
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
