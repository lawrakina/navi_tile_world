using System;
using System.Collections;
using Project_60.Movement;
using UnityEngine;

namespace Core.Movement
{
    public class MovementByPath : MovementBehaviourBase
    {
        [SerializeField] private PathFinder _pathFinder;
        [SerializeField] private float _nextPointTolerance = 0.2f;
        [SerializeField] private bool _updateRotation = true;
        [SerializeField] private float _angularSpeed = 180f;
        [SerializeField] private float _stopTimeBetweenPoint;
        
        private bool _isActive = true;
        private bool _pathFound;
        private Vector3 _targetPosition;
        
        public float AngularSpeed
        {
            get => _angularSpeed;
            set => _angularSpeed = value;
        }
        
        public float StopTime
        {
            get => _stopTimeBetweenPoint;
            set => _stopTimeBetweenPoint = value;
        }

        public override void StartMove()
        {
            _isActive = true;
            Calculate();
        }

        public override void StopMove()
        {
            _isActive = false;
        }

        public override void MoveTo(Vector3 targetPosition, Action onReachCallback = null)
        {
            _targetPosition = targetPosition;
        }

        private void Update()
        {
            if(_isActive == false)
                return;
            
            if(_pathFound == false)
                return;
            
            UpdatePosition();
            UpdateRotation();
        }

        private void UpdatePosition()
        {
            Vector3 dir = (_targetPosition - transform.position).normalized;
            transform.position += Time.deltaTime * Speed * dir;

            if (Vector3.Distance(_targetPosition, transform.position) < _nextPointTolerance)
            {
                FinishPath();
            }
        }

        private void UpdateRotation()
        {
            if(_updateRotation == false)
                return;
            
            Vector3 dir = (_targetPosition - transform.position).normalized;
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, 
                Quaternion.LookRotation(dir), 
                _angularSpeed * Time.deltaTime);
        }
        
        private void Calculate()
        {
            if (_pathFinder.TryGetNextPoint(out Vector3 nextPoint) && 
                Vector3.Distance(transform.position, nextPoint) > _nextPointTolerance)
            {
                _targetPosition = nextPoint;
                _pathFound = true;
                return;
            }

            _pathFound = false;
            Debug.Log("PATH NOT FOUND");
        }

        private void FinishPath()
        {
            _pathFound = false;
            if (_stopTimeBetweenPoint > 0)
            {
                StartCoroutine(StopProcess());
            }
            else
            {
                Calculate();
            }
        }

        private IEnumerator StopProcess()
        {
            yield return new WaitForSeconds(_stopTimeBetweenPoint);
            Calculate();
        }
    }
}