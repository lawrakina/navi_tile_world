using System;
using UnityEngine;

namespace Core.Movement
{
    public class DirectMovementBehaviour : MovementBehaviourBase
    {
        [SerializeField] private Vector3 _direction = Vector3.forward;

        private bool _isActive = true;
        private Transform _transform;
        
        private void Awake()
        {
            _transform = transform;
            _isActive = true;
        }

        public void SetDirection(Vector3 direction)
        {
            _direction = direction;
        }
        
        private void Update()
        {
            if(_isActive == false)
                return;
            
            _transform.position += Time.deltaTime * Speed * _direction;
        }

        public override void StartMove()
        {
            _isActive = true;
        }

        public override void StopMove()
        {
            _isActive = false;
        }

        public override void MoveTo(Vector3 targetPosition, Action onReachCallback = null)
        {
            _direction = targetPosition - _transform.position;
        }
    }
}