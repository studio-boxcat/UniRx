using System;
using UniRx.Operators;

namespace UniRx
{
    // Take, Skip, etc..
    public static partial class Observable
    {
        public static IObservable<T> Take<T>(this IObservable<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (count < 0) throw new ArgumentOutOfRangeException("count");

            if (count == 0) return Empty<T>();

            // optimize .Take(count).Take(count)
            var take = source as TakeObservable<T>;
            if (take != null)
            {
                return take.Combine(count);
            }

            return new TakeObservable<T>(source, count);
        }

        public static IObservable<T> TakeWhile<T>(this IObservable<T> source, Func<T, bool> predicate)
        {
            return new TakeWhileObservable<T>(source, predicate);
        }

        public static IObservable<T> TakeWhile<T>(this IObservable<T> source, Func<T, int, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");

            return new TakeWhileObservable<T>(source, predicate);
        }

        public static IObservable<T> TakeUntil<T, TOther>(this IObservable<T> source, IObservable<TOther> other)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (other == null) throw new ArgumentNullException("other");

            return new TakeUntilObservable<T, TOther>(source, other);
        }

        public static IObservable<T> Skip<T>(this IObservable<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (count < 0) throw new ArgumentOutOfRangeException("count");

            // optimize .Skip(count).Skip(count)
            var skip = source as SkipObservable<T>;
            if (skip != null)
            {
                return skip.Combine(count);
            }

            return new SkipObservable<T>(source, count);
        }

        public static IObservable<T> SkipWhile<T>(this IObservable<T> source, Func<T, bool> predicate)
        {
            return new SkipWhileObservable<T>(source, predicate);
        }

        public static IObservable<T> SkipWhile<T>(this IObservable<T> source, Func<T, int, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");

            return new SkipWhileObservable<T>(source, predicate);
        }

        public static IObservable<T> SkipUntil<T, TOther>(this IObservable<T> source, IObservable<TOther> other)
        {
            return new SkipUntilObservable<T, TOther>(source, other);
        }

        // first, last, single

        public static IObservable<T> First<T>(this IObservable<T> source)
        {
            return new FirstObservable<T>(source, false);
        }
        public static IObservable<T> First<T>(this IObservable<T> source, Func<T, bool> predicate)
        {
            return new FirstObservable<T>(source, predicate, false);
        }

        public static IObservable<T> FirstOrDefault<T>(this IObservable<T> source)
        {
            return new FirstObservable<T>(source, true);
        }

        public static IObservable<T> FirstOrDefault<T>(this IObservable<T> source, Func<T, bool> predicate)
        {
            return new FirstObservable<T>(source, predicate, true);
        }
    }
}