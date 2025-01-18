#if CSHARP_7_OR_LATER || (UNITY_2018_3_OR_NEWER && (NET_STANDARD_2_0 || NET_4_6))
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#endif

using System;
using System.Threading;

namespace UniRx
{
    public interface IReadOnlyReactiveProperty<T> : IObservable<T>
    {
        T Value { get; }
    }

    internal interface IObserverLinkedList<T>
    {
        void UnsubscribeNode(ObserverNode<T> node);
    }

    internal sealed class ObserverNode<T> : IObserver<T>, IDisposable
    {
        readonly IObserver<T> observer;
        IObserverLinkedList<T> list;

        public ObserverNode<T> Previous { get; internal set; }
        public ObserverNode<T> Next { get; internal set; }

        public ObserverNode(IObserverLinkedList<T> list, IObserver<T> observer)
        {
            this.list = list;
            this.observer = observer;
        }

        public void OnNext(T value)
        {
            observer.OnNext(value);
        }

        public void OnError(Exception error)
        {
            observer.OnError(error);
        }

        public void OnCompleted()
        {
            observer.OnCompleted();
        }

        public void Dispose()
        {
            var sourceList = Interlocked.Exchange(ref list, null);
            if (sourceList != null)
            {
                sourceList.UnsubscribeNode(this);
                sourceList = null;
            }
        }
    }

    /// <summary>
    /// Lightweight property broker.
    /// </summary>
    public sealed class ReactiveProperty<T> : IReadOnlyReactiveProperty<T>, IDisposable, IObserverLinkedList<T> where T : IEquatable<T>
    {
        T value = default(T);
        ObserverNode<T> root;
        ObserverNode<T> last;
        bool isDisposed = false;

        public T Value
        {
            get => value;
            set
            {
                if (!this.value.Equals(value))
                {
                    SetValue(value);
                    if (isDisposed)
                        return;

                    RaiseOnNext(ref value);
                }
            }
        }

        public ReactiveProperty(T initialValue)
        {
            SetValue(initialValue);
        }

        void RaiseOnNext(ref T value)
        {
            var node = root;
            while (node != null)
            {
                node.OnNext(value);
                node = node.Next;
            }
        }

        private void SetValue(T value)
        {
            this.value = value;
        }

        IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
        {
            if (isDisposed)
            {
                observer.OnCompleted();
                return Disposable.Empty;
            }

            // raise latest value on subscribe
            observer.OnNext(value);

            // subscribe node, node as subscription.
            var next = new ObserverNode<T>(this, observer);
            if (root == null)
            {
                root = last = next;
            }
            else
            {
                last.Next = next;
                next.Previous = last;
                last = next;
            }
            return next;
        }

        void IObserverLinkedList<T>.UnsubscribeNode(ObserverNode<T> node)
        {
            if (node == root)
            {
                root = node.Next;
            }
            if (node == last)
            {
                last = node.Previous;
            }

            if (node.Previous != null)
            {
                node.Previous.Next = node.Next;
            }
            if (node.Next != null)
            {
                node.Next.Previous = node.Previous;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (isDisposed) return;

            var node = root;
            root = last = null;
            isDisposed = true;

            while (node != null)
            {
                node.OnCompleted();
                node = node.Next;
            }
        }

        public override string ToString()
        {
            return (value == null) ? "(null)" : value.ToString();
        }
    }
}