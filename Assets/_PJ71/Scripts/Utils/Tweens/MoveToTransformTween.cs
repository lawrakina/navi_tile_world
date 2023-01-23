using System;
using UnityEngine;

namespace pj40.Core.Tweens
{
    [Serializable]
    public class MoveToTransformTween<T, U>
        where T : MovementTypeBase
        where U : Component
    {
        public Vector3 TargetOffset { get; }

        public MoveToTransformTween(T movementTypeBase, Vector3 targetOffset,
            float minimumDistance = .2f)
        {
            TargetOffset = targetOffset;
            _movementType = movementTypeBase;
            _minimumDistance = minimumDistance;
        }

        private Vector3 _targetPosition;
        private Transform _targetTransform;

        public Vector3 TargetPosition
        {
            get
            {
                if (_targetTransform != null)
                    return _targetTransform.position;

                if (_getTarget != null)
                    return _getTarget.Invoke();

                return _targetPosition;
            }
        }

        public U TweenObject { get; private set; }

        public Transform TweenObjectTransform => TweenObject.transform;

        private T MovementType => _overrideMovementType ?? _movementType;

        private T _movementType;
        private T _overrideMovementType;
        
        private float _minimumDistance;
        private Func<Vector3> _getTarget;

        private bool _isFinished = true;
        private Action<U> _completeAction;
        private Transform _forward;

        public void StartTween(U tweenObject, Transform target, float speed, Action<U> onComplete = default,
            T movementOverride = default)
        {
            TweenObject = tweenObject;

            _targetTransform = target;
            _getTarget = null;

            _isFinished = false;

            _overrideMovementType = movementOverride;
            
            MovementType.Speed = speed;
            MovementType.Init(TweenObjectTransform.position, TargetPosition);

            _completeAction = onComplete;
        }

        public void StartTween(U tweenObject, Func<Vector3> target, float speed, Action<U> onComplete = default,
            T movementOverride = default)
        {
            TweenObject = tweenObject;

            _targetTransform = null;
            _getTarget = target;

            _isFinished = false;

            _overrideMovementType = movementOverride;
            
            MovementType.Speed = speed;
            MovementType.Init(TweenObjectTransform.position, TargetPosition);

            _completeAction = onComplete;
        }

        public void StartTween(U tweenObject, Vector3 target, float speed, Transform forwardLooking = null,
            Action<U> onComplete = default, T movementOverride = default)
        {
            TweenObject = tweenObject;
            _forward = forwardLooking;

            _targetPosition = target;
            _targetTransform = null;
            _getTarget = null;

            _isFinished = false;

            _overrideMovementType = movementOverride;
            
            MovementType.Speed = speed;
            MovementType.Init(TweenObjectTransform.position, TargetPosition);

            _completeAction = onComplete;
        }

        public void StartTween(U tweenObject, Func<Vector3> target, float speed, Transform forwardLooking = null,
            Action<U> onComplete = default, T movementOverride = default)
        {
            TweenObject = tweenObject;
            _forward = forwardLooking;

            _getTarget = target;
            _targetTransform = null;

            _isFinished = false;

            _overrideMovementType = movementOverride;
            
            MovementType.Speed = speed;
            MovementType.Init(TweenObjectTransform.position, TargetPosition);

            _completeAction = onComplete;
        }

        public void Kill()
        {
            _isFinished = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>is finish</returns>
        public bool Update(float dt)
        {
            if (_isFinished)
                return true;

            if (TweenObject == null)
            {
                _isFinished = true;
                return true;
            }
            
            var position = _movementType.NextPosition(TargetPosition, dt);
            TweenObjectTransform.position = position;

            var currentDistance = CalculateFullDistance(TweenObjectTransform.position, TargetPosition);

            if (_forward != null)
                TweenObjectTransform.transform.forward = _forward.forward;

            if (currentDistance > _minimumDistance)
                return false;

            _isFinished = true;

            _completeAction?.Invoke(TweenObject);
            return true;
        }

        private float CalculateFullDistance(Vector3 currentPosition, Vector3 targetPosition)
        {
            return Mathf.Sqrt(
                Mathf.Pow(targetPosition.x - currentPosition.x, 2) +
                Mathf.Pow(targetPosition.y - currentPosition.y, 2) +
                Mathf.Pow(targetPosition.z - currentPosition.z, 2)
            );
        }
    }
}