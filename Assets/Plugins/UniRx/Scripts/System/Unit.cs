using System;

namespace UniRx
{
    public struct Unit : IEquatable<Unit>
    {
        public static readonly Unit Default = new();
        public bool Equals(Unit other) => true;
        public override bool Equals(object obj) => obj is Unit;
        public override int GetHashCode() => 0;
        public override string ToString() => "()";
    }
}