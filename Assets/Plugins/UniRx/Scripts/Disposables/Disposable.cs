using System;

namespace UniRx
{
    public static class Disposable
    {
        public static readonly IDisposable Empty = EmptyDisposable.Singleton;

        class EmptyDisposable : IDisposable
        {
            public static readonly EmptyDisposable Singleton = new();
            public void Dispose() { }
        }
    }
}