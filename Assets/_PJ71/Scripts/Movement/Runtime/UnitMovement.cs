using System;
using System.Collections;
using System.Collections.Generic;
using Core.Input.Commands;
using UnityEngine;
using UnityEngine.AI;


namespace Core.Movement {
    public class UnitMovement : MovementBehaviourBase {
        #region AutoMotion

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private float _checkPosHasBeenReached = 0.5f;
        [SerializeField] private float _recalulatePathTime = 0.5f;


        #region PrivateData

        private bool _isReseted;
        private bool _isFinished;
        private bool _isStopping;
        private Vector3 _targetPosition;
        private Coroutine _checkingPosReachedCoroutine;
        private MoveCommand _currentMoveCommand;
        private Stack<MoveCommand> _commandsStack = new Stack<MoveCommand>();

        #endregion


        public event Action OnTargetReached;

        public bool IsDestinationReach =>
            Vector3.Distance(_rigidbody.position, _targetPosition) < Settings.StoppingDistance;

        private void Start() {
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = false;
            _navMeshAgent.speed = Settings.Speed;
            _navMeshAgent.angularSpeed = Settings.AngularSpeed;
        }

        private void OnDisable() {
            _isReseted = false;
            if (_checkingPosReachedCoroutine != null)
            {
                StopCoroutine(_checkingPosReachedCoroutine);
                _checkingPosReachedCoroutine = null;
            }
        }
        
        public void ToDefault() {
            StopFollowing();
            _navMeshAgent.stoppingDistance = 1;
            _navMeshAgent.acceleration = 8;
            ClearCommands();
        }

        private void FixedUpdate() {
            if (_manualControl)
            {
                _rigidbody.velocity = Vector3.zero;
                if (_recalculateRoutine == null)
                    return;

                if (_indexInPath >= _path.corners.Length)
                {
                    Calculate();

                    if (_indexInPath >= _path.corners.Length)
                        return;
                }

                var point = _path.corners[_indexInPath];
                point.y = transform.position.y;

                var velocity = GetVelocityAtTarget(point);

                var direction = velocity * Time.fixedDeltaTime;

                var from1 = VectorToMovementAxis(point);
                var to1 = VectorToMovementAxis(transform.position);

                var dir1 = from1 - to1;

                direction = Vector3.ClampMagnitude(direction, dir1.magnitude);

                // if (IsReachPoint(transform.position, TargetPoint))
                // return;

                var from = transform.position;
                var to = from + direction;

                // if (Vector3.Distance(transform.position, _path.corners[_path.corners.Length - 1]) < _correctOffset)
                // {
                //     transform.position = Vector3.MoveTowards(transform.position, _targetPoint.Value, _speedMoving * Time.deltaTime);
                //     return;
                // }
                // _navMeshAgent.speed = Mathf.Lerp(1, 10, Vector3.Distance(transform.position, _targetPoint.Value)) * _speedCoef;
                // _navMeshAgent.speed = Vector3.Distance(transform.position, _targetPoint.Value) > _distanceDelay ? _maxSpeed : _minSpeed;
                // if (_navMeshAgent.speed < 1f)
                // _navMeshAgent.speed = 1f;

                if (IsReachPoint(from, to))
                    transform.position = to;

                if (Vector3.Distance(point, transform.position) < _checkPosHasBeenReached)
                {
                    _indexInPath++;
                }


                return;
            }
            if (_isStopping)
                return;

            if (_isFinished)
                return;

            var dir = _navMeshAgent.velocity.normalized;
            _rigidbody.position += Time.deltaTime * Settings.Speed * dir;
            _rigidbody.velocity = Vector3.zero;
            _navMeshAgent.nextPosition = _rigidbody.position;

            if (Settings.UpdateRotation)
            {
                UpdateRotation(dir);
            }

            if (IsDestinationReach)
            {
                _isFinished = true;
                OnTargetReached?.Invoke();
                _currentMoveCommand = null;
            }
        }

        private void UpdateRotation(Vector3 dir) {
            dir.y = 0;
            if (dir == Vector3.zero)
                return;

            transform.rotation = Quaternion.RotateTowards(_rigidbody.rotation, Quaternion.LookRotation(dir),
                                                          Settings.AngularSpeed * Time.deltaTime);
        }

