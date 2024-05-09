using System;

namespace UniRx.Operators
{
    internal class SkipObservable<T> : OperatorObservableBase<T>
    {
        readonly IObservable<T> source;
        readonly int count;

        public SkipObservable(IObservable<T> source, int count)
        {
            this.source = source;
            this.count = count;
        }

        // optimize combiner

        public IObservable<T> Combine(int count)
        {
            // use sum
            // xs = 6
            // xs.Skip(2) = 4
            // xs.Skip(2).Skip(3) = 1

            return new SkipObservable<T>(source, this.count + count);
        }

        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
        {
            {
                return source.Subscribe(new Skip(this, observer, cancel));
            }
        }

        class Skip : OperatorObserverBase<T, T>
        {
            int remaining;

            public Skip(SkipObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
            {
                this.remaining = parent.count;
            }

            public override void OnNext(T value)
            {
                if (remaining <= 0)
                {
                    base.observer.OnNext(value);
                }
                else
                {
                    remaining--;
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