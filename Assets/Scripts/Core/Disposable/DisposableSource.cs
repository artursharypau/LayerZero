using System;

namespace LayerZero.Core.Disposable
{
    public sealed class DisposableSource : IDisposable
    {
        private readonly Action _action;

        public DisposableSource(Action action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void Dispose()
        {
            _action();
        }
    }
}
