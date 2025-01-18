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
    }
}