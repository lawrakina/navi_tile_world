using UnityEngine;


namespace NavySpade._PJ71.UI {
    public class ButtonAnimation  : MonoBehaviour {
        [SerializeField] private Animation _animation;

        public void StartAnimation() {
            _animation.Play();
        }
    }
}