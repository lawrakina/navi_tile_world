using System;
using UnityEngine;

namespace NavySpade._PJ71.Movement.Moveables
{
    [Serializable]
    [AddTypeMenu("Transform")]
    public class MoveableTransform : IMoveable
    {
        [SerializeField] private Transform _transform;

        private MoveSettings _moveSettings;
        private Vector3 _targetPosition;
        private Action _destinationReached;

        public bool IsDestinationReach => 
            Vector3.Distance(_transform.position, _targetPosition) < _moveSettings.StoppingDistance;
        
        public void Init(MoveSettings moveSettings)
        {
            _moveSettings = moveSettings;
        }

        public void Update()
        {
            var dir = (_targetPosition - _transform.position).normalized;
            _transform.position += Time.deltaTime * _moveSettings.Speed * dir;

            if (_moveSettings.UpdateRotation)
            {
                UpdateRotation(dir);
            }

            if (IsDestinationReach)
            {
                _destinationReached?.Invoke();
            }
        }

        public void MoveTo(Vector3 position, Action destinationReachedCallback = null)
        {
            _targetPosition = position;
            _destinationReached = destinationReachedCallback;
        }

        public void StopMove() { }
        public void StartMove() { }

        private void UpdateRotation(Vector3 dir)
        {
            _transform.rotation = Quaternion.RotateTowards(_transform.rotation, Quaternion.LookRotation(dir),
                _moveSettings.AngularSpeed * Time.deltaTime);
        }
    }
}