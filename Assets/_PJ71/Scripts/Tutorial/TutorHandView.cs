using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NavySpade.pj46.Tutor
{
    public class TutorHandView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private AnimationCurve _movementCurve;
        [SerializeField] private float _speed;

        private Transform _from;
        private Transform _to;
        private bool _isShow;

        private void Start()
        {
            _rectTransform.gameObject.SetActive(true);
        }

        public void MoveFingerToFrom(Transform from, Transform to)
        {
            gameObject.SetActive(true);
            _rectTransform.gameObject.SetActive(true);
            _isShow = true;
            
            _from = from;
            _to = to;
            StartCoroutine(MovingFromTo(from, to));
        }

        public void MoveFingerToFrom(RectTransform from, RectTransform to)
        {
            gameObject.SetActive(true);
            _rectTransform.gameObject.SetActive(true);
            _isShow = true;
            
            StartCoroutine(MovingFromTo(from, to));
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        [Button]
        private void Reload()
        {
            StartCoroutine(MovingFromTo(_from, _to));
        }

        private IEnumerator MovingFromTo(Transform from, Transform to)
        {
            float progress = 0;
            while (true)
            {
                progress += Time.deltaTime * _speed;
                
                Vector3 fromScreenPos = Camera.main.WorldToScreenPoint(from.position);
                Vector3 toScreenPos = Camera.main.WorldToScreenPoint(to.position);
                
                _rectTransform.position = Vector3.Lerp(
                    fromScreenPos,
                    toScreenPos,
                    _movementCurve.Evaluate(progress));
                
                _rectTransform.localScale = Vector3.one;
                yield return null;

                if (progress >= 1)
                    progress = 0;
            }
        }
        
        private IEnumerator MovingFromTo(RectTransform from, RectTransform to)
        {
            float progress = 0;
            while (true)
            {
                progress += Time.deltaTime * _speed;
                
                _rectTransform.position = Vector3.Lerp(
                    from.position,
                    to.position,
                    _movementCurve.Evaluate(progress));
                
                _rectTransform.localScale = Vector3.one;
                yield return null;

                if (progress >= 1)
                    progress = 0;
            }
        }
    }
}