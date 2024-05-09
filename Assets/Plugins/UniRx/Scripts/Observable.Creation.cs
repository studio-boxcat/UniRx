using System;
using UniRx.Operators;

namespace UniRx
{
    public static partial class Observable
    {
        /// <summary>
        /// Empty Observable. Returns only OnCompleted.
        /// </summary>
        public static IObservable<T> Empty<T>()
        {
            return ImmutableEmptyObservable<T>.Instance;
        }

        /// <summary>
        /// Non-Terminating Observable. It's no returns, never finish.
        /// </summary>
        public static IObservable<T> Never<T>()
        {
            return ImmutableNeverObservable<T>.Instance;
        }

        /// <summary>
        /// Non-Terminating Observable. It's no returns, never finish. witness is for type inference.
        /// </summary>
        public static IObservable<T> Never<T>(T witness)
        {
            return ImmutableNeverObservable<T>.Instance;
        }

        /// <summary>
        /// Return single sequence Immediately, optimized for Unit(no allocate memory).
        /// </summary>
        public static IObservable<Unit> Return(Unit value)
        {
            return ImmutableReturnUnitObservable.Instance;
        }

        /// <summary>
        /// Same as Observable.Return(Unit.Default); but no allocate memory.
        /// </summary>
        public static IObservable<Unit> ReturnUnit()
        {
            return ImmutableReturnUnitObservable.Instance;
        }
    }
}