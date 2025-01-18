using System;
using Cysharp.Threading.Tasks;

namespace UniRx
{
    public class ReactiveCommand<T> : IObservable<T>, IDisposable
    {
        readonly Subject<T> trigger = new Subject<T>();

        public bool IsDisposed { get; private set; }

        /// <summary>Push parameter to subscribers when CanExecute.</summary>
        public bool Execute(T parameter)
        {
            trigger.OnNext(parameter);
            return true;
        }

        /// <summary>Subscribe execute.</summary>
        public IDisposable Subscribe(IObserver<T> observer)
        {
            return trigger.Subscribe(observer);
        }

        /// <summary>
        /// Stop all subscription and lock CanExecute is false.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed) return;

            IsDisposed = true;
            trigger.OnCompleted();
            trigger.Dispose();
        }
    }

    public static class ReactiveCommandExtensions
    {
        public static UniTask<T> WaitUntilExecuteAsync<T>(this ReactiveCommand<T> source)
        {
            var tcs = new UniTaskCompletionSource<T>();

            var disposable = new SingleAssignmentDisposable();
            disposable.Disposable = source.Subscribe(x =>
            {
                disposable.Dispose(); // finish subscription.
                tcs.TrySetResult(x);
            }, ex => tcs.TrySetException(ex), () => tcs.TrySetCanceled());

            return tcs.Task;
        }
    }
}
