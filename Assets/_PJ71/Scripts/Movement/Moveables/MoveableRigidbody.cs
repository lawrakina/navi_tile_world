using System;
using UnityEngine;

namespace NavySpade._PJ71.Movement.Moveables
{
    [Serializable]
    [AddTypeMenu("Rigidbody")]
    public class MoveableRigidbody : IMoveable
    {
        [SerializeField] private Rigidbody _rigidbody;

        private MoveSettings _moveSettings;
        private Vector3 _targetPosition;
        private Action _destinationReached;

        public bool IsDestinationReach => 
            Vector3.Distance(_rigidbody.position, _targetPosition) < _moveSettings.StoppingDistance;

        public void Init(MoveSettings moveSettings)
        {
            _moveSettings = moveSettings;
        }

        public void Update()
        {
            var dir = (_targetPosition - _rigidbody.position).normalized;
            _rigidbody.velocity = Time.deltaTime * _moveSettings.Speed * dir;

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

        public void StopMove()
        {
            _rigidbody.velocity = Vector3.zero;
        }

        public void StartMove()
        {
            _rigidbody.isKinematic = false;
        }

        private void UpdateRotation(Vector3 dir)
        {
            _rigidbody.rotation = Quaternion.RotateTowards(_rigidbody.rotation, Quaternion.LookRotation(dir),
                _moveSettings.AngularSpeed * Time.deltaTime);
        }
    }
}