using System;
using System.Threading;
using UniRx.InternalUtil;

namespace UniRx
{
    public static class Observer
    {
        internal static IObserver<T> CreateSubscribeObserver<T>(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            return new Subscribe<T>(onNext, onError, onCompleted);
        }

        internal static IObserver<T> CreateSubscribeWithStateObserver<T, TState>(TState state, Action<T, TState> onNext, Action<Exception, TState> onError, Action<TState> onCompleted)
        {
            return new Subscribe<T, TState>(state, onNext, onError, onCompleted);
        }

        // same as AnonymousObserver...
        class Subscribe<T> : IObserver<T>
        {
            readonly Action<T> onNext;
            readonly Action<Exception> onError;
            readonly Action onCompleted;

            int isStopped = 0;

            public Subscribe(Action<T> onNext, Action<Exception> onError, Action onCompleted)
            {
                this.onNext = onNext;
                this.onError = onError;
                this.onCompleted = onCompleted;
            }

            public void OnNext(T value)
            {
                if (isStopped == 0)
                {
                    onNext(value);
                }
            }

            public void OnError(Exception error)
            {
                if (Interlocked.Increment(ref isStopped) == 1)
                {
                    onError(error);
                }
            }


            public void OnCompleted()
            {
                if (Interlocked.Increment(ref isStopped) == 1)
                {
                    onCompleted();
                }
            }
        }

        // with state
        class Subscribe<T, TState> : IObserver<T>
        {
            readonly TState state;
            readonly Action<T, TState> onNext;
            readonly Action<Exception, TState> onError;
            readonly Action<TState> onCompleted;

            int isStopped = 0;

            public Subscribe(TState state, Action<T, TState> onNext, Action<Exception, TState> onError, Action<TState> onCompleted)
            {
                this.state = state;
                this.onNext = onNext;
                this.onError = onError;
                this.onCompleted = onCompleted;
            }

            public void OnNext(T value)
            {
                if (isStopped == 0)
                {
                    onNext(value, state);
                }
            }

            public void OnError(Exception error)
            {
                if (Interlocked.Increment(ref isStopped) == 1)
                {
                    onError(error, state);
                }
            }


            public void OnCompleted()
            {
                if (Interlocked.Increment(ref isStopped) == 1)
                {
                    onCompleted(state);
                }
            }
        }
    }

    public static partial class ObservableExtensions
    {
        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext)
        {
            return source.Subscribe(Observer.CreateSubscribeObserver(onNext, Stubs.Throw, Stubs.Nop));
        }

        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            return source.Subscribe(Observer.CreateSubscribeObserver(onNext, onError, onCompleted));
        }

        public static IDisposable SubscribeWithState<T, TState>(this IObservable<T> source, TState state, Action<T, TState> onNext)
        {
            return source.Subscribe(Observer.CreateSubscribeWithStateObserver(state, onNext, Stubs<TState>.Throw, Stubs<TState>.Ignore));
        }
    }

    internal static class Stubs
    {
        public static readonly Action Nop = () => { };
        public static readonly Action<Exception> Throw = ex => { ex.Throw(); };
    }

    internal static class Stubs<T>
    {
        public static readonly Action<T> Ignore = (T t) => { };
        public static readonly Action<Exception, T> Throw = (ex, _) => { ex.Throw(); };
    }
}