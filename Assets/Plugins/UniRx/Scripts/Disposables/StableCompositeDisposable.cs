using System;
using System.Threading;

namespace UniRx
{
    /// <summary>
    /// Represents a group of disposable resources that are disposed together.
    /// </summary>
    public abstract class StableCompositeDisposable : IDisposable
    {
        /// <summary>
        /// Creates a new group containing two disposable resources that are disposed together.
        /// </summary>
        /// <param name="disposable1">The first disposable resoruce to add to the group.</param>
        /// <param name="disposable2">The second disposable resoruce to add to the group.</param>
        /// <returns>Group of disposable resources that are disposed together.</returns>
        public static IDisposable Create(IDisposable disposable1, IDisposable disposable2)
        {
            if (disposable1 == null) throw new ArgumentNullException("disposable1");
            if (disposable2 == null) throw new ArgumentNullException("disposable2");

            return new Binary(disposable1, disposable2);
        }

        /// <summary>
        /// Disposes all disposables in the group.
        /// </summary>
        public abstract void Dispose();

        class Binary : StableCompositeDisposable
        {
            int disposedCallCount = -1;
            private volatile IDisposable _disposable1;
            private volatile IDisposable _disposable2;

            public Binary(IDisposable disposable1, IDisposable disposable2)
            {
                _disposable1 = disposable1;
                _disposable2 = disposable2;
            }

            public override void Dispose()
            {
                if (Interlocked.Increment(ref disposedCallCount) == 0)
                {
                    _disposable1.Dispose();
                    _disposable2.Dispose();
                }
            }
        }
    }
}