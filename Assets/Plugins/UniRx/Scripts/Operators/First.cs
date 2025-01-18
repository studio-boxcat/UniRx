using System;

namespace UniRx.Operators
{
    internal class FirstObservable<T> : OperatorObservableBase<T>
    {
        readonly IObservable<T> source;
        readonly bool useDefault;

        public FirstObservable(IObservable<T> source, bool useDefault)
        {
            this.source = source;
            this.useDefault = useDefault;
        }

        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
        {
            return source.Subscribe(new First(this, observer, cancel));
        }

        class First : OperatorObserverBase<T, T>
        {
            readonly FirstObservable<T> parent;
            bool notPublished;

            public First(FirstObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
            {
                this.parent = parent;
                this.notPublished = true;
            }

            public override void OnNext(T value)
            {
                if (notPublished)
                {
                    notPublished = false;
                    observer.OnNext(value);
                    try { observer.OnCompleted(); }
                    finally { Dispose(); }
                    return;
                }
            }

            public override void OnError(Exception error)
            {
                try { observer.OnError(error); }
                finally { Dispose(); }
            }

            public override void OnCompleted()
            {
                if (parent.useDefault)
                {
                    if (notPublished)
                    {
                        observer.OnNext(default(T));
                    }
                    try { observer.OnCompleted(); }
                    finally { Dispose(); }
                }
                else
                {
                    if (notPublished)
                    {
                        try { observer.OnError(new InvalidOperationException("sequence is empty")); }
                        finally { Dispose(); }
                    }
                    else
                    {
                        try { observer.OnCompleted(); }
                        finally { Dispose(); }
                    }
                }
            }
        }
    }
}