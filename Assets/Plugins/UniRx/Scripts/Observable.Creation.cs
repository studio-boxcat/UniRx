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
        /// Return single sequence Immediately, optimized for Unit(no allocate memory).
        /// </summary>
        public static IObservable<Unit> Return(Unit value)
        {
            return ImmutableReturnUnitObservable.Instance;
        }
    }
}