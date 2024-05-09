using System;

namespace UniRx.Operators
{
    // implements note : all field must be readonly.
    public abstract class OperatorObservableBase<T> : IObservable<T>, IOptimizedObservable<T>
    {
        public IDisposable Subscribe(IObserver<T> observer)
        {
            var subscription = new SingleAssignmentDisposable();

            {
                subscription.Disposable = SubscribeCore(observer, subscription);
            }

            return subscription;
        }

        protected abstract IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel);
    }
}