        public override void StartMove() {
            _isStopping = false;
        }

        public override void StopMove() {
            _isStopping = true;
        }

        public void ExecutePreviousMoveCommand() {
            if (_commandsStack.Count <= 0)
                return;

            _currentMoveCommand = _commandsStack.Pop();
            MoveTo(_currentMoveCommand.Movement, _currentMoveCommand.ReachCallback);
        }

        public void MoveTo(MoveCommand moveCommand, bool savePreviousCommand = false) {
            if (savePreviousCommand && _currentMoveCommand != null)
            {
                _commandsStack.Push(_currentMoveCommand);
            }

            _currentMoveCommand = moveCommand;
            MoveTo(_currentMoveCommand.Movement, _currentMoveCommand.ReachCallback);
        }

        public override void MoveTo(Vector3 targetPosition, Action onReachCallback = null) {
            _targetPosition = targetPosition;
            OnTargetReached = onReachCallback;

            if (_isReseted == false)
            {
                _navMeshAgent.enabled = false;
                _navMeshAgent.enabled = true;
                _isReseted = true;
            }

            _navMeshAgent.SetDestination(_targetPosition);
            _isFinished = false;
            StartCheckPosition();
        }

        public void MoveTo(Transform target) {
            _navMeshAgent.SetDestination(target.position);

            _isFinished = false;

            StartCheckPosition();
        }

        private void StartCheckPosition() {
            if (_checkingPosReachedCoroutine == null)
            {
                _checkingPosReachedCoroutine = StartCoroutine(CheckingPositionHasBeenReached());
            }
        }

