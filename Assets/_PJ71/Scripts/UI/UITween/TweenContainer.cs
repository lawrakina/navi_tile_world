using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NavySpade._PJ71.UI.UITween
{
    public class TweenContainer : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private AnimationCurve _animationCurve;
        
        private Action _callback;
        private float _speed;

        
        public void StartTween(Vector3 to, TweenExtraData extraData, Action callback)
        {
            _icon.sprite = extraData.Icon;
            _speed = extraData.Speed;
            
            _callback = callback;
            StartCoroutine(Tweening(transform.position, to));
        }
        
        private IEnumerator Tweening(Vector3 from, Vector3 to)
        {
            float progress = 0;
            while (progress < 1)
            {
                progress += Time.deltaTime * _speed;
                transform.position = Vector3.Lerp(from, to, _animationCurve.Evaluate(progress));
                yield return null;
            }
            
            _callback?.Invoke();
        }
    }
}