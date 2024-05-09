using System;

namespace UniRx.Operators
{
    internal class ImmediateReturnObservable<T> : IObservable<T>, IOptimizedObservable<T>
    {
        readonly T value;

        public ImmediateReturnObservable(T value)
        {
            this.value = value;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            observer.OnNext(value);
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }

    internal class ImmutableReturnUnitObservable : IObservable<Unit>, IOptimizedObservable<Unit>
    {
        internal static ImmutableReturnUnitObservable Instance = new ImmutableReturnUnitObservable();

        ImmutableReturnUnitObservable()
        {

        }

        public IDisposable Subscribe(IObserver<Unit> observer)
        {
            observer.OnNext(Unit.Default);
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }

    internal class ImmutableReturnTrueObservable : IObservable<bool>, IOptimizedObservable<bool>
    {
        internal static ImmutableReturnTrueObservable Instance = new ImmutableReturnTrueObservable();

        ImmutableReturnTrueObservable()
        {

        }

        public IDisposable Subscribe(IObserver<bool> observer)
        {
            observer.OnNext(true);
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }

    internal class ImmutableReturnFalseObservable : IObservable<bool>, IOptimizedObservable<bool>
    {
        internal static ImmutableReturnFalseObservable Instance = new ImmutableReturnFalseObservable();

        ImmutableReturnFalseObservable()
        {

        }

        public IDisposable Subscribe(IObserver<bool> observer)
        {
            observer.OnNext(false);
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }

    internal class ImmutableReturnInt32Observable : IObservable<int>, IOptimizedObservable<int>
    {
        static ImmutableReturnInt32Observable[] Caches = new ImmutableReturnInt32Observable[]
        {
            new ImmutableReturnInt32Observable(-1),
            new ImmutableReturnInt32Observable(0),
            new ImmutableReturnInt32Observable(1),
            new ImmutableReturnInt32Observable(2),
            new ImmutableReturnInt32Observable(3),
            new ImmutableReturnInt32Observable(4),
            new ImmutableReturnInt32Observable(5),
            new ImmutableReturnInt32Observable(6),
            new ImmutableReturnInt32Observable(7),
            new ImmutableReturnInt32Observable(8),
            new ImmutableReturnInt32Observable(9),
        };

        public static IObservable<int> GetInt32Observable(int x)
        {
            if (-1 <= x && x <= 9)
            {
                return Caches[x + 1];
            }

            return new ImmediateReturnObservable<int>(x);
        }

        readonly int x;

        ImmutableReturnInt32Observable(int x)
        {
            this.x = x;
        }

        public IDisposable Subscribe(IObserver<int> observer)
        {
            observer.OnNext(x);
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }
}
