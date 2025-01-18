using System;
using UniRx.Operators;

namespace UniRx
{
    // Standard Query Operators

    // onNext implementation guide. enclose otherFunc but onNext is not catch.
    // try{ otherFunc(); } catch { onError() }
    // onNext();

    public static partial class Observable
    {
        public static IObservable<T> Where<T>(this ReactiveCommand<T> source, Func<T, bool> predicate)
        {
            return new WhereObservable<T>(source, predicate);
        }
    }
}