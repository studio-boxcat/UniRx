using System;

namespace UniRx.Operators
{
    internal class WhereObservable<T> : OperatorObservableBase<T>
    {
        readonly IObservable<T> source;
        readonly Func<T, bool> predicate;

        public WhereObservable(IObservable<T> source, Func<T, bool> predicate)
        {
            this.source = source;
            this.predicate = predicate;
        }

        // Optimize for .Where().Select()

        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
        {
            return source.Subscribe(new Where(this, observer, cancel));
        }

        class Where : OperatorObserverBase<T, T>
        {
            readonly WhereObservable<T> parent;

            public Where(WhereObservable<T> parent, IObserver<T> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                this.parent = parent;
            }

            public override void OnNext(T value)
            {
                var isPassed = false;
                try
                {
                    isPassed = parent.predicate(value);
                }
                catch (Exception ex)
                {
                    try { observer.OnError(ex); } finally { Dispose(); }
                    return;
                }

                if (isPassed)
                {
                    observer.OnNext(value);
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