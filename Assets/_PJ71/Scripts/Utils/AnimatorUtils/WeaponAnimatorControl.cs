using System.Collections;
using NavySpade._PJ71.Utils.AnimatorUtils;

namespace NS.Core.Utils.AnimatorUtils
{
    public class WeaponAnimatorControl : AnimatorController
    {
        private int _fireAnimHash = UnityEngine.Animator.StringToHash("Fire");
        private int _idleAnimHash = UnityEngine.Animator.StringToHash("idle");
        
        public IEnumerator PlayShootingAnim(params AnimActionCallbackData[] eventCallbacks)
        {
            yield return PlayAndWait(_fireAnimHash, eventCallbacks);
        }

        public void PlayIdle()
        {
            Animator.Play(_idleAnimHash);
        }
    }
}