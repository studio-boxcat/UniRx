using System;
using UniRx.Operators;

namespace UniRx
{
    public static partial class Observable
    {
        public static IObservable<T> AsObservable<T>(this IObservable<T> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            // optimize, don't double wrap
            if (source is UniRx.Operators.AsObservableObservable<T>)
            {
                return source;
            }

            return new AsObservableObservable<T>(source);
        }
    }
}