using System;

namespace UniRx.Operators
{
    internal class CatchObservable<T, TException> : OperatorObservableBase<T>
        where TException : Exception
    {
        readonly IObservable<T> source;
        readonly Func<TException, IObservable<T>> errorHandler;

        public CatchObservable(IObservable<T> source, Func<TException, IObservable<T>> errorHandler)
        {
            this.source = source;
            this.errorHandler = errorHandler;
        }

        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
        {
            return new Catch(this, observer, cancel).Run();
        }

        class Catch : OperatorObserverBase<T, T>
        {
            readonly CatchObservable<T, TException> parent;
            SingleAssignmentDisposable sourceSubscription;
            SingleAssignmentDisposable exceptionSubscription;

            public Catch(CatchObservable<T, TException> parent, IObserver<T> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                this.parent = parent;
            }

            public IDisposable Run()
            {
                this.sourceSubscription = new SingleAssignmentDisposable();
                this.exceptionSubscription = new SingleAssignmentDisposable();

                this.sourceSubscription.Disposable = parent.source.Subscribe(this);
                return StableCompositeDisposable.Create(sourceSubscription, exceptionSubscription);
            }

            public override void OnNext(T value)
            {
                base.observer.OnNext(value);
            }

            public override void OnError(Exception error)
            {
                var e = error as TException;
                if (e != null)
                {
                    IObservable<T> next;
                    try
                    {
                        if (parent.errorHandler == Stubs.CatchIgnore<T>)
                        {
                            next = Observable.Empty<T>(); // for avoid iOS AOT
                        }
                        else
                        {
                            next = parent.errorHandler(e);
                        }
                    }
                    catch (Exception ex)
                    {
                        try { observer.OnError(ex); } finally { Dispose(); };
                        return;
                    }

                    exceptionSubscription.Disposable = next.Subscribe(observer);
                }
                else
                {
                    try { observer.OnError(error); } finally { Dispose(); };
                    return;
                }
            }

            public override void OnCompleted()
            {
                try { observer.OnCompleted(); } finally { Dispose(); };
            }
        }
    }
}