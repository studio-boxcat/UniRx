using System;
using System.Collections.Generic;
using UniRx.InternalUtil;
using UniRx.Operators;

namespace UniRx
{
    // Standard Query Operators

    // onNext implementation guide. enclose otherFunc but onNext is not catch.
    // try{ otherFunc(); } catch { onError() }
    // onNext();

    public static partial class Observable
    {
        public static IObservable<T> Where<T>(this IObservable<T> source, Func<T, bool> predicate)
        {
            // optimized path
            var whereObservable = source as UniRx.Operators.WhereObservable<T>;
            if (whereObservable != null)
            {
                return whereObservable.CombinePredicate(predicate);
            }

            return new WhereObservable<T>(source, predicate);
        }

        public static IObservable<T> Where<T>(this IObservable<T> source, Func<T, int, bool> predicate)
        {
            return new WhereObservable<T>(source, predicate);
        }

        public static IObservable<T> DefaultIfEmpty<T>(this IObservable<T> source)
        {
            return new DefaultIfEmptyObservable<T>(source, default(T));
        }

        public static IObservable<T> DefaultIfEmpty<T>(this IObservable<T> source, T defaultValue)
        {
            return new DefaultIfEmptyObservable<T>(source, defaultValue);
        }

        public static IObservable<TSource> Distinct<TSource>(this IObservable<TSource> source)
        {
#if !UniRxLibrary
            var comparer = UnityEqualityComparer.GetDefault<TSource>();
#else
            var comparer = EqualityComparer<TSource>.Default;
#endif

            return new DistinctObservable<TSource>(source, comparer);
        }

        public static IObservable<TSource> Distinct<TSource>(this IObservable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            return new DistinctObservable<TSource>(source, comparer);
        }

        public static IObservable<TSource> Distinct<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
#if !UniRxLibrary
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
#else
            var comparer = EqualityComparer<TKey>.Default;
#endif

            return new DistinctObservable<TSource, TKey>(source, keySelector, comparer);
        }

        public static IObservable<TSource> Distinct<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new DistinctObservable<TSource, TKey>(source, keySelector, comparer);
        }

        public static IObservable<T> DistinctUntilChanged<T>(this IObservable<T> source)
        {
#if !UniRxLibrary
            var comparer = UnityEqualityComparer.GetDefault<T>();
#else
            var comparer = EqualityComparer<T>.Default;
#endif

            return new DistinctUntilChangedObservable<T>(source, comparer);
        }

        public static IObservable<T> DistinctUntilChanged<T>(this IObservable<T> source, IEqualityComparer<T> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new DistinctUntilChangedObservable<T>(source, comparer);
        }

        public static IObservable<T> DistinctUntilChanged<T, TKey>(this IObservable<T> source, Func<T, TKey> keySelector)
        {
#if !UniRxLibrary
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
#else
            var comparer = EqualityComparer<TKey>.Default;
#endif

            return new DistinctUntilChangedObservable<T, TKey>(source, keySelector, comparer);
        }

        public static IObservable<T> DistinctUntilChanged<T, TKey>(this IObservable<T> source, Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new DistinctUntilChangedObservable<T, TKey>(source, keySelector, comparer);
        }
    }
}