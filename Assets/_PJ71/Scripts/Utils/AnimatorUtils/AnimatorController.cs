using System.Collections;
using UnityEngine;

namespace NavySpade._PJ71.Utils.AnimatorUtils
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorController : MonoBehaviour
    {
        protected AnimActionCallbackData[] EventCallbacks;

        private Animator _animator;

        protected Animator Animator => _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public IEnumerator PlayAndWait(int animHash, params AnimActionCallbackData[] eventCallbacks)
        {
            return PlayAndWait(animHash, 0, 0, eventCallbacks);
        }
        
        public IEnumerator PlayAndWait(int animHash, int layer, float normalizedTime, params AnimActionCallbackData[] eventCallbacks)
        {
            _animator.Play(animHash, layer, normalizedTime);
            EventCallbacks = eventCallbacks;

            yield return null;
            while (_animator.GetCurrentAnimatorStateInfo(layer).normalizedTime < 1)
            {
                yield return null;
            }
        }

        private void EventInvoked(AnimEvent eventName)
        {
            foreach (var animActionCallbackData in EventCallbacks)
            {
                if (animActionCallbackData.AnimEvent == eventName)
                {
                    animActionCallbackData.EventCallback?.Invoke();
                }
            }
        }
    }
}