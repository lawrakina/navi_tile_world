using System;
using UnityEngine;
using UnityEngine.AI;

namespace NavySpade._PJ71.Movement.Moveables
{
    [Serializable]
    [AddTypeMenu("NavMeshAgent")]
    public class MoveableNavMeshAgent : IMoveable
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        
        private MoveSettings _moveSettings;
        private Vector3 _targetPosition;
        private Action _destinationReached;

        public bool IsDestinationReach => _navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance;
        
        
        public void Init(MoveSettings moveSettings)
        {
            _moveSettings = moveSettings;
            _navMeshAgent.speed = _moveSettings.Speed;
            _navMeshAgent.angularSpeed = _moveSettings.AngularSpeed;
        }

        public void Update()
        {
            if (IsDestinationReach)
            {
                _destinationReached?.Invoke();
            }
        }
        
        public void MoveTo(Vector3 position, Action destinationReachedCallback = null)
        {
            _targetPosition = position;
            _navMeshAgent.SetDestination(_targetPosition);
            _destinationReached = destinationReachedCallback;
            _navMeshAgent.isStopped = false;
        }

        public void StopMove()
        {
            _navMeshAgent.isStopped = true;
        }

        public void StartMove()
        {
            _navMeshAgent.isStopped = false;
        }
    }
}