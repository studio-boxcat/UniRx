using System; // require keep for Windows Universal App
using UnityEngine;

namespace UniRx.Triggers
{
    public abstract class ObservableTriggerBase : MonoBehaviour
    {
        /// <summary>This function is called when the MonoBehaviour will be destroyed.</summary>
        void OnDestroy()
        {
            RaiseOnCompletedOnDestroy();
        }

        protected abstract void RaiseOnCompletedOnDestroy();
    }
}