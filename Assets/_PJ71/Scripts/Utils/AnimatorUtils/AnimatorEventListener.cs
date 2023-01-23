using System;
using UnityEngine;

namespace NavySpade._PJ71.Utils.AnimatorUtils
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorEventListener : MonoBehaviour
    {
        public event Action<AnimEvent> EventFired;

        public void FireAnimEvent(AnimEvent eventType)
        {
            EventFired?.Invoke(eventType);
        }  
    }
}