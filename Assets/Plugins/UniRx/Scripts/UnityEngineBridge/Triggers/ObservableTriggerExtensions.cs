using System; // require keep for Windows Universal App
using UnityEngine;

namespace UniRx.Triggers
{
    // for GameObject
    public static partial class ObservableTriggerExtensions
    {
        /// <summary>Update is called every frame, if the MonoBehaviour is enabled.</summary>
        public static IObservable<Unit> UpdateAsObservable(this GameObject gameObject)
        {
            if (gameObject == null) return Observable.Empty<Unit>();
            return GetOrAddComponent<ObservableUpdateTrigger>(gameObject).UpdateAsObservable();
        }

        static T GetOrAddComponent<T>(GameObject gameObject)
            where T : Component
        {
            var component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }
    }
}
