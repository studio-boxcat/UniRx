using System;
using UniRx.Operators;

namespace UniRx
{
    public static partial class Observable
    {
        public static IObservable<TAccumulate> Scan<TSource, TAccumulate>(this IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            return new ScanObservable<TSource, TAccumulate>(source, seed, accumulator);
        }
    }
}
