using System;
using Core.Movement;
using NaughtyAttributes;
using NavySpade._PJ71.Squad;
using UnityEngine;

namespace NavySpade
{
    public class PlayerMovement : MovementBehaviourBase
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private bool _hasRotation;
        [SerializeField] private bool _resetYPos;

        [SerializeField] [ShowIf(nameof(_hasRotation))]
        private Transform _useObjectRotation;

        private JoystickInputProvider _inputProvider;
        private Vector3 _previewFrameMovementDirection;
        private bool _isActive;
        
        public SquadPlayer Squad { get; set; }

        public void Start()
        {
            _inputProvider = JoystickInputProvider.Instance;
        }

        public void Init(SquadPlayer squadPlayer)
        {
            Squad = squadPlayer;
            _isActive = true;
        }

        private void FixedUpdate()
        {
            if (_inputProvider == null)
                return;

            if (_isActive == false)
                return;

            Vector3 dir = GetDirection();
            Vector3 result = Time.fixedDeltaTime * Speed * dir;
            
            if (result != Vector3.zero)
            {
                _previewFrameMovementDirection = result;
            }
            
            _rigidbody.velocity = result;
            Squad.SquadMovement.SetVelocity = result;
            LookAt(_previewFrameMovementDirection);
        }
        
        private Vector3 GetDirection()
        {
            Vector3 rawDir = _inputProvider.Value;
            Vector3 dir = rawDir;
            if (_hasRotation)
            {
                Vector3 eulerRotation = _useObjectRotation.rotation.eulerAngles;
                dir = Quaternion.Euler(0, eulerRotation.y, 0) * new Vector3(rawDir.x, 0, rawDir.y);
            }

            return dir;
        }

        private void LookAt(Vector3 dir)
        {
            if (dir == Vector3.zero)
            {
                return;
            }

            _rigidbody.rotation = Quaternion.RotateTowards(
                _rigidbody.rotation,
                Quaternion.LookRotation(dir),
                Time.fixedDeltaTime * Settings.AngularSpeed);
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
        }
    }
}