        private IEnumerator CheckingPositionHasBeenReached() {
            while (true)
            {
                if (IsDestinationReach == false)
                {
                    _isFinished = false;
                }

                yield return new WaitForSeconds(_checkPosHasBeenReached);
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(_targetPosition, 0.5f);
        }

        public void ClearCommands() {
            _commandsStack.Clear();
        }

        public void SetAngular(float i) {
            AngularSpeed = i;
            _navMeshAgent.angularSpeed = i;
        }

        #endregion


        [SerializeField] private float _distanceDelay = 1.7f;
        [SerializeField] private float _minSpeed = 5f;
        [SerializeField] private float _maxSpeed = 7f;
        [SerializeField] private float _speedMoving = 5f;
        [SerializeField] private float _correctOffset = 0.01f;


        #region SquadMovement

        #region PrivateData

        private NavMeshPath _path;
        private int _indexInPath;
        private Coroutine _recalculateRoutine;
        private Vector3? _targetPoint;
        private bool _manualControl;

        #endregion


        public void Init(float stopingdistance, float acceleration) {
            _navMeshAgent.stoppingDistance = stopingdistance;
            _navMeshAgent.acceleration = acceleration;
            Init();
        }

        public void Init() {
            _manualControl = true;
            _path = new NavMeshPath();
            StopMove();
            StartFollowing();
        }

        public void Move(Vector3 position) {
            if(_targetPoint != null && (position - _targetPoint.Value).magnitude > 0.1f)
                transform.LookAt(position);
            SetTargetPoint(position);
        }

        public Vector3 TargetPoint => _targetPoint == null ? default : _targetPoint.Value;

        public void SetTargetPoint(Vector3 value) {
            _targetPoint = value;
            if (NavMesh.SamplePosition(value, out var hit, float.PositiveInfinity, NavMesh.AllAreas))
            {
                _targetPoint = hit.position;
            }
        }

        public void StartFollowing() {
            // _rigidbody.isKinematic = true;
            if (_recalculateRoutine != null)
                return;

            _navMeshAgent.enabled = true;
            // _navMeshAgent.speed += 1.3f;
            Calculate();
            _recalculateRoutine = StartCoroutine(CalculatePath());
        }

        public void StopFollowing() {
            if (_recalculateRoutine == null)
                return;

            StopCoroutine(_recalculateRoutine);
            _recalculateRoutine = null;
        }

        private IEnumerator CalculatePath() {
            while (true)
            {
                yield return new WaitForSeconds(_recalulatePathTime);
                Calculate();
            }
        }

        private void Calculate() {
            if (_targetPoint == null)
                return;

            _navMeshAgent.nextPosition = _rigidbody.position;

            if (_navMeshAgent.isOnNavMesh)
                _navMeshAgent.CalculatePath(_targetPoint.Value, _path);

            _indexInPath = 1;
        }

        // private void LateUpdate() {
        //     if (_recalculateRoutine == null)
        //         return;
        //
        //     if (_indexInPath >= _path.corners.Length)
        //     {
        //         Calculate();
        //
        //         if (_indexInPath >= _path.corners.Length)
        //             return;
        //     }
        //
        //     var point = _path.corners[_indexInPath];
        //     point.y = _rigidbody.position.y;
        //
        //     var velocity = GetVelocityAtTarget(point);
        //
        //     var direction = velocity * Time.deltaTime;
        //     
        //     var from1 = VectorToMovementAxis(point);
        //     var to1 = VectorToMovementAxis(_rigidbody.position);
        //
        //     var dir = from1 - to1;
        //
        //     direction = Vector3.ClampMagnitude(direction, dir.magnitude);
        //
        //     // if (IsReachPoint(_rigidbody.position, TargetPoint))
        //         // return;
        //
        //     var from = _rigidbody.position;
        //     var to = from + direction;
        //
        //     // if (Vector3.Distance(_rigidbody.position, _path.corners[_path.corners.Length - 1]) < _correctOffset)
        //     // {
        //     //     _rigidbody.position = Vector3.MoveTowards(_rigidbody.position, _targetPoint.Value, _speedMoving * Time.deltaTime);
        //     //     return;
        //     // }
        //     // _navMeshAgent.speed = Mathf.Lerp(1, 10, Vector3.Distance(transform.position, _targetPoint.Value)) * _speedCoef;
        //     // _navMeshAgent.speed = Vector3.Distance(transform.position, _targetPoint.Value) > _distanceDelay ? _maxSpeed : _minSpeed;
        //     // if (_navMeshAgent.speed < 1f)
        //     // _navMeshAgent.speed = 1f;
        //
        //     if (IsReachPoint(from, to))
        //         _rigidbody.position = to;
        //
        //     if (Vector3.Distance(point, _rigidbody.position) < _checkPosHasBeenReached)
        //     {
        //         _indexInPath++;
        //     }
        // }

        private void OnDrawGizmosSelected() {
            if (_path == null)
                return;

            var startPoint = transform.position;
            Gizmos.color = Color.red;

            foreach (var pathCorner in _path.corners)
            {
                Gizmos.DrawLine(startPoint, pathCorner);
                startPoint = pathCorner;
            }
        }

        public Vector3 GetVelocityAtTarget(Vector3 targetPoint) {
            var from = VectorToMovementAxis(targetPoint);
            var to = VectorToMovementAxis(_rigidbody.position);

            var dir = from - to;
            dir.Normalize();

            return dir * _speedMoving;
        }

        private bool IsReachPoint(Vector3 from, Vector3 to) {
            from = VectorToMovementAxis(from);
            to = VectorToMovementAxis(to);

            return Vector3.Distance(from, to) <
                   _checkPosHasBeenReached + GetVelocityAtTarget(to).magnitude * Time.fixedDeltaTime;
        }

        protected Vector3 VectorToMovementAxis(Vector3 vector) => VectorToMovementAxis(vector, Axis.TopDown);

        public Vector3 VectorToMovementAxis(Vector3 vector, Axis axis) {
            if ((axis & Axis.X) != Axis.X)
                vector.x = 0;
            if ((axis & Axis.Y) != Axis.Y)
                vector.y = 0;
            if ((axis & Axis.Z) != Axis.Z)
                vector.z = 0;

            return vector;
        }

        [Flags] public enum Axis {
            X = 1,
            Y = 2,
            Z = 4,
            TopDown = X | Z,
            Full3D = X | Y | Z,
            Full2D = X | Y
        }

        #endregion
    }
}