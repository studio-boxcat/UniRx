using System; // require keep for Windows Universal App
using UnityEngine;

namespace UniRx.Triggers
{
    [DisallowMultipleComponent]
    public class ObservableEnableTrigger : ObservableTriggerBase
    {
        Subject<Unit> onDisable;

        /// <summary>This function is called when the behaviour becomes disabled () or inactive.</summary>
        void OnDisable()
        {
            if (onDisable != null) onDisable.OnNext(Unit.Default);
        }

        /// <summary>This function is called when the behaviour becomes disabled () or inactive.</summary>
        public IObservable<Unit> OnDisableAsObservable()
        {
            return onDisable ?? (onDisable = new Subject<Unit>());
        }

        protected override void RaiseOnCompletedOnDestroy()
        {
            if (onDisable != null)
            {
                onDisable.OnCompleted();
            }
        }
    }
}