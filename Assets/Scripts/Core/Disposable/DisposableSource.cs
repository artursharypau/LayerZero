using System;

namespace LayerZero.Core.Core.Disposable
{
    public class DisposableSource : IDisposable
    {
        private readonly Action _action;

        public DisposableSource(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            _action();
        }
    }
}
