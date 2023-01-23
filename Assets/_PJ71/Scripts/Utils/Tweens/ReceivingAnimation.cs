using System;
using System.Collections.Generic;
using NavySpade.Modules.Pooling.Runtime;
using UnityEngine;

namespace pj40.Core.Tweens
{
    public abstract class ReceivingAnimation<T, V> : MonoBehaviour 
        where T : MovementTypeBase
        where V : Component
    {
        [SerializeField] private float _speed;
        
        private List<MoveToTransformTween<T, V>> _tweens;
        private ObjectPool<MoveToTransformTween<T, V>> _tweensPoolComponents;

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        private void Awake()
        {
            _tweens = new List<MoveToTransformTween<T, V>>(25);
            _tweensPoolComponents = new ObjectPool<MoveToTransformTween<T, V>>();

            _tweensPoolComponents.Initialize(
                25,
                () => new MoveToTransformTween<T, V>(InitializeMovementTypeBase(), Vector3.zero));
        }

        protected abstract T InitializeMovementTypeBase();

        private void Update()
        {
            for (var i = 0; i < _tweens.Count; i++)
            {
                var tween = _tweens[i];

                if (!tween.Update(Time.deltaTime))
                    continue;
                    
                if(tween.TweenObject != null)
                    tween.TweenObjectTransform.position = tween.TargetPosition;

                _tweensPoolComponents.Return(tween);
                _tweens.RemoveAt(i);
                i--;
            }
        }

        public void ClearAllTweens()
        {
            foreach (var tween in _tweens)
            {
                tween.Kill();
                _tweensPoolComponents.Return(tween);
            }
            
            _tweens.Clear();
        }

        public void PlayAnimation(V target, Vector3 endPoint, bool isLookAtForward, Action<V> endCallback = null)
        {
            if (enabled == false)
            {
                target.transform.parent = transform;
                endCallback?.Invoke(target);
                return;
            }

            var tween = _tweensPoolComponents.Get();
            tween.StartTween(target, endPoint, _speed, isLookAtForward ? transform : null, endCallback);
            
            _tweens.Add(tween);
        }

        public MoveToTransformTween<T,V> PlayAnimation(V target, Func<Vector3> endPoint, bool isLookAtForward, Action<V> endCallback = null)
        {
            if (enabled == false)
            {
                target.transform.parent = transform;
                endCallback?.Invoke(target);
                return null;
            }

            var tween = _tweensPoolComponents.Get();
            tween.StartTween(target, endPoint, _speed, isLookAtForward ? transform : null, endCallback);
            
            _tweens.Add(tween);
            return tween;
        }
        
        public MoveToTransformTween<T,V> PlayAnimation(V target, Func<Vector3> endPoint, Transform lookForwardTr, Action<V> endCallback = null)
        {
            if (enabled == false)
            {
                target.transform.parent = transform;
                endCallback?.Invoke(target);
                return null;
            }

            var tween = _tweensPoolComponents.Get();
            tween.StartTween(target, endPoint, _speed, lookForwardTr, endCallback);
            
            _tweens.Add(tween);
            return tween;
        }
    }
}