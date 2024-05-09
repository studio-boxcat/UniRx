using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
    internal class TakeLastObservable<T> : OperatorObservableBase<T>
    {
        readonly IObservable<T> source;

        // count
        readonly int count;

        public TakeLastObservable(IObservable<T> source, int count)
        {
            this.source = source;
            this.count = count;
        }

        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
        {
            {
                return new TakeLast(this, observer, cancel).Run();
            }
        }

        // count
        class TakeLast : OperatorObserverBase<T, T>
        {
            readonly TakeLastObservable<T> parent;
            readonly Queue<T> q;

            public TakeLast(TakeLastObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
            {
                this.parent = parent;
                this.q = new Queue<T>();
            }

            public IDisposable Run()
            {
                return parent.source.Subscribe(this);
            }

            public override void OnNext(T value)
            {
                q.Enqueue(value);
                if (q.Count > parent.count)
                {
                    q.Dequeue();
                }
            }

            public override void OnError(Exception error)
            {
                try { observer.OnError(error); } finally { Dispose(); }
            }

            public override void OnCompleted()
            {
                foreach (var item in q)
                {
                    observer.OnNext(item);
                }
                try { observer.OnCompleted(); } finally { Dispose(); }
            }
        }
    }
}