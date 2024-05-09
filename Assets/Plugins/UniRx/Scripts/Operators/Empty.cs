using System;

namespace UniRx.Operators
{
    internal class ImmutableEmptyObservable<T> : IObservable<T>
    {
        internal static readonly ImmutableEmptyObservable<T> Instance = new();

        public IDisposable Subscribe(IObserver<T> observer)
        {
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }
}