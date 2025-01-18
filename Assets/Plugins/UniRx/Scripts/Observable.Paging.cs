using System;
using UniRx.Operators;

namespace UniRx
{
    // Take, Skip, etc..
    public static partial class Observable
    {
        // first, last, single

        public static IObservable<T> First<T>(this IObservable<T> source)
        {
            return new FirstObservable<T>(source, false);
        }
    }
}