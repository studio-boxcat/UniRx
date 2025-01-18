using System; // require keep for Windows Universal App
using UnityEngine;

#if !(UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5)
using UnityEngine.EventSystems;
#endif

namespace UniRx.Triggers
{
    // for Component
    public static partial class ObservableTriggerExtensions
    {
        #region ObservableEnableTrigger

        /// <summary>This function is called when the behaviour becomes disabled () or inactive.</summary>
        public static IObservable<Unit> OnDisableAsObservable(this Component component)
        {
            if (component == null || component.gameObject == null) return Observable.Empty<Unit>();
            return GetOrAddComponent<ObservableEnableTrigger>(component.gameObject).OnDisableAsObservable();
        }

        #endregion

        #region ObservableUpdateTrigger

        /// <summary>Update is called every frame, if the MonoBehaviour is enabled.</summary>
        public static IObservable<Unit> UpdateAsObservable(this Component component)
        {
            if (component == null || component.gameObject == null) return Observable.Empty<Unit>();
            return GetOrAddComponent<ObservableUpdateTrigger>(component.gameObject).UpdateAsObservable();
        }

        #endregion
    }
}