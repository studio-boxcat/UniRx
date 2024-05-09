using System;

namespace UniRx.Operators
{
    internal class ImmutableEmptyObservable<T> : IObservable<T>, IOptimizedObservable<T>
    {
        internal static ImmutableEmptyObservable<T> Instance = new ImmutableEmptyObservable<T>();

        ImmutableEmptyObservable()
        {
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }
}