using System;

namespace UniRx.Operators
{
    internal class TakeObservable<T> : OperatorObservableBase<T>
    {
        readonly IObservable<T> source;
        readonly int count;

        public TakeObservable(IObservable<T> source, int count)
        {
            this.source = source;
            this.count = count;
        }

        // optimize combiner

        public IObservable<T> Combine(int count)
        {
            // xs = 6
            // xs.Take(5) = 5         | xs.Take(3) = 3
            // xs.Take(5).Take(3) = 3 | xs.Take(3).Take(5) = 3

            // use minimum one
            return (this.count <= count)
                ? this
                : new TakeObservable<T>(source, count);
        }

        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
        {
            {
                return source.Subscribe(new Take(this, observer, cancel));
            }
        }

        class Take : OperatorObserverBase<T, T>
        {
            int rest;

            public Take(TakeObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
            {
                this.rest = parent.count;
            }

            public override void OnNext(T value)
            {
                if (rest > 0)
                {
                    rest -= 1;
                    base.observer.OnNext(value);
                    if (rest == 0)
                    {
                        try { observer.OnCompleted(); } finally { Dispose(); };
                    }
                }
            }

            public override void OnError(Exception error)
            {
                try { observer.OnError(error); } finally { Dispose(); }
            }

            public override void OnCompleted()
            {
                try { observer.OnCompleted(); } finally { Dispose(); }
            }
        }
    }
}