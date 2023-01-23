using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace NavySpade._PJ71.Utils.AnimatorUtils
{
    public class UnitAnimatorController : AnimatorController
    {
        public enum AnimType
        {
            Idle,
            Run,
            Gather,
            Attack,
        }

        [SerializeField] private bool _autoUpdateMovementState = true;
        [SerializeField] private bool _checkFromJoystick = true;
        [SerializeField] private float _checkTime = 0.02f;
        [SerializeField] private Rig _attackRig;

        private readonly int _movementSpeed = Animator.StringToHash("Speed");
        private readonly int _shootAxeTrigger = Animator.StringToHash("ShootAxe");
        private readonly int _shootWeaponTrigger = Animator.StringToHash("ShootWeapon");
        private readonly int _moveTrigger = Animator.StringToHash("MoveTrigger");
        private readonly int _moveBool = Animator.StringToHash("MoveBool");
        
        private readonly int _idleAnim = Animator.StringToHash("Idle");
        private readonly int _runAnim = Animator.StringToHash("Run");
        private readonly int _attackAnim = Animator.StringToHash("Shoot");
        
        private float _currentTime;
        private Vector3 _lastPosition;
        private JoystickInputProvider _inputProvider;
        private AnimType _currentAnim;

        public Rig AttackRig => _attackRig;

        private void Start()
        {
            _inputProvider = JoystickInputProvider.Instance;
        }

        public void PlayAnimation(AnimType animType)
        {
            int animHash = GetAnimHash(animType);
            Animator.Play(animHash, 0, 0);
        }

        public void PlayAnimation(AnimType animType, params AnimActionCallbackData[] eventCallbacks)
        {
            int animHash = GetAnimHash(animType);
            EventCallbacks = eventCallbacks;
            Animator.Play(animHash, 0, 0);
        }

        public void SetTrigger(AnimType animType, params AnimActionCallbackData[] eventCallbacks)
        {
            EventCallbacks = eventCallbacks;
            Animator.SetTrigger(GetAnimTrigger(animType));
        }

        public IEnumerator PlayAndWait(AnimType animType, params AnimActionCallbackData[] eventCallbacks)
        {
            int animHash = GetAnimHash(animType);
            EventCallbacks = eventCallbacks;
            yield return PlayAndWait(animHash, EventCallbacks);
        }
        
        public IEnumerator PlayAndWait(AnimType animType, int layer, float normalizedTime, params AnimActionCallbackData[] eventCallbacks)
        {
            int animHash = GetAnimHash(animType);
            EventCallbacks = eventCallbacks;
            yield return PlayAndWait(animHash, layer, normalizedTime, EventCallbacks);
        }

        private int GetAnimTrigger(AnimType animType)
        {
            switch (animType)
            {
                case AnimType.Gather:
                    return _shootAxeTrigger;
                case AnimType.Attack:
                    return _shootWeaponTrigger;
                default:
                    return _moveTrigger;
            }
        }
        
        private int GetAnimHash(AnimType animType)
        {
            switch (animType)
            {
                case AnimType.Idle:
                    return _idleAnim;
                case AnimType.Run:
                    return _runAnim;
                case AnimType.Attack:
                    return _attackAnim;
                default:
                    return _idleAnim;
            }
        }

        private void Update()
        {
            if(_autoUpdateMovementState == false)
                return;

            if (_checkFromJoystick)
            {
                var speed = _inputProvider.Value.magnitude;
                Animator.SetFloat(_movementSpeed, speed);
            }
            else
            {
                _currentTime += Time.deltaTime;
                if (_currentTime > _checkTime)
                {
                    var deltaPos = transform.position - _lastPosition;
                    var deltaNormalize = deltaPos / Time.deltaTime;
                    var unitsPerSecond = deltaNormalize.magnitude;
                    
                    Animator.SetFloat(_movementSpeed, unitsPerSecond);
                    _lastPosition = transform.position;
                    _currentTime = 0;
                }
            }
        }

        public void SetMoveBool(bool isMove)
        {
            Animator.SetBool(_moveBool, isMove);
        }
    }
}