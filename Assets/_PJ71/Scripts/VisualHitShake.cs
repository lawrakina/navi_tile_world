using DG.Tweening;
using UnityEngine;

namespace NavySpade.Project_51.Scripts.Resources.ResourcesObjects.Visual
{
    public class VisualHitShake : MonoBehaviour
    {
        [SerializeField] private float _duration;
        [SerializeField] private float _strength;
        [SerializeField] private int _vibrato;
        [SerializeField] private float _randomness;

        private Tween _shakeTween;

        public void Play()
        {
            _shakeTween?.Kill();
            _shakeTween = transform.DOShakeRotation(_duration, _strength, _vibrato, _randomness);
        }

        private void OnDestroy()
        {
            _shakeTween?.Kill();
        }
    }